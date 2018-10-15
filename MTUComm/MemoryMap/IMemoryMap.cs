using System;
using System.Collections.Generic;
using System.Text;

namespace MTUComm
{
    public interface  IMemoryMap
    {
        int MtuType { get; }
        int MtuId { get; }

        Boolean Shipbit { get; }

        int DailyRead { get; }
        String DailySnap { get; }

        int MessageOverlapCount { get; }
        int ReadInterval { get; }


        int BatteryVoltage { get; }

        int MtuFirmwareVersionFormatFlag { get; }
        string MtuFirmwareVersion { get; }

        string PcbNumber { get; }

        int P1MeterType { get; }
        int P2MeterType { get; }

        ulong P1MeterId { get; }
        ulong P2MeterId { get; }

        uint P1Reading { get; }
        int P2Reading { get; }

        int P1Scaler { get; }
        int P2Scaler { get; }

    }


}
