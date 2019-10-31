using System;
using System.Xml.Serialization;

namespace Xml.UnitTest
{
    public class UnitTest_Register
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("bytes")]
        public string Bytes { get; set; }

        public byte[] GetBytes ()
        {
            string[] outputXml = this.Bytes.Trim ().Split ( new char[]{ ' ' } );
            byte[] ar = new byte[ outputXml.Length ];
            for ( int i = 0; i < ar.Length; i++ )
                ar[ i ] = Convert.ToByte ( outputXml[ i ], 16 );

            Console.WriteLine ( "UnitTest - GetBytes: " + BitConverter.ToString ( ar ).Replace ( "-", " " ) );

            return ar;
        }
    }
}
