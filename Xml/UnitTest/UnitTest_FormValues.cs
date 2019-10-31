using Library;
using System.Reflection;
using System.Xml.Serialization;

namespace Xml.UnitTest
{
    [XmlRoot("Form")]
    public class UnitTest_FormValues
    {
        [XmlElement("Entry",IsNullable=false)]
        public UnitTest_FormValue[] ListValues { get; set; }

        [XmlElement("Global",IsNullable=false)]
        public UnitTest_FormGlobal[] ListGlobals { get; set; }

        public void PrepareValues ()
        {
            // Set Library.Data values
            if ( this.ListValues != null )
                foreach ( UnitTest_FormValue field in this.ListValues )
                {
                    Data.SetTemp ( field.Id, field.Value );

                    Utils.Print ( "UnitTest - PrepareValues: " + field.Id + " = " + field.Value );
                }

            // Set Global values
            if ( this.ListGlobals != null )
            {
                Global global = Singleton.Get.Configuration.Global;
                foreach ( UnitTest_FormGlobal field in this.ListGlobals )
                    Utils.SetPropertyValue ( Singleton.Get.Configuration.Global, field.Id, field.Value );
            }
        }
    }
}
