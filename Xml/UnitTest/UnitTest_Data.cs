using Library;
using System.IO;
using System.Linq;

namespace Xml.UnitTest
{
    public class UnitTest_Data
    {
        public readonly UnitTest_DumpMemoryMap  MemoryMap;
        public readonly UnitTest_FormValues     Form;
        public readonly UnitTest_WriteResponses WriteResponses;
        public readonly UnitTest_Results        Results;

        public UnitTest_Data (
            string folder )
        {
            string prefix = folder.Split ( new char[]{ '/' } ).Last ();

            this.MemoryMap      = Utils.DeserializeXml<UnitTest_DumpMemoryMap>  ( Path.Combine ( folder, prefix + "_MemoryDump.xml"     ) );
            this.Form           = Utils.DeserializeXml<UnitTest_FormValues>     ( Path.Combine ( folder, prefix + "_Form.xml"           ) );
            this.WriteResponses = Utils.DeserializeXml<UnitTest_WriteResponses> ( Path.Combine ( folder, prefix + "_WriteResponses.xml" ) );
            this.Results        = Utils.DeserializeXml<UnitTest_Results>        ( Path.Combine ( folder, prefix + "_Results.xml"        ) );

            // Prepare output dictionary
            this.Results.PrepareOutputs ();
        }

        public void PrepareValues ()
        {
            // Prepare values to use from Library.Data and Global
            this.Form.PrepareValues ();
        }
    }
}
