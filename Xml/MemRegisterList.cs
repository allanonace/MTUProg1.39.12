using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Registers")]
    public class MemRegisterList
    {
        [XmlElement("Register")]
        public MemRegister[] Registers { get; set; }
    }
}
