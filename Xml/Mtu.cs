using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    public class Mtu
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlElement("HexNum")]
        public string HexNum { get; set; }
 
        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("Port")]
        public List<Port> Ports { get; set; }
    }
}
