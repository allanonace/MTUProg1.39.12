using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ble.net.sampleapp.Models
{

    public class DeviceItem
    {

        public string DeviceID { get; set; }
        public string MacAddress { get; set; }
        public string BatteryLevel { get; set; }
        public string RssiLevel { get; set; }
        public string imageBattery { get; set; }
        public string imageRssi { get; set; }
    }
    
   
}

