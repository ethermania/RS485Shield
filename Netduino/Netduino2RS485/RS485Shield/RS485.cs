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
using System.IO;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace EtherMania.com.RS485Shield
{

    public class RS485Shield
    {
        private static SerialPort com;
        public static OutputPort txrxEnable;

        // Build a new RS485 Handler with specified settings
        public RS485Shield(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Cpu.Pin txRxPin)
        {
            com = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            txrxEnable = new OutputPort(txRxPin, false);
            com.Open();
        }

        // Optionally adds rx and error handlers
        public void initHandlers(SerialDataReceivedEventHandler rxHandler, SerialErrorReceivedEventHandler errorHandler)
        {
            if (com != null)
            {
                if (rxHandler != null)
                {
                    com.DataReceived += rxHandler;
                }

                if (errorHandler != null)
                {
                    com.ErrorReceived += errorHandler;
                }
            }
        }

        public int Write(byte[] buffer)
        {
            if ((com == null) || (buffer.Length == 0))
                return 0;

            // Perform the physiscal write on the RS485 bus
            txrxEnable.Write(true);
            com.Write(buffer, 0, buffer.Length);
            com.Flush();
            txrxEnable.Write(false);
            com.DiscardInBuffer();

            return buffer.Length;
        }

        public int rxBytesLength()
        {
            if (com == null)
                return 0;

            return com.BytesToRead;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (com == null)
                return 0;

            return com.Read(buffer, offset, count);
        }
    }
}
