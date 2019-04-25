using System;
using nexus.protocols.ble.scan;
using System.Linq;
using System.Collections.Generic;
using ble_library;
using nexus.protocols.ble.scan.advertisement;

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
        private BleSerial blSerial;

        public BleSerial BlInterfaz
        {
            set { this.blSerial = value; }
        }

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
        public int BatteryLevelFix
        {
            get
            {
                int posEnd = this.puck.Advertisement.ManufacturerSpecificData.Count()-1;
                int batt= this.puck.
                        Advertisement.
                        ManufacturerSpecificData.
                        ElementAt(posEnd).Data.Skip(4).Take(1).ToArray()[0];
                Console.Write($"******************* Fix Serial number: {SerialNumber} - Bateria: {batt.ToString()}" + Environment.NewLine);
                return BatteryRound(batt);
            }
        }
        public int BatteryLevel
        {
            get
            {
                int batt = BatteryLevelFix;

                int battSerial = this.blSerial == null?-1:this.blSerial.GetBatteryLevel().
                        Take(1).ToArray()[0];
                if (battSerial >= 0 && battSerial <= 100 && battSerial <= batt)
                    batt = battSerial;
                Console.Write($"******************* Refresh Serial number: {SerialNumber} - Bateria: {batt.ToString()}" + Environment.NewLine);

                return BatteryRound(batt);
            }
        }

        private int BatteryRound(int batt)
        {

            if (batt >= 91) batt = 100;
            else if (batt >= 81) batt = 90;
            else if (batt >= 71) batt = 80;
            else if (batt >= 61) batt = 70;
            else if (batt >= 51) batt = 60;
            else if (batt >= 41) batt = 50;
            else if (batt >= 31) batt = 40;
            else if (batt >= 21) batt = 30;
            else if (batt >= 11) batt = 20;
            else batt = 10;

            Console.Write($"******************* ***************************** - Bateria: {batt.ToString()}" + Environment.NewLine);

            return batt;
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
        public string BatteryLevelIconFix
        {
            get
            {
                int b = this.BatteryLevelFix;

                if (b >= 75) return this.iconsBattery[0]; // High
                else if (b >= 45) return this.iconsBattery[1]; // Mid
                else if (b >= 15) return this.iconsBattery[2]; // Low
                else return this.iconsBattery[3]; // Empty
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
