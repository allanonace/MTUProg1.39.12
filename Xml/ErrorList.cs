using System.Xml.Serialization;
using Library;

namespace Xml
{
    [XmlRoot("Errors")]
    public class ErrorList
    {
        [XmlElement("Error")]
        public Error[] List { get; set; }
    }
}
