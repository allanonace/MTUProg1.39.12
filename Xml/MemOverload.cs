using System.Xml.Serialization;

namespace Xml
{
    public sealed class MemOverload
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlArray("Registers")]
        [XmlArrayItem("Register", typeof(MemOverloadRegister))]
        public MemOverloadRegister[] Registers { get; set; }

        [XmlElement("Operation")]
        public string Operation { get; set; }

        [XmlElement("Method")]
        public string Method { get; set; }
    }
}
