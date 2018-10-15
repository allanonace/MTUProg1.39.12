using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Param")]
    public class Param
    {

        [XmlAttribute("name")]
        public String Name { get; set; }

        [XmlAttribute("description")]
        public String Description { get; set; }

        [XmlAttribute("address")]
        public int Address { get; set; }

        [XmlAttribute("length")]
        public int Length { get; set; }

        [XmlAttribute("readonly")]
        public bool Readonly { get; set; }

    }
}
