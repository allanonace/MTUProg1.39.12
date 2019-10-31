using System.Xml.Serialization;

namespace Xml.UnitTest
{
    public class UnitTest_FormValue
    {
        [XmlAttribute("port")]
        public string PortString { get; set; }

        public int Port
        {
            get
            {
                if ( ! string.IsNullOrEmpty ( this.PortString ) )
                    return int.Parse ( this.PortString );
                return 0;
            }
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
