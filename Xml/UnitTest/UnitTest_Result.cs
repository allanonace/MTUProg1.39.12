using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Xml.UnitTest
{
    public class UnitTest_Result
    {
        private List<KeyValuePair<string,UnitTest_Output>> dictionary;

        public dynamic this[ string id ]
        {
            get
            {
                int index = this.dictionary.FindIndex ( o => o.Key.Equals ( id ) );
                if ( index >= 0 )
                {
                    // Allows multiple items with the same identifier and each time one is used, it is also removed
                    UnitTest_Output output = this.dictionary.ElementAt ( index ).Value;
                    this.dictionary.RemoveAt ( index );

                    return output;
                }

                return null;
            }
        }
        
        [XmlAttribute("interface")]
        public string InterfaceAction { get; set; }

        [XmlElement("Output")]
        public UnitTest_Output[] Outputs { get; set; }

        public void PrepareDictionary ()
        {
            try
            {
                this.dictionary = new List<KeyValuePair<string,UnitTest_Output>> ();
                foreach ( UnitTest_Output output in this.Outputs )
                    this.dictionary.Add ( new KeyValuePair<string, UnitTest_Output> ( output.Id, output ) );
            }
            catch ( Exception )
            {
                // only for testing
            }
        }
    }
}
