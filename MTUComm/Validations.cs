using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using MTUComm.MemoryMap;
using Xml;

namespace MTUComm
{
    public sealed class Validations
    {
        #region Constants

        private const string PREFIX_AUX     = "_AllowEmptyField";
        private const string EXCEP_NAME_SGN = "_";
        private const string EXCEP_VALIDATE = "Deserializing XML has failed due to validate required fields [ " + EXCEP_NAME_SGN + " ]";

        #endregion

        public static bool IsNumeric<T> ( dynamic value )
        {
            if ( value is null )
                return false;
            
            else if ( value is Int32  ||
                      value is UInt32 ||
                      value is UInt64 )
                return true;

            // String conversion for used types
            else if ( value is string )
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

            // Validation for other numeric types
            string chars = value.ToString ();
            if ( char.Equals ( chars[ 0 ], "-" ) )
                chars = chars.Remove ( 0, 1 );

            // NOTA: No funciona con numeros negativos
            return chars.All ( c => char.IsDigit ( c ) );
        }

        public static bool NumericBytesLimit<T> ( dynamic value, int numBytes )
        {
            if ( ! IsNumeric<T> ( value ) )
                return false;

            bool isString = ( value is string );

            switch ( Type.GetTypeCode( typeof(T)) )
            {
                #region Int

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
                    
                    int iLimit = 0;
                    try
                    {
                        iLimit = Convert.ToInt32 ( Math.Pow ( 2, numBytes * 8 ) );
                    }
                    // Launchs error when result is bigger than ulong upper limit
                    catch ( Exception e )
                    {
                        iLimit = int.MaxValue;
                    }

                    return ( valueInt < iLimit );

                #endregion
                #region UInt

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

                    uint uLimit = 0;
                    try
                    {
                        uLimit = Convert.ToUInt32 ( Math.Pow ( 2, numBytes * 8 ) );
                    }
                    // Launchs error when result is bigger than ulong upper limit
                    catch ( Exception e )
                    {
                        uLimit = uint.MaxValue;
                    }

                    return ( valueUInt < uLimit );

                #endregion
                #region ULong

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

                    ulong ulLimit = 0;
                    try
                    {
                        ulLimit = Convert.ToUInt64 ( Math.Pow ( 2, numBytes * 8 ) );
                    }
                    // Launchs error when result is bigger than ulong upper limit
                    catch ( Exception e )
                    {
                        ulLimit = ulong.MaxValue;
                    }

                    return ( valueULong < ulLimit );

                #endregion
            }
            return false;
        }

        public static bool NumericTypeLimit<T> ( dynamic value )
        {
            if ( ! IsNumeric<T> ( value ) )
                return false;

            bool isString = ( value is string );

            switch ( Type.GetTypeCode( typeof(T)) )
            {
                #region Int

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
                    return ( valueInt >= Int32.MinValue &&
                             valueInt <= Int32 .MaxValue );

                #endregion
                #region UInt

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
                    return ( valueUInt >= UInt32.MinValue &&
                             valueUInt <= UInt32 .MaxValue );

                #endregion
                #region ULong

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
                    return ( valueULong >= UInt64.MinValue &&
                             valueULong <= UInt64.MaxValue );

                #endregion
            }
            return false;
        }

        public static bool TextLength (
            string value,
            int  maxLength,
            int  minLength    = 0,
            bool maxInclusive = true,
            bool minInclusive = true )
        {
            if ( string.IsNullOrEmpty ( value ) )
                return false;

            int length = value.Length;
            return ( ( ! minInclusive && length >  minLength ||
                         minInclusive && length >= minLength ) &&
                     ( ! maxInclusive && length <  maxLength ||
                         maxInclusive && length <= maxLength ) );
        }

        public static T DeserializeXml<T> ( TextReader reader )
        {
            Type typeXml  = typeof ( T );
            T deserialize = ( T )new XmlSerializer ( typeXml ).Deserialize ( reader );

            try
            {
                DeserializeXml_Logic ( deserialize, typeXml.GetProperties () );
            }
            catch ( Exception e )
            {
                // Capture validation error with element name and launch new exception
                throw new MemoryMapXmlValidationException ( EXCEP_VALIDATE.Replace ( EXCEP_NAME_SGN, e.Message ) );
            }
            return deserialize;
        }

        private static bool DeserializeXml_Logic ( dynamic instance, PropertyInfo[] psInfo, int index = 1 )
        {
            Type typeAt = typeof ( XmlElementAttribute );
            Type typeInstance = instance.GetType ();

            XmlElementAttribute attribute;
            foreach ( PropertyInfo pinfo in psInfo )
            {
                // We only want to work with XmlElements, without not XmlIgnore properties
                if ( ( attribute = ( XmlElementAttribute )pinfo.GetCustomAttribute ( typeAt ) ) != null )
                {
                    object value = pinfo.GetValue ( instance );

                    // Element not nullable and without value ( empty or null )
                    if ( ! attribute.IsNullable &&
                         ( value is string &&
                           // Empty string elements
                           ( string.IsNullOrEmpty ( ( string )value ) ||
                           // Int elements with string auxiliary method to allow correct deserialization
                             string.Equals ( value, MemRegister.ERROR_STR ) ||
                           // Bool elements with string auxiliary method to allow correct deserialization
                             pinfo.Name.Contains ( PREFIX_AUX ) &&
                             typeInstance.GetProperty (
                                 pinfo.Name.Substring ( 0, pinfo.Name.IndexOf ( PREFIX_AUX ) ) )
                                 .GetValue ( instance ) == MemRegister.ERROR_VAL ) ||
                           // Value of different type than string, for arrays mainly
                           ! ( value is string ) && value == null ) )
                        throw new Exception ( attribute.ElementName + " #" + index );

                    // Element is an array
                    else if ( value != null &&
                              pinfo.PropertyType.IsArray )
                    {
                        // Iterate for each entry of registers or overloads array
                        int i = 0;
                        foreach ( var register in ( Array )value )
                            DeserializeXml_Logic ( register, register.GetType().GetProperties (), ++i );
                    }
                }
            }

            return true;
        }
    }
}
