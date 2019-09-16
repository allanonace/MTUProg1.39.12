using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Registers")]
    public class UnitTestRegisters
    {
        [XmlElement("Register")]
        public UnitTestRegister[] ListRegisters { get; set; }

        [XmlElement("Value")]
        public UnitTestValue[] ListValues { get; set; }
    }
}
