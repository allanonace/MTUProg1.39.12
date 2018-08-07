using System;
using Lexi.Interfaces;

namespace ble_library
{
    public class BleSerial : ISerial
    {
        //SerialPort base_serial;

        public BleSerial(string portName)
        {
            //base_serial = new SerialPort(portName, 1200, Parity.None, 8, StopBits.Two);
            //base_serial.Open();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return 0;
            //return base_serial.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            //base_serial.Write(buffer, offset, count);
        }

        public void Close()
        {
            //base_serial.Close();
        }

        public Boolean IsOpen()
        {
            return false;
            //return base_serial.IsOpen;
        }

        public void Open()
        {
            //base_serial.Open();
        }

        public int BytesToRead()
        {
            return 0;
            //return base_serial.BytesToRead;
        }

        public Boolean isEcho()
        {
            return true;
        }
    }
}
