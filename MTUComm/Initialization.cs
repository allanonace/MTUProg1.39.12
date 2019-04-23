using System;
using Lexi;
using nexus.protocols.ble;
using ble_library;
using Library;

namespace MTUComm
{
    public sealed class Initialization
    {
        public static void Load ( IBluetoothLowEnergyAdapter adapter )
        {
            // Initializes Bluetooth
            BleSerial ble = new BleSerial ( adapter );
            
            // Initializes Lexi
            Lexi.Lexi lexi = new Lexi.Lexi ( ble, 10000 );

            // Set singleton references            
            Singleton.Set = ble;
            Singleton.Set = lexi;
        }
        
        /// <summary>
        /// Clases que tendran que tratarse como singleton:
        /// - Configuration
        /// - MTUComm
        /// - Logger
        /// - Errors
        /// - 
        /// </summary>
        
        public static void Unload ()
        {
            // Closes bluetooth port
            Singleton.Get.BleSerial.Close ();
        }
    }
}
