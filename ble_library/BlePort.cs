using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;
using nexus.core;
using nexus.protocols.ble;
using nexus.protocols.ble.gatt;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

==== BASE ====
namespace ble_library
{
    /*
    ObserverReporter Class.
    Contains all methods that allow to know the connection status
    */
    public class ObserverReporter : IObserver<ConnectionState>
    {
        private IDisposable unsubscriber;
        private BlePort blePort;

        public ObserverReporter(BlePort port)
        {
            blePort = port;
        }

        public virtual void Subscribe(IObservable<ConnectionState> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public virtual void OnCompleted()
        {

        }

        public virtual void OnError(Exception error)
==== BASE ====
        {
==== BASE ====
==== BASE ====

==== BASE ====
        }

        public void OnNext(ConnectionState value)
        {
            if (value == ConnectionState.Disconnected)
            {
                Task.Factory.StartNew(blePort.DisconnectDevice).Wait();
            }
            else
            {
                // test
                value = value;
            }
        }
    }
==== BASE ====

    /*
    BluetoothStatusReporter Class.
    Contains all methods that allow to know the bluetooth adapter status
    */
    public class BluetoothStatusReporter : IObserver<EnabledDisabledState>
    {
        private IDisposable unsubscriber;
        private BlePort blePort;

        public BluetoothStatusReporter(BlePort port)
        {
            blePort = port;

        }

        public virtual void Subscribe(IObservable<EnabledDisabledState> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }


        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(EnabledDisabledState value)
        {
            if (value == EnabledDisabledState.Disabled)
            {
                Task.Factory.StartNew(blePort.DisconnectDevice).Wait();

            }
            else
            {
                // test
                value = value;
            }
        }
    }

    public class BlePort
    {
        ////public TaskCompletionSource<bool> waitForACK;
==== BASE ====
        
        private Queue<byte> buffer_ble_data;
        private byte[] buffer_array;
        private IBluetoothLowEnergyAdapter adapter;
        private IBleGattServerConnection gattServer_connection;
==== BASE ====
        private IDisposable Listen_aes_conection_Handler;
        private IDisposable Listen_ack_response_Handler;
        private IDisposable Listen_Characteristic_Notification_Handler;
        private IDisposable Listen_Battery_level;

        public static int NO_CONNECTED = 0;
        public static int CONNECTING = 1;
        public static int CONNECTED = 2;

        public const int NO_ERROR = 0;
        public const int CONECTION_ERRROR = 1;
        public const int DYNAMIC_KEY_ERROR = 2;
        public const int NO_DYNAMIC_KEY_ERROR = 3;

        private int timeOutSeconds = 5;

        private int isConnected;
        private int connectionError;
        private List<IBlePeripheral> BlePeripheralList;
        private IBlePeripheral ble_peripheral;

        private byte[] dynamicPass;
        private bool isPaired = true;
        private bool isScanning;
        private byte cipheredDataSentCounter;

        private byte[] writeSavedBuffer;
        private int writeSavedOffset;
        private int writeSavedCount;

        private ISettings saved_settings;
        private byte[] static_pass = { 0x54, 0x68, 0x69, 0x73, 0x20, 0x69, 0x73, 0x20, 0x74, 0x68, 0x65, 0x20, 0x50, 0x61, 0x73, 0x73, 0x77, 0x6f, 0x72, 0x64, 0x20, 0x66, 0x6f, 0x72, 0x20, 0x41, 0x63, 0x6c, 0x61, 0x72, 0x61, 0x2e };
        private byte[] say_hi = { 0x48, 0x69, 0x2c, 0x49, 0x27, 0x6d, 0x41, 0x63, 0x6c, 0x61, 0x72, 0x61, 0x00, 0x00, 0x00, 0x00 };

        private const int LENGTH_HEADER = 3;
        private const int MAX_LENGTH_DATA = 16;
        private const int LENGTH_DATA_FRAME = 20;
        private const int INDEX_LENGTH_DATA = 2;
        private const int INDEX_CYPHER_COUNT = 1;

        private byte[] batteryLevel;
        
        public SemaphoreSlim semaphore { get; private set; }
        private TaskCompletionSource<bool> ackException;

        public int TimeOutSeconds { get => timeOutSeconds; set => timeOutSeconds = value; }

        /// <summary>
        /// Initizalize Bluetooth LE Serial Port
        /// </summary>
        /// <param name="adapter_app">The Bluetooth Low Energy Adapter from the OS</param>
        public BlePort(IBluetoothLowEnergyAdapter adapter_app)
        {
            adapter = adapter_app;
            buffer_ble_data = new Queue<byte>();

            writeSavedBuffer = new byte[] { };
            writeSavedOffset = 0;
            writeSavedCount = 0;

            isConnected = NO_CONNECTED;
            connectionError = NO_ERROR;
            isScanning = false;
            cipheredDataSentCounter = 1;
            saved_settings = CrossSettings.Current;

            BlePeripheralList = new List<IBlePeripheral>();

            batteryLevel = new byte[] { 0x00 };
            
            this.semaphore = new SemaphoreSlim ( 0, 1 );
        }

        /// <summary>
        /// Returns the Connection status with the Bluetooth device
        /// </summary>
        /// <returns>The Bluetooth connection status.</returns>
        public int GetConnectionStatus()
        {
            return isConnected;
        }

        /// <summary>
        /// Returns the Connection error
        /// </summary>
        /// <returns> Connection error.</returns>
        public int GetConnectionError()
        {
            return connectionError;
        }

        /// <summary>
        /// Returns the byte array from buffer and drops the element out of the queue
        /// </summary>
        /// <returns>The byte array from the buffer that is dropped out the queue</returns>
        public byte GetBufferElement()
        {
            return buffer_ble_data.Dequeue();
        }

        /// <summary>
        /// Returns the number of bytes to read from the buffer
        /// </summary>
        /// <returns>The number of bytes to read from the buffer</returns>
        public int BytesReadCount ()
        {
            return buffer_ble_data.Count;
        }

        public byte[] BytesRead
        {
            get { return buffer_array; }
        }

        /// <summary>
        /// Clears the buffer queue
        /// </summary>
        public void ClearBuffer()
        {
            buffer_ble_data.Clear();
        }

        /// <summary>
        /// Returns the Bluetooth LE Peripherals detected by the scan
        /// </summary>
        /// <returns>The Bluetooth LE periphals around the scanning device</returns>
        public List<IBlePeripheral> GetBlePeripherals()
        {
            return BlePeripheralList;
        }

        /// <summary>
        /// If bluetooth antenna is enabled on device, starts scanning devices. If not, turns it on, and proceeds to scan.
        /// </summary>
==== BASE ====
        public async Task StartScan()
        {
            await BluetoothEnable();
            if (adapter.CurrentState.IsEnabledOrEnabling())
            {
                //Task.Factory.StartNew(ScanForBroadcasts);
                await ScanForBroadcasts();
            }
            else
            {
                // New empty BlePeripheralList
                List<IBlePeripheral> BlePeripheralListAux = new List<IBlePeripheral>();
                BlePeripheralList = BlePeripheralListAux;
            }
        }

        /// <summary>
        /// Check if the BLE interface is executing a scan
        /// </summary>
        /// <returns>True if the BLE interface is executing a scan. False in other case </returns>
        public Boolean IsScanning()
        {
            return isScanning;
        }

        /// <summary>
        /// Enables bluetooth antenna on device.
        /// </summary>
        private async Task BluetoothEnable()
        {
            if (adapter.AdapterCanBeEnabled && adapter.CurrentState.IsDisabledOrDisabling())
            {
                await adapter.EnableAdapter();
            }
        }

        /// <summary>
        /// Listen to the characteristic notifications of a peripheral
        /// </summary>
        private void Listen_Characteristic_Notification()
        {
            // TODO: comprobar que existe servicio?
            try
            {
                // Will also stop listening when gattServer
                // is disconnected, so if that is acceptable,
                // you don't need to store this disposable.
                Listen_Characteristic_Notification_Handler = gattServer_connection.NotifyCharacteristicValue(
                   new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                   new Guid("00000003-0000-1000-8000-00805f9b34fb"),
                   UpdateBuffer
                );
            }
            catch (GattException ex)
            {
                Utils.Print(ex.ToString());
            }

            try
            {
                // Will also stop listening when gattServer
                // is disconnected, so if that is acceptable,
                // you don't need to store this disposable.
                Listen_ack_response_Handler = gattServer_connection.NotifyCharacteristicValue(
                   new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                   new Guid("00000002-0000-1000-8000-00805f9b34fb"),
                    UpdateACKBuffer
                );
            }
            catch (GattException ex)
            {
                Utils.Print(ex.ToString());
            }
        }

        /// <summary>
        /// Stops listening to the characteristic notifications of a peripheral
        /// </summary>
        private void Stop_Listen_Characteristic_Notification()
        {
            if (!adapter.CurrentState.IsDisabledOrDisabling())
            {
                try
                {
                    Listen_Characteristic_Notification_Handler.Dispose();
                }
                catch (Exception e)
                {
                    Utils.Print(e.StackTrace);
                }
                try
                {
                    Listen_ack_response_Handler.Dispose();
                }
                catch (Exception e)
                {
                    Utils.Print(e.StackTrace);
                }

                try
                {
                    Listen_Battery_level.Dispose();
                }
                catch (Exception e3)
                {
                    Utils.Print(e3.StackTrace);
                }
            }
        }

        public long timeInit;

        /// <summary>
        /// Writes a number of bytes via Bluetooth LE to the peripheral gatt connnection
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the bytes.</param>
        /// <param name="count">The maximum number of bytes to read. Fewer bytes are read if count is greater than the number of bytes in the input buffer.</param>
        public async Task Write_Characteristic ( byte[] buffer, int offset, int count )
        {
            writeSavedBuffer = new byte[] { };
            writeSavedBuffer = buffer;
            writeSavedOffset = offset;
            writeSavedCount  = count;

            // Data frame sent will always have a length of 20
            byte[] ret = new byte[ LENGTH_DATA_FRAME ];

            try
            {
                byte[] dataToCipher = new byte[ MAX_LENGTH_DATA ];

                for ( int i = 0; i < count; i++ )
                    dataToCipher[ i ] = buffer[ i + offset ];

                byte frameId = 0x02;
                byte dataCount = (byte)count;

                // Data frame { 2 | CipherCount | Buffer.Length | Buffer_AESx16 | 0 }.Length = 20
                ret = new byte[]
                {
                    frameId,
                    cipheredDataSentCounter,
                    dataCount
                }.ToArray ()
                .Concat ( AES_Encrypt ( dataToCipher, dynamicPass ) )
                .Concat ( new byte[] { 0x00 } ).ToArray ();
                
                Utils.PrintDeep ( "BlePort.WriteCharacteristic.. " +
                "StreamToPrepare = " + Utils.ByteArrayToString ( buffer ) + " | " +
                "NumBytesToWrite " + count.ToString ( "D2" ) + " | " +
                "Offset " + offset.ToString ( "D2" ) );
                
                Utils.PrintDeep ( "BlePort.WriteCharacteristic.. " +
                "StreamToWrite = " +
                "0x02 + " +
                "CipherCount 0x" + cipheredDataSentCounter.ToString ( "D2" ) + " ( " + Convert.ToInt32 ( cipheredDataSentCounter.ToString (), 16 ) + " ) + " +
                "NumBytesToWrite 0x" + dataCount + " ( " + Convert.ToInt32 ( dataCount.ToString (), 16 ) + " ) + " +
                "Buffer.AESx16 + 0x0 " +
                "= " + Utils.ByteArrayToString ( ret ) + " [ Length " + ret.Length + " ]" );

                cipheredDataSentCounter++;
                if ( cipheredDataSentCounter <= 0 )
                    cipheredDataSentCounter = 1;
                
                Utils.PrintDeep ( "BlePort.WriteCharacteristic.. Waiting semaphore" );
                
                await this.semaphore.WaitAsync ();
                
                long nextTimeInit = DateTimeOffset.Now.ToUnixTimeMilliseconds ();
                
                Utils.PrintDeep ( "BlePort.WriteCharacteristic.. My turn | Start at " + timeInit +
                    ( ( timeInit > 0 ) ? " [ Since prev. " + ( nextTimeInit - timeInit ) + " ms. ]" : string.Empty ) );
                
                timeInit = nextTimeInit;
                
                // Controls exception inside method UpdateACKBuffer
                ackException = new TaskCompletionSource<bool> ();

                await gattServer_connection.WriteCharacteristicValue(
                    new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                    new Guid("00000002-0000-1000-8000-00805f9b34fb"),
                    ret
                );
                
                // Controls exception inside method UpdateACKBuffer
                if ( await ackException.Task )
                    throw new Exception ();
                
                Utils.PrintDeep ( "BlePort.WriteCharacteristic.. Already writen" );
            }
            catch (GattException ex)
            {
                Utils.PrintDeep ( "BlePort.WriteCharacteristic -> ERROR: " + ex.Message + " ( GattException )" );
                
                throw ex;
            }
            catch ( Exception exg )
            {
                Utils.PrintDeep ( "BlePort.WriteCharacteristic -> ERROR: " + exg.Message );
                
                throw exg;
            }
        }

        /// <summary>
        /// Updates buffer with the notification data received 
        /// </summary>
        private void UpdateBuffer ( byte[] bytes )
        {
            try
            {
                int bytesOfData = bytes[ INDEX_LENGTH_DATA ];
                if ( bytesOfData > 0 )
                {
                    byte[] tempArray = new byte[ bytesOfData ];
        
                    // Third byte is the length of bytes to read from data frame = 16
                    // Data frame { 2 | CipherCount | Buffer.Length | >>> Buffer_AESx16 <<< | 0 }.Length = 20
                    Array.Copy ( AES_Decrypt ( bytes.Skip ( LENGTH_HEADER ).Take ( MAX_LENGTH_DATA ).ToArray (), dynamicPass ), 0, tempArray, 0, bytesOfData );
        
                    // FIFO collection to read data frames in the same order received
                    for ( int i = 0; i < tempArray.Length; i++ )
                        buffer_ble_data.Enqueue ( tempArray[ i ] );

                    buffer_array = buffer_ble_data.ToArray ();
                    
                    Utils.PrintDeep ( "BlePort.UpdateBuffer.. " +
                    "Stream = " + Utils.ByteArrayToString ( bytes ) +
                    " | Stream.Decrypted = " + Utils.ByteArrayToString ( tempArray ) +
                    " [ +" + bytesOfData + " = " + buffer_ble_data.Count + " received ]" );
                }
                else
                    Utils.PrintDeep ( "BlePort.UpdateBuffer.. Waiting data" );
                
==== BASE ====
                Utils.PrintDeep ( "BlePort.UpdateBuffer.. " +
                "Stream = " + Utils.ByteArrayToString ( bytes ) +
                " | Stream.Decrypted = " + Utils.ByteArrayToString ( tempArray ) +
                " [ +" + bytesOfData + " = " + buffer_ble_data.Count + " received ]" );
                
==== BASE ====
                //Utils.Print("Rx buffer updated");
            }
            catch ( Exception e )
            {
                Utils.PrintDeep ( "BlePort.UpdateBuffer -> ERROR: " + e.Message );
                
                throw e;
            }
        }

        /// <summary>
        /// Updates buffer with the notification data received 
        /// </summary>
        /// <param name="ble_device">The Bluetooth LE peripheral to connect.</param>
        public async Task ConnectoToDevice(IBlePeripheral ble_device, bool isBounded)
        {
            try
            {
                isConnected = CONNECTING;
                connectionError = NO_ERROR;
                var connection = await adapter.ConnectToDevice(
                    // The IBlePeripheral to connect to
                    ble_device,
                    // TimeSpan or CancellationToken to stop the
                    // connection attempt.
                    // If you omit this argument, it will use
                    // BluetoothLowEnergyUtils.DefaultConnectionTimeout
                    TimeSpan.FromSeconds(TimeOutSeconds),
                    // Optional IProgress<ConnectionProgress>
                    progress =>
                    {
                            //                    Utils.Print(progress);
                            //dialogs.Toast("Progreso: " + progress.ToString());
                        }
                );

                if (connection.IsSuccessful())
                {
                    gattServer_connection = connection.GattServer;
                    //                Utils.Print(gattServer_connection.State); // e.g. ConnectionState.Connected
                    // the server implements IObservable<ConnectionState> so you can subscribe to its state
                    gattServer_connection.Subscribe(new ObserverReporter(this));

                    adapter.CurrentState.Subscribe(new BluetoothStatusReporter(this));

                    gattServer_connection.SubscribeOnError ( ( Exception e ) =>
                    {
                        Utils.PrintDeep ( "GATT server error: " + e.Message );
                    });
                    
                    gattServer_connection.SubscribeOnComplete ( () =>
                    {
                        Utils.PrintDeep ( "GATT server: On Complete" );
                    });
                    
                    gattServer_connection.SubscribeOnNext ( ( ConnectionState state ) =>
                    {
                        Utils.PrintDeep ( "GATT server: On Next = " + state );
                    });

                    ble_peripheral = ble_device;

                    await AESConnectionVerifyAsync(ble_peripheral, isBounded);
                }
                else
                {
                    // Do something to inform user or otherwise handle unsuccessful connection.
                    //                Utils.Print("Error connecting to device. result={0:g}", connection.ConnectionResult);
                    // e.g., "Error connecting to device. result=ConnectionAttemptCancelled"
                }
            }
            catch (Exception e)
            {
                Utils.Print(e);
                
                await this.DisconnectDevice ();
            }
        }

        /// <summary>
        /// Updates AES buffer with the notification data received 
        /// </summary>
        private async void UpdateAESBuffer(byte[] bytes)
        {
            Utils.PrintDeep("BlePort.UpdateAESBuffer: " + Utils.ByteArrayToString(bytes));
            if (isConnected == CONNECTING)
            {
                if (bytes.Take(1).ToArray().SequenceEqual(new byte[] { 0xCC }))
                {
                    Utils.PrintDeep("BlePort.UpdateAESBuffer -> 0xCC -> NO");
                    isPaired = false;
                    saved_settings.AddOrUpdateValue("session_dynamicpass", string.Empty);
                    saved_settings.AddOrUpdateValue("session_peripheral", string.Empty);
                    saved_settings.AddOrUpdateValue("responsehi", string.Empty);
                    saved_settings.AddOrUpdateValue("session_peripheral_DeviceId", string.Empty);

                    Listen_Characteristic_Notification();
                    try
                    {
                        Listen_aes_conection_Handler.Dispose();
                    }
                    catch (Exception e)
                    {
                        Utils.Print(e.StackTrace);
                    }
                    connectionError = DYNAMIC_KEY_ERROR;
                    await DisconnectDevice();
                    // this.adapter.DisableAdapter();

                    //  this.adapter.EnableAdapter();
                }

                if (bytes.Take(1).ToArray().SequenceEqual(new byte[] { 0x11 }))
                {
                    Utils.PrintDeep("BlePort.UpdateAESBuffer -> 0x11 -> SI");
                    isPaired = true;
                    saved_settings.AddOrUpdateValue("responsehi", isPaired.ToString());
                    saved_settings.AddOrUpdateValue("session_peripheral", ble_peripheral.Advertisement.DeviceName);
                    var data = ble_peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray();
                    saved_settings.AddOrUpdateValue("session_peripheral_DeviceId", System.Convert.ToBase64String(data));

                    if (dynamicPass != null)
                    {
                        saved_settings.AddOrUpdateValue("session_dynamicpass", System.Convert.ToBase64String(dynamicPass));
                    }

                    Listen_Characteristic_Notification();
                    try
                    {
                        Listen_aes_conection_Handler.Dispose();
                    }
                    catch (Exception e)
                    {
                        Utils.Print(e.StackTrace);
                    }
                    isConnected = CONNECTED;
                    connectionError = NO_ERROR;
                    cipheredDataSentCounter = 1;
                }
            }
        }

        /// <summary>
        /// Updates Ack buffer with the notification data received 
        /// </summary>
        private async void UpdateACKBuffer ( byte[] bytes )
        {
            bool fail = false;
        
            try
            {
                Utils.PrintDeep ( "BlePort.UpdateACKBuffer.. " + Utils.ByteArrayToString ( bytes ) +
                " | CipherCount " + bytes[ 1 ].ToString ( "D2" ) );
            
                //bytes.Skip(3).Take(1).ToArray().SequenceEqual(new byte[] { 0x01 }))
                if ( bytes[ LENGTH_HEADER ] == 0x01 )
                {
                    Utils.PrintDeep ( "BlePort.UpdateACKBuffer.. NO" );
                
                    //cipheredDataSentCounter = bytes.Skip(1).Take(1).ToArray()[0];
                    cipheredDataSentCounter = bytes[ INDEX_CYPHER_COUNT ]++;
                    if ( cipheredDataSentCounter <= 0 )
                        cipheredDataSentCounter = 1;
                    
                    Utils.PrintDeep ( "* BlePort.UpdateACKBuffer.. IF" +
                        " Buffer " + Utils.ByteArrayToString ( writeSavedBuffer ) +
                        " | CipheredDataSentCounter " + cipheredDataSentCounter +
                        " | Offset " + writeSavedOffset.ToString ( "D2" ) +
                        " | count " + writeSavedCount.ToString ( "D2" ) );
                    
                    await Write_Characteristic(writeSavedBuffer, writeSavedOffset, writeSavedCount);
                    
                    return;
                }
            }
            catch ( Exception e )
            {
                Utils.PrintDeep ( "BlePort.UpdateACKBuffer -> ERROR: " + e.Message );
                
                fail = true;
            }
            
            // From this method is not possible to launch
            // an exception because is not bubbled as expected
            ackException.SetResult ( fail );
            
            if ( ! fail )
            {
                long timeEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds ();
                Utils.PrintDeep ( "BlePort.UpdateACKBuffer.. Release semaphore | Finish at " + timeEnd + " [ Dif = " + ( timeEnd - timeInit ) + " ms. ]" );

                this.semaphore.Release ();
            }
        }

        private void CosasError(Exception e)
        {
            Console.Write(e);
        }

        /// <summary>
        /// AES Verification to connect Bluetooth LE peripheral 
        /// </summary>
        private async Task AESConnectionVerifyAsync(IBlePeripheral ble_device, bool isBounded)
        {
            try
            {
                // Will also stop listening when gattServer
                // is disconnected, so if that is acceptable,
                // you don't need to store this disposable.


                //try
                //{
                //    // Will also stop listening when gattServer
                //    // is disconnected, so if that is acceptable,
                //    // you don't need to store this disposable.
                //    Listen_ack_response_Handler = gattServer_connection.NotifyCharacteristicValue(
                //       new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                //       new Guid("00000002-0000-1000-8000-00805f9b34fb"),
                //        UpdateACKBuffer
                //    );

                //}
                //catch (GattException ex)
                //{
                //    Utils.Print(ex.ToString());
                //}

                //gattServer_connection.NotifyCharacteristicValue(
                //   new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                //   new Guid("00000004-0000-1000-8000-00805f9b34fb"),
                //   UpdateAESBuffer,
                //   CosasError
                //);
                //await Task.Delay(1000);

                //IEnumerable<Guid> ListAllServices = gattServer_connection.ListAllServices();
                //gattServer_connection.ListServiceCharacteristics();
                // TO-DO: comprobar que tiene servicios y caracteristicas de un PUK? consultar Maria.            


                /*
                ArrayList ListAllServices = new ArrayList();
                ArrayList ListAllCharacteristics = new ArrayList();
                try
                {                    
                    foreach (var guid in await gattServer_connection.ListAllServices())
                    {
                        ListAllServices.Add(guid);
                        ListAllCharacteristics.Add("_______________________________");
                        Utils.Print("_______________________________");
                        ListAllCharacteristics.Add("Service: " + "\n\r" + guid + "\n\r");
                        Utils.Print("Service: " + "\n\r" + guid + "\n\r");
                        ListAllCharacteristics.Add("________Caracteristics_________");
                        Utils.Print("________Caracteristics_________");
                        foreach (var DescriptionOrGuid in await gattServer_connection.ListServiceCharacteristics(guid))
                        {
                            ListAllCharacteristics.Add(DescriptionOrGuid);
                            Utils.Print(DescriptionOrGuid);
                        }
                    }
                }
                catch (Exception j)
                {

                }
                */

                bool hayServicio = false;
                int reintentos = 5;

                do
                {
                    //  Task.Delay(500).Wait();
                    try
                    {
                        hayServicio = await gattServer_connection.ServiceExists(new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"));
                    }
                    catch (Exception test)
                    {
                        Utils.Print(test.StackTrace); reintentos = 1;
                    }

                    reintentos--;
                }
                while (!hayServicio && (reintentos > 0));


                Thread.Sleep(400);


                Listen_aes_conection_Handler = gattServer_connection.NotifyCharacteristicValue(
                   new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                   new Guid("00000041-0000-1000-8000-00805f9b34fb"),
                   UpdateAESBuffer
                //                   , CosasError
                );

                byte[] PassH_crypt = new byte[] { };
                byte[] PassL_crypt = new byte[] { };
                byte[] ticks = new byte[] { };

                //Read Pass H data from Characteristic
                PassH_crypt = await gattServer_connection.ReadCharacteristicValue(
                    new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                    new Guid("00000040-0000-1000-8000-00805f9b34fb")
                );

                //Read Pass L data from Characteristic
                PassL_crypt = await gattServer_connection.ReadCharacteristicValue(
                    new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                    new Guid("00000042-0000-1000-8000-00805f9b34fb")
                );

                //Read Pass L data from Characteristic
                ticks = await gattServer_connection.ReadCharacteristicValue(
                    new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                    new Guid("00000044-0000-1000-8000-00805f9b34fb")
                );

                bool isOnState = true;
                for (int i = 0; i < PassH_crypt.Length; i++)
                {
                    if (PassH_crypt[i] != 0x00)
                    {
                        isOnState = false;
                    }
                }
                for (int i = 0; i < PassL_crypt.Length; i++)
                {
                    if (PassL_crypt[i] != 0x00)
                    {
                        isOnState = false;
                    }
                }
                if (isBounded && !isOnState)
                {
                    isBounded = false;
                }

                if (isBounded)
                {
                    // YOU CAN RETURN THE PASS BY GETTING THE STRING AND CONVERTING IT TO BYTE ARRAY TO AUTO-PAIR
                    dynamicPass = System.Convert.FromBase64String(saved_settings.GetValueOrDefault("session_dynamicpass", string.Empty));
                    byte[] hi_msg;

                    Array.Copy(ticks, 0, say_hi, 12, 4);
                    hi_msg = AES_Encrypt(say_hi, dynamicPass);

                    await gattServer_connection.WriteCharacteristicValue(
                      new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                      new Guid("00000041-0000-1000-8000-00805f9b34fb"),
                      hi_msg
                    );

                }
                else
                {
                    Thread.Sleep(400);
                    if (isOnState)
                    {
                        Listen_Characteristic_Notification();
                        try
                        {
                            Listen_aes_conection_Handler.Dispose();
                        }
                        catch (Exception e)
                        {
                            Utils.Print(e.StackTrace);
                        }
                        connectionError = NO_DYNAMIC_KEY_ERROR;
                        await DisconnectDevice();
                        return;
                    }
                    byte[] hi_msg;
                    /*
                    byte[] PassH_crypt = new byte[] { };
                    byte[] PassL_crypt = new byte[] { };

                    //Read Pass H data from Characteristic
                    PassH_crypt = await gattServer_connection.ReadCharacteristicValue(
                        new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                        new Guid("00000040-0000-1000-8000-00805f9b34fb")
                    );

                    //Read Pass L data from Characteristic
                    PassL_crypt = await gattServer_connection.ReadCharacteristicValue(
                        new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                        new Guid("00000042-0000-1000-8000-00805f9b34fb")
                    );
                    */
                    byte[] PassH_decrypt = AES_Decrypt(PassH_crypt, static_pass);
                    byte[] PassL_decrypt = AES_Decrypt(PassL_crypt, static_pass);

                    //Generate dynamic password
                    dynamicPass = new byte[PassH_decrypt.Length + PassL_decrypt.Length];

                    Array.Copy(PassH_decrypt, 0, dynamicPass, 0, PassH_decrypt.Length);
                    Array.Copy(PassL_decrypt, 0, dynamicPass, PassH_decrypt.Length, PassL_decrypt.Length);

                    Array.Copy(ticks, 0, say_hi, 12, 4);
                    hi_msg = AES_Encrypt(say_hi, dynamicPass);

                    await gattServer_connection.WriteCharacteristicValue(
                      new Guid("ba792500-13d9-409b-8abb-48893a06dc7d"),
                      new Guid("00000041-0000-1000-8000-00805f9b34fb"),
                      hi_msg
                    );
                }
                Thread.Sleep(400);

                Listen_Battery_level = gattServer_connection.NotifyCharacteristicValue(
                    new Guid("1d632100-dc5a-41ab-bdbb-7cff9901210d"),
                    new Guid("0000000c-0000-1000-8000-00805f9b34fb"),
                    UpdateBatteryLevel
                 );

                batteryLevel = await gattServer_connection.ReadCharacteristicValue(
                    new Guid("1d632100-dc5a-41ab-bdbb-7cff9901210d"),
                    new Guid("0000000c-0000-1000-8000-00805f9b34fb")
                );
            }
            catch ( Exception ex )
            {
                Utils.Print(ex.ToString());
                
                await this.DisconnectDevice ();
            }
        }

        /// <summary>
        /// Updates Ack buffer with the notification data received 
        /// </summary>
        private void UpdateBatteryLevel(byte[] bytes)
        {
            batteryLevel = bytes;
        }

        /// <summary>
        /// AES Decryptation algorithm
        /// </summary>
        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
        
            try
            {

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.None;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            }
            catch ( Exception e )
            {
                Utils.PrintDeep ( "BlePorts.AES_Decrypt -> ERROR: " + e.Message );
            }

            return decryptedBytes;
        }

        /// <summary>
        /// AES Encryptation algorithm
        /// </summary>
        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.None;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// Disconnects from Bluetooth LE peripheral 
        /// </summary>
        public async Task DisconnectDevice ()
        {
            try
            {
                if ( ! adapter.CurrentState.IsDisabledOrDisabling () )
                {
                    Stop_Listen_Characteristic_Notification ();
                    try
                    {
                        Listen_ack_response_Handler.Dispose();
                    }
                    catch (Exception e1)
                    {
                        Utils.Print(e1.StackTrace);
                    }

                    try
                    {
                        Listen_aes_conection_Handler.Dispose();
                    }
                    catch (Exception e2)
                    {
                        Utils.Print(e2.StackTrace);
                    }

                    try
                    {
                        Listen_Battery_level.Dispose();
                    }
                    catch (Exception e3)
                    {
                        Utils.Print(e3.StackTrace);
                    }
                }

                isConnected = NO_CONNECTED;
                await gattServer_connection.Disconnect();

            }
            catch ( Exception e )
            {
                
            }
        }

        /// <summary>
        /// Scans for Bluetooth LE peripheral broadcasts 
        /// </summary>
        private async Task ScanForBroadcasts()
        {
            try
            {
                // Utils.Print($"--------------------------------------------------------------Empieza el escaneo: {isScanning.ToString()} thread: {Thread.CurrentThread.ManagedThreadId}");
                if (!isScanning)
                {
                    //List<IBlePeripheral> BlePeripheralListAux = new List<IBlePeripheral>();
                    // Utils.Print($"--------------------------------------------------------------Escaneando: thread: {Thread.CurrentThread.ManagedThreadId}");
                    BlePeripheralList.Clear();
                    isScanning = true;

                    ScanFilter filter = new ScanFilter
                    {
                        AdvertisedDeviceName = "Aclara",
                         IgnoreRepeatBroadcasts = false

                    };
                    await adapter.ScanForBroadcasts(
                        // Optional scan filter to ensure that the observer will only receive peripherals
                        // that pass the filter. If you want to scan for everything around, omit this argument.
                        filter,//.SetIgnoreRepeatBroadcasts(false),
                        // IObserver<IBlePeripheral> or Action<IBlePeripheral> will be triggered for each discovered peripheral
                        // that passes the above can filter (if provided).
                        (IBlePeripheral peripheral) =>
                        {
                            try
                            {
                            // read the advertising data...
                            var adv = peripheral.Advertisement;


                                //Utils.PrintDeep("--------" + Utils.ByteArrayToString(peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()));
                               
                                if (adv.DeviceName != null)
                                {
                                    Utils.PrintDeep("-------" + adv.DeviceName);
                                    if (adv.DeviceName.Equals("Aclara"))
                                    {
                                        Utils.PrintDeep("--------" + Utils.ByteArrayToString(peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()));
                                        if (BlePeripheralList.Any(p => p.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray().SequenceEqual(peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray())))
                                        {
                                            BlePeripheralList[BlePeripheralList.FindIndex(f => f.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray().SequenceEqual(peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()))] = peripheral;
                                        }
                                        else
                                        {
                                            BlePeripheralList.Add(peripheral);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        },
                        // TimeSpan or CancellationToken to stop the scan
                        // If you omit this argument, it will use BluetoothLowEnergyUtils.DefaultScanTimeout
                        TimeSpan.FromSeconds(timeOutSeconds)
                    );
                    //BlePeripheralList = BlePeripheralListAux;
                }
                isScanning = false;
                //  Utils.Print($"-----------------------------------------------------Escaneado terminado, encontrados: {BlePeripheralList.Count}, thread: {Thread.CurrentThread.ManagedThreadId}");
                // scanning has been stopped when code reached this point
            }
            catch (Exception e)
            {

            }
        }

        public byte[] GetBatteryLevel()
        {
            //Task.Factory.StartNew(GetBatteryLevelAsync);
            return batteryLevel;
        }

        /*
        private async Task GetBatteryLevelAsync()
        {
            //Read Pass H data from Characteristic
            batteryLevel = await gattServer_connection.ReadCharacteristicValue(
                    new Guid("00002100-13d9-409b-8abb-48893a06dc7d"),
                    new Guid("0000000c-0000-1000-8000-00805f9b34fb")
                );
        }
        */
    }
}
