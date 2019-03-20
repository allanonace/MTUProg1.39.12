using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Text;

namespace MTUComm
{
    public sealed class Aux
    {
        public static string NormalizeBooleans (
            string input )
        {
            string       pattern = "(?:\"|>)(?i)(#)(?:\"|<)";
            List<string> words   = new List<string> () { "true", "false" };

            return Regex.Replace ( input,
                pattern.Replace ( "#", String.Join ( "|", words ) ),
                entry => entry.Value.ToLower() );
        }
        
        public static T DeserializeXml<T> (
            string path,
            bool isFromResources = false )
        where T : class
        {
            T data;
        
            using ( StreamReader streamReader =
                    ( ! isFromResources ) ? new StreamReader(path) : GetResourceStreamReader ( path ) )
            {
                string fileContent = Aux.NormalizeBooleans ( streamReader.ReadToEnd () );
                using (StringReader reader = new StringReader(fileContent))
                {
                    data = (T)( new XmlSerializer ( typeof( T ) ) ).Deserialize(reader);
                }
            }
            
            return data;
        }
        
        public static StreamReader GetResourceStreamReader (
            string fileName )
        {
            Stream path = typeof ( MTUComm ).Assembly.GetManifestResourceStream ( "MTUComm.Resource." + fileName );
            return new StreamReader ( path );
        }
        
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
    }
}
