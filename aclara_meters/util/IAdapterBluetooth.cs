using nexus.protocols.ble;
using System;
using System.Collections.Generic;
using System.Text;

namespace aclara_meters.util
{
    public interface IAdapterBluetooth
    {
        IBluetoothLowEnergyAdapter GetNativeBleAdapter();
    }
}
