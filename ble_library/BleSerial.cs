using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lexi.Interfaces;
using Library;
using nexus.protocols.ble;
using nexus.protocols.ble.scan;

namespace ble_library
{
    public class BleSerial : ISerial
    {
        private BlePort ble_port_serial;

        /// <summary>
        /// Initialize Bluetooth LE Serial port
        /// </summary>
        /// <param name="adapter">The Bluetooth Low Energy Adapter from the OS</param>
        public BleSerial(IBluetoothLowEnergyAdapter adapter)
        {     
            ble_port_serial = new BlePort(adapter);
        }

        private void ExceptionCheck(byte[] buffer, int offset, int count){
            if (buffer == null)
            {
                throw new ArgumentException("Parameter cannot be null", nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentException("Parameter cannot be less than Zero", nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentException("Parameter cannot be less than Zero", nameof(count));
            }

            if (buffer.Length < offset + count)
            {
                throw new ArgumentException("Incorrect buffer size", nameof(buffer));
            }
        }

        /// <summary>
        /// Reads a number of characters from the ISerial input buffer and writes them into an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the bytes.</param>
        /// <param name="count">The maximum number of bytes to read. Fewer bytes are read if count is greater than the number of bytes in the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            Utils.PrintDeep ( "-------READ_START--------" );
        
            Utils.PrintDeep ( "BleSerial.Read.." +
                " Offset " + offset.ToString ( "D2" ) +
                " | Count " + count.ToString ( "D2" ) );
        
            ExceptionCheck(buffer, offset, count);

            try
            {
                for ( int i = 0; i < count; i++ )
                {
                    // FIFO collection to read data frames in the same order received
                    buffer[ i+offset ] = ble_port_serial.GetBufferElement ();
                    
                    // Last two bytes are the CRC
                    if ( ble_port_serial.BytesToRead () == 0 )
                    {
                        Utils.PrintDeep ( "BleSerial.Read.." +
                        " DataFrames " + ( i+1 ).ToString ( "D2" ) +
                        " | Buffer " + Utils.ByteArrayToString ( buffer ) );
                    
                        return i+1;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.PrintDeep ( "BleSerial.Read -> ERROR: " + e.Message );
            
                throw e;
            }
            finally
            {
                Utils.PrintDeep ( "-------READ_FINISH-------" );
            }

            return 0;
        }

        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which to begin copying bytes to the port.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <remarks></remarks>
        public async Task Write(byte[] buffer, int offset, int count)
        {
            Utils.PrintDeep ( "-------WRITE_START-------" );
        
            Utils.PrintDeep ( "BleSerial.Write.." +
                " Buffer " + Utils.ByteArrayToString ( buffer ) +
                " | Offset ( is always ) " + offset.ToString ( "D2" ) +
                " | NumBytesToWrite " + count.ToString ( "D2" ) );
        
            try
            {
                ExceptionCheck ( buffer, offset, count );
            
                ble_port_serial.ClearBuffer ();

                if ( ble_port_serial.semaphore.CurrentCount <= 0 )
                    ble_port_serial.semaphore.Release ();
                    
                ble_port_serial.timeInit = 0L;
            
                int totalBytesToWrite = count;
                int bytesWritten = 0;
                do
                {
                    // Each data frame max length is 16
                    int bytesDataFrame = ( totalBytesToWrite < 16 ) ? totalBytesToWrite : 16;
    
                    Utils.PrintDeep ( "BleSerial.Write.." +
                        " Next stream " + bytesDataFrame.ToString ( "D2" ) + " bytes" +
                        " | Written " + bytesWritten.ToString ( "D2" ) + "/" + count.ToString ( "D2" ) );
                    
                    await ble_port_serial.Write_Characteristic ( buffer, bytesWritten + offset, bytesDataFrame );
                    
                    totalBytesToWrite -= bytesDataFrame;
                    bytesWritten      += bytesDataFrame;
                }
                while ( totalBytesToWrite > 0 );
            }
            catch ( Exception e )
            {
                Utils.PrintDeep ( "BleSerial.Write -> ERROR: " + e.Message );
            
                throw e;
            }
            finally
            {
                Utils.PrintDeep ( "-------WRITE_FINISH------" );
            }
        }

        /// <summary>
        /// Closes the port connection, sets the <c>IsOpen</c> property to false, and disposes of the internal Stream object.
        /// </summary>
        /// <remarks></remarks>
        public void Close()
        {
            Task.Factory.StartNew(ble_port_serial.DisconnectDevice);
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of the ISerial object.
        /// </summary>
        /// <returns>Boolean value indicating the open or closed status of the ISerial object</returns>
        /// <remarks>The IsOpen property tracks whether the port is open for use by the caller, not whether the port is open by any application on the machine.</remarks>
        public Boolean IsOpen()
        {
            return (ble_port_serial.GetConnectionStatus() == BlePort.CONNECTED);
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of the ISerial object.
        /// </summary>
        /// <returns>Int value indicating the open or closed status of the ISerial object</returns>
        /// <remarks>The IsOpen property tracks whether the port is open for use by the caller, not whether the port is open by any application on the machine.</remarks>
        public int GetConnectionStatus()
        {
            return ble_port_serial.GetConnectionStatus();
        }

        /// <summary>
        /// Gets a value indicating the error on the connectio
        /// </summary>
        /// <returns>int value indicating the error on the connectio</returns>
        public int GetConnectionError()
        {
            return ble_port_serial.GetConnectionError();
        }

        /// <summary>
        /// Starts the device scanning
        /// </summary>
        /// <remarks></remarks>
        public async Task Scan(){
            
            await ble_port_serial.StartScan();
        }

        public Boolean IsScanning()
        {
            return ble_port_serial.IsScanning();
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <param name="blePeripheral">The object that contains the device information.</param>
        /// <remarks></remarks>
        public void Open(IBlePeripheral blePeripheral, bool isBounded = false)
        {
            if(!IsOpen())
            {
                Task.Factory.StartNew(() => ble_port_serial.ConnectoToDevice(blePeripheral,isBounded));
         
            }else{
                // TO-DO: mantenemos la misma conexion o cerramos y volvemos a abrir otra
            }
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <remarks></remarks>
        public void Open()
        {
         
        }

        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        /// <returns>Number of bytes of data in the receive buffer</returns>
        /// <remarks>
        /// The receive buffer includes the serial driver's receive buffer as well as internal buffering in the <c>ISerial</c> object itself.
        /// </remarks>
        public int BytesToRead()
        {
            return ble_port_serial.BytesToRead();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public Boolean isEcho()
        {
            return true;
        }

        /// <summary>
        /// Gets the BLE device list
        /// </summary>
        /// <returns>The BLE device list</returns>
        /// <remarks></remarks>
        public List <IBlePeripheral> GetBlePeripheralList() {
            return ble_port_serial.GetBlePeripherals();
        }

        /// <summary>
        /// Gets the BLE device Battery Level
        /// </summary>
        /// <returns>The BLE device Battery Level</returns>
        /// <remarks></remarks>
        public byte[] GetBatteryLevel(){
            return ble_port_serial.GetBatteryLevel();
        }
        /// <summary>
        /// Gets the TimeOut in seconds
        /// </summary>
        /// <returns>Gets the TimeOut in seconds</returns>
        /// <remarks></remarks>
        public void SetTimeOutSeconds(int sec)
        {
            ble_port_serial.TimeOutSeconds = sec;
        }
    }
}