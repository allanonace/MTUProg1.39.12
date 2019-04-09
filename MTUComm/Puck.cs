using System;
using nexus.protocols.ble.scan;
using System.Linq;
using System.Collections.Generic;

namespace MTUComm
{
    public class Puck
    {
        private List<string> iconsBattery = new List<string> ()
        {
            "battery_toolbar_high", // >= 75
            "battery_toolbar_mid",  // >= 45 < 75
            "battery_toolbar_low",  // >= 15 < 45
            "battery_toolbar_empty" // <  15
        };
    
        private List<string> iconsRSSI = new List<string> ()
        {
            "rssi_toolbar_high", // >  -60
            "rssi_toolbar_mid",  // <= -60 > -80
            "rssi_toolbar_low",  // <= -80 > -90
            "rssi_toolbar_empty" // <= -90
        };
    
        private IBlePeripheral puck;
        
        public Puck () { }

        public Puck (
            IBlePeripheral puck )
        {
            this.puck = puck;
        }

        public IBlePeripheral Device
        {
            set { this.puck = value; }
            get { return this.puck;  }
        }
        
        public void RemovePuck ()
        {
            this.puck = null;
        }

        public int BatteryLevel
        {
            get
            {
                return this.puck.
                            Advertisement.
                            ManufacturerSpecificData.
                            ElementAt ( 0 ).Data.Skip ( 4 ).Take ( 1 ).ToArray ()[ 0 ];
            }
        }
        
        public int RSSI
        {
            get { return this.puck.Rssi; }
        }
        
        public string Name
        {
            get
            {
                if ( this.puck != null )
                    return this.puck.Advertisement.DeviceName;
                return string.Empty;
            }
        }
        
        public string SerialNumber
        {
            get
            {
                return this.DecodeId ( this.ManofacturerData );
            }
        }
        
        public string BatteryLevelIcon
        {
            get
            {
                int b = this.BatteryLevel;
                
                if      ( b >= 75 ) return this.iconsBattery[ 0 ]; // High
                else if ( b >= 45 ) return this.iconsBattery[ 1 ]; // Mid
                else if ( b >= 15 ) return this.iconsBattery[ 2 ]; // Low
                else                return this.iconsBattery[ 3 ]; // Empty
            }
        }
        
        public string RSSIIcon
        {
            get
            {
                int r = this.RSSI;
                
                if      ( r <= -90 ) return this.iconsRSSI[ 3 ]; // Empty
                else if ( r <= -80 ) return this.iconsRSSI[ 2 ]; // Low
                else if ( r <= -60 ) return this.iconsRSSI[ 1 ]; // Mid
                else                 return this.iconsRSSI[ 0 ]; // High
            }
        }

        public byte[] ManofacturerData
        {
            get
            {
                return this.puck.
                        Advertisement.
                        ManufacturerSpecificData.
                        ElementAt ( 0 ).Data.Take ( 4 ).ToArray ();
            }
        }

        private string DecodeId (
            byte[] id )
        {
            string s;
            try
            {
                s = System.Text.Encoding.ASCII.GetString(id.Take(2).ToArray());
                byte[] byte_aux = new byte[4];
                byte_aux[0] = id[3];
                byte_aux[1] = id[2];
                byte_aux[2] = 0;
                byte_aux[3] = 0;
                int num= BitConverter.ToInt32(byte_aux, 0);
                s += num.ToString("0000");
            }
            catch (Exception)
            {
                s = BitConverter.ToString(id);
            }
            return s;
        }
        
        public string GetProperty (
            string id )
        {
            return this.GetType ().GetProperty ( id ).GetValue ( this, null ).ToString ();
        }
    }
}
