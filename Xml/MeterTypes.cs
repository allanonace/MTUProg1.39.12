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

        public List<Meter> FindByPortTypeAndFlow(string portType, int flow)
        {
            List<string> portTypes = new List<string>();
            bool findInMeterId = false;
            switch (portType)
            {
                case "E":
                case "RW":
                case "M":
                case "S4K":
                case "4KL":
                case "GUT":
                case "R":
                case "G":
                case "P":
                case "T":
                case "W":
                case "I":
                case "K":
                case "L":
                case "B":
                case "CH4":
                    portTypes.Add(portType);
                    break;
                default:
                    findInMeterId = true; // numeric port type
                    try
                    {
                        UInt32.Parse(portType);
                        portTypes.Add(portType); // only one meter type (i.e. "3101")
                    }
                    catch (Exception e)
                    {
                        portTypes.AddRange(portType.Split('|')); // multiple meter types (i.e. "3101|3102|3103")
                    }
                    break;
            }
            List<Meter> meters;
            if (findInMeterId)
            {
                meters = Meters.FindAll(m => portTypes.Contains(m.Id.ToString()) && m.Flow.Equals(flow));
            }
            else
            {
                meters = Meters.FindAll(m => portTypes.Contains(m.Type) && m.Flow.Equals(flow));
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
