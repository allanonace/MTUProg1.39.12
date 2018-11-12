using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("MeterTypes")]
    public class MeterTypes
    {
        [XmlElement("FileVersion")]
        public string FileVersion { get; set; }

        [XmlElement("FileDate")]
        public string FileDate { get; set; }

        [XmlElement("Meter")]
        public List<Meter> Meters { get; set; }

        public Meter FindByMterId(int meterId)
        {
            Meter meter = Meters.Find(x => x.Id == meterId);
            if (meter == null)
            {
                throw new MeterNotFoundException("Meter not found");
            }
            return meter;
        }

        public List<Meter> FindByEncoderTypeAndLiveDigits(int encoderType, int liveDigits)
        {
            List<Meter> meters = Meters.FindAll(x => (x.EncoderType == encoderType && x.LiveDigits == liveDigits));
            if (meters == null)
            {
                throw new MeterNotFoundException("Meter not found");
            }
            return meters;
        }

        public List<Meter> FindByPortTypeAndFlow(string portType, int flow = -1)
        {
            List<string> portTypes = new List<string>();
            bool findInMeterId = false;

            var isNumeric = int.TryParse(portType, out int portTypeNumber);

            if (isNumeric)
            {
                portTypes.Add(portType); // "3101"
                findInMeterId = true;
            }
            else if(portType.Equals("S4K") || portType.Equals("4KL") || portType.Equals("GUT") || portType.Equals("CH4"))
            {
                portTypes.Add(portType); // "S4K", "4KL", "GUT", "CH4"
            }
            else if (portType.Contains("|"))
            {
                portTypes.AddRange(portType.Split('|')); // multiple meter types (i.e. "3101|3102|3103")
                findInMeterId = true;
            }
            else
            {
                foreach (char c in portType)
                {
                    portTypes.Add(c.ToString());
                }
            }
            List<Meter> meters;
            if (findInMeterId)
            {
                if (flow > -1)
                {
                    meters = Meters.FindAll(m => portTypes.Contains(m.Id.ToString()) && m.Flow.Equals(flow));
                }
                else
                {
                    meters = Meters.FindAll(m => portTypes.Contains(m.Id.ToString()));
                }
            }
            else
            {
                if (flow > -1)
                {
                    meters = Meters.FindAll(m => portTypes.Contains(m.Type) && m.Flow.Equals(flow));
                }
                else
                {
                    meters = Meters.FindAll(m => portTypes.Contains(m.Type));
                }
            }
            return meters;
        }

        public List<string> GetVendorsFromMeters(List<Meter> meters)
        {
            HashSet<string> vendors = new HashSet<string>();
            meters.ForEach(m => vendors.Add(m.Vendor));

            return vendors.ToList();
        }

        public List<string> GetModelsByVendorFromMeters(List<Meter> meters, string vendor)
        {
            HashSet<string> models = new HashSet<string>();

            List<Meter> filteredMeters = meters.FindAll(x => x.Vendor == vendor);

            filteredMeters.ForEach(m => models.Add(m.Model));

            return models.ToList();
        }

        public List<string> GetNamesByModelAndVendorFromMeters(List<Meter> meters, string vendor, string model)
        {
            HashSet<string> names = new HashSet<string>();

            List<Meter> filteredMeters = meters.FindAll(x => (x.Vendor == vendor && x.Model == model));

            filteredMeters.ForEach(m => names.Add(m.Display));

            return names.ToList();
        }


        public List<Meter> GetMetersByModelAndVendorFromMeters(List<Meter> meters, string vendor, string model)
        {
            HashSet<string> names = new HashSet<string>();

            List<Meter> filteredMeters = meters.FindAll(x => (x.Vendor == vendor && x.Model == model));
            return filteredMeters;

        }
    }
}
