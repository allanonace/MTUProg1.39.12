using System.Xml.Serialization;

namespace Xml
{
    public class MtuID
    {
        [XmlAttribute("id")]
        public string ID { get; set; }
    }
}
