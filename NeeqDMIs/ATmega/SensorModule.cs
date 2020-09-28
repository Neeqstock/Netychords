using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace NeeqDMIs.ATmega
{
    public class SensorModule
    {
        private SerialPort _serialPort;

        private bool isConnectionOk = false;
        public bool IsConnectionOk { get => isConnectionOk; set => isConnectionOk = value; }

        private string portPrefix = "COM";
        public string PortPrefix { get => portPrefix; set => portPrefix = value; }

        private List<ISensorReaderBehavior> behaviors = new List<ISensorReaderBehavior>();
        public List<ISensorReaderBehavior> Behaviors { get => behaviors; set => behaviors = value; }

        public SensorModule(string portPrefix, int baudRate)
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        public bool Connect(int portNumber)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            _serialPort.PortName = portPrefix + portNumber.ToString();

            try
            {
                _serialPort.Open();
            }
            catch
            {
                isConnectionOk = false;
                return false;
            }

            isConnectionOk = true;
            return true;
        }

        public void Write(string str)
        {
            if(isConnectionOk)
            _serialPort.Write(str);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                for (int i = 0; i < behaviors.Count; i++)
                {
                    behaviors[i].ReceiveSensorRead(_serialPort.ReadLine());
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// Closes the port. Returns true if the connection get closed. Returns false if the connection was already closed.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
