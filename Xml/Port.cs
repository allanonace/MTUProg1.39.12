using System;
using System.Xml.Serialization;

namespace Xml
{
    public class Port
    {
        [XmlAttribute("Number")]
        public int Number { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
 
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Tamper")]
        public string TamperSerialize { get; set; }

        [XmlIgnore]
        public byte Tamper
        {
            get
            {
                byte tamper = 0;
                if (TamperSerialize.StartsWith("int"))
                {
                    tamper = 4;
                }
                else if (TamperSerialize.StartsWith("ext"))
                {
                    tamper = 8;
                }
                else
                {
                    // do nothing
                }
                return tamper;
            }
        }
    }
}
