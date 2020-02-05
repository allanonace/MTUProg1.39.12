using System.Collections.Generic;

namespace Xml
{
    public class MtuAux
    {
        public static bool GetPortTypes (
            string portType, // From the mtu.xml
            out List<string> portTypes )
        {
            portTypes = new List<string>();

            bool isNumeric = int.TryParse ( portType, out int portTypeNumber );

            // Meter ID ( i.e. "3101" )
            if ( isNumeric )
            {
                portTypes.Add ( portType );
                return true;
            }
            // Multiple meter IDs (i.e. "3101|3102|3103")
            else if ( portType.Contains ( "|" ) )
            {
                portTypes.AddRange ( portType.Split ( '|' ) );
                return true;
            }
            // Predefined string
            else if ( IsPredefinedType ( portType ) )
                portTypes.Add ( portType );
            // String
            else
                foreach ( char c in portType )
                    portTypes.Add ( c.ToString () );

            return false;
        }

        public static bool IsPredefinedType ( string type )
        {
            return type.Equals("s4k") ||
                   type.Equals("4kl") ||
                   type.Equals("gut") ||
                   type.Equals("ch4");
        }
    }
}
