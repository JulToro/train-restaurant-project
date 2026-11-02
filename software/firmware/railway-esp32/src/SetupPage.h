#pragma once

#include <Arduino.h>

// R"HTML(...)HTML" permite escribir HTML con saltos de línea y comillas sin escaparlas.
// PROGMEM mantiene esta página constante en flash. El JavaScript corre en el celular.
const char SETUP_PAGE[] PROGMEM = R"HTML(<!doctype html>
<html lang="es">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Railway · Configurar Wi-Fi</title>
  <style>
    * { box-sizing: border-box; }
    body { margin: 0; padding: 32px 16px; font: 16px/1.5 system-ui, sans-serif;
      background: #f1f4f2; color: #18342b; }
    main { max-width: 520px; margin: auto; padding: 28px; background: white;
      border: 1px solid #d5dfd9; border-radius: 16px; }
    h1 { margin: 4px 0 12px; font-size: 1.7rem; line-height: 1.2; }
    .brand { font-size: .8rem; font-weight: 700; letter-spacing: .15em; }
    p { margin: 12px 0; }
    label { display: block; margin: 18px 0 6px; font-weight: 600; }
    input, select, button { width: 100%; min-height: 46px; padding: 10px 12px;
      font: inherit; border: 1px solid #9aafa1; border-radius: 8px; }
    button { margin-top: 14px; background: #1b6047; color: white; font-weight: 600; cursor: pointer; }
    button.secondary { background: white; color: #1b6047; }
    button:disabled { opacity: .55; cursor: wait; }
    :focus-visible { outline: 3px solid #b38836; outline-offset: 3px; }
    .hint { color: #52675d; font-size: .88rem; }
    #status { padding: 12px; background: #edf4ef; border-radius: 8px; overflow-wrap: anywhere; }
    #status[data-error="true"] { background: #fff0ec; color: #8c2e1a; }
    fieldset { border: 0; padding: 0; margin: 0; min-width: 0; }
  </style>
</head>
<body>
<main>
  <div class="brand">RAILWAY</div>
  <h1>Conecta tu ESP32</h1>
  <p>Selecciona tu red de 2,4 GHz e introduce su contraseña.</p>
  <button id="scan" class="secondary" type="button" disabled>Buscar redes</button>
  <label for="network">Redes encontradas</label>
  <select id="network" disabled><option value="">Pulsa «Buscar redes»</option></select>
  <form id="wifi-form">
    <fieldset id="fields" disabled>
      <label for="ssid">Nombre de la red</label>
      <input id="ssid" name="ssid" required autocomplete="off" placeholder="Selecciona o escribe el nombre">
      <label for="password">Contraseña del Wi-Fi</label>
      <input id="password" name="password" type="password" required minlength="8" maxlength="63"
        autocomplete="new-password" aria-describedby="password-hint">
      <p id="password-hint" class="hint">De 8 a 63 caracteres ASCII. Se guardará si la conexión funciona.</p>
      <button type="submit">Conectar y guardar</button>
    </fieldset>
  </form>
  <p id="status" role="status" aria-live="polite">Consultando el ESP32…</p>
  <p class="hint">Esta página funciona sin Internet. Después de guardar, la red temporal se cerrará.</p>
</main>
<script>
// Estas instrucciones se ejecutan en el navegador, no como código C++ del ESP32.
const byId = (id) => document.getElementById(id);
const scanButton = byId('scan');
const networkSelect = byId('network');
const fields = byId('fields');
const statusText = byId('status');
let token = '';
let scanning = false;
let submitting = false;
let scanRequested = false;
let finished = false;
let attempts = 0;
let scannedState = '';

function showStatus(message, error = false) {
  statusText.textContent = message;
  statusText.dataset.error = String(error);
}

async function request(path, options = {}) {
  const response = await fetch(path, { ...options, cache: 'no-store' });
  const data = await response.json();
  if (!response.ok) throw new Error(data.error || 'No se pudo completar la operación.');
  return data;
}

function setBusy(busy) {
  scanButton.disabled = busy;
  fields.disabled = busy;
  networkSelect.disabled = busy || networkSelect.options.length < 2;
}

async function loadNetworks() {
  const networks = await request('/api/networks');
  // textContent/Option tratan el SSID como texto, aunque contenga etiquetas HTML.
  networkSelect.replaceChildren(new Option('Selecciona tu red', ''));
  const seen = new Set();
  for (const network of networks) {
    if (!network.ssid || seen.has(network.ssid)) continue;
    seen.add(network.ssid);
    const option = new Option(`${network.ssid} · ${network.rssi} dBm${network.open ? ' · abierta' : ''}`, network.ssid);
    option.disabled = network.open; // Esta versión requiere una red con contraseña.
    networkSelect.add(option);
  }
  showStatus(networks.length ? 'Selecciona tu red o escribe su nombre.' : 'No se encontraron redes. Puedes escribir el nombre.');
}

// Las opciones se muestran con su señal, pero el formulario utiliza el SSID exacto.
networkSelect.addEventListener('change', () => { byId('ssid').value = networkSelect.value; });

scanButton.addEventListener('click', async () => {
  setBusy(true);
  try {
    await request('/api/scan', { method: 'POST', headers: { 'X-Setup-Token': token } });
    scanRequested = true;
    scannedState = '';
    showStatus('Buscando redes…');
  } catch (error) {
    showStatus(error.message, true);
    setBusy(false);
  }
});

byId('wifi-form').addEventListener('submit', async (event) => {
  event.preventDefault(); // Evita que el navegador recargue la página al enviar.
  const ssid = byId('ssid').value;
  const password = byId('password').value;
  if (!ssid || new TextEncoder().encode(ssid).length > 32 || !/^[\x20-\x7e]{8,63}$/.test(password)) {
    showStatus('Revisa el nombre (máximo 32 bytes) y la contraseña (8 a 63 caracteres ASCII).', true);
    return;
  }
  setBusy(true);
  try {
    await request('/api/connect', {
      method: 'POST',
      headers: { 'X-Setup-Token': token },
      body: new URLSearchParams({ ssid, password })
    });
    byId('password').value = '';
    submitting = true;
    showStatus('Conectando al Wi-Fi… Esto puede tardar hasta 15 segundos.');
  } catch (error) {
    showStatus(error.message, true);
    setBusy(false);
  }
});

async function pollStatus() {
  if (finished) return;
  try {
    const state = await request('/api/status');
    token = state.token;
    attempts = 0;
    scanning = state.scan === 'scanning';
    if (state.closing) {
      finished = true;
      setBusy(true);
      showStatus(`Configuración guardada. IP: ${state.ip}. La red Railway-Setup se cerrará en unos segundos. Vuelve al Wi-Fi de tu casa.`);
      return;
    }
    if (scanRequested && state.scan !== 'scanning' && scannedState !== state.scan) {
      scannedState = state.scan;
      if (state.scan === 'done') await loadNetworks();
      if (state.scan === 'error') showStatus('La búsqueda falló. Intenta de nuevo.', true);
      scanRequested = false;
    }
    if (submitting && state.state !== 'CONNECTING') {
      submitting = false;
      if (state.state === 'SAVE_FAILED') {
        showStatus('Conectó, pero no se pudo guardar la configuración. Intenta de nuevo.', true);
      } else if (state.state !== 'CONNECTED') {
        showStatus('No se pudo conectar. Revisa la contraseña y vuelve a intentarlo.', true);
      }
    }
    if (statusText.textContent === 'Consultando el ESP32…') {
      showStatus('Listo. Pulsa «Buscar redes» para comenzar.');
    }
    setBusy(scanning || state.state === 'CONNECTING' || submitting);
  } catch (error) {
    attempts++;
    setBusy(true);
    showStatus('No se puede consultar el ESP32. Mantén la conexión con Railway-Setup; si el portal se cerró, usa wifi setup por USB.', true);
    if (attempts >= 5) return;
  }
  // Una consulta a la vez, sin acumular peticiones si la conexión está lenta.
  setTimeout(pollStatus, 1000);
}
pollStatus();
</script>
</body>
</html>)HTML";
