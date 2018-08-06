using System;
using System.Linq;
using Acr.UserDialogs;
using nexus.protocols.ble;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;

namespace ble_library
{
    public class BleMainClass
    {     
        public static void init(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
        {

            InterfacesPorConsola();
            dialogs.Toast("La libreria ha cargado correctamente");
            dialogs.Alert("Inicialización de la librería");

            BluetoothEnable(adapter);
           
            if(adapter.CurrentState.IsEnabledOrEnabling()){
                ScanForBroadcasts(adapter,dialogs);
            }

        }


        private async static void BluetoothEnable(IBluetoothLowEnergyAdapter adapter){
            if (adapter.AdapterCanBeEnabled && adapter.CurrentState.IsDisabledOrDisabling())
            {
                await adapter.EnableAdapter();
            }
        }


        private async static void ScanForBroadcasts(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
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
                dialogs.Alert("Nombre Dispositivo: " + adv.DeviceName);

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

                // dialogs.Alert("Servicios: "+serv);
                //dialogs.Alert("Compañia: " + adv.ManufacturerSpecificData.FirstOrDefault().CompanyName());
                //dialogs.Alert("Datos Servicio: " + adv.ServiceData);
                 Console.WriteLine(adv.ManufacturerSpecificData.FirstOrDefault().CompanyName());
                 Console.WriteLine(adv.ServiceData);

                 //  connect to the device

               },
                // TimeSpan or CancellationToken to stop the scan
                TimeSpan.FromSeconds(30)
                // If you omit this argument, it will use
                // BluetoothLowEnergyUtils.DefaultScanTimeout
            );

            // scanning has been stopped when code reached this point

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
