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
    public class BleMainClass
    {
        public static IBluetoothLowEnergyAdapter adapter;
        public static IUserDialogs dialogs;

        public static IBlePeripheral ble_device;
        public static IBleGattServerConnection gattServer_connection;

        public static String m_connectionState;
        public static String Connection
        {
            get { return m_connectionState; }
            private set
            {
                if (value != ConnectionState.Disconnected.ToString())
                {
                    m_connectionState = value;
                }
            }
        }

        public static Boolean Connection_app;

        public class ObserverReporter : IObserver<ConnectionState>
        {
            private IDisposable unsubscriber;

            public virtual void Subscribe(IObservable<ConnectionState> provider)
            {
                unsubscriber = provider.Subscribe(this);
                dialogs.Toast("Subscribed to Device");
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
                // Do nothing.
            }

            public void OnNext(ConnectionState value)
            {
                Console.WriteLine("Status: " + value.ToString());
                dialogs.Toast("Status: " + value.ToString());

                if (value == ConnectionState.Disconnected)
                {
                    dialogs.Toast("Device disconnected");
                    try{
                        DisconnectFromDevice();
                    }catch(Exception e){
                        
                    }
                    Connection_app = false;
                }
            }
        }

        public static void init(IBluetoothLowEnergyAdapter adapter_app, IUserDialogs dialogs_app)
        {
			adapter = adapter_app;
            dialogs = dialogs_app;

            Connection_app = false;

           


            Device.StartTimer(
              TimeSpan.FromSeconds(3),
              () =>
              {
                  InterfacesPorConsola();
                  dialogs.Toast("La libreria ha cargado correctamente");

                  BluetoothEnable();

                  if (adapter.CurrentState.IsEnabledOrEnabling())
                  {
                      ScanForBroadcasts();
                  }
                  return false;
              });
        }

        private async static void BluetoothEnable()
        {
            if (adapter.AdapterCanBeEnabled && adapter.CurrentState.IsDisabledOrDisabling())
            {
                await adapter.EnableAdapter();
            }
        }




        public static IDisposable Listen_Characteristic_Notification_Handler;
        //public static Byte[] m_bytearray_notification_readmtu;
        public async static void Listen_Characteristic_Notification_ReadMTU(){
            
            try
            {
          





                Guid ServicioIndicate = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                Guid CaracterisicoIndicate = new Guid("00000003-0000-1000-8000-00805f9b34fb");

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

        public static bool Stop_Notification = false;

        public async static void Stop_Listen_Characteristic_Notification_ReadMTU()
        {
            Listen_Characteristic_Notification_Handler.Dispose();
            Stop_Notification = true;

        }

        public static string hex;
        public static Byte[] val;


  
        public static Byte[] m_bytearray_write_characteristic_read;
        public async static void Write_Characteristic_ReadMTU()
        {
            await WriteCharacteristicMethodAsync();
          
            Stop_Notification = false;
        }

        private static async Task WriteCharacteristicMethodAsync()
        {
            
        
            try
            {
                await WriteCurrentBytesGUIDAsync();
            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private static async Task WriteCurrentBytesGUIDAsync()
        {
            try
            {
                ValueAsHexBytes = new Byte[] { };
                m_bytearray_write_characteristic_read = new Byte[] { };

                var bytes_temp_characteristic_read = gattServer_connection.WriteCharacteristicValue(
                    new Guid("2cf42000-7992-4d24-b05d-1effd0381208"),
                    new Guid("00000002-0000-1000-8000-00805f9b34fb"),
                    new byte[] { (byte)0x00, (byte)0x00, (byte)0x05, (byte)0x25, (byte)0x80, (byte)0x00, (byte)0xFF, (byte)0x5C }
                );

                UpdateDisplayedValue(await bytes_temp_characteristic_read);

            }
            catch (GattException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


        //static byte[] bufferFull = new byte[]{};
        //static List<byte> al_list = new List<byte>();

        public static BleSerial interfaceBle;

        public static Byte[] ValueAsHexBytes;

        private static void UpdateDisplayedValue(byte[] bytes )
        {
         
            //bufferFull = bufferFull.Concat(bytes).ToArray();



           // byte[] ret = new byte[ValueAsHexBytes.Length + bytes.Length];
           // Array.Copy(ValueAsHexBytes, 0, ret, 0, ValueAsHexBytes.Length);
           // Array.Copy(bytes, 0, ret, ValueAsHexBytes.Length, bytes.Length);

           // ValueAsHexBytes = ret;

            //ble_library.BleSerial.buffer_interface = ValueAsHexBytes;

            //ble_library.BleSerial.buffer_interface = ble_library.BleSerial.buffer_interface.Concat(bytes).ToArray();




            byte[] ret = new byte[ValueAsHexBytes.Length + bytes.Length];
            Array.Copy(ValueAsHexBytes, 0, ret, 0, ValueAsHexBytes.Length);
            Array.Copy(bytes, 0, ret, ValueAsHexBytes.Length, bytes.Length);



            ValueAsHexBytes = ret;

            ble_library.BleSerial.buffer_interface = ValueAsHexBytes;


           
            //interfaceBle.Read(bytes, interfaceBle.BytesToRead(), interfaceBle.BytesToRead() + bytes.Length);
            //interfaceBle.Write(bytes, 0, interfaceBle.BytesToRead() + bytes.Length);
            //Console.WriteLine("Bytes interfaceBle: " + interfaceBle.BytesToRead().ToString());
            //Console.WriteLine("Bytes interfaceBle buffer sent: " + bytes.EncodeToBase16String());



            //Console.WriteLine("Bytes characteristic_read: " + bufferFull.EncodeToBase16String());
           
        }

        public async static void ConnectoToDevice(){

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
                    dialogs.Toast("Progreso: " + progress.ToString());
                }
            );

            if (connection.IsSuccessful())
            {
                gattServer_connection = connection.GattServer;
                // ... do things with gattServer here...

                Console.WriteLine(gattServer_connection.State); // e.g. ConnectionState.Connected
                                                                // the server implements IObservable<ConnectionState> so you can subscribe to its state

                gattServer_connection.Subscribe(new ObserverReporter());
 
                Connection = "Reading Services";

                Connection_app = true;

                try{
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
               

               // dialogs.Alert("\n\r" + string.Join("\n\r", ListAllServices.ToArray()) + "\n\r");
                dialogs.Alert("\n\r"  + string.Join("\n\r", ListAllCharacteristics.ToArray()) + "\n\r");


                interfaceBle = new BleSerial("Read MTU");
                ValueAsHexBytes = new Byte[] { };
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

        public static ArrayList ListAllServices;
        public static ArrayList ListAllCharacteristics;


        public async static void DisconnectFromDevice(){
            await gattServer_connection.Disconnect();
            dialogs.Toast("Disconnected from device");
            Connection_app = false;
            Stop_Notification = true;

        }

        private async static void ScanForBroadcasts()
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

                //dialogs.Alert("Servicios: "+serv);
                //dialogs.Alert("Compañia: " + adv.ManufacturerSpecificData.FirstOrDefault().CompanyName());
                //dialogs.Alert("Datos Servicio: " + adv.ServiceData);

                Console.WriteLine(adv.ManufacturerSpecificData.FirstOrDefault().CompanyName());
                Console.WriteLine(adv.ServiceData);

                //Show dialog with name
                if(adv.DeviceName!=null){
                    if (adv.DeviceName.Contains("Aclara"))
                    {
                        dialogs.Alert("Nombre Dispositivo: " + adv.DeviceName);
               
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



		public static void DoSomething(InterfazHija familia)
        {
            // Do awesome stuff here.
        }

        private static void InterfacesPorConsola()
        {
            Console.WriteLine("Este es el padre");  
            InterfazPadre Padre = new BleMainInterface();
            Padre.ONE();

            Console.WriteLine("Este es el hijo");  
            InterfazHijo Hijo = new BleMainInterface();
            Hijo.ONE();
            Hijo.TWO();

            Console.WriteLine("Esta es la hija");  
            InterfazHija Hija = new BleMainInterface();
            Hija.ONE();
            Hija.THREE();

            Console.WriteLine("Esta es la familia"); 
            InterfazFamilia Familia = new BleMainInterface();
            Familia.ONE();
            Familia.TWO();
            Familia.THREE();

            Console.WriteLine("Este es un invitado"); 
            InterfazInvitado Invitado = new BleMainInterface();
            Invitado.FOUR();

            Console.ReadLine();  
        }

    }

}