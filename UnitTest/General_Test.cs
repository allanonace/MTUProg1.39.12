using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using System.IO;
using Xml;
using Library;

namespace UnitTest
{
    public class General_Test
    {
        private const string FOLDER = "Aclara_Test_Files";

        private string GetPath ( string subfolder )
        {
            return Path.Combine( Environment.GetFolderPath ( Environment.SpecialFolder.Desktop ), FOLDER, subfolder );
        }

        [Fact]
        public void TestGlobal()
        {
            Global g = Utils.DeserializeXml<Global>( GetPath ( "global.xml" ) );
            Utils.Print ( g.LexiAttempts + " | " + g.LexiTimeout );
        }
    }
}
