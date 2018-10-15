using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{

    public class MtuMemory
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("memory")]
        public int Memory { get; set; }
    }
}
