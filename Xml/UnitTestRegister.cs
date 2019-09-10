using System.Xml.Serialization;

namespace Xml
{
    public class UnitTestRegister
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("readmtu")]
        public string ReadFromMTU { get; set; }

        [XmlAttribute("used")]
        public string Used { get; set; }
    }
}
