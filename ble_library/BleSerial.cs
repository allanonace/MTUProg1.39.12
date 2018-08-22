﻿using System;
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
        public BlePort ble_port_serial;

        public BleSerial(string portName)
        {
            ble_port_serial = new BlePort();
        }

        public void initConfig(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
        {
            ble_port_serial.init(adapter, dialogs);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] = ble_port_serial.getBuffer_ble_data().ElementAt(offset + i);
            }

            return buffer.Length;

            /*
            long identificador_valor = 0;

            for (int i = offset; i < offset + count; i++)
            {
                identificador_valor = (long)((long)identificador_valor + (long)((long)ble_port_serial.getBuffer_ble_data().ElementAt(i) * Math.Pow(2, 8 * (i-offset) ) ) );
            }
           
            return (int)identificador_valor;
            */
        }

        public void ShowBuffer_Console()
        {
            for (int i = 0; i < BytesToRead(); i++)
            {
                Console.Write(Convert.ToString(ble_port_serial.getBuffer_ble_data().ElementAt(i), 16)+" ");
             
            }
        }

        public byte GetBufferElement()
        {
            return ble_port_serial.getBuffer_ble_data().Dequeue();
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            ble_port_serial.clearBuffer_ble_data();
            ble_port_serial.Listen_Characteristic_Notification();
            ble_port_serial.Write_Characteristic(buffer);
        }

        public void Close()
        {
            ble_port_serial.DisconnectDevice();
        }

        public Boolean IsOpen()
        {
            if(ble_port_serial.getConnection_app())
            {
                return true;
            }
            return false;

        }

        public void Scan(){
            ble_port_serial.startScan();
        }

        public void Open()
        {
            if(!IsOpen()){
                ble_port_serial.ConnectoToDevice();
            }else{
                Close();
            }
        }

        public int BytesToRead()
        {
            return ble_port_serial.getBuffer_ble_data().Count;
        }

        public Boolean isEcho()
        {
            return true;
        }

    }
}