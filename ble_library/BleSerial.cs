using System;
using Lexi.Interfaces;
using System.IO.Ports;
using System.Linq;
using nexus.protocols.ble;
using Acr.UserDialogs;
using System.Numerics;
using System.Threading;

namespace ble_library
{
    public class BleSerial : ISerial
    {
        private BlePort ble_port_serial;

        public BleSerial(string portName)
        {
            ble_port_serial = new BlePort();
        }

        public void InitConfig(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
        {
            ble_port_serial.init(adapter, dialogs);
        }

        private void ExceptionCheck(byte[] buffer, int offset, int count){
            if (buffer == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "buffer");
            }

            if (offset < 0)
            {
                throw new System.ArgumentException("Parameter cannot be less than Zero", "offset");
            }

            if (count < 0)
            {
                throw new System.ArgumentException("Parameter cannot be less than Zero", "count");
            }

            if (buffer.Length < offset + count)
            {
                throw new System.ArgumentException("Incorrect buffer size", "buffer");
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            ExceptionCheck(buffer, offset, count);

            int readedElements = 0;

            try{
                for (int i = 0; i < count; i++)
                {
                    if (ble_port_serial.BytesToRead() == 0)
                    {
                        return readedElements;
                    }
                    buffer[i+offset] = ble_port_serial.GetBufferElement();
                    readedElements++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }  
         
            return readedElements;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            ExceptionCheck(buffer, offset, count);
            ble_port_serial.clearBuffer_ble_data();   // TO-DO
            ble_port_serial.Write_Characteristic(buffer, offset, count);
        }

        public void Close()
        {
            ble_port_serial.DisconnectDevice();
        }

        public Boolean IsOpen()
        {
            return ble_port_serial.getConnection_app();
        }

        public void Scan(){
            ble_port_serial.startScan();
        }

        public void Open()
        {
            if(!IsOpen())
            {
                ble_port_serial.ConnectoToDevice();
            }else{
                // TO-DO: mantenemos la misma conexion o cerramos y volvemos a abrir otra
            }
        }

        public int BytesToRead()
        {
            return ble_port_serial.BytesToRead();
        }

        public Boolean isEcho()
        {
            return true;
        }
    }
}