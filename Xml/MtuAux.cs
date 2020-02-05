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
                portTypes.Add ( portType.ToLower () );
            // String
            else
                foreach ( char c in portType )
                    portTypes.Add ( c.ToString ().ToLower());

            return false;
        }

        // FIXME: This method can be removed and treath all Meter typers in the same way in GetPortTypes, only working with full strings
        // FIXME: but for the moment it is useful to have this method, to avoid all string except setflow, because we don't know the utility of the rest
        public static bool IsPredefinedType (
            string type )
        {
            type = type.ToLower ();

            return type.Equals ( "s4k" ) || // e.g. MTU 168 HexNum 4291-065-MBS2W
                   type.Equals ( "4kl" ) || // 
                   type.Equals ( "gut" ) || // e.g. MTU 102 HexNum 501-2009-002
                   type.Equals ( "ch4" ) || // e.g. MTU 169 HexNum 4221-079-YBS2W
                   type.Equals ( "setflow" );
        }
    }
}
