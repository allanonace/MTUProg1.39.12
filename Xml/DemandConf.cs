using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("DemandConf")]
    public class DemandConf
    {
        [XmlElement("Demand")]
        public List<Demand> Demands { get; set; }
    }
}
