using Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Library.Exceptions;
using Xamarin.Essentials;

namespace MTUComm
{
    public sealed class Mobile
    {
        public sealed class ConfigData
        {
            private const string CER_HEADER = "-----BEGIN CERTIFICATE-----\n";
            private const string CER_FOOTER = "\n-----END CERTIFICATE-----";
            private const string CER_INIT   = "MII";
            private const string XML_CER = ".cer";

            public string ftpDownload_User;
            public string ftpDownload_Pass;
            public string ftpDownload_Host;
            public int    ftpDownload_Port;
            public string ftpDownload_Path;
            public bool HasIntune;
            public bool HasFTP;
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
                
                this.ftpDownload_User = string.Empty;
                this.ftpDownload_Pass = string.Empty;
                this.ftpDownload_Host = string.Empty;
                this.ftpDownload_Port = 22;
                this.ftpDownload_Path = string.Empty;
                this.HasIntune = false;
                this.HasFTP = false;
               
            }

            public void TestCertificateIOS ()
            {
                /*
                // Load certificate from resources
                //X509Certificate2 cer = Utils.GetCertificateFromResources ( "certificate.cer" );

                // Install certificate in the keystore for the app
                X509Store store = new X509Store ( StoreName.Root, StoreLocation.CurrentUser );
                store.Open ( OpenFlags.ReadWrite );
                //store.Add ( cer );

                // Load certificate from keystore
                foreach ( var c in store.Certificates )
                    Utils.Print ( "- " + c.Subject + " | " + c.Issuer + " | " + c.NotAfter );

                store.Close ();
                */

                // NOTE: Certificates deployed using Intune are not present in the keychain
                foreach ( var storeLocation in Enum.GetValues ( typeof ( StoreLocation ) ) )
                    foreach ( var storeName in Enum.GetValues ( typeof ( StoreName ) ) )
                        RecoverCerts ( ( StoreName )storeName, ( StoreLocation )storeLocation );
            }

            private void RecoverCerts ( StoreName storeName, StoreLocation storeLocation )
            {
                Utils.Print ( "---------------------" );
                Utils.Print ( "Recover Certificates: Store Name '" + storeName + "' and Location '" + storeLocation + "'" );

                // Install certificate in the keystore for the app
                X509Store store = new X509Store ( storeName, storeLocation );
                store.Open ( OpenFlags.ReadWrite );

                foreach ( var c in store.Certificates )
                    Utils.Print ( "- " + c.Subject + " | " + c.Issuer + " | " + c.NotAfter );

                store.Close ();
            }

            public void RecoverAppFilesTree ()
            {
                /*
                Using ios-deploy: ios-deploy --nolldb --list --bundle_id com.aclara.programmer
                //
                /Documents/
                /Documents/LogsUni/
                /Documents/.config/
                /Documents/.config/.mono/
                /Documents/.config/.mono/certs/
                /Documents/.config/.mono/certs/Trust/
                /Documents/Logs/
                /Library/
                /Library/global.xml
                /Library/Caches/
                /Library/Caches/com.aclara.programmer/
                /Library/Caches/com.aclara.programmer/MAMURLCache/
                /Library/Caches/com.aclara.programmer/MAMURLCache/Cache.db
                /Library/Caches/com.aclara.programmer/MAMURLCache/Cache.db-wal
                /Library/Caches/com.aclara.programmer/MAMURLCache/Cache.db-shm
                /Library/Caches/.IntuneMAM/
                /Library/Caches/.IntuneMAM/com.microsoft.intune.ApplicationInsights/
                /Library/Caches/.IntuneMAM/com.microsoft.intune.ApplicationInsights/regularPrio/
                /Library/Caches/.IntuneMAM/com.microsoft.intune.ApplicationInsights/metaData/
                /Library/Caches/.IntuneMAM/com.microsoft.intune.ApplicationInsights/metaData/metaData
                /Library/Caches/Snapshots/
                /Library/Caches/Snapshots/com.aclara.programmer/
                /Library/Caches/Snapshots/com.aclara.programmer/77B9721D-8B75-4CCB-AEB3-356A40AD0472@2x.ktx
                /Library/Caches/Snapshots/com.aclara.programmer/8897336D-4446-46AC-B916-A3FC2FDEB36F@2x.ktx
                /Library/Caches/Snapshots/com.aclara.programmer/downscaled/
                /Library/Caches/Snapshots/com.aclara.programmer/downscaled/B5D8074E-7B0B-40C1-BC86-B8F2A15BE80B@2x.ktx
                /Library/user.xml
                /Library/mtu.xml
                /Library/meter.xml
                /Library/certificate.txt
                /Library/alarm.xml
                /Library/.IntuneMAM/
                /Library/.IntuneMAM/NBUConfig.plist
                /Library/.IntuneMAM/Config.plist
                /Library/.IntuneMAM/com.aclara.programmer066B69C4-B148-49CC-8C5A-6D0B3C45A307-0.txt
                /Library/Preferences/
                /Library/Preferences/com.aclara.programmer.plist
                /Library/demandconf.xml
                /SystemData/
                /tmp/
                */

                //string publicDir  = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );
                //string privateDir = Environment.GetFolderPath ( Environment.SpecialFolder.Personal );

                var    baseDirs  = Directory.EnumerateDirectories ( "./" );
                foreach ( var directory in baseDirs )
                {
                    Utils.Print ( "------------------> " + directory );
                }
            }

            /*
            public void LoadCertFromKeychain ()
            {
                // https://docs.microsoft.com/es-es/dotnet/api/system.security.cryptography.x509certificates.storename?view=netframework-4.8

                GetCol ( StoreName.AddressBook );
                GetCol ( StoreName.AuthRoot );
                GetCol ( StoreName.CertificateAuthority );
                GetCol ( StoreName.Disallowed );
                GetCol ( StoreName.My );
                GetCol ( StoreName.Root );
                GetCol ( StoreName.TrustedPeople );
                GetCol ( StoreName.TrustedPublisher );
            }
            
            private void GetCol ( StoreName name )
            {
                X509Store store = new X509Store ( name );
                store.Open ( OpenFlags.ReadWrite );
                //X509Certificate2 certificate = new X509Certificate2 ();

                X509Certificate2Collection storecollection = (X509Certificate2Collection)store.Certificates;
                
                Console.WriteLine("Store name: {0}", store.Name);
                Console.WriteLine("Store location: {0} , count: {1}", store.Location, storecollection.Count );
                foreach (X509Certificate2 x509 in storecollection)
                    Console.WriteLine("certificate name: {0}", x509.Subject);

                store.Close ();
                store.Dispose ();
            }
            */

            public X509Certificate2 CreateCertificate(string sCert = null, string sFileCer = null)
            {
                string content;
                byte[] contentBytes;
                X509Certificate2 certNew = null;
                try
                {
                    if (!String.IsNullOrEmpty(sCert))
                    {
                        content = sCert;

                        if (!content.StartsWith(CER_INIT))
                            contentBytes = Convert.FromBase64String(content);
                        else contentBytes = Encoding.ASCII.GetBytes(CER_HEADER + content + CER_FOOTER);


                        certNew = new X509Certificate2(contentBytes);
                    }
                    else if (!String.IsNullOrEmpty(sFileCer))
                    {
                        sFileCer = Path.Combine(ConfigPath, sFileCer);
                        certNew = new X509Certificate2(sFileCer);

                        File.Delete(sFileCer);
                    }
                    return certNew;


                }
                catch (Exception e)
                {
                    if (Errors.IsOwnException(e))
                        throw e;
                    else throw new CertificateInstalledNotValidException();
                }
            }

            public bool StoreCertificate(X509Certificate2 cert)
            {
                bool bFound = false;
                try
                {
                    if (cert!= null)
                    {
                        X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                        store.Open(OpenFlags.ReadWrite);

                        X509Certificate2Collection certRemove = new X509Certificate2Collection();
                        foreach (var c in store.Certificates)
                        {
                            if (c.Equals(cert))
                            {
                                bFound = true;
                                break;
                            }
                            else if (c.NotAfter <= cert.NotAfter)
                            {
                                certRemove.Add(c);

                            }
                            else 
                            {
                                bFound=true;  // if date is previous not save in store
                            }

                        }
                        if (certRemove.Count > 0)
                            store.RemoveRange(certRemove);
                        if (!bFound)
                            store.Add(cert);
                        store.Close();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    if (Errors.IsOwnException(e))
                        throw e;
                    else throw new CertificateInstalledNotValidException();
                }
            }


            public bool GenerateCertFromStore()
            {
                try
                {
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);

                    foreach (var c in store.Certificates)
                    {
                        this.certificate = c;
                        foreach (var c2 in store.Certificates)
                        {
                            if (!c.Equals(c2))
                                if (c.NotAfter <= c2.NotAfter)
                                    this.certificate = c2; //
                        }
                    }

                    store.Close();

                    if (IsCertLoaded)
                        // Check if certificate is not valid/has expired
                        if (DateTime.Compare(this.certificate.NotAfter, DateTime.Today) < 0)
                            throw new CertificateInstalledExpiredException();

                    return true;
                }
                catch (Exception e)
                {
                    if (Errors.IsOwnException(e))
                        throw e;
                    else throw new CertificateInstalledNotValidException();
                }
            }


            public void GenerateCert (string sCertificate = null)
            {
                string content;
                try
                {
                    string path = Path.Combine ( Mobile.ConfigPath, "certificate.txt" );

                    if (File.Exists(path))
                    {
                        // NOTE: Full certificate file should be converted to base64 and not only the data that appear when
                        // open the file with some text editor. The resulting string will be without header and footer strings
                        // and seems that always starting with "MII..." and finish with "=="
                        // https://www.base64encode.org
                        // e.g. Aclara certificate in base64
                        // base64cert = "MIICxDCCAaygAwIBAgIQV5fB/SvFm4VD...uG6kKdF9AF/rljtVCFVH6T9PFI2Q==";

                        // /Library/Frameworks/Xamarin.iOS.framework/Versions/12.2.1.13/src/Xamarin.iOS/mcs/class/Mono.Security/Mono.Security.X509/X509Certificate.cs
                        // NOTE: Method PEM needs to find the header and footer strings previous to start with certificate
                        // parsing/generation, and these both const should be concatenated with the cert in base64
                        // e.g. -----BEGIN CERTIFICATE----- + base64cert + -----END CERTIFICATE----- // Each part converted to byte array
                        content = File.ReadAllText(path);

                    }
                    else if (!String.IsNullOrEmpty(sCertificate)) //intune
                    {
                        content = sCertificate;
                    }
                    else return;

                    byte[] contentBytes;

                    // If the cer file used was already in base64 format, was saved in the
                    // txt file doubly converted in base64 and directly with header and footer
                    if ( ! content.StartsWith ( CER_INIT ) )
                         contentBytes = Convert.FromBase64String ( content );
                    else contentBytes = Encoding.ASCII.GetBytes ( CER_HEADER + content + CER_FOOTER );
                    
                    this.certificate = new X509Certificate2 ( contentBytes );

                    

                    // Check if certificate is not valid/has expired
                    if (DateTime.Compare(this.certificate.NotAfter, DateTime.Today) < 0)
                        throw new CertificateInstalledExpiredException();
                }
                catch ( Exception e )
                {
                    if ( Errors.IsOwnException ( e ) )
                         throw e;
                    else throw new CertificateInstalledNotValidException ();
                }
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
        
        private const string PATH_CONFIG    = "Config";
        public  const string PATH_LOGS      = "Logs";
        private const string PATH_LOGSUNI   = "LogsUni";
        public  const string PATH_BACKUP    = "Backup";
        public  const string PATH_EVENTS    = "Events";
        public  const string PATH_IMAGES    = "Images";

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

        public static void CreateDirectoryIfNotExist ( string path )
        {
            if ( ! Directory.Exists ( path ) )
                Directory.CreateDirectory ( path );
        }

        public  static ConfigData configData;
        private static string     pathCachePublic;
        private static string     pathCacheConfig;
        private static string     pathCacheLogs;
        private static string     pathCacheLogsUni;
        private static string     pathCacheLogsUser;
        private static string     pathCacheLogsUserBackup;
        private static string     pathCacheEvents;
        private static string     pathCacheImages;

        public static string ConfigPublicPath
        {
            get
            {
                return pathCachePublic;
            }
            set
            {
                CreateDirectoryIfNotExist(value);
                pathCachePublic = value;
            }
        }
        public static string ConfigPath
        {
            get
            {
                return pathCacheConfig;
            }
            set
            {
                CreateDirectoryIfNotExist(value);
                pathCacheConfig = value;
            }
        }

        public static string LogPath
        {
            get
            {
                return pathCacheLogs;
            }
            set
            {
                string path = Path.Combine(value, PATH_LOGS);
                CreateDirectoryIfNotExist(path);
                pathCacheLogs = path;
            }
        }

        public static string LogUniPath
        {
            get
            {
                CreateDirectoryIfNotExist(pathCacheLogsUni);
                return pathCacheLogsUni;
            }
            set
            {
                string path = Path.Combine(value, PATH_LOGSUNI);
                CreateDirectoryIfNotExist(path);
                pathCacheLogsUni = path;
            }
        }

        public static string LogUserPath
        {
            get
            {
                return pathCacheLogsUser;
            }
            set
            {
                string path = Path.Combine(LogPath, value);
                CreateDirectoryIfNotExist(path);
                pathCacheLogsUser = path;
            }
        }

        public static string LogUserBackupPath
        {
            get
            {
                return pathCacheLogsUserBackup;
            }
            set
            {
                string path = Path.Combine(LogPath, value);
                CreateDirectoryIfNotExist(path);
                path = Path.Combine(path, PATH_BACKUP);
                CreateDirectoryIfNotExist(path);
                pathCacheLogsUserBackup = path;
            }
        }

        public static string EventPath
        {
            get
            {
                CreateDirectoryIfNotExist(pathCacheEvents);
                return pathCacheEvents;
            }
            set
            {
                string path = Path.Combine(pathCacheLogsUser, value + "_" + PATH_EVENTS);
                CreateDirectoryIfNotExist(path);
                pathCacheEvents = path;
            }
        }

        public static string ImagesPath
        {
            get
            {
                CreateDirectoryIfNotExist(pathCacheImages);
                return pathCacheImages;
            }
            set
            {
                string path = Path.Combine(pathCacheLogsUser, value + "_" + PATH_IMAGES);
                CreateDirectoryIfNotExist(path);
                pathCacheImages = path;
            }
        }

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
        
        public static void RecurReadFolders ( string PATH, int numLevel = 0 )
        {
            string space = string.Empty.PadLeft ( numLevel * 4, ' ' );
            
            Utils.Print ( space + "FOLDER: " + PATH );
            
            if ( Directory.GetFiles ( PATH ).Length > 0 )
            {
                Utils.Print ( space + "· FILES.." );
                foreach ( string file in Directory.EnumerateFiles ( PATH ) )
                {
                    Utils.Print ( space + "  · " + file );
                    
                    string backPath = Mobile.ConfigPublicPath + "/BACKUP";
                    if ( ! Directory.Exists ( backPath ) )
                        Directory.CreateDirectory ( backPath );
                    
                    string newPath  = file.Replace ( PATH, backPath );
                    
                    Utils.Print ( "--------> new path: " + newPath );
                    
                    File.Copy ( file, newPath, true );
                }
            }
        
            foreach ( string folder in Directory.EnumerateDirectories ( PATH ) )
            {
                if ( folder.Contains ( "BACKUP" ) )
                    continue;
            
                RecurReadFolders ( folder, numLevel + 1 );
            }
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
