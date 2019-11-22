using System;

namespace Library
{
    public static class MyExtensions
    {
        public static bool IsSystemException (
            this Exception exception )
        {
            return ( exception.GetType ().FullName.StartsWith ( "System." ) );
        }
    }
}
