using System;
using System.Xml;
using Library;
using System.ComponentModel;
using System.Globalization;

namespace Xml
{
    public class XmlAux
    {
    // Return the result as dynamic
    private static T Validate<T> (
      ref dynamic valueOrNode,
      bool nullable,
      int  vMin,
      int  vMax,
      out bool ok )
    {
      dynamic result = valueOrNode;
      ok = false;
      
      // When .NET is deserializing, use an XmlNode array to set the value of the properties
      if ( valueOrNode.GetType () == typeof ( System.Xml.XmlNode[] ) )
        result = valueOrNode[ 0 ].Value;

      Utils.Print (
        result + " " +
        nullable + " " + 
        ( result is string ) + " " + 
        ( result == null ) + " " + 
        typeof ( T ) );

      // Allow to set an empty or null value, but should only be used with string variables
      if ( nullable &&
           ( result is string && string.IsNullOrEmpty ( result.ToString () ) ||
             result == null ) )
      {
        Utils.Print ( "Null" );

        result = ( T )( object )string.Empty;

        ok = true;
      }
      // The value to be set is not null nor an empty string
      else if ( result != null &&
                ! string.IsNullOrEmpty ( result.ToString () ) )
      {
        string str = result.ToString ();

        // Verifies string length
        if ( typeof ( T ) == typeof ( string ) )
        {
          Utils.Print ( "String" );

          int length = str.Length;
          if ( vMin > -1 && length < vMin ||
               vMax > -1 && length > vMax )
            return result;
        }
        // Verifies the numerical value within the range
        else
        {
          Utils.Print ( "Number: " + str );

          dynamic num = -1;
          try
          {
            TypeConverter converter =
              TypeDescriptor.GetConverter ( typeof ( T ) );

            num = ( T )converter.ConvertFromString (
              null, CultureInfo.InvariantCulture, str );
          }
          catch ( Exception e )
          {
            // That was not a valid numerical value
            return result;
          }

          if ( vMin > -1 && num < vMin )
            num = vMin;
          
          else if ( vMax > -1 && num > vMax )
            num = vMax;
          
          result = ( T )( object )num;
          
          Utils.Print ( "Number End: " + result );
        }

        ok = true;
      }

      return result;
    }

    public static T Set_Logic<T> (
      bool    nullable, // Allow to set a null value ( only for string values )
      dynamic value,            // Value to set ( in string format, to allow it be empty )
      dynamic def,              // Default value ( in string format, to allow it be empty )
      int     min,         // Minimum value or length
      int     max )        // Maximum value or length
    {
      try
      {
        bool ok;

        // Try to set the desired or the default value
        dynamic result = Validate<T> ( ref value, nullable, min, max, out ok );
        if ( ok )
        {
          Utils.Print ( "Value: " + result.GetType () + " " + value.GetType () );

          return result;
        }
        else
        {
          result = Validate<T> ( ref def, nullable, min, max, out ok );
          if ( ok )
          {
            Utils.Print ( "Default: " + result.GetType () + " " + value.GetType () );

            return result;
          }
          // Both the desired and the default value are not valid ( empty or null )
          else throw new Exception ();
        }
      }
      catch ( Exception e )
      {
        Utils.Print ( "Error: " +
        "Value '" + value +
        "' Default '" + def +
        "' Type '" + typeof ( T ) + "'" +
        "\n" + e.Message );
        throw new System.Exception ();
      }
    }

    // NOTE: Only for string values, because the other tipes do not allow an empty value
    public static T SetAllowEmpty<T> (
      dynamic value,
      dynamic def = null,
      int     min = -1,
      int     max = -1 )
    {
      return Set_Logic<T> ( true, value, def, min, max );
    }

    public static T Set<T> (
      dynamic value,
      dynamic def = null,
      int     min = -1,
      int     max = -1 )
    {
      return Set_Logic<T> ( false, value, def, min, max );
    }

        /*
        Example..

        public string P1; // The type of variable indicates the T that will be used in the logic
        public dynamic P1_AllowEmptyAndDefault
        {
            set { Set ( ref P1, value, def: 3, min: 2, max: 4 ); }
        }

        P1_AllowEmptyAndDefault = 1;
        Utils.Print ( "Value: " + t.P1 ); // 2 -> Minimum value
        P1_AllowEmptyAndDefault = 5;
        Utils.Print ( "Value: " + t.P1 ); // 4 -> Maximum value
        P1_AllowEmptyAndDefault = "lalala";
        Utils.Print ( "Value: " + t.P1 ); // 3 -> Default value
        P1_AllowEmptyAndDefault = null;
        Utils.Print ( "Value: " + t.P1 ); // 3 -> Default value

        public string P1;
        public dynamic P1_AllowEmptyAndDefault
        {
            set { Set ( ref P1, value ); } // No default value
        }

        P1_AllowEmptyAndDefault = null; // or = "lalala"
        Utils.Print ( "Value: " + t.P1 ); // Exception!
        */
    }
}
