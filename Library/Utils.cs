using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

        #endregion

        #region AES

        public static string EncryptStringToBase64_Aes (
            string text,
            string key,
            byte[] iv = null )
        {
            byte[] arKey = Encoding.ASCII.GetBytes ( key );
            byte[] encrypted = EncryptStringToBytes_Aes ( text, arKey, iv );
            return Convert.ToBase64String ( encrypted );
        }
    
        public static string DecryptStringToBase64_Aes (
            string encryptedText,
            string key,
            byte[] iv = null )
        {
            byte[] arText = Convert.FromBase64String ( encryptedText );
            byte[] arKey  = Encoding.ASCII.GetBytes ( key );
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
            
            return ( T )( object )value;
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

        public static T GetNumericValueFromBytes<T> (
            byte[] data,
            int startAt,
            int size )
        where T : struct
        {
            // NOTE: C# for the moment does not allow to use mathematical operators with T as one of the operands
            // One walkaround can be to operate with longer type and the convert to target type, usually smaller
            long value = 0L;
            for ( int i = 0; i < size; i++ )
                value += data[ i + startAt ] << ( i * 8 );

            // NOTE: Can be used ( T )( object )value nor ( T )value
            return ( T )Convert.ChangeType ( value, typeof( T ) );
        }

        #endregion
    }
}
