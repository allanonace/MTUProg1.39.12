using System;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MTUComm
{
    public sealed class Mobile
    {
        public sealed class ConfigData
        {
            private const string CER_HEADER = "-----BEGIN CERTIFICATE-----\n";
            private const string CER_FOOTER = "\n-----END CERTIFICATE-----";
        
            public string ftpUser;
            public string ftpPass;
            public string ftpHost;
            public int    ftpPort;
            public string ftpPath;
            public X509Certificate2 certificate { private set; get; }
            public byte[] lastRandomKey;
            public byte[] lastRandomKeySha;
            public bool   isMtuEncrypted;

            public string RandomKeyAndShaEncryptedInBase64
            {
                get { return this.GetRandomKeyAndShaConverted (); }
            }
            
            public string RandomKeyAndShaInBase64
            {
                get { return this.GetRandomKeyAndShaConverted ( false ); }
            }
            
            public bool IsCertLoaded
            {
                get { return this.certificate != null; }
            }

            public ConfigData ()
            {
                this.lastRandomKey    = new byte[ 0 ];
                this.lastRandomKeySha = new byte[ 0 ];
                
                this.ftpUser = "aclara";
                this.ftpPass = "aclara1234";
                this.ftpHost = "159.89.29.176";
                this.ftpPort = 22;
                this.ftpPath = "/home/aclara";
            }

            public void GenerateCert (
                string base64cert )
            {
                // NOTE: Full certificate file should be converted to base64 and not only the data that appear when
                // open the file with some text editor. The resulting string will be without header and footer strings
                // and seems that always starting with "MII..."
                // https://www.base64encode.org
                // e.g. Aclara certificate in base64
                // base64cert = "MIICxDCCAaygAwIBAgIQV5fB/SvFm4VDwxNIjmx3LzANBgkqhkiG9w0BAQUFADAeMRwwGgYDVQQDExNOZXctVGVzdC1EZXYtQWNsYXJhMB4XDTE1MDQxNTA0MDAwMFoXDTI1MDQyMjA0MDAwMFowHjEcMBoGA1UEAxMTTmV3LVRlc3QtRGV2LUFjbGFyYTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANOISmTy1kRTeOPqajIm+y27q676LFKodBpgrm0M3imYpwnVd+aTnVdk7+NT5vSA1c9dB5PSojh/UfGg2kWDe5gNj2ZA+KaemXFqvl8YI/D6XjoNz3JqoqocjF4/hJnrUdwqOoUL6WPtbWEhCnzin/cVkKx5qxMrOh9qAzp+qYAqyJ26Aocr+nlM7oHRtBUmYRKZbpkNAnpiIV/Q6quSR5Qzsf4XrhvkPDkf2ZX8DvcJmAbXEAaBVa2ORsY9qA86jIphui5kwI9JPcw9hTZy1QxvNcZAijtPyC6AKDuRyEv0Awa1gcSBBRsf0HbeCSD91U/O51+alP3hLhA9tcxddx0CAwEAATANBgkqhkiG9w0BAQUFAAOCAQEAGuTqwTvEgaTl/E2jdG9RUD3zN9MhRCijJIpjv9NdkkH13LK5Sn9up1+DraaccA5h2El9kiXDHYWPA/qRMq1auhNcmTFVYjeQSNW0tyuTqbQiG/8fwZiAZrGn6UmOU/vzzhkyv05x5KzVAEwp94fU/J+kOIJVH0ff5jnMeYHARc1sY6JgXgJKoJbdS4Q4wG2RHj5yFAixv/zwS1XBy2GWtsz03aucNQzBIbk1uTIv2eyYqFMhSGT36vkfJFidRcR3H4FWnvInWoWmxlGcs0MS3bNOAv5ij55h0rREGJ9WdJmI/gw84aA4itFwwUuG6kKdF9AF/rljtVCFVH6T9PFI2Q==";

                // /Library/Frameworks/Xamarin.iOS.framework/Versions/12.2.1.13/src/Xamarin.iOS/mcs/class/Mono.Security/Mono.Security.X509/X509Certificate.cs
                // NOTE: Method PEM needs to find the header and footer strings previous to start with certificate
                // parsing/generation, and these both const should be concatenated with the cert in base64
                // e.g. -----BEGIN CERTIFICATE----- + base64cert + -----END CERTIFICATE----- // Each part converted to byte array
                this.certificate = new X509Certificate2 ( Encoding.ASCII.GetBytes ( CER_HEADER + base64cert + CER_FOOTER ) );
            }

            private string GetRandomKeyAndShaConverted (
                bool encrypted = true )
            {
                // Concatenate random key and random key sha
                byte[] keyAndSha = new byte[ this.lastRandomKey.Length + this.lastRandomKeySha.Length ];
                Array.Copy ( this.lastRandomKey, keyAndSha, this.lastRandomKey.Length );
                Array.Copy ( this.lastRandomKeySha, 0, keyAndSha, this.lastRandomKey.Length, this.lastRandomKeySha.Length );
                
                // Encrypt new byte array with public key
                if ( encrypted )
                {
                    MtuSha256 crypto = new MtuSha256 ();
                    keyAndSha = crypto.encryptBytes ( keyAndSha, this.certificate );
                    
                    crypto = null;
                }
                
                // Clear random key and its sha from memory
                Array.Clear ( this.lastRandomKey,    0, this.lastRandomKey.Length    );
                Array.Clear ( this.lastRandomKeySha, 0, this.lastRandomKeySha.Length );
                
                // Reset encrypted status
                this.isMtuEncrypted = false;
                
                return Convert.ToBase64String ( keyAndSha );
            }
        }

        public  const string ID_DIC_INTUNE  = "AppConfig";
        public  const string ID_FTP_HOST    = "ftpHost";
        public  const string ID_FTP_PORT    = "ftpPort";
        public  const string ID_FTP_USER    = "ftpUser";
        public  const string ID_FTP_PASS    = "ftpPass";
        public  const string ID_FTP_PATH    = "ftpPath";
        public  const string ID_CERTIFICATE = "certificate";
        
        private const string PATH_CONFIG    = ".Config";
        private const string PATH_LOGS      = ".Logs";

        private const string APP_SUBF       = "com.aclara.mtu.programmer/files/";
        private const string PREFAB_PATH    = "/data/data/" + APP_SUBF;
        private const string SEARCH_PATH    = "Android/data/" + APP_SUBF;
        private const string XML_MTUS       = "Mtu.xml";

        private enum PATHS
        {
            STORAGE_EMULATED_ZERO,
            STORAGE_EMULATED_LEGACY,
            STORAGE_SDCARD_ZERO,
            SDCARD_MNT,
            SDCARD,
            //DATA_MEDIA_ZERO,
            //DATA_MEDIA,
            //DATA_ZERO,
            //DATA
        }

        private static string[] paths =
        {
            "/storage/emulated/0/",      // Espacio de trabajo del usuario cero/0
            "/storage/emulated/legacy/", // Enlace simbolico a "/storage/emulated/0/"
            "/storage/sdcard0/",         // Android >= 4.0
            "/mnt/sdcard/",              // Android < 4.0
            "/sdcard/",                  // Enlace simbolico a "/storage/sdcard0/" y "/mnt/sdcard/"
            //"/data/media/0/",            // 
            //"/data/media/",
            //"/data/0/",
            //"/data/"
        };

        public  static ConfigData configData;
        private static string     pathCache;
        private static string     pathCacheConfig;
        private static string     pathCacheLogs;

        static Mobile ()
        {
            configData = new ConfigData ();
        }

        private static string GetEnumPath ( PATHS ePath )
        {
            return paths[ (int)ePath ];
        }

        private static string GetPathForAndroid ()
        {
            // Works with dev unit ZTE but not with Alcatel
            if ( Directory.Exists ( PREFAB_PATH ) &&
                 File.Exists(PREFAB_PATH + XML_MTUS ) )
                return PREFAB_PATH;

            // Search the first valid path to recover XML files
            // Works with dev unit Alcatel but no with ZTE
            PATHS ePath;
            string path;
            string[] names = Enum.GetNames(typeof(PATHS));
            for (int i = 0; i < names.Length; i++)
            {
                Enum.TryParse<PATHS>(names[i], out ePath);
                path = GetEnumPath(ePath);

                if ( Directory.Exists ( path )) //&& File.Exists(path + SEARCH_PATH + XML_MTUS ) )
                    return path + SEARCH_PATH;
            }

            return null;
        }

        public static string GetPath ()
        {
            // Use cached path
            if ( ! string.IsNullOrEmpty ( pathCache ) )
                return pathCache;
        
            /*
            string PRIVATE = Environment.GetFolderPath ( Environment.SpecialFolder.Resources );
            Directory.CreateDirectory ( Path.Combine ( PRIVATE, ".Config" ) );
            File.Create ( Path.Combine ( PRIVATE, ".Config/Test.txt" ) );
            RecurReadFolders ( PRIVATE );
            */
        
            // Try to recover new valid path for the current connected device
            string path = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );

            if ( Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android )
            {
                if ( string.IsNullOrEmpty ( ( path = Mobile.GetPathForAndroid () ) ) )
                    return string.Empty;
            }

            return ( pathCache = path );
        }
        
        public static string GetPathConfig ()
        {
            if ( ! string.IsNullOrEmpty ( pathCacheConfig ) )
                return pathCacheConfig;
        
            string path = Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.Resources ), PATH_CONFIG );
        
            if ( ! Directory.Exists ( path ) )
                Directory.CreateDirectory ( path );
        
            return ( pathCacheConfig = path );
        }
        
        public static string GetPathLogs ()
        {
            if ( ! string.IsNullOrEmpty ( pathCacheLogs ) )
                return pathCacheLogs;
        
            string path = Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.Resources ), PATH_LOGS );
        
            if ( ! Directory.Exists ( path ) )
                Directory.CreateDirectory ( path );
        
            return ( pathCache = path );
        }
        
        private static void RecurReadFolders ( string PATH, int numLevel = 0 )
        {
            string space = string.Empty.PadLeft ( numLevel * 4, ' ' );
            
            Console.WriteLine ( space + "FOLDER: " + PATH );
            
            if ( Directory.GetFiles ( PATH ).Length > 0 )
            {
                Console.WriteLine ( space + "· FILES.." );
                foreach ( string file in Directory.EnumerateFiles ( PATH ) )
                    Console.WriteLine ( space + "  · " + file );
            }
        
            foreach ( string folder in Directory.EnumerateDirectories ( PATH ) )
                RecurReadFolders ( folder, numLevel + 1 );
        }

        public static bool IsNetAvailable ()
        {
            List<ConnectionProfile> profiles =
                new List<ConnectionProfile> (
                    Connectivity.ConnectionProfiles );

            return ( Connectivity.NetworkAccess == NetworkAccess.Internet &&
                     ( profiles.Contains ( ConnectionProfile.WiFi     ) ||
                       profiles.Contains ( ConnectionProfile.Cellular ) ) );
        }
        
        public static StreamReader GetResourcePath (
            string fileName )
        {
            Stream path = typeof ( MTUComm ).Assembly.GetManifestResourceStream ( "MTUComm.Resource." + fileName );
            return new StreamReader ( path );

            /*
            string xml = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }
            
            return xml;
            */
        }
    }
}
