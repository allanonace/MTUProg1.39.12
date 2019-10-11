using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aclara_meters.util;
using Foundation;
using nexus.protocols.ble;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(aclara_meters.iOS.AdapterBluetooth))]
namespace aclara_meters.iOS
{
    class AdapterBluetooth : IAdapterBluetooth
    {
        public IBluetoothLowEnergyAdapter GetNativeBleAdapter()
        {
            var bluetooth = BluetoothLowEnergyAdapter.ObtainDefaultAdapter();
            return bluetooth;
        }
    }
}