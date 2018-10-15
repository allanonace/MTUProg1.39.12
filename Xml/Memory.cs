using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{

    public class Memory
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("description")]
        public String Description { get; set; }

        [XmlElement("Param")]
        public List<Param> Params { get; set; }
    }
}
