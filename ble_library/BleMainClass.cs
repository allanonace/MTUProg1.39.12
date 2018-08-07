using System;
using System.Collections;
using System.Linq;
using Acr.UserDialogs;
using nexus.protocols.ble;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using Xamarin.Forms;

namespace ble_library
{
    public class BleMainClass
    {
        public static String buffer;
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

                    foreach (var guid in await gattServer_connection.ListAllServices())
                    {
                        //Debug.WriteLine($"service: {known.GetDescriptionOrGuid(guid)}");
                        ListAllServices.Add(guid);
                        dialogs.Alert("Service: " + guid);

                    }
                }catch(Exception j){
                    
                }
               


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

        public async static void DisconnectFromDevice(){
            await gattServer_connection.Disconnect();
            dialogs.Toast("Disconnected from device");
            Connection_app = false;
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
                        buffer += adv.DeviceName;
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


		public static void MostrarBuffer(){
          
            Device.StartTimer(
             TimeSpan.FromSeconds(3),
             () =>
             {  
                 dialogs.Alert(buffer);
                 return false;
             });

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