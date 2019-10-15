using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Library.Exceptions;

namespace Library
{
    public static class Utils
    {
        private static DateTime eventEpoch = new DateTime ( 1970, 1, 1, 0, 0, 0 );

        #region Log

        private static bool DEEP_MODE = true;
    
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

        #endregion

        #region Math

        public static string NormalizeBooleans (
            string input )
        {
            string       pattern = "(?:\"|>)(?i)(#)(?:\"|<)";
            List<string> words   = new List<string> () { "true", "false" };

            return Regex.Replace ( input,
                pattern.Replace ( "#", String.Join ( "|", words ) ),
                entry => entry.Value.ToLower() );
        }

        public static byte[] DateTimeToFourBytes (
            DateTime dateTime )
        {
            return new byte[]
            {
                ( byte )dateTime.Month,
                ( byte )dateTime.Day,
                ( byte )( dateTime.Year - 2000 ),
                0x00
            };
        }

        public static byte[] GetTimeSinceDate (
            DateTime date,
            DateTime init = default ( DateTime ) )
        {
            if ( init == default ( DateTime ) )
                init = eventEpoch;

            // fixup any times before epoch to prevent rollover
            if ( date.CompareTo ( init ) < 0 )
                date = init;

            return BitConverter.GetBytes ( ( long )date.Subtract ( init ).TotalSeconds );
        }

        #endregion

        #region IO

        public static T DeserializeXml<T> (
            string path,
            bool isFromResources = false )
        where T : class
        {
            T result;
        
            using ( StreamReader streamReader =
                    ( ! isFromResources ) ? new StreamReader ( path ) : GetResourceStreamReader ( path ) )
            {
                string fileContent = NormalizeBooleans ( streamReader.ReadToEnd () );
                using (StringReader reader = new StringReader(fileContent))
                {
                    result = (T)( new XmlSerializer ( typeof( T ) ) ).Deserialize(reader);
                }
            }
            
            return result;
        }
        
        public static StreamReader GetResourceStreamReader (
            string fileName )
        {
            Stream path = AppDomain.CurrentDomain.GetAssemblies ()
                .First ( x => x.FullName.Contains ( "MTUComm" ) )
                .GetManifestResourceStream ( "MTUComm.Resource." + fileName );
            //Stream path = classProject.Assembly.GetManifestResourceStream ( classProject.Name + ".Resource." + fileName );
            
            return new StreamReader ( path );
        }
        
        public static StreamReader GetStreamReader (
            string fileName )
        {
            return new StreamReader ( fileName );
        }

        public static X509Certificate2 GetCertificateFromResources (
            string fileName )
        {
            using ( StreamReader streamReader = GetResourceStreamReader ( fileName ) )
            {
                using ( MemoryStream memStream = new MemoryStream () )
                {
                    streamReader.BaseStream.CopyTo ( memStream );
                    return new X509Certificate2 ( memStream.ToArray () );
                }
            }
        }

        public static void WriteToGlobal (
            string  tagName,
            dynamic value = null )
        {
            try
            {
                if ( string.IsNullOrEmpty ( tagName ) )
                    return;

                String    uri = Path.Combine ( Data.Get.ConfigPath, Data.Get.XmlGlobal );
                XDocument doc = XDocument.Load ( uri );

                doc.Root.SetElementValue ( tagName, value );
                doc.Save ( uri );

                // Update instance
                Utils.SetPropertyValue ( Singleton.Get.Configuration.Global, tagName, value );
            }
            catch ( Exception e )
            {
                throw new GlobalChangedException ();
            }
        }

        #endregion

        #region Color

        public static byte[] ConvertHexToRGB (
            string hex )
        {
            string r, g, b;
            byte[] rgb = new byte[ 3 ];

            hex = hex.Replace ( "#", string.Empty );

            switch ( hex.Length )
            {
                case 1:
                    r = g = b = hex + hex;
                    break;
                case 2:
                    r = g = b = hex;
                    break;
                case 3:
                    r = hex.Substring ( 0, 1 );
                    g = hex.Substring ( 1, 1 );
                    b = hex.Substring ( 2, 1 );
                    r = r + r;
                    g = g + g;
                    b = b + b;
                    break;
                case 6:
                    r = hex.Substring ( 0, 2 );
                    g = hex.Substring ( 2, 2 );
                    b = hex.Substring ( 4, 2 );
                    break;
                default:
                    return null;
            }

            return new byte[] {
                byte.Parse ( r, NumberStyles.AllowHexSpecifier ),
                byte.Parse ( g, NumberStyles.AllowHexSpecifier ),
                byte.Parse ( b, NumberStyles.AllowHexSpecifier ) };
        }

        #endregion

        #region AES

        public static string EncryptStringToBase64_Aes (
            string text,
            string key,
            byte[] iv = null )
        {
            byte[] arKey = Encoding.UTF8.GetBytes ( key );
            byte[] encrypted = EncryptStringToBytes_Aes ( text, arKey, iv );
            return Convert.ToBase64String ( encrypted );
        }
    
        public static string DecryptStringToBase64_Aes (
            string encryptedText,
            string key,
            byte[] iv = null )
        {
            byte[] arText = Convert.FromBase64String ( encryptedText );
            byte[] arKey  = Encoding.UTF8.GetBytes ( key );
            return DecryptStringFromBytes_Aes ( arText, arKey, iv );
        }
    
        public static byte[] EncryptStringToBytes_Aes (
            string text,
            byte[] key,
            byte[] iv = null )
        {
            byte[] encrypted;
    
            using ( Aes aesAlg = Aes.Create () )
            {
                aesAlg.Key = key;
                aesAlg.IV  = ( iv != null ) ? iv : new byte[ aesAlg.BlockSize / 8 ];
                // The initialization vector ( IV ) property is automatically set to a new random value whenever you create
                // a new instance of one of the SymmetricAlgorithm classes or when you manually call the GenerateIV method
                // The size of the IV property must be the same as the BlockSize property divided by 8
                // NOTE: If IV is not used ( filling it with zeros ), the generated encrypted string will always be the same
                
                ICryptoTransform encryptor = aesAlg.CreateEncryptor ( aesAlg.Key, aesAlg.IV );
                using ( MemoryStream msEncrypt = new MemoryStream () )
                {
                    using ( CryptoStream csEncrypt = new CryptoStream ( msEncrypt, encryptor, CryptoStreamMode.Write ) )
                    {
                        using ( StreamWriter swEncrypt = new StreamWriter ( csEncrypt ) )
                        {
                            swEncrypt.Write ( text );
                        }
                        encrypted = msEncrypt.ToArray ();
                    }
                }
            }
    
            return encrypted;
        }
        
        public static string DecryptStringFromBytes_Aes (
            byte[] encryptedText,
            byte[] key,
            byte[] iv = null )
        {
            string plaintext = string.Empty;
    
            using ( Aes aesAlg = Aes.Create () )
            {
                aesAlg.Key = key;
                aesAlg.IV  = ( iv != null ) ? iv : new byte[ aesAlg.BlockSize / 8 ];
    
                ICryptoTransform decryptor = aesAlg.CreateDecryptor ( aesAlg.Key, aesAlg.IV );
                using (MemoryStream msDecrypt = new MemoryStream ( encryptedText ) )
                {
                    using ( CryptoStream csDecrypt = new CryptoStream ( msDecrypt, decryptor, CryptoStreamMode.Read ) )
                    {
                        using ( StreamReader srDecrypt = new StreamReader ( csDecrypt ) )
                        {
                            plaintext = srDecrypt.ReadToEnd ();
                        }
                    }
                }
            }
    
            return plaintext;
        }

        #endregion

        #region Arrays

        public static string ArrayToString<T> (
            T[] entries )
        where T : struct
        {
            if ( entries.Length <= 0 )
                return string.Empty;
        
            StringBuilder stbr = new StringBuilder ();
            foreach ( var entry in entries )
                stbr.AppendFormat ( "{0} ", entry );
            
            return stbr.ToString ().Substring ( 0, stbr.Length - 1 );
        }

        #endregion

        #region Bytes

        public static string ByteToBits (
            object value )
        {
            string str = string.Empty;
            
            BitArray ar = new BitArray ( new byte[] { byte.Parse ( value.ToString () ) } );
            for ( int i = ar.Length - 1; i >= 0; i-- )
                str += ( ar[ i ] ) ? 1 : 0;
            
            return str;
        }
    
        public static T GetNumFromBytes<T> ( byte[] data )
        {
            var value = 0;
            for ( int i = 0; i < data.Length; i++ )
                value += data[ i ] << ( i * 8 );
            
            return ( T )Convert.ChangeType ( value, typeof( T ) );
        }
        
        public static string ByteArrayToString (
            byte[] bytes )
        {
            if ( bytes.Length <= 0 )
                return string.Empty;
        
            StringBuilder hex = new StringBuilder ( bytes.Length * 2 );
            foreach ( byte b in bytes )
                hex.AppendFormat ( "{0:x2} ", b );
            
            return hex.ToString ().Substring ( 0, hex.Length - 1 );
        }

        public static T CalculateNumericFromBytes<T> (
            byte[] data,
            int startAt,
            int size )
        where T : struct
        {
            // NOTE: C# for the moment does not allow to use mathematical operators with T as one of the operands
            // One walkaround can be to operate with longer type and the convert to target type, usually smaller
            decimal value = 0m;
            for ( int i = 0; i < size; i++ )
                value += data[ i + startAt ] << ( i * 8 );

            // NOTE: Can be used ( T )( object )value nor ( T )value
            try
            {
                return ( T )Convert.ChangeType ( value, typeof( T ) );
            }
            catch ( Exception e )
            {
                return default ( T );
            }
        }

        public static T ConvertToNumericFromBytes<T> (
            byte[] data,
            int startAt = 0,
            int size    = 0 )
        where T : struct
        {
            if ( size == 0 ) size = data.Length;

            byte[] sub = data.Skip ( startAt ).Take ( size ).ToArray ();

            // Add zeros up to the required length
            TypeCode code;
            byte[] subZeros = null;
            switch ( code = Type.GetTypeCode ( typeof ( T ) ) )
            {
                case TypeCode.Byte  :
                    subZeros = new byte[ 1 ]; break; // 1 byte

                case TypeCode.Int16 :
                case TypeCode.UInt16:
                    subZeros = new byte[ 2 ]; break; // 2 bytes

                case TypeCode.Int32 :
                case TypeCode.UInt32:
                    subZeros = new byte[ 4 ]; break; // 4 bytes

                case TypeCode.Int64 :
                case TypeCode.UInt64:
                case TypeCode.Double:
                    subZeros = new byte[ 8 ]; break; // 8 bytes
            }
            sub.CopyTo ( subZeros, 0 );
            
            dynamic result = default ( T );
            switch ( code )
            {
                // short, int, ushort, uint and double
                // NOTE: The array must contain the exact number of indexes required for each case
                case TypeCode.Byte  : result = subZeros [ 0 ];                        break; // 1 byte
                case TypeCode.Int16 : result = BitConverter.ToInt16  ( subZeros, 0 ); break; // 2 bytes
                case TypeCode.Int32 : result = BitConverter.ToInt32  ( subZeros, 0 ); break; // 4 bytes
                case TypeCode.Int64 : result = BitConverter.ToInt64  ( subZeros, 0 ); break; // 8 bytes
                case TypeCode.UInt16: result = BitConverter.ToUInt16 ( subZeros, 0 ); break; // 2 bytes
                case TypeCode.UInt32: result = BitConverter.ToUInt32 ( subZeros, 0 ); break; // 4 bytes
                case TypeCode.UInt64: result = BitConverter.ToUInt64 ( subZeros, 0 ); break; // 8 bytes
                case TypeCode.Double: result = BitConverter.ToDouble ( subZeros, 0 ); break; // 8 bytes
            }

            return ( T )( object )result;
        }

        #endregion

        #region String

        public static string FirstCharToCapital (
            object value )
        {
            Type type = value.GetType ();
            if ( type.IsClass ||
                 type.IsArray )
            {
                return string.Empty;
            }

            string str = value.ToString ();
            return str[ 0 ].ToString ().ToUpper () + str.Substring ( 1 );
        }

        public static string StringToBase64 (
            string text )
        {
            if ( ! Utils.StringIsInBase64 ( text ) )
                 return Convert.ToBase64String ( Encoding.UTF8.GetBytes ( text ) );
            else return text;
        }

        public static byte[] StringToByteArrayBase64 (
            string text )
        {
            if ( ! Utils.StringIsInBase64 ( text ) )
                 return Encoding.UTF8.GetBytes ( Utils.StringToBase64 ( text ) );
            else return Encoding.UTF8.GetBytes ( text );
        }

        public static bool StringIsInBase64 (
            string text )
        {
            try
            {
                Convert.FromBase64String ( text );
            }
            catch ( Exception )
            {
                return false;
            }
            return true;
        }

        public static byte[] ByteArrayFromBase64 (
            string text,
            out bool wasInBase64 )
        {
            if ( ( wasInBase64 = Utils.StringIsInBase64 ( text ) ) )
                return Convert.FromBase64String ( text );
            return null;
        }

        public static string StringFromBase64 (
            string text,
            out bool wasInBase64 )
        {
            if ( ( wasInBase64 = Utils.StringIsInBase64 ( text ) ) )
                return Encoding.UTF8.GetString ( Convert.FromBase64String ( text ) );
            return text;
        }

        public static string StringFromBase64 (
            string text )
        {
            return Utils.StringFromBase64 ( text, out bool wasInBase64 );
        }

        #endregion

        #region Type

        public static bool IsBool (
            object value )
        {
            if ( value is bool )
                return true;

            if ( ! ( value is string ) )
                return false;

            bool ok;
            return bool.TryParse ( value.ToString (), out ok );
        }

        public static T ParseIntToEnum<T> ( int index, T defaultValue )
        {
            Type typeEnum = typeof ( T );
            
            if ( Enum.IsDefined ( typeEnum, index ) )
                return ( T )Enum.Parse ( typeEnum, index.ToString () );
            return defaultValue;
        }

        #endregion        

        #region Properties

        public static void SetPropertyValue (
            dynamic instance,
            string propName,
            dynamic value )
        {
            instance.GetType ().GetProperty ( propName ).SetValue ( instance, value );
        }

        #endregion
    }
}
