using System;
using System.Text.RegularExpressions;

namespace Library
{
    public static class MyExtensions
    {
        public static bool IsSystemException (
            this Exception exception )
        {
            return ( exception.GetType ().FullName.StartsWith ( "System." ) );
        }

        public static string GetValue (
            this Match match,
            string tag )
        {
            string value = string.Empty;

            try
            {
                if ( match.Groups.Count > 0 )
                    value = match.Groups[ tag ].Value;
            }
            catch ( Exception ) { }

            return value;
        }

        public static bool IsValueNull (
            this Match match,
            string tag )
        {
            return string.IsNullOrEmpty ( match.GetValue ( tag ) );
        }
    }
}
