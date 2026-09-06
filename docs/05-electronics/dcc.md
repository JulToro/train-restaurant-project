para controlar desde el computador las vias DCC existen unos componentes llamados
DCC-EX

Hay dos opciones
Primera Opcion
Arduino Mega 2560 + DCC-EX Firmaware + EX-MotorShield8874 - Fuente

Segunda Opcion (Viable)
Solo utilizar DCC-EX EX-CommandStation/Booster 1 — EX-CSB1
Es varias cosas metidas en una sola placa:

microcontrolador;
Command Station;
generador de señal DCC;
drivers de potencia;
Wi-Fi;
USB-C;
pantalla OLED;
medición/protección de corriente.

Digitrax DCS52 Zephyr Express

Sistema LocoNet de Digitrax sirve 

                    PC
                     │
                    USB
                     │
                   DCS52
                     │
          ┌──────────┴──────────┐
          │                     │
         DCC                 LocoNet
          │                     │
       Rieles        ┌──────────┼─────────┐
          │          │          │         │
       Trenes     Sensores   Desvíos   Booster


Nota:
El sistema de digitrax entrega solo 3A mientras que bd150 entrega 5A, osea que para escala G puede no funcionar, pero se puede solucionar con Boosters-

El busooster DB210 y DB210 ayuda a entregar mas potencia de 3/5/8A

Digitrax especifica que el DCS52 puede mantener hasta 20 direcciones de locomotora simultáneamente y hasta 20 throttles



Se necesita un decoderDCC en cada locomotora



Para hacer Desvios: 

                    ┌── Mesa 10
───────────────<────┤
                    └── Mesa 11

si se tienen bifulcaciones se necesita 
Servo
o
motor de turnout
o
decoder de accesorios


Booster adicional:
Su función principalmente es proporcionar más potencia DCC.

                  Central
                     │
         ┌───────────┼───────────┐
         ▼           ▼           ▼
      Booster A   Booster B   Booster C
         │           │           │
      Sector A    Sector B    Sector C


Software JMRI - .Net

Tener encuenta que DCC-EX CSB1 Me permite mas programación, mucha mas libertad

https://www.ebay.com/itm/298409053282?_skw=dcc+ex+command+station&itmmeta=01M1S7JQVDS3G32JJBXDC7CCM8&hash=item457a90cc62:g:OdYAAeSwUGhqK~WY&itmprp=enc%3AAQALAAAA8GfYFPkwiKCW4ZNSs2u11xC6a5VslaPxER8iRFKKzNwdi8SwCKmApRpRvKDQMbh1HT%2FQPS%2F8TKUvbzxTZ%2BVNCNpJNgT5Pc7HnrFEvHsAzF9efY8ZC3qFcrf4E6uNHfTnL86Da%2Fz0EDEcBMUp3Q7JMb9VQwsZmESx99lT1jjqSkKAD%2BTMP69aTN2D%2FIodVckUlBZBzfLm%2FbYakzh2KkP%2BUAzq02%2BnZkDtLY4hun40oQNIqLx0bNNtMvV8XnPhtfZKkf%2BElVzatQhz00SsqTH%2BbWtijoYp2Nb9jDSDESJ8fPSvEjcLnSXDFkEl7xnDCopZjA%3D%3D%7Ctkp%3ABk9SR-79yqeOaA