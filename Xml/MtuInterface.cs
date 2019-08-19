using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    public class MtuInterface
    {
        [XmlAttribute("family")]
        public string Family { get; set; }

        [XmlElement("Mtu")]
        public List<MtuID> MtuIDs { get; set; }
    }
}
