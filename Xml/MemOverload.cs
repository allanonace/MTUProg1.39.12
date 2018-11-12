using System.Xml.Serialization;

namespace Xml
{
    public sealed class MemOverload
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Registers")]
        [XmlArrayItem("Register")]
        public MemOverloadRegister[] Registers { get; set; }

        [XmlElement("Custom")]
        public string Custom { get; set; }
    }
}
