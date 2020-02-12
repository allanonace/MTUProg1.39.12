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

            // Automatically detect, only the first time, the family to use with the current MTU
            string family = mtu.GetFamily ();

            #if DEBUG

            // Force some error cases in debug mode
            DebugOptions debug = config.Debug;
            if ( debug != null )
            {
                if ( debug.ForceMtu_UnknownMap )
                    family = string.Empty;
            }

            #endif

            if ( string.IsNullOrEmpty ( family ) )
                throw new MtuDoesNotBelongToAnyFamilyException ();
            else
                Utils.Print ( "Family selected: MTU " + mtu.Id + " -> " + family );
            
            iInfo = xmlInterfaces.Interfaces.Find ( x => x.Family.Equals ( family ) );

            if ( iInfo == null )
                throw new InterfaceNotFoundException_Internal ();
        }
    }
}
