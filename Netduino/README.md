Netduino2RS485 sample code
==========================

Netduino2RS485 solution contains three different projects. It was implemented to provide a sample bench test for the RS485 shield mounted on top of Netduino and Netduino Plus running .Net microframework version 4.2.0.0.

We suppose to have two devices on the same bus: Mr. Ping and Mr. Pong. 
Mr. Ping, each second, sends a welcome string on the bus. Mr. Pong receives the string and answers with the proper string only if the welcome string has been detected, or with the incoming chars if the welcome string was not detected.
The full RS485 data transfer are dump on the debug console.

For best clarity, a third project is shared by Ping and Pong projects. This third project implements a class used to read and write byte[] buffers through/from the RS485 bus.

The hardware setup used to test the project is:

1. A Netduino 1st generation (with upgraded firmware to 4.2.0.1) mounting an RS485 shield so configured:
- JP2: Run
- JP3: Closed (Bus terminator on)
- JP4: Closed (Bus balance is on)
- JP5: DOWN Closed, UP Open
- JP6: DOWN Closed, UP Open

2. A Netduino 2 Plus mounting an RS485 shield so configured:
- JP2: Run
- JP3: Closed (Bus terminator on)
- JP4: Open (Bus balance is off)
- JP5: DOWN Closed, UP Open
- JP6: DOWN Closed, UP Open

When running, the LED1 present on the two shields should blink on each transmission frame time.


