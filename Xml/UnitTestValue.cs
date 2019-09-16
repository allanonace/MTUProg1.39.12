using System.Xml.Serialization;

namespace Xml
{
    public class UnitTestValue
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
