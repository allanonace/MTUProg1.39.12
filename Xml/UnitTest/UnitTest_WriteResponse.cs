using System.Xml.Serialization;

namespace Xml.UnitTest
{
    public class UnitTest_WriteResponse
    {
        // NOTE: Numbers separated by spaces ( e.g. 23 46 1 152 45 2 54... )

        private string type;
        private string input;
        private string output;

        public bool IsRead
        {
            get { return this.type.ToLower ().Equals ( "read" ); }
        }

        public bool IsWrite
        {
            get { return ! this.type.ToLower ().Equals ( "read" ); }
        }

        [XmlAttribute("type")]
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [XmlAttribute("input")]
        public string Input
        {
            get { return this.input; }
            set { this.input = value.ToUpper (); }
        }

        [XmlAttribute("output")]
        public string Output
        {
            get { return this.output; }
            set { this.output = value.ToUpper (); }
        }
    }
}
