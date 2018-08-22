using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using nexus.core.text;
using nexus.protocols.ble;
using nexus.protocols.ble.gatt;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using Xamarin.Forms;

namespace ble_library
{
    public class BlePort
    {
        private static Queue<byte> buffer_ble_data;
        private IBluetoothLowEnergyAdapter adapter;
        private IUserDialogs dialogs;
        private IBlePeripheral ble_device;
        private static IBleGattServerConnection gattServer_connection;
        private static IDisposable Listen_Characteristic_Notification_Handler;
        private static Boolean Connection_app;
        private ArrayList ListAllServices;
        private ArrayList ListAllCharacteristics;
        private Guid ServicioWrite;
        private Guid CaracterisicoWrite;
        private Guid ServicioIndicate;
        private Guid CaracterisicoIndicate;
       



        public static void SetConnectionApp(Boolean status){
            Connection_app = status;
        }

        public void setServicioWrite(Guid value)
        {
            ServicioWrite = value;
        }

        public void setCaracterisicoWrite(Guid value)
        {
            CaracterisicoWrite = value;
        }


        public void setServicioIndicate(Guid value)
        {
            ServicioIndicate = value;
        }

        public void setCaracterisicoIndicate(Guid value)
        {
            CaracterisicoIndicate = value;
        }

     
        public byte GetBufferElement()
        {
            return buffer_ble_data.Dequeue();
        }

        public Boolean getConnection_app(){
            return Connection_app;
        }

        public int BytesToRead()
        {
            return buffer_ble_data.Count;
        }

        public void clearBuffer_ble_data()
        {
            buffer_ble_data.Clear();
        }

        public class ObserverReporter : IObserver<ConnectionState>
        {
            private IDisposable unsubscriber;

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
                    try{
                        DisconnectFromDevice();
                    }catch(Exception e){
                        throw e;
                    }

                    SetConnectionApp(false);
               
                }
            }
        }

        public void init(IBluetoothLowEnergyAdapter adapter_app, IUserDialogs dialogs_app)
        {
			adapter = adapter_app;
            dialogs = dialogs_app;

            Connection_app = false;


           
        }


        public void startScan(){

             Device.StartTimer(
             TimeSpan.FromSeconds(3),
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

        private async void BluetoothEnable()
        {
            if (adapter.AdapterCanBeEnabled && adapter.CurrentState.IsDisabledOrDisabling())
            {
                await adapter.EnableAdapter();
            }
        }

       
        private void Listen_Characteristic_Notification()
        {
            try
            {
         
                // Will also stop listening when gattServer
                // is disconnected, so if that is acceptable,
                // you don't need to store this disposable.

                Listen_Characteristic_Notification_Handler = gattServer_connection.NotifyCharacteristicValue(
                   ServicioIndicate,
                   CaracterisicoIndicate,
                   UpdateDisplayedValue
                 
                );

            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



        private  static void Stop_Listen_Characteristic_Notification()
        {
            try{
                Listen_Characteristic_Notification_Handler.Dispose();
            }catch(Exception e){
                
            }
        }
       
        public async void Write_Characteristic(byte[] buffer, int offset, int count)
        {
            try
            {
               
                byte[] ret = new byte[count];
              
                for (int i = 0; i < count; i++){
                    ret[i] = buffer[i + offset];
                }


               
                await WriteCurrentBytesGUIDAsync(ret);
            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

  


        private async Task WriteCurrentBytesGUIDAsync(byte[] buffer)
        {
            try
            {

                var bytes_temp_characteristic_read = gattServer_connection.WriteCharacteristicValue(
                    ServicioWrite,
                    CaracterisicoWrite,
                    buffer
                );

                UpdateDisplayedValue(await bytes_temp_characteristic_read);

            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


        private static void UpdateDisplayedValue(byte[] bytes )
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

        public async void ConnectoToDevice(){
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

                gattServer_connection.Subscribe(new ObserverReporter());

                Connection_app = true;

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

                setCaracterisicoIndicate(new Guid("00000003-0000-1000-8000-00805f9b34fb"));
                setServicioIndicate(new Guid("2cf42000-7992-4d24-b05d-1effd0381208"));

                setServicioWrite(new Guid("2cf42000-7992-4d24-b05d-1effd0381208"));
                setCaracterisicoWrite(new Guid("00000002-0000-1000-8000-00805f9b34fb"));

                buffer_ble_data = new Queue<byte>();
                Listen_Characteristic_Notification();
            }
            else
            {
                // Do something to inform user or otherwise handle unsuccessful connection.
                Console.WriteLine("Error connecting to device. result={0:g}", connection.ConnectionResult);
                dialogs.Alert("Error connecting to device. result={0:g}", connection.ConnectionResult.ToString());
                // e.g., "Error connecting to device. result=ConnectionAttemptCancelled"
                Connection_app = false;
            }

        }

       


        public async static void DisconnectFromDevice(){
            Stop_Listen_Characteristic_Notification();
            await gattServer_connection.Disconnect();
            Connection_app = false;
        }


        public async void DisconnectDevice()
        {
            Stop_Listen_Characteristic_Notification();
            await gattServer_connection.Disconnect();
            Connection_app = false;
        }


        private async void ScanForBroadcasts()
        {
            await adapter.ScanForBroadcasts(
            // Optional scan filter to ensure that the
            // observer will only receive peripherals
            // that pass the filter. If you want to scan
            // for everything around, omit this argument.
            new ScanFilter()

               .SetIgnoreRepeatBroadcasts(true),
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
                        //dialogs.Alert("Nombre Dispositivo: " + adv.DeviceName);
          
                        ble_device = peripheral;
                    } 
                }
                //  connect to the device
               },
                // TimeSpan or CancellationToken to stop the scan
                TimeSpan.FromSeconds(30)
                // If you omit this argument, it will use
                // BluetoothLowEnergyUtils.DefaultScanTimeout
            );

            // scanning has been stopped when code reached this point
        }

    }

}