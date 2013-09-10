/* --------------------------------------------------------
 * RS485 Shield Library and Test Example
 * (c) 2013 EtherMania.com 
 * Author: Marco Signorini
 * Tested with Netduino and Netduino 2 Plus fw 4.2.0.1
 * --------------------------------------------------------
 * Released under GPL http://www.gnu.org/licenses/gpl.html
 * --------------------------------------------------------
 * Libraries are intended to be used with the RS485 Shield
 * developed by EtherMania. Info at www.EtherMania.com
 * --------------------------------------------------------
 * For Feasibility Evaluation Only, in Laboratory and/or
 * Development Environments. The RS485 Shield is not a 
 * complete product. It is intended solely for use for 
 * preliminary feasibility evaluation in laboratory and/or
 * development environments by technically qualified 
 * electronics experts who are familiar with the dangers
 * and application risks associated with handling electri-
 * cal mechanical components, systems and subsystems. It 
 * should not be used as all of part of finished end
 * product.
 * You agree to defend, indemnify and hold the Supplier, 
 * its licensors and their representatives harmless
 * from and against any and all claims, damages, losses,
 * expenses, costs and liabilities arising out of or in
 * connection with any use of the RS485 shield and software
 * that is not in accordance with the previous statements.
 * This obligation shall apply wheter Claims arise under 
 * law of tort or any other legal theory, and even if
 * the RS485 shield and software fail to perform as 
 * described or expected
 * --------------------------------------------------------
 */


using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using EtherMania.com.RS485Shield;

namespace Ping
{
    public class Program
    {
                        
        private static RS485Shield rs485 = null;
        private static String pingString = "Hi! This is PING";
        private static long timePeriod = 1000 * 1000 * 10;
        private static byte[] txBuffer = System.Text.Encoding.UTF8.GetBytes(pingString);

        public static void Main()
        {
            // Instantiate the main RS485Shield handler
            rs485 = new RS485Shield("COM1", 9600, Parity.None, 8, StopBits.One, Pins.GPIO_PIN_D2);

            // Add a suitable RxEvent Handler Callback
            rs485.initHandlers(new SerialDataReceivedEventHandler(comDataReceived), null);

            long lastTick = Utility.GetMachineTime().Ticks;

            // Endless loop. 
            while (true)
            {

                long curTick = Utility.GetMachineTime().Ticks;
                if ((curTick - lastTick) > timePeriod)
                {
                    // Because I'm the "ping actor", just start a ping-pong game
                    rs485.Write(txBuffer);

                    // repeat each second
                    lastTick = curTick;
                }
            }
        }

        // Receive data back and print out to the debug console
        private static void comDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (rs485 == null)
                return;

            int dataLength = rs485.rxBytesLength();

            byte[] rxBuffer = new byte[dataLength];
            rs485.Read(rxBuffer, 0, dataLength);

            // Conver to string
            String rxString = "";
            try
            {
                rxString = new String(System.Text.Encoding.UTF8.GetChars(rxBuffer));
            }
            catch (System.Exception exception)
            {
            }

            Debug.Print(rxString);
        }
    }
}
