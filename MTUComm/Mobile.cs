﻿using Library;
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

            public X509Certificate2 certificate { private set; get; }

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

            public string FtpDownload_User { get; set; }
            public string FtpDownload_Pass { get; set; }
            public string FtpDownload_Host { get; set; }
            public int FtpDownload_Port { get; set; }
            public string FtpDownload_Path { get; set; }
            public bool HasIntune { get; set; }
            public bool HasFTP { get; set; }
            public byte[] LastRandomKey { get; set; }
            public byte[] LastRandomKeySha { get; set; }
            public bool IsMtuEncrypted { get; set; }

            public ConfigData ()
            {
                this.LastRandomKey    = new byte[ 0 ];
                this.LastRandomKeySha = new byte[ 0 ];
                
                this.FtpDownload_User = string.Empty;
                this.FtpDownload_Pass = string.Empty;
                this.FtpDownload_Host = string.Empty;
                this.FtpDownload_Port = 22;
                this.FtpDownload_Path = string.Empty;
                this.HasIntune = false;
                this.HasFTP = false;
               
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
                catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
                {
                    if ( Errors.IsOwnException ( e ) )
                         throw e;
                    else throw new CertificateInstalledNotValidException ();
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
                catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
                {
                    if ( Errors.IsOwnException ( e ) )
                         throw e;
                    else throw new CertificateInstalledNotValidException ();
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
                catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
                {
                    if ( Errors.IsOwnException ( e ) )
                         throw e;
                    else throw new CertificateInstalledNotValidException ();
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
                catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
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
                byte[] keyAndSha = new byte[ this.LastRandomKey.Length + this.LastRandomKeySha.Length ];
                Array.Copy ( this.LastRandomKey, keyAndSha, this.LastRandomKey.Length );
                Array.Copy ( this.LastRandomKeySha, 0, keyAndSha, this.LastRandomKey.Length, this.LastRandomKeySha.Length );
                
                // Encrypt new byte array with public key
                if ( encrypted )
                {
                    MtuSha256 crypto = new MtuSha256 ();
                    keyAndSha = crypto.encryptBytes ( keyAndSha, this.certificate );
                    
                    crypto = null;
                }
                
                // Clear random key and its sha from memory
                Array.Clear ( this.LastRandomKey,    0, this.LastRandomKey.Length    );
                Array.Clear ( this.LastRandomKeySha, 0, this.LastRandomKeySha.Length );
                
                // Reset encrypted status
                this.IsMtuEncrypted = false;
                
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
        public  const string PATH_NODES     = "Nodes";
        public  const string PATH_IMAGES    = "Images";

        private const string APP_SUBF       = "com.aclara.mtu.programmer/files/";
 
        public static void CreateDirectoryIfNotExist ( string path )
        {
            if ( ! Directory.Exists ( path ) )
                Directory.CreateDirectory ( path );
        }

        private static string     pathCachePublic;
        private static string     pathCacheConfig;
        private static string     pathCacheLogs;
        private static string     pathCacheLogsUni;
        private static string     pathCacheLogsUser;
        private static string     pathCacheLogsUserBackup;
        private static string     pathCacheEvents;
        private static string     pathCacheImages;
        private static string     pathCacheNodes;

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

        public static string NodePath
        {
            get
            {
                CreateDirectoryIfNotExist(pathCacheNodes);
                return pathCacheNodes;
            }
            set
            {
                string path = Path.Combine(pathCacheLogsUser, value + "_" + PATH_NODES);
                CreateDirectoryIfNotExist(path);
                pathCacheNodes = path;
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

        public static ConfigData ConfData { get; set; }

        static Mobile ()
        {
            ConfData = new ConfigData ();
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
