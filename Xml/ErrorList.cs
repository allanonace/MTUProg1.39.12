using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Errors")]
    public class ErrorList
    {
        [XmlElement("Error")]
        public Error[] Errors { get; set; }
    }
}
