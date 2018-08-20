using System;
using Lexi.Interfaces;
using System.IO.Ports;
using System.Linq;

namespace ble_library
{
    public class BleSerial : ISerial
    {
        public static Byte[] buffer_interface;
        public static ISerial base_serial;
        public static Lexi.Lexi interface_lexi;

        public static int valueRead;

        public static Byte[] buffer_interface_write;

       // public static Byte[] write_buffer;
       
        public Byte[] getBufferInterface(){
            return buffer_interface;
        }

        /*
        public Byte[] getWriteBuffer()
        {
            return write_buffer;
        }
*/

        public Lexi.Lexi getInterface_lexi()
        {
            return interface_lexi;
        }






        public BleSerial(string portName)
        {
            
            buffer_interface = new Byte[] { };

            try{
                base_serial = this;
            }catch(Exception e){
                
            }
            interface_lexi = new Lexi.Lexi(base_serial, 400);

            //base_serial.Open();

            //base_serial = new ISerial(portName, 1200, Parity.None, 8, StopBits.Two);
          
        }



        public int Read(byte[] buffer, int offset, int count)
        {

            /*
            Console.WriteLine(BitConverter.ToString(interface_lexi.Read(0, 1)));
            Console.WriteLine("");
            Console.WriteLine(BitConverter.ToString(interface_lexi.Read(1, 9)));
            Console.WriteLine("");
            Console.WriteLine(BitConverter.ToString(interface_lexi.Read(600, 9)));
            Console.WriteLine("");
            interface_lexi.Write(64, new byte[] { 0x01 });
            */

            valueRead = BitConverter.ToInt32(buffer.Skip(offset).Take(count).ToArray(), 0);

            return BitConverter.ToInt32(buffer.Skip(offset).Take(count).ToArray(), 0);
            //return BitConverter.ToInt16(interface_lexi.Read(offset count));
            //return base_serial.Read(buffer, offset, count);
        }


        public void Write(byte[] buffer, int offset, int count)
        {

            // write_buffer = write_buffer.Concat(buffer).ToArray();

            //base_serial.Write(buffer, offset, count);
            //base_serial.Write(buffer, offset, count);

            buffer_interface_write = buffer;
        }

        public void Close()
        {
           // base_serial.Close();
            //interface_lexi.Write(64, new byte[] { 0x01 });
        }

        public Boolean IsOpen()
        {
            return true;
            //return base_serial.IsOpen();
        }

        public void Open()
        {
           // return true;
           // base_serial.Open();
        }

        public int BytesToRead()
        {
            return buffer_interface.Length;
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
