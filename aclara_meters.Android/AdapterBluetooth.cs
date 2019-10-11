using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aclara_meters.util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using nexus.protocols.ble;

[assembly: Xamarin.Forms.Dependency(typeof(aclara_meters.Droid.AdapterBluetooth))]
namespace aclara_meters.Droid
{
    class AdapterBluetooth : IAdapterBluetooth
    {
        public IBluetoothLowEnergyAdapter GetNativeBleAdapter()
        {
            // 
            var bleAdapter = BluetoothLowEnergyAdapter.ObtainDefaultAdapter(Android.App.Application.Context);
            return bleAdapter;
        }
    }
}