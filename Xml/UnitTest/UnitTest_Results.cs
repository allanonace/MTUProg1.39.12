using System.Linq;
using System.Xml.Serialization;

namespace Xml.UnitTest
{
    [XmlRoot("Results")]
    public class UnitTest_Results
    {
        //[XmlElement("Result",IsNullable=false)]
        [XmlElement("Result")]
        public UnitTest_Result[] ListValues { get; set; }

        public void PrepareOutputs ()
        {
            if ( this.ListValues != null )
                foreach ( UnitTest_Result result in this.ListValues )
                    result.PrepareDictionary ();
        }

        public UnitTest_Result GetInterface (
            string id )
        {
            return this.ListValues.First ( result => result.InterfaceAction.Equals ( id ) );
        }
    }
}
