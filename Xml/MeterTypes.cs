using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    /// <summary>
    /// Class used to map the Meter.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="MeterTypes">
    /// <item>
    ///   <term>FileVersion</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>FileDate</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Meters</term>
    ///   <description>List of <see cref="Meter"/> entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("MeterTypes")]
    public class MeterTypes
    {
        [XmlElement("FileVersion")]
        public string FileVersion { get; set; }

        [XmlElement("FileDate")]
        public string FileDate { get; set; }

        [XmlElement("Meter")]
        public List<Meter> Meters { get; set; }

        public bool ContainsNumericType (
            string number )
        {
            int numInt;
            if ( ! int.TryParse ( number, out numInt ) )
                return false;

            return this.Meters.Any ( meter => meter.Type.Equals ( number ) );
        }

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

        public List<Meter> FindByEncoderTypeAndLiveDigits (
            byte encoderType,
            int  liveDigits )
        {
            // Filter by protocol and livedigits
            List<Meter> meters = Meters.FindAll ( x => (
                x.EncoderType == encoderType &&
                x.LiveDigits  == liveDigits
            ));
            
            // Filter only by protocol
            if ( meters.Count == 0 )
                meters = Meters.FindAll ( x => (
                    x.EncoderType == encoderType
                ));

            return meters;
        }
        
        public List<Meter> FindAllForEncodersAndEcoders ()
        {
            return Meters.FindAll ( meter => meter.IsForEncoderOrEcoder ); // Type "E"
        }

        public List<Meter> FindByDialDescription (
            int    NumberOfDials,
            int    DriveDialSize,
            string UnitOfMeasure,
            int    mtuFlow )
        {
            List<Meter> meters = Meters.FindAll ( x => (
                x.NumberOfDials == NumberOfDials &&
                x.DriveDialSize == DriveDialSize &&
                x.UnitOfMeasure.Equals ( UnitOfMeasure ) &&
                x.MeterTypeFlow == ( ( mtuFlow == 1 ) ? "Neg" : "Pos" )
            ));

            if ( meters == null )
                return new List<Meter> ();

            return meters;
        }

        public List<Meter> FindByPortTypeAndFlow (
            Mtu mtu,
            int portNumber = 0 )
        {
            int  flow = mtu.Flow;
            Port port = mtu.Ports[ portNumber ];
        
            // The port type is required to be supported but the flow can be differente
            // and maybe the Mtu port has only certain Meter IDs supported
            return Meters.FindAll ( meter => port.IsThisMeterSupported ( meter ) &&
                                             ( ( flow <= -1 ) || meter.Flow.Equals ( flow ) ) );
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
