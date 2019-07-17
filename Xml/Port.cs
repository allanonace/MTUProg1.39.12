using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Xml
{
    public class Port
    {
        public Port ()
        {
            this.CertainMeterIds = new List<string> ();
        }

        [XmlAttribute("Number")]
        public int Number { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
 
        // Info loaded from mtu.xml, within port region of each <Mtu> entry
        // and can be a string ( e.g. MR ), a number or a list of numbers/ids ( x|x )
        // and special cases that are numeric Meter types ( e.g. <Type>122</Type> )
        // This property is only used during the preloading process in Configuration
        [XmlElement("Type")]
        public string Type { get; set; }
        
        // Set in Configuration constructor to know exactly the port type
        // of each Mtu entry, not the Meter ID nor a list of numbers/ids
        [XmlIgnore]
        public string TypeString { get; set; }

        [XmlIgnore]
        public bool IsSpecialCaseNumType { get; set; }

        public bool IsThisMeterSupported (
            Meter meter )
        {
            return ( ( ! this.IsSpecialCaseNumType && // Type as string, comparing characters
                       this.TypeString.ToList ().Intersect ( meter.Type.ToList () ).Count () > 0 ||
                       this.IsSpecialCaseNumType && // Type as number
                       this.TypeString.Equals ( meter.Type ) ) &&
                     ( ! this.HasCertainMeterIds ||
                       this.CertainMeterIds.Contains ( meter.Id.ToString () ) ) );
        }

        // Set in Configuration constructor to know if only some Meters are supported,
        // because the port type of the Mtu entry has certain IDs listed
        [XmlIgnore]
        public List<string> CertainMeterIds { get; set; }

        [XmlIgnore]
        public bool HasCertainMeterIds
        {
            get { return this.CertainMeterIds.Count > 0; }
        }

        [XmlIgnore]
        public bool IsForEncoderOrEcoder
        {
            get { return this.TypeString.Equals ( "E" ); }
        }

        [XmlIgnore]
        public bool IsForPulse
        {
            get { return ! this.TypeString.Equals ( "E" ); }
        }
        
        [XmlIgnore]
        public int MeterProtocol { get; set; }
        
        [XmlIgnore]
        public int MeterLiveDigits { get; set; }

        [XmlElement("Tamper")]
        public string TamperSerialize { get; set; }

        [XmlIgnore]
        public byte Tamper
        {
            get
            {
                byte tamper = 0;
                if ( TamperSerialize.StartsWith("int") )
                    tamper = 4;

                else if ( TamperSerialize.StartsWith("ext") )
                    tamper = 8;

                return tamper;
            }
        }

        public String GetProperty (
            String Name )
        {
            return this.GetType().GetProperty(Name).GetValue(this, null).ToString();
        }
    }
}
