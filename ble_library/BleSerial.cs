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
    /// <summary>
    /// Implementation of the <see cref="ISerial"/> interface to
    /// use BLE ( Bluetooth Low Energy ) communication that complies with the LExI protocol.
    /// </summary>
    public class BleSerial : ISerial
    {
        private BlePort ble_port_serial;

        /// <summary>
        /// Initializes the BLE ( Bluetooth Low Energy ) Serial port.
        /// </summary>
        /// <param name="adapter">The bluetooth adapter from the OS</param>
        public BleSerial(IBluetoothLowEnergyAdapter adapter)
        {     
            ble_port_serial = new BlePort(adapter);
        }

        private void ExceptionCheck(byte[] buffer, int offset, int count)
        {
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
        /// Reads a number of characters from the input buffer and writes them into an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">Bytes to write</param>
        /// <param name="offset">Zero based byte offset</param>
        /// <param name="count">The maximum number of bytes to read</param>
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
                // NOTE: Sometimes happens that the LExI command response with a "trash" value,
                // NOTE: starting echo stream by other value than 0x25
                //int initFromByte = count - ble_port_serial.BytesReadCount (); // Bytes esperados - Bytes recuperados
                //if ( initFromByte < 0 ) initFromByte *= -1;

                for ( int i = 0; i < count; i++ )
                {
                    // FIFO collection to read data frames in the same order received
                    buffer[ i+offset ] = ble_port_serial.GetBufferElement ();
                    
                    // Last two bytes are the CRC
                    if ( ble_port_serial.BytesReadCount () == 0 )
                    {
                        Utils.PrintDeep ( "BleSerial.Read.." +
                        " DataFrames " + ( i+1 ).ToString ( "D2" ) +
                        " | Buffer " + Utils.ByteArrayToString ( buffer ) );
                    
                        return i+1;
                    }
                }
            }
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                Utils.PrintDeep ( "BleSerial.Read -> ERROR: " + e.Message );
            
                throw;
            }
            finally
            {
                Utils.PrintDeep ( "-------READ_FINISH-------" );
            }

            return 0;
        }

        /// <summary>
        /// Writes a specific number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">Bytes to write</param>
        /// <param name="offset">Zero based byte offset</param>
        /// <param name="count">Number of bytes to write</param>
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
                    
                ble_port_serial.TimeInit = 0L;
            
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
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                Utils.PrintDeep ( "BleSerial.Write -> ERROR: " + e.Message );
            
                throw;
            }
            finally
            {
                Utils.PrintDeep ( "-------WRITE_FINISH------" );
            }
        }

        /// <summary>
        /// Closes the port connection and disposes of the internal Stream object.
        /// </summary>
        public void Close()
        {
            Task.Factory.StartNew(ble_port_serial.DisconnectDevice);
        }

        /// <summary>
        /// Indicates if the connection status is open or closed.
        /// </summary>
        /// <returns><see langword="true"/> if connection status is open.</returns>
        public Boolean IsOpen()
        {
            return (ble_port_serial.GetConnectionStatus() == BlePort.CONNECTED);
        }

        /// <summary>
        /// Returns a value indicating the open or closed status of the ISerial object.
        /// </summary>
        /// <returns>Int value indicating the open or closed status of the ISerial object</returns>
        public int GetConnectionStatus()
        {
            return ble_port_serial.GetConnectionStatus();
        }

        /// <summary>
        /// Returns a value indicating the error on the connection.
        /// </summary>
        /// <returns>Connection error.</returns>
        public int GetConnectionError()
        {
            return ble_port_serial.GetConnectionError();
        }

        /// <summary>
        /// Starts the device scanning.
        /// </summary>
        public async Task Scan()
        {
            await ble_port_serial.StartScan();
        }

        /// <summary>
        /// Indicates if the device scanning is performing or not.
        /// </summary>
        /// <returns><see langword="true"/> if the the device sanning is active.</returns>
        public Boolean IsScanning()
        {
            return ble_port_serial.IsScanning();
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <param name="blePeripheral">Device information.</param>
        public void Open (
            IBlePeripheral blePeripheral,
            bool isBounded = false )
        {
            if(!IsOpen())
            {
                Task.Factory.StartNew(() => ble_port_serial.ConnectoToDevice(blePeripheral,isBounded));
         
            }else{
                // TO-DO: mantenemos la misma conexion o cerramos y volvemos a abrir otra
            }
        }

        public void Open() 
        {
            // not used
        }

        /// <summary>
        /// Returns the number of bytes of data received stored in the buffer.
        /// </summary>
        public int BytesReadCount ()
        {
            return ble_port_serial.BytesReadCount ();
        }

        /// <summary>
        /// Buffer of bytes of data received.
        /// </summary>
        public byte[] BytesRead ()
        {
            return ble_port_serial.BytesRead;
        }

        public Boolean isEcho ()
        {
            return true;
        }

        /// <summary>
        /// Gets the BLE ( Bluetooth Low Energy ) device list.
        /// </summary>
        /// <returns>Device list.</returns>
        public List <IBlePeripheral> GetBlePeripheralList ()
        {
            return ble_port_serial.GetBlePeripherals ();
        }

        /// <summary>
        /// Gets the BLE ( Bluetooth Low Energy ) device Battery Level.
        /// </summary>
        /// <returns>Battery Level.</returns>
        public byte[] GetBatteryLevel()
        {
            return ble_port_serial.GetBatteryLevel ();
        }

        /// <summary>
        /// Gets the timeout in seconds.
        /// </summary>
        /// <returns>Number of seconds.</returns>
        public void SetTimeOutSeconds (
            int sec )
        {
            ble_port_serial.TimeOutSeconds = sec;
        }
    }
}