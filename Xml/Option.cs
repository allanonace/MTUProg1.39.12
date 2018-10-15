using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    public class Option
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("required")]
        public bool Required { get; set; }

        [XmlElement("display")]
        public string Display { get; set; }

        [XmlElement("list")]
        public List<string> OptionList { get; set; }
    }
}
