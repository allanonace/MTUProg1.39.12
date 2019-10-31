using System;
using Library;
using Library.Exceptions;
using Xml;
using System.Linq;

namespace MTUComm
{
    public class InterfaceAux
    {
        private static Interface iInfo;

        public static void ResetInfo ()
        {
            iInfo = null;
        }

        public static string GetmemoryMapTypeByMtuId (
            Mtu mtu )
        {
            if ( iInfo == null )
                GetInterfaceBytMtuId ( mtu );

            return iInfo.Family;
        }

        public static int GetmemoryMapSizeByMtuId (
            Mtu mtu )
        {
            if ( iInfo == null )
                GetInterfaceBytMtuId ( mtu );

            return iInfo.MemorymapSize;
        }

        private static void GetInterfaceBytMtuId (
            Mtu mtu )
        {
            Configuration   config        = Singleton.Get.Configuration;
            InterfaceConfig xmlInterfaces = config.Interfaces;
            Port            port1         = mtu.Ports[ 0 ];
            string family;

            if ( ! mtu.HasFamilySet )
            {
                // Gas MTUs of family 33xx should use family 31xx32xx memorymap
                if ( mtu.IsFamily33xx &&
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
                    catch ( Exception )
                    {
                        throw new InterfaceNotFoundException_Internal ();
                    }
                    
                    family = iInfoMtu.Family;
                }

                mtu.SetFamily ( family );
            }
            else
            {
                family = mtu.GetFamily ();
            }

            if ( iInfo == null )
                iInfo = xmlInterfaces.Interfaces.Find ( x => x.Family.Equals ( family ) );

            if ( iInfo == null )
                throw new InterfaceNotFoundException_Internal ();
        }
    }
}
