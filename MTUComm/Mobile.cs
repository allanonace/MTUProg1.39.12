using System;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MTUComm
{
    public sealed class Mobile
    {
        public sealed class ConfigData
        {
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
            
            public ConfigData ()
            {
                this.lastRandomKey    = new byte[ 0 ];
                this.lastRandomKeySha = new byte[ 0 ];
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

            public void GenerateCert (
                string base64cert )
            {
                base64cert   = base64cert.Replace ( "cert_file: ", string.Empty );
                byte[] bytes = Convert.FromBase64String ( base64cert );
                this.certificate = new X509Certificate2 ( bytes );
            }
        }

        public  const string ID_DIC_INTUNE  = "AppConfig";
        public  const string ID_FTP_HOST    = "ftpHost";
        public  const string ID_FTP_PORT    = "ftpPort";
        public  const string ID_FTP_USER    = "ftpUser";
        public  const string ID_FTP_PASS    = "ftpPass";
        public  const string ID_FTP_PATH    = "ftpPath";
        public  const string ID_CERTIFICATE = "certificate";

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
                {
                    return path + SEARCH_PATH;
                }
            }

            return null;
        }

        public static string GetPath ()
        {
            // Use cached path
            if ( ! string.IsNullOrEmpty ( pathCache ) )
                return pathCache;
        
            // Try to recover new valid path for the current connected device
            string path = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );

            if ( Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android )
            {
                if ( string.IsNullOrEmpty ( ( path = Mobile.GetPathForAndroid () ) ) )
                    return string.Empty;
            }

            return ( pathCache = path );
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
    }
}
