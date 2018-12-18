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
                meter = new Meter();
                meter.Id = meterId;
                meter.Display = "Not Installed";
                meter.Type = "NOTFOUND";
                return meter;

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

        public List<Meter> FindByDialDescription(int NumberOfDials, int DriveDialSize, int UnitOfMeasure, int flow)
        {
            List<Meter> meters = Meters.FindAll(x => (
                x.NumberOfDials.Equals(NumberOfDials.ToString()) &&
                x.DriveDialSize.Equals(DriveDialSize.ToString()) &&
                x.UnitOfMeasure.Equals(UnitOfMeasure.ToString()) &&
                x.Flow == flow
                ));
            if (meters == null)
            {
                throw new MeterNotFoundException("Meter not found");
            }
            return meters;
        }


        public List<Meter> FindByPortTypeAndFlow(string portType, int flow = -1)
        {
            List<string> portTypes;
            List<Meter> meters;

            bool isNumeric = GetPortTypes(portType, out portTypes);

            if (isNumeric)
            {
                // portType match and no flow match required
                // OR
                // portType match and flow match
                meters = Meters.FindAll(m => portTypes.Contains(m.Id.ToString()) && ((flow <= -1) || m.Flow.Equals(flow)));
            }
            else
            {
                // portType match and no flow match required
                // OR
                // portType match and flow match
                meters = Meters.FindAll(m => MatchMeterTypes(portTypes, m) && ((flow <= -1) || m.Flow.Equals(flow)));
            }
            return meters;
        }

        private bool GetPortTypes(string portType, out List<string> portTypes)
        {
            portTypes = new List<string>();

            bool isNumeric = int.TryParse(portType, out int portTypeNumber);

            if (isNumeric)
            {
                portTypes.Add(portType); // "3101"
                return true; // numeric
            }
            else if (IsStringPortType(portType))
            {
                portTypes.Add(portType); // "S4K", "4KL", "GUT", "CH4"
            }
            else if (portType.Contains("|"))
            {
                portTypes.AddRange(portType.Split('|')); // multiple meter types (i.e. "3101|3102|3103")
                return true; // numeric
            }
            else
            {
                foreach (char c in portType)
                {
                    portTypes.Add(c.ToString());
                }
            }
            return false; // string or char
        }

        private bool MatchMeterTypes(List<string> mtuMeterTypes, Meter meter) //List<string> meterTypes)
        {
            List<string> meterTypes = new List<string>();

            if (IsStringPortType(meter.Type))
            {
                meterTypes.Add(meter.Type);
            }
            else
            {
                foreach (char c in meter.Type)
                {
                    meterTypes.Add(c.ToString());
                }
            }

            IEnumerable<string> r = mtuMeterTypes.Intersect(meterTypes);
            bool match = r.Count() > 0;
            return match;
        }

        private bool IsStringPortType(string portType)
        {
            return portType.Equals("S4K") || portType.Equals("4KL") || portType.Equals("GUT") || portType.Equals("CH4");
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
