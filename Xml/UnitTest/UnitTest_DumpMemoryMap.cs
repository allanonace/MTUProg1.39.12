using System.Xml.Serialization;

namespace Xml.UnitTest
{
    [XmlRoot("MemoryDump")]
    public class UnitTest_DumpMemoryMap
    {
        [XmlElement("Register")]
        public UnitTest_Register[] ListRegisters { get; set; }
    }
}
