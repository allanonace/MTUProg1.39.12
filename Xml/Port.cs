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
        public string Tamper { get; set; }
    }
}
