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
        public BlePort ble_port_serial;

        public BleSerial(string portName)
        {
            ble_port_serial = new BlePort();
        }

        public void initConfig(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
        {
            ble_port_serial.init(adapter, dialogs);
        }

        Byte[] array2;


        public int Read(byte[] buffer, int offset, int count)
        {
            long identificador_valor = 0;
            array2 = new Byte[] { };
            if(buffer!=null){
                
            }else{
                for (int i = 1; i < ble_port_serial.getBuffer_ble_data().Count -1; i++){
                    
                    byte[] tempArray = new byte[ ble_port_serial.getBuffer_ble_data().ElementAt(i)[2] ];
                    Array.Copy(ble_port_serial.getBuffer_ble_data().ElementAt(i), 3, tempArray, 0, ble_port_serial.getBuffer_ble_data().ElementAt(i)[2] ); 
                    byte[] ret = new byte[array2.Length + tempArray.Length];
                    Buffer.BlockCopy(array2, 0, ret, 0, array2.Length);
                    Buffer.BlockCopy(tempArray, 0, ret, array2.Length, tempArray.Length);
                    array2 =  ret;
                }

                byte[] identificador = new byte[count];
                Array.Copy(array2, offset, identificador, 0, count);

                for (int i = 0; i < count; i++)
                {
                    identificador_valor= (long)((long) identificador_valor + (long)( (long) identificador[i] * Math.Pow(2, 8 * i)));
                }

                Thread.Sleep(20);
                return (int)identificador_valor;

            }
            return 0;
        }


        public byte[] GetBufferElement()
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

        /*
        SerialPort base_serial;



        public BleSerial(String action)
        {
            Open();
            buffer_interface = new Byte[] { };
            Console.WriteLine("Accion: " + action);




        }

        public void Read(byte[] buffer, int offset, int count)
        {
            //int prevSize = buffer_interface.Length;

            buffer_interface = buffer_interface.Concat(buffer).ToArray();


            //Console.WriteLine("Buffer Bytes Encoded: " + buffer_interface.EncodeToBase16String());


    
            ////
            ///             return ble_serial.Read(buffer, offset, count);
        }

        public int ReadDecode(byte[] buffer, int offset, int count)
        {
            Lexi.Lexi interface_lexi = new Lexi.Lexi();
           
            interface_lexi.Read(new BleSerial("Read MTU"), 0, 1, 400 );
                          
            return buffer_interface.Length;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            
            //ble_serial.Write(buffer, offset, count);
        }


        public void Close()
        {
            BleMainClass.Stop_Listen_Characteristic_Notification_ReadMTU();
            Is_Open = false;
            //ble_serial.Close();
        }

        public Boolean IsOpen()
        {
            
            return Is_Open;
            //return ble_serial.IsOpen();
        }

        public void Open()
        {
            Is_Open = true;
            //ble_serial.Open();
        }

        public int BytesToRead()
        {
           
            return buffer_interface.Length;
          

            //return ble_serial.BytesToRead();
        }

        public Boolean isEcho()
        {
            return true;
        }
*/



       
   

        /*
        private ISerial m_serial;
        private int m_timeout;
        private readonly byte[] _buffer;
        private SerialPort base_serial;

        public BleSerial(ISerial serial, int timeout)
        {
            m_serial = serial;
            m_timeout = timeout;
        }

        public BleSerial()
        {
            //set default read wait to response timeout to 500ms
            m_timeout = 500;
        }


        public int Read(byte[] buffer, int offset, int count)
        {
            // concat
            bufferFull = bufferFull.Concat(BleMainClass.m_bytearray_write_characteristic_read).ToArray();
           
            _buffer = new byte[count];

       

            if (m_serial == null)
            {
                throw new ArgumentNullException("No Serial interface defined");
            }

            return m_serial.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (m_serial == null)
            {
                throw new ArgumentNullException("No Serial interface defined");
            }

            m_serial.Write(buffer, offset, count);
        }

        public void Close()
        {
            BleMainClass.Stop_Listen_Characteristic_Notification_ReadMTU();
        }

        public Boolean IsOpen()
        {
            if(!BleMainClass.Stop_Notification){
                return true;
            }

            return false;
        }

        public void Open()
        {

        
        }

        public int BytesToRead()
        {
            return 0;
           
        }

        public Boolean isEcho()
        {
            return true;
        }
        */
