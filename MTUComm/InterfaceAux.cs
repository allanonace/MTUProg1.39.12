using System.Collections.Generic;
using Library;
using Library.Exceptions;
using Xml;

namespace MTUComm
{
    public class InterfaceAux
    {
        public static string GetmemoryMapTypeByMtuId (
            Mtu mtu )
        {
            Interface iInfo = GetInterfaceBytMtuId ( mtu );

            return iInfo.Memorymap;
        }

        public static int GetmemoryMapSizeByMtuId (
            Mtu mtu )
        {
            Interface iInfo = GetInterfaceBytMtuId ( mtu );

            return iInfo.MemorymapSize;
        }

        private static Interface GetInterfaceBytMtuId (
            Mtu mtu )
        {
            Configuration   config        = Singleton.Get.Configuration;
            InterfaceConfig xmlInterfaces = config.interfaces;
            Port            port1         = mtu.Ports[ 0 ];

            int interfaceIndex;

            // Gas MTUs of family 33xx should use family 31xx32xx memorymap
            if ( mtu.IsFamilly33xx &&
                 mtu.Port1.IsForPulse &&
                 ( port1.TypeString.Contains ( "M" ) ||
                   port1.TypeString.Contains ( "R" ) ||
                   ( ! port1.IsSpecialCaseNumType &&
                     port1.HasCertainMeterIds     &&
                     config.getMeterTypeById ( int.Parse ( port1.CertainMeterIds[ 0 ] ) ).IsForGas ) ) )
            {
                 interfaceIndex = 1; // Family 31xx32xx
            }
            else
            {
                MtuInterface iInfoMtu = xmlInterfaces.MtuInterfaces.Find ( x => x.Id == mtu.Id );

                if ( iInfoMtu == null )
                    throw new InterfaceNotFoundException_Internal ();
                else
                    interfaceIndex = iInfoMtu.Interface;
            }

            Interface iInfo = xmlInterfaces.Interfaces.Find(x => x.Id == interfaceIndex );

            if ( iInfo == null )
                throw new InterfaceNotFoundException_Internal ();
            
            InterfaceConfig.currentIndexType = interfaceIndex;
            
            return iInfo;
        }
    }
}
