using System;
using System.IO;
using System.IO.Compression;
using Library;
using System.Linq;
using System.Text;
using System.Web;

namespace MTUComm
{
    /// <summary>
    /// Compresses strings using the algorithm Deflate or GZip and encoding
    /// the result to URL format, to allow to send data directly within the URL.
    /// </summary>
    public sealed class Compression
    {
        /// <summary>
        /// Types of compression available.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="ALGORITHM">
        /// <item>
        ///     <term>ALGORITHM.NOTHING</term>
        ///     <description>Use to not compress</description>
        /// </item>
        /// <item>
        ///     <term>ALGORITHM.DEFLATE</term>
        ///     <description>Use to compress using Deflate algorithm</description>
        /// </item>
        /// <item>
        ///     <term>LexiAction.GZIP</term>
        ///     <description>Use to compress using GZip algorithm</description>
        /// </item>
        /// </list>
        /// </para>
        /// <para>&#160;</para>
        /// </summary>
        /// <remarks>
        /// GZip is simply the Deflate algorithm plus a checksum and header/footer.
        /// <para>
        /// Deflate is faster and smaller, so naturally not doing a checksum is faster
        /// but then you can not detect corrupt streams.
        /// </para>
        /// </remarks>
        public enum ALGORITHM
        {
            NOTHING,
            DEFLATE,
            GZIP
        }
        
        private const string DEFLATE = "deflate";
        private const string GZIP    = "gzip";

        /// <summary>
        /// Returns a string generated randomly with the specific length.
        /// <para>
        /// The method uses these characters 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'.
        /// </para>
        /// </summary>
        /// <param name="length">Number of characters.</param>
        /// <returns></returns>
        public static string RandomString ( int length )
        {
            Random random = new Random();
        
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public static string GetUriParameter ()
        {
            string paramGlobal = Singleton.Get.Configuration.Global.Compression.ToLower ();
            
            string param = "&compress=";
            switch ( paramGlobal )
            {
                case DEFLATE:
                case GZIP   : param += paramGlobal; break;
                default     : param = string.Empty; break;
            }

            return param;
        }

        /// <summary>
        /// Compresses a string using the algorithm indicated in the Global.xml configuration file.
        /// <para>
        /// See <see cref="Xml.Global"/> to show the class used to map Global.xml file.
        /// </para>
        /// </summary>
        /// <remarks>
        /// By default, the Compression tag in Global.xml is set to an empty string, NO compression.
        /// </remarks>
        /// <param name="input">String to compress</param>
        /// <param name="times">Number of times to apply the compression</param>
        /// <returns>Compressed and URL encoded string.</returns>
        public static string CompressToUrlUsingGlobal ( string input, int times = 1 )
        {
            string paramGlobal = Singleton.Get.Configuration.Global.Compression.ToLower ();
        
            ALGORITHM algorithm = ALGORITHM.NOTHING;
            switch ( paramGlobal )
            {
                case DEFLATE: algorithm = ALGORITHM.DEFLATE; break;
                case GZIP   : algorithm = ALGORITHM.GZIP;    break;
            }
            
            return HttpUtility.UrlEncode ( Compress ( input, algorithm, times ) );
        }
        
        /// <summary>
        /// Compresses a string using the selected algorithm and encodes the result string to URL format.
        /// </summary>
        /// <param name="input">String to compress</param>
        /// <param name="algorithm">Algorithm to use ( See ALGORITHM enumeration )</param>
        /// <param name="times">Number of times to apply the compression</param>
        /// <returns>Compressed and URL encoded string.</returns>
        public static string CompressToUrl ( string input, ALGORITHM algorithm = ALGORITHM.DEFLATE, int times = 1 )
        {
            return HttpUtility.UrlEncode ( Compress ( input, algorithm, times ) );
        }
        
        /// <summary>
        /// Compresses a string using the selected algorithm.
        /// </summary>
        /// <param name="input">String to compress</param>
        /// <param name="algorithm">Algorithm to use ( See ALGORITHM enumeration )</param>
        /// <param name="times">Number of times to apply the compression</param>
        /// <returns>Compressed string.</returns>
        public static string Compress ( string input, ALGORITHM algorithm = ALGORITHM.DEFLATE, int times = 1 )
        {
            string result = Compress_Logic ( input, algorithm );
            
            if ( --times > 0 )
                return Compress ( result, algorithm, times );
            return result;
        }
        
        /// <summary>
        /// Decompresses a string using the selected algorithm.
        /// </summary>
        /// <param name="input">String to decompress</param>
        /// <param name="algorithm">Algorithm to use ( See ALGORITHM enumeration )</param>
        /// <param name="times">Number of times to apply the decompression</param>
        /// <returns>Decompressed string.</returns>
        public static string Decompress ( string input, ALGORITHM algorithm = ALGORITHM.DEFLATE, int times = 1 )
        {
            string result = Decompress_Logic ( input, algorithm );
            
            if ( --times > 0 )
                return Decompress ( result, algorithm, times );
            return result;
        }
        
        /*
        · Simular la web php: http://sandbox.onlinephpfunctions.com
            <?php
            if (function_exists("gzinflate"))
                 echo "gzinflate OK \n";
            else echo "gzinflate NO";
            
            $a = gzinflate(base64_decode("cy0qyi9SyC2tBOOC1KJ8hcLSVCAHCCoV0osSy1IB"));
            echo $a;
            ?>
        · Simular la logica del app: https://dotnetfiddle.net
            using System;
            using System.IO;
            using System.IO.Compression;
            using System.Text;
            
            public class Program
            {
                public static void Main()
                {
                    using ( var outStream = new MemoryStream () )
                    {
                        string input = "Error muy muy pero que muuuuy grave";
                        
                        using ( DeflateStream stream = new DeflateStream ( outStream, CompressionMode.Compress ) )
                            using (var ms = new MemoryStream ( Encoding.UTF8.GetBytes ( input ) ) )
                                ms.CopyTo ( stream );
            
                        string converted = Convert.ToBase64String ( outStream.ToArray() );
                        
                        Console.WriteLine ( converted );
                    }
                }
            }
        · Convertir a url: https://www.url-encode-decode.com
        */
        
        private static string Compress_Logic ( string input, ALGORITHM algorithm )
        {
            if ( algorithm == ALGORITHM.NOTHING )
                return Convert.ToBase64String ( Encoding.UTF8.GetBytes ( input ) );
        
            using ( var outStream = new MemoryStream () )
            {
                dynamic stream = null;
                switch ( algorithm )
                {
                    case ALGORITHM.DEFLATE:
                        stream = new DeflateStream ( outStream, CompressionMode.Compress );
                        break;
                    case ALGORITHM.GZIP:
                        stream = new GZipStream ( outStream, CompressionMode.Compress );
                        break;
                }
                
                using ( stream )
                    using (var ms = new MemoryStream ( Encoding.UTF8.GetBytes ( input ) ) )
                        ms.CopyTo ( stream );
    
                return Convert.ToBase64String ( outStream.ToArray() );
            }
        }
    
        private static string Decompress_Logic ( string input, ALGORITHM algorithm )
        {
            if ( algorithm == ALGORITHM.NOTHING )
                return Encoding.UTF8.GetString ( Convert.FromBase64String ( input ) );
        
            using ( var inStream = new MemoryStream ( Convert.FromBase64String ( input ) ) )
            {
                dynamic stream = null;
                switch ( algorithm )
                {
                    case ALGORITHM.DEFLATE:
                        stream = new DeflateStream ( inStream, CompressionMode.Decompress );
                        break;
                    case ALGORITHM.GZIP:
                        stream = new GZipStream ( inStream, CompressionMode.Decompress );
                        break;
                }
                
                using ( stream )
                {
                    using ( var ms = new MemoryStream () )
                    {
                        stream.CopyTo ( ms );
                        return Encoding.UTF8.GetString ( ms.ToArray () );
                    }
                }
            }
        }
    }
}
