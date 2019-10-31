using System.Xml.Serialization;

namespace Xml.UnitTest
{
    public class UnitTest_FormGlobal
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
