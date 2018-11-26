using System;
using System.Linq;

namespace MTUComm
{
    public sealed class Validations
    {
        public static bool IsNumeric<T> ( dynamic value )
        {
            // 1. Direct validation
            if ( value is Int32  ||
                 value is UInt32 ||
                 value is UInt64 )
                return true;

            // 2. String conversion for used types
            if ( value is string )
            {
                string valueString = ( string )( object )value;
                switch ( Type.GetTypeCode( typeof(T)) )
                {
                    case TypeCode.Int32:
                        int valueInt = 0;
                        if ( ! int.TryParse ( valueString, out valueInt ) )
                            return false;
                        return true;
                    
                    case TypeCode.UInt32:
                        uint valueUInt = 0;
                        if ( ! uint.TryParse ( valueString, out valueUInt ) )
                            return false;
                        return true;

                    case TypeCode.UInt64:
                        ulong valueULong = 0;
                        if ( ! ulong.TryParse ( valueString, out valueULong ) )
                            return false;
                        return true;
                }
            }

            // 3. Validation for other numeric types
            string chars = value.ToString ();
            if ( char.Equals ( chars[ 0 ], "-" ) )
                chars = chars.Remove ( 0, 1 );

            // NOTA: No funciona con numeros negativos
            return chars.All ( c => char.IsDigit ( c ) );
        }

        public static bool NumericBytesLimit<T> ( dynamic value, int numBytes )
            where T : struct
        {
            if ( ! IsNumeric<T> ( value ) )
                return false;

            bool isString = ( value is string );

            switch ( Type.GetTypeCode( typeof(T)) )
            {
                case TypeCode.Int32:
                    int valueInt = 0;
                    if ( isString )
                    {
                        if ( ! int.TryParse ( value, out valueInt ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueInt = Convert.ToInt32 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueInt < ( int )Math.Pow ( 2, numBytes * 8 ) );

                case TypeCode.UInt32:
                    uint valueUInt = 0;
                    if ( isString )
                    {
                        if ( ! uint.TryParse ( value, out valueUInt ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueUInt = Convert.ToUInt32 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueUInt < ( uint  )Math.Pow ( 2, numBytes * 8 ) );

                case TypeCode.UInt64:
                    ulong valueULong = 0;
                    if ( isString )
                    {
                        if ( ! ulong.TryParse ( value, out valueULong ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueULong = Convert.ToUInt64 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueULong < ( ulong )Math.Pow ( 2, numBytes * 8 ) );
            }
            return false;
        }

        public static bool NumericTypeLimit<T> ( dynamic value )
            where T : struct
        {
            if ( ! IsNumeric<T> ( value ) )
                return false;

            bool isString = ( value is string );

            switch ( Type.GetTypeCode( typeof(T)) )
            {
                case TypeCode.Int32 :
                    int valueInt = 0;
                    if ( isString )
                    {
                        if ( ! int.TryParse ( value, out valueInt ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueInt = Convert.ToInt32 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueInt >= Int32.MinValue && valueInt <= Int32 .MaxValue );

                case TypeCode.UInt32:
                    uint valueUInt = 0;
                    if ( isString )
                    {
                        if ( ! uint.TryParse ( value, out valueUInt ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueUInt = Convert.ToUInt32 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueUInt >= UInt32.MinValue && valueUInt <= UInt32 .MaxValue );

                case TypeCode.UInt64:
                    ulong valueULong = 0;
                    if ( isString )
                    {
                        if ( ! ulong.TryParse ( value, out valueULong ) )
                            return false;
                    }
                    else
                    {
                        try
                        {
                            valueULong = Convert.ToUInt64 ( value );
                        }
                        catch ( Exception e )
                        {
                            return false;
                        }
                    }
                    return ( valueULong >= UInt64.MinValue && valueULong <= UInt64.MaxValue );
            }
            return false;
        }

        public static bool TextLength ( string value, int maxLength, int minLength = 0 )
        {
            if ( string.IsNullOrEmpty ( value ) )
                return false;

            return ( value.Length >= minLength && value.Length <= maxLength );
        }
    }
}
