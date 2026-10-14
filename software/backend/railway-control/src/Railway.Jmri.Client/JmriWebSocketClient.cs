using System.Net.WebSockets;
using System.Text;
using System.Diagnostics;

namespace Railway.Jmri.Client
{
    /// <summary>
    /// Mantiene una conexion WebSocket con JMRI, recibe mensajes y envia heartbeats.
    /// Supervisa las caidas y ejecuta reintentos segun las opciones configuradas.
    /// </summary>
    /// <remarks>
    /// Los eventos se entregan en una cola independiente. Las operaciones no esperan
    /// a que sus suscriptores terminen de procesar las notificaciones.
    /// </remarks>
    public class JmriWebSocketClient : IJmriConnection
    {
        // Estados y Opciones
        private readonly JmriClientOptions _options;

        private ClientWebSocket? _socket;
        private CancellationTokenSource? _lifetimeCts;
        private Task? _receiveTask;
        private readonly object _notificationLock = new();
        private Task _notificationTask = Task.CompletedTask;
        private bool _disposed;
        private int _disconnectNotificationPending;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        public JmriConnectionState State { get; private set; } = JmriConnectionState.Disconnected;
        public bool IsConnected => State == JmriConnectionState.Connected;

        // Eventos / Notificaciones
        // Permite a la consola reaccionar y mostrar diagnosticos
        public event Action? Connected;
        public event Action<string>? Disconnected; // Razón de desconexión
        public event Action<int, TimeSpan>? Reconnecting; // Intento actual y tiempo de espera
        public event Action<string>? MessageReceived; // Mensaje JSON crudo recibido desde JMRI

        /// <summary>
        /// Valida el intervalo del heartbeat y guarda las opciones sin iniciar la conexion.
        /// </summary>
        /// <param name="options">URL e intervalos de conexion. No deben modificarse durante el uso del cliente.</param>
        /// <exception cref="ArgumentNullException">Las opciones son nulas.</exception>
        /// <exception cref="ArgumentOutOfRangeException">El intervalo del heartbeat no es mayor que cero.</exception>
        public JmriWebSocketClient(JmriClientOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (options.HeartbeatIntervalSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(options.HeartbeatIntervalSeconds),
                    options.HeartbeatIntervalSeconds,
                    "El intervalo del heartbeat debe ser mayor que cero.");
            }
            
            _options = options;
        }

        /// <summary>
        /// Establece la conexion inicial y arranca la supervision de recepcion,
        /// heartbeat y reconexion en segundo plano.
        /// </summary>
        /// <param name="cancellationToken">Cancela la espera del semaforo o el intento inicial de conexion.</param>
        /// <returns>Una tarea que termina tras iniciar la supervision; no representa toda la vida de la conexion.</returns>
        /// <remarks>
        /// Encola <see cref="Connected"/> cuando conecta. Un fallo inicial se propaga al llamador;
        /// los reintentos automaticos se aplican a caidas posteriores.
        /// Para detener un cliente conectado, use <see cref="DisconnectAsync"/>.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">El cliente ya fue liberado.</exception>
        /// <exception cref="InvalidOperationException">El estado no permite conectar o la URL no usa ws/wss.</exception>
        /// <exception cref="UriFormatException">La URL no tiene un formato valido.</exception>
        /// <exception cref="WebSocketException">No se pudo establecer la conexion inicial.</exception>
        /// <exception cref="OperationCanceledException">Se cancelo la espera o la conexion inicial.</exception>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            await _connectionLock.WaitAsync(cancellationToken);

            try
            {

                ObjectDisposedException.ThrowIf(_disposed, this);

                if (State != JmriConnectionState.Disconnected && State != JmriConnectionState.Failed)
                    throw new InvalidOperationException("El cliente ya esta conectado o intentando conectarse.");

                var uri = new Uri(_options.WebSocketUrl);

                if (uri.Scheme != "ws" && uri.Scheme != "wss")
                    throw new InvalidOperationException("La URL del WebSocket debe usar ws:// o wss://.");

                _socket?.Dispose();
                _socket = new ClientWebSocket();

                State = JmriConnectionState.Connecting;

                try
                {
                    await _socket.ConnectAsync(uri, cancellationToken);
                    State = JmriConnectionState.Connected;

                    Interlocked.Exchange(ref _disconnectNotificationPending, 1);

                    _lifetimeCts = new CancellationTokenSource();
                    QueueNotification(Connected, handler => ((Action)handler)());
                    _receiveTask = SuperviseConnectionAsync(_lifetimeCts.Token);

                }
                catch (OperationCanceledException)
                {
                    _socket.Dispose();
                    _socket = null;
                    State = JmriConnectionState.Disconnected;
                    throw;
                }
                catch
                {
                    _socket.Dispose();
                    _socket = null;
                    State = JmriConnectionState.Failed;
                    throw;
                }
            }
            finally
            {
                _connectionLock.Release();
            }

        }

        /// <summary>
        /// Solicita detener la supervision, espera su finalizacion y libera el socket
        /// y el token interno. Deja el estado en Disconnected y notifica el cierre si corresponde.
        /// </summary>
        /// <param name="cancellationToken">Cancela solo la espera del semaforo; una vez adquirido, se completa la limpieza.</param>
        /// <returns>Una tarea que termina al liberar los recursos, sin esperar la entrega de eventos.</returns>
        /// <remarks>
        /// Puede llamarse cuando la conexion ya termino. Cancela tambien los reintentos
        /// pendientes; no garantiza un cierre ordenado del protocolo WebSocket.
        /// </remarks>
        /// <exception cref="OperationCanceledException">Se cancelo la espera para iniciar la desconexion.</exception>
        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            await _connectionLock.WaitAsync(cancellationToken);

            try
            {
                try
                {
                    _lifetimeCts?.Cancel();
                    if (_receiveTask is not null)
                    {
                        await _receiveTask;
                    }
                }
                finally
                {
                    _socket?.Dispose();
                    _socket = null;

                    _lifetimeCts?.Dispose();
                    _lifetimeCts = null;

                    _receiveTask = null;
                    State = JmriConnectionState.Disconnected;
                }
            }
            finally
            {
                _connectionLock.Release();

            }

            NotifyDisconnectedOnce("Desconexion solicitada por el cliente.");
        }

        /// <summary>
        /// Envia un mensaje de texto completo en UTF-8, serializando los envios concurrentes.
        /// </summary>
        /// <param name="message">Texto que se enviara; no se valida su contenido JSON.</param>
        /// <param name="cancellationToken">Cancela la espera del semaforo de envio o la operacion de envio.</param>
        /// <returns>Una tarea que termina al enviar, sin esperar una respuesta de JMRI.</returns>
        /// <exception cref="ArgumentNullException">El mensaje es nulo.</exception>
        /// <exception cref="ObjectDisposedException">El cliente o el socket ya fue liberado.</exception>
        /// <exception cref="InvalidOperationException">No existe un socket abierto.</exception>
        /// <exception cref="WebSocketException">Ocurrio un error de comunicacion durante el envio.</exception>
        /// <exception cref="OperationCanceledException">Se cancelo la espera o el envio.</exception>
        public async Task SendAsync(string message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            await _sendLock.WaitAsync(cancellationToken);

            try
            {
                ObjectDisposedException.ThrowIf(_disposed, this);

                var socket = _socket;

                if (socket is null || socket.State != WebSocketState.Open)
                {
                    throw new InvalidOperationException(
                        "No hay una conexion WebSocket abierta.");
                }

                var bytes = Encoding.UTF8.GetBytes(message);

                await socket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: cancellationToken);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        /// <summary>
        /// Marca el cliente como liberado y reutiliza DisconnectAsync para detenerlo y limpiar sus recursos.
        /// </summary>
        /// <returns>Una tarea de valor que representa la limpieza iniciada por esta llamada.</returns>
        /// <remarks>
        /// Las llamadas posteriores no repiten la limpieza. Si se llama simultaneamente,
        /// una llamada posterior puede terminar antes de que finalice la primera.
        /// No espera a que se entreguen las notificaciones pendientes.
        /// </remarks>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            await DisconnectAsync();

            return;
        }

        /// <summary>
        /// Recibe fragmentos hasta completar cada mensaje, los decodifica como UTF-8
        /// y encola MessageReceived. Ante cierre remoto o error de WebSocket, libera
        /// el socket y notifica la desconexion una sola vez.
        /// </summary>
        /// <param name="cancellationToken">Detiene la recepcion de esta conexion.</param>
        /// <returns>Una tarea que termina al cancelar, recibir un cierre o detectar un error de comunicacion.</returns>
        /// <remarks>Responde al cierre remoto. Rechaza mensajes binarios y no inicia reintentos por su cuenta.</remarks>
        private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
        {
            var socket = _socket ?? throw new InvalidOperationException("El socket no está inicializado.");

            var buffer = new byte[4096];
            string? disconnectedReason = null;

            try
            {

                while (true)
                {
                    using var message = new MemoryStream();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            disconnectedReason = result.CloseStatusDescription ?? "Closed by JMRI";
                            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Cierre recibido", cancellationToken);

                            return;
                        }

                        if (result.MessageType != WebSocketMessageType.Text)
                        {
                            throw new WebSocketException("Se esperaba un mensaje de texto.");
                        }

                        message.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    var text = Encoding.UTF8.GetString(message.ToArray());
                    QueueNotification(MessageReceived, handler => ((Action<string>)handler)(text));
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // DisconnectAsync se encarga del cierre voluntario.
            }
            catch (WebSocketException ex)
            {
                disconnectedReason = ex.Message;
            }
            finally
            {
                if (disconnectedReason is not null)
                {
                    socket.Dispose();

                    State = JmriConnectionState.Disconnected;

                    NotifyDisconnectedOnce(disconnectedReason);
                }
            }
        }

        /// <summary>
        /// Encola una notificacion sin ejecutarla en la tarea que recibe mensajes.
        /// Invoca los suscriptores en orden y registra las excepciones sincronicas
        /// de cada uno sin interrumpir a los demas.
        /// </summary>
        /// <param name="subscribers">Suscriptores presentes al encolar; si son nulos, no se realiza trabajo.</param>
        /// <param name="invokeSubscriber">Accion que invoca un suscriptor con los argumentos del evento.</param>
        private void QueueNotification(Delegate? subscribers, Action<Delegate> invokeSubscriber)
        {
            if (subscribers is null)
                return;

            lock (_notificationLock)
            {
                _notificationTask = _notificationTask.ContinueWith
                (_ =>
                    {
                        foreach (var subscriber in subscribers.GetInvocationList())
                        {
                            try
                            {
                                invokeSubscriber(subscriber);
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("JMRI event subscriber failed: {0}", ex);
                            }
                        }
                    }, 
                    CancellationToken.None, 
                    TaskContinuationOptions.None, 
                    TaskScheduler.Default
                );
            }
        }

        /// <summary>
        /// Alterna la ejecucion de cada conexion con los reintentos tras una caida.
        /// Termina cuando se solicita detener el cliente o se agotan los intentos.
        /// </summary>
        /// <param name="cancellationToken">Controla toda la supervision, incluidas las esperas de reconexion.</param>
        /// <returns>Una tarea que abarca recepcion, heartbeat y reintentos durante la vida del cliente.</returns>
        private async Task SuperviseConnectionAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await RunConnectionAsync(cancellationToken);

                    // Si DisconnectAsync solicito detener el cliente, no reconectar.
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    await ReconnectAsync(cancellationToken);

                    // ReconnectAsync debe dejar Failed si agota los intentos.
                    if (State == JmriConnectionState.Failed)
                        return;
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // DisconnectAsync se encarga del cierre voluntario.
            }
        }
        /// <summary>
        /// Espera el intervalo configurado e intenta conectar un socket nuevo por intento.
        /// Encola Reconnecting antes de cada espera y Connected cuando logra conectar.
        /// </summary>
        /// <param name="cancellationToken">Cancela las esperas y los intentos de conexion.</param>
        /// <returns>Una tarea que termina con estado Connected o Failed si se agotan los intentos.</returns>
        /// <remarks>
        /// Libera los sockets de intentos fallidos. El supervisor inicia la recepcion
        /// y el heartbeat de la nueva conexion; este metodo no los inicia.
        /// </remarks>
        /// <exception cref="OperationCanceledException">Se solicito detener los reintentos.</exception>
        private async Task ReconnectAsync(CancellationToken cancellationToken)
        {
            State = JmriConnectionState.Reconnecting;

            for (var attempt = 1; attempt <= _options.MaxReconnectAttempts; attempt++)
            {
                var delay = TimeSpan.FromSeconds(_options.ReconnectIntervalSeconds);

                var currentAttempt = attempt;

                QueueNotification(Reconnecting, handler => ((Action<int, TimeSpan>)handler)(currentAttempt, delay));

                await Task.Delay(delay, cancellationToken);

                var socket = new ClientWebSocket();

                try
                {
                    await socket.ConnectAsync(new Uri(_options.WebSocketUrl), cancellationToken);
                    _socket = socket;
                    State = JmriConnectionState.Connected;

                    Interlocked.Exchange(ref _disconnectNotificationPending, 1);

                    QueueNotification(Connected, handler => ((Action)handler)());

                    return;
                }
                catch (WebSocketException)
                {
                    socket.Dispose();
                }
                catch
                {
                    socket.Dispose();
                    throw;
                }
            }

            State = JmriConnectionState.Failed;
        }

        /// <summary>
        /// Consume atomicamente la marca de notificacion pendiente y encola
        /// Disconnected como maximo una vez por conexion exitosa.
        /// </summary>
        /// <param name="reason">Motivo del cierre o del error de comunicacion.</param>
        private void NotifyDisconnectedOnce(string reason)
        {
            if (Interlocked.Exchange(ref _disconnectNotificationPending, 0) == 0)
                return;

            QueueNotification(Disconnected, handler =>
                ((Action<string>)handler)(reason));
        }

        /// <summary>
        /// Envia un ping JSON cada HeartbeatIntervalSeconds mientras la conexion este activa.
        /// El primer ping se envia despues del primer intervalo.
        /// </summary>
        /// <param name="cancellationToken">Cancela la espera del temporizador o el envio del ping.</param>
        /// <returns>Una tarea que permanece activa hasta cancelar o fallar un envio.</returns>
        /// <remarks>No verifica las respuestas pong ni aplica un tiempo limite para recibirlas.</remarks>
        private async Task HeartbeatLoopAsync(CancellationToken cancellationToken)
        {
            var interval = TimeSpan.FromSeconds(_options.HeartbeatIntervalSeconds);

            using var timer = new PeriodicTimer(interval);

            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await SendAsync("{\"type\":\"ping\"}", cancellationToken);
            }
        }

        /// <summary>
        /// Ejecuta recepcion y heartbeat para un socket. Cuando una tarea termina,
        /// cancela la otra, espera ambas y libera el socket antes de volver al supervisor.
        /// </summary>
        /// <param name="cancellationToken">Detiene ambas tareas cuando se solicita apagar el cliente.</param>
        /// <returns>Una tarea que representa la ejecucion y limpieza de una sola conexion.</returns>
        /// <remarks>
        /// Usa un token enlazado para cancelar las tareas de esta conexion sin cancelar
        /// los reintentos futuros. Notifica la caida si no fue un cierre solicitado por el cliente.
        /// </remarks>
        private async Task RunConnectionAsync(CancellationToken cancellationToken)
        {
            var socket = _socket ?? throw new InvalidOperationException("No existe un socket.");

            using var connectionCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var receiveTask = ReceiveLoopAsync(connectionCts.Token);
            var heartbeatTask = HeartbeatLoopAsync(connectionCts.Token);
            string reason = "La conexion termino.";

            try
            {
                // Cuando una termina, detenemos tambien la otra.
                await Task.WhenAny(receiveTask, heartbeatTask);
                connectionCts.Cancel();

                // Esperamos ambas antes de permitir la reconexion.
                await Task.WhenAll(receiveTask, heartbeatTask);
            }
            catch (OperationCanceledException)
                when (connectionCts.IsCancellationRequested)
            {
                // Cancelacion del cliente o de la tarea restante.
            }
            catch (WebSocketException ex)
            {
                reason = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                // El envio puede encontrar el socket cerrado o liberado.
                reason = ex.Message;
            }
            finally
            {
                socket.Dispose();

                if (!cancellationToken.IsCancellationRequested)
                {
                    State = JmriConnectionState.Disconnected;
                    NotifyDisconnectedOnce(reason);
                }
            }
        }
    }
}
