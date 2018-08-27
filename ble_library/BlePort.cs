using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using nexus.protocols.ble;
using nexus.protocols.ble.gatt;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using Xamarin.Forms;

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
            Console.WriteLine("Status Report Completed");
        }

        public virtual void OnError(Exception error)
        {

        }

        public void OnNext(ConnectionState value)
        {
            Console.WriteLine("Status: " + value.ToString());

            if (value == ConnectionState.Disconnected)
            {
                //dialogs.Toast("Device disconnected");
                try
                {
                    blePort.DisconnectDevice();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }

    public class BlePort
    {
        private Queue<byte> buffer_ble_data;
        private IBluetoothLowEnergyAdapter adapter;
        private IBleGattServerConnection gattServer_connection;
        private IDisposable Listen_Characteristic_Notification_Handler;
        private Boolean isConnected;
        private List<IBlePeripheral> BlePeripheralList;
//      private ArrayList ListAllServices;
//      private ArrayList ListAllCharacteristics;

        /// <summary>
        /// Initizalize Bluetooth LE Serial Port
        /// </summary>
        /// <param name="adapter_app">The Bluetooth Low Energy Adapter from the OS</param>
        public BlePort(IBluetoothLowEnergyAdapter adapter_app)
        {
            adapter = adapter_app;
            buffer_ble_data = new Queue<byte>();
            isConnected = false;
            busy = false;
        }

        /// <summary>
        /// Returns the Connection status with the Bluetooth device
        /// </summary>
        /// <returns>The Bluetooth connection status.</returns>
        public Boolean GetConnectionStatus()
        {
            return isConnected;
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
        public int BytesToRead()
        {
            return buffer_ble_data.Count;
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
        public void StartScan(){
            
             Device.StartTimer(
             TimeSpan.FromSeconds(1),
             () =>
             {

                 BluetoothEnable();

                 if (adapter.CurrentState.IsEnabledOrEnabling())
                 {
                     ScanForBroadcasts();
                 }
                 return false;
             });

        }

        /// <summary>
        /// Enables bluetooth antenna on device.
        /// </summary>
        private async void BluetoothEnable()
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
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Stops listening to the characteristic notifications of a peripheral
        /// </summary>
        private  void Stop_Listen_Characteristic_Notification()
        {
            try{
                Listen_Characteristic_Notification_Handler.Dispose();
            }catch(Exception e){
                
            }
        }
       

        /// <summary>
        /// Writes a number of bytes via Bluetooth LE to the peripheral gatt connnection
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the bytes.</param>
        /// <param name="count">The maximum number of bytes to read. Fewer bytes are read if count is greater than the number of bytes in the input buffer.</param>
        public async void Write_Characteristic(byte[] buffer, int offset, int count)
        {
            try
            {               
                byte[] ret = new byte[count];
              
                for (int i = 0; i < count; i++){
                    ret[i] = buffer[i + offset];
                }

                await gattServer_connection.WriteCharacteristicValue(
                    new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                    new Guid("00000002-0000-1000-8000-00805f9b34fb"),
                    ret
                );
            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// Updates buffer with the notification data received 
        /// </summary>
        private void UpdateBuffer(byte[] bytes )
        {
            if(bytes.Length == 20)
            {
                byte[] tempArray = new byte[bytes[2]];
                Array.Copy(bytes, 3, tempArray, 0, bytes[2]);
                for (int i = 0; i < tempArray.Length; i++)
                {
                    buffer_ble_data.Enqueue(tempArray[i]);
                }
            }
        }

        /// <summary>
        /// Updates buffer with the notification data received 
        /// </summary>
        /// <param name="ble_device">The Bluetooth LE peripheral to connect.</param>
        public async void ConnectoToDevice(IBlePeripheral ble_device){
            var connection = await adapter.ConnectToDevice(
                // The IBlePeripheral to connect to
                ble_device,
                // TimeSpan or CancellationToken to stop the
                // connection attempt.
                // If you omit this argument, it will use
                // BluetoothLowEnergyUtils.DefaultConnectionTimeout
                TimeSpan.FromSeconds(15),
                // Optional IProgress<ConnectionProgress>
                progress => {
                    Console.WriteLine(progress);
                    //dialogs.Toast("Progreso: " + progress.ToString());
                }
            );

            if (connection.IsSuccessful())
            {
                gattServer_connection = connection.GattServer;

                Console.WriteLine(gattServer_connection.State); // e.g. ConnectionState.Connected
                                                                // the server implements IObservable<ConnectionState> so you can subscribe to its state

                gattServer_connection.Subscribe(new ObserverReporter(this));                
                Listen_Characteristic_Notification();
                isConnected = true;

                // TO-DO: comprobar que tiene servicios y caracteristicas de un PUK? consultar Maria.
                /*
                try
                {
                    ListAllServices = new ArrayList();
                    ListAllCharacteristics = new ArrayList();

                    foreach (var guid in await gattServer_connection.ListAllServices())
                    {
                        ListAllServices.Add(guid);
                        ListAllCharacteristics.Add("_______________________________");                  
                        ListAllCharacteristics.Add("Service: " + "\n\r" + guid + "\n\r");
                        ListAllCharacteristics.Add("________Caracteristics_________");                  
                        foreach (var DescriptionOrGuid in await gattServer_connection.ListServiceCharacteristics(guid))
                        {
                            ListAllCharacteristics.Add(DescriptionOrGuid);
                        }

                    }

                }catch(Exception j){
                    
                }
                */                
            }
            else
            {
                // Do something to inform user or otherwise handle unsuccessful connection.
                Console.WriteLine("Error connecting to device. result={0:g}", connection.ConnectionResult);
                // e.g., "Error connecting to device. result=ConnectionAttemptCancelled"
                isConnected = false;
            }

        }

        /// <summary>
        /// Disconnects from Bluetooth LE peripheral 
        /// </summary>
        public async void DisconnectDevice()
        {
            if (isConnected)
            {
                Stop_Listen_Characteristic_Notification();
                await gattServer_connection.Disconnect();
                isConnected = false;
            }
        }

        private bool busy;

        /// <summary>
        /// Scans for Bluetooth LE peripheral broadcasts 
        /// </summary>
        private async void ScanForBroadcasts()
        {
            if(!busy){
                
                BlePeripheralList = new List<IBlePeripheral> { };
                busy = true;
                await adapter.ScanForBroadcasts(
                // Optional scan filter to ensure that the
                // observer will only receive peripherals
                // that pass the filter. If you want to scan
                // for everything around, omit this argument.
                new ScanFilter()

                   .SetIgnoreRepeatBroadcasts(false),
                    // IObserver<IBlePeripheral> or Action<IBlePeripheral>
                    // will be triggered for each discovered peripheral
                    // that passes the above can filter (if provided).
                    (IBlePeripheral peripheral) =>
                    {
                    // read the advertising data...
                    var adv = peripheral.Advertisement;
                    Console.WriteLine(adv.DeviceName);

                    String serv = adv.Services
                                    .Select
                                     (x => {
                                         var name = adv.DeviceName;
                                         return name != null || name.Equals("")
                                                    ? x.ToString()
                                                    : x.ToString() + " (" + name + ")";
                                     }
                                     ).ToString();

                    serv = serv + ", ";

                    Console.WriteLine(serv);
                    Console.WriteLine(adv.ManufacturerSpecificData.FirstOrDefault().CompanyName());
                    Console.WriteLine(adv.ServiceData);

                    //Show dialog with name
                    if(adv.DeviceName!=null){
                        if (adv.DeviceName.Contains("Aclara"))
                        {
                            if(BlePeripheralList.Any(p => p.DeviceId.Equals(peripheral.DeviceId)))
                            {
                                BlePeripheralList[BlePeripheralList.FindIndex(f => f.DeviceId.Equals(peripheral.DeviceId))] = peripheral;
                            }else{
                                BlePeripheralList.Add(peripheral);
                              
                            }
                        } 
                    }
                    //  connect to the device
                   },
                    // TimeSpan or CancellationToken to stop the scan
                    TimeSpan.FromSeconds(10)
                    // If you omit this argument, it will use
                    // BluetoothLowEnergyUtils.DefaultScanTimeout
                );  
            }
            busy = false;
            // scanning has been stopped when code reached this point
        }

    }

}