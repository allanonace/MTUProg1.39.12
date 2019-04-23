using System;
using System.Text;

namespace Library
{
    public class Utils
    {
        private static bool DEEP_MODE = false;
    
        public static void PrintDeep (
            object element,
            bool   newLine = true )
        {
            if ( DEEP_MODE )
                Print ( element, newLine );
        }
    
        public static void Print (
            object element,
            bool   newLine = true )
        {
            #if DEBUG
            
            if ( newLine )
                 Console.WriteLine ( element.ToString () );
            else Console.Write ( element.ToString () );
            
            #endif
        }
        
        public static string ByteArrayToString (
            byte[] bytes )
        {
            StringBuilder hex = new StringBuilder ( bytes.Length * 2 );
            foreach ( byte b in bytes )
                hex.AppendFormat ( "{0:x2} ", b );
            
            return hex.ToString ().Substring ( 0, hex.Length - 1 );
        }
    }
}
