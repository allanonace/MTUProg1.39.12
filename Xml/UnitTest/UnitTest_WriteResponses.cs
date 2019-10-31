using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml.UnitTest
{
    [XmlRoot("Responses")]
    public class UnitTest_WriteResponses
    {
        [XmlElement("Response")]
        public List<UnitTest_WriteResponse> ListRegisters { get; set; }
    }
}
