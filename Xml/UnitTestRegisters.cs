using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Registers")]
    public class UnitTestRegisters
    {
        [XmlElement("Register")]
        public UnitTestRegister[] List { get; set; }
    }
}
