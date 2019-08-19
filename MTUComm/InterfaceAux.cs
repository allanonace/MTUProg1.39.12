using System;
using Library;
using Library.Exceptions;
using Xml;
using System.Linq;

namespace MTUComm
{
    public class InterfaceAux
    {
        public static string GetmemoryMapTypeByMtuId (
            Mtu mtu )
        {
            Interface iInfo = GetInterfaceBytMtuId ( mtu );

            return iInfo.Family;
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

            string family;

            // Gas MTUs of family 33xx should use family 31xx32xx memorymap
            if ( mtu.IsFamilly33xx &&
                 mtu.Port1.IsForPulse &&
                 ( port1.TypeString.Contains ( "M" ) ||
                   port1.TypeString.Contains ( "R" ) ||
                   ( ! port1.IsSpecialCaseNumType &&
                     port1.HasCertainMeterIds     &&
                     config.getMeterTypeById ( int.Parse ( port1.CertainMeterIds[ 0 ] ) ).IsForGas ) ) )
            {
                 family = "31xx32xx";
            }
            else
            {
                string mtuId = mtu.Id.ToString ();
                MtuInterface iInfoMtu;
                try
                {
                    iInfoMtu = xmlInterfaces.MtuInterfaces
                        .Select ( intf => new MtuInterface
                        {
                            Family = intf.Family,
                            MtuIDs = intf.MtuIDs
                                .Where ( m => m.ID.Equals ( mtuId ) ).ToList ()
                        })
                        .First ( intf => intf.MtuIDs.Count > 0 );
                }
                catch ( Exception e )
                {
                    throw new InterfaceNotFoundException_Internal ();
                }
                
                family = iInfoMtu.Family;
            }

            Interface iInfo = xmlInterfaces.Interfaces.Find ( x => x.Family.Equals ( family ) );

            if ( iInfo == null )
                throw new InterfaceNotFoundException_Internal ();
            
            InterfaceConfig.currentFamily = family;
            
            return iInfo;
        }
    }
}
