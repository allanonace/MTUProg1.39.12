using System;
using System.Threading.Tasks;

namespace aclara_meters.Helpers
{
    public class ConexionBluetooth
    {
        private static ConexionBluetooth instance = null;

        private ConexionBluetooth() { }

        public static ConexionBluetooth Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConexionBluetooth();
                return instance;
            }
        }

        public async Task<int> SearchDevices(int TimeOutseconds)
        {
            await FormsApp.ble_interface.Scan();

            return FormsApp.ble_interface.GetBlePeripheralList().Count;
        }

        public void  PairDevice(bool isBounded  = false)
        {
            FormsApp.ble_interface.Open(FormsApp.peripheral, isBounded);
           
        }

    }
}
