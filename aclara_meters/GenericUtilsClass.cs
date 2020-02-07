using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using Library;
using MTUComm;
using Library.Exceptions;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;

namespace aclara_meters
{
    public static class GenericUtilsClass
    {
        private const string XML_EXT = ".xml";
        private const string CER_TXT = "certificate.txt";
        private const string XML_CER = ".cer";
        private const string FIL_VER = ".ver";
        private const string INSTALL_MODE = "InstallMode";

        private static string[] filesToCheck =
            {
                "alarm",
                "demandconf",
                "global",
                "meter",
                "mtu",
                "user",
            };

        public static int NumFilesUploaded { get; set; }

        public async static Task<bool> UploadFiles (Boolean UploadPrompt = true, Boolean AllLogs = true )
        {
            Global global = Singleton.Get.Configuration.Global; 
        
            // Path where the file should be saved once downloaded (locally)
            string path = ( AllLogs ) ? Mobile.LogPath : Mobile.LogUserPath;
            
            // Only upload if there are files available
            List<FileInfo> filesToUpload = LogFilesToUpload ( path );
            
            var upload = false;
            if (filesToUpload.Count > 0) upload = true;

            if (UploadPrompt &&
                 filesToUpload.Count > 0)
                upload = await Application.Current.MainPage.DisplayAlert(
                        "Pending log files",
                        "Do you want to Upload them?",
                        "Ok", "Cancel");
  
            
            if ( ! upload )
                return false;
        
            // The FTP credentiales are not present in Global.xml
            if ( ! global.IsFtpUploadSet )
            {
                await Errors.ShowAlert ( new FtpCredentialsMissingException () );
                return false;
            }
            
            // Has the devices internet connection
            if ( ! Mobile.IsNetAvailable () )
            {
                await Errors.ShowAlert ( new NoInternetException () );
                return false;
            }
            
            // Cancel action
            bool cancelled = false;
            System.Action OnCancel = () =>
            {
                cancelled = true;
            };

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool> ();

            Device.BeginInvokeOnMainThread ( async () =>
            {
                // Progress bar
                using ( Acr.UserDialogs.IProgressDialog progress = UserDialogs.Instance.Progress ( "Uploading", OnCancel, "Cancel" ) )
                {
                    // Is necessary for appear the progress bar right now
                    await Task.Delay ( 10 );
                
                    NumFilesUploaded = 0;
                    
                    SftpClient sftp = null;
        
                    try
                    {
                        string remotePath = global.ftpRemotePath;
                        using ( sftp = new SftpClient ( global.ftpRemoteHost, 22, global.ftpUserName, global.ftpPassword ) )
                        {
                            sftp.Connect();
                            
                            // If not exist create the remote path from global.xml
                            if ( ! sftp.Exists ( remotePath ) )
                                sftp.CreateDirectory ( remotePath );
        
                            using ( MD5 md5Hash = MD5.Create () )
                            {
                                byte[] md5Local;
                                byte[] md5Remote;
                                foreach ( FileInfo file in filesToUpload )
                                {
                                    if ( cancelled )
                                        throw new Exception ();
                                
                                    // Is necessary for update the progress bar
                                    await Task.Delay ( 10 );
                                
                                    // Upload local file to the FTP server
                                    using ( FileStream fileStream = new FileStream ( file.FullName, FileMode.Open ) )
                                    {
                                        // Folder path
                                        remotePath = Path.Combine(global.ftpRemotePath,file.Directory.Name); // Logs + User folder
                                        
                                        if ( ! sftp.Exists ( remotePath ) )
                                            sftp.CreateDirectory ( remotePath );

                                        string sTick = DateTime.Now.Ticks.ToString();
                                        string sName;

                                        if (file.Name.Contains("jpg"))
                                            sName = file.Name.Substring(0, file.Name.Length - 4) + "-" + sTick + ".jpg";
                                        else if(file.Name.ToLower().Contains("mtuid"))
                                            sName = file.Name.Substring(0, file.Name.Length - 4) + "-" + sTick + ".xml";
                                        else
                                            sName = file.Name.Substring(0, 10) + "-" + sTick + "Log.xml";
                                       
                                        remotePath = Path.Combine(remotePath, sName);
                                        sftp.UploadFile ( fileStream, remotePath, null );
                                    }
            
                                    // Compare local and remote files
                                    using ( StreamReader stream = new StreamReader ( file.FullName ) )
                                        md5Local = md5Hash.ComputeHash ( Encoding.UTF8.GetBytes ( stream.ReadToEnd () ) );
                                    
                                    md5Remote = md5Hash.ComputeHash ( Encoding.UTF8.GetBytes ( sftp.ReadAllText ( remotePath ) ) );
        
                                    // If both files are equal, move local file to backup folder
                                    if ( Enumerable.SequenceEqual ( md5Local, md5Remote ) )
                                    {
                                        // Only create backup file ( "moving" log to backup folder ) in interactive mode
                                        if ( ! Data.Get.IsFromScripting )
                                        {
                                            string url_to_copy = Mobile.LogUserBackupPath;
                                            if ( ! Directory.Exists ( url_to_copy ) )
                                                Directory.CreateDirectory ( url_to_copy );
                    
                                            File.Copy ( file.FullName, Path.Combine ( url_to_copy, file.Name ), true );
                                        }
                                        File.Delete ( file.FullName );
                                        
                                        NumFilesUploaded += 1;
                                        
                                        progress.PercentComplete = ( int )( NumFilesUploaded * 100 / filesToUpload.Count );
                                        
                                        Utils.PrintDeep ( "- " + file.Directory.Name + " uploaded" );
                                    }
                                }
                            }
        
                            sftp.Disconnect ();
                        }
                    }
                    catch ( Exception )
                    {
                        // Catch all exceptions and then always show the number of
                        // files uploaded using the exception FtpUpdateLogsException
                    }
                    finally
                    {
                        if ( sftp != null )
                            sftp.Dispose ();
                                                                        
                        // Is necessary for appear the progress full ( 100% )
                        await Task.Delay ( 10 );
                        
                        tcs.SetResult ( NumFilesUploaded == filesToUpload.Count );
                    }
                }
            });

            bool result = await tcs.Task; // Wait for upload completion
            
            // Finish ok if all files have been uploaded
            if ( result )
                return true;
            
            if ( ! cancelled )
                 await Errors.ShowAlert ( new FtpUpdateLogsException ( NumFilesUploaded + "/" + filesToUpload.Count ) );
            else await PageLinker.ShowAlert ( "Uploading canceled", "Only uploaded " + NumFilesUploaded + " / " + filesToUpload.Count + " log files" );
            
            return false;
        }

        public static int NumLogFilesToUpload(string path) 
        {
            return LogFilesToUpload(path).Count;
        }

        public static int NumBackupFiles()
        {
            return BackupFiles().Count;
        }

        public static List<FileInfo> LogFilesToUpload(string path, bool AllFiles = false, bool Events = true, bool Images = true )
        {
            List<FileInfo> local_array_files = new List<FileInfo>();

            DirectoryInfo info = new DirectoryInfo(path);
            info = new DirectoryInfo(info.FullName);

            FileInfo[] files = info.GetFiles("*.xml", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTimeUtc).ToArray();

            foreach (FileInfo file in files)
            {
                if ( file.Directory.Name.ToLower () == Mobile.PATH_BACKUP.ToLower () ||
                     file.Directory.Name.ToLower () == Mobile.PATH_LOGS  .ToLower () )
                    continue;

                if (!Events && file.Name.ToLower().Contains("mtuid"))
                    continue;

                string dayfix = file.Name.ToLower().Substring(0, 10);
                bool LogDateOk = DateTime.TryParseExact(dayfix, "MMddyyyyHH", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                if (LogDateOk)  // activity log
                {
                    if (!AllFiles)  // check the latest file
                    {
                        date = date.ToUniversalTime();
                        TimeSpan diff = date - DateTime.UtcNow;

                        int hours = (int)diff.TotalHours;
                        if (hours < 0)
                            local_array_files.Add(file);
                    }
                    else local_array_files.Add(file);
                }
                else // other logs
                {
                    local_array_files.Add(file);
                }
            }

            if ( Images )
            {
                FileInfo[] filesIm = info.GetFiles("*.jpg", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTimeUtc).ToArray();

                foreach (FileInfo file in filesIm)
                {
                    if (file.Directory.Name.ToLower() == Mobile.PATH_BACKUP.ToLower() ||
                         file.Directory.Name.ToLower() == Mobile.PATH_LOGS.ToLower())
                        continue;

                    local_array_files.Add(file);
                }

            }

            return local_array_files;
        }

        public static List<FileInfo> BackupFiles()
        {
            List<FileInfo> local_array_files = new List<FileInfo>();

            DirectoryInfo info = new DirectoryInfo(Path.Combine(Mobile.LogUserBackupPath,".."));

            FileInfo[] files = info.GetFiles("*.xml", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTimeUtc).ToArray();

            foreach (FileInfo file in files)
            {
                if ( file.Directory.Name.ToLower () != Mobile.PATH_BACKUP.ToLower () )
                    continue;

                local_array_files.Add(file);
            }
            return local_array_files;
        }

        public static bool TestFtpCredentials(string host,string user,string pass,string path, int port = 22)
        {
            bool ok = true;

            try
            {

                using (SftpClient sftp = new SftpClient(host, port, user, pass))
                {
                    sftp.Connect();

                    if (!sftp.Exists(path))
                        ok = false;


                    sftp.Disconnect();
                }
            }
            catch (Exception)
            {
                ok = false;
            }

            return ok;
        }

        public static void SetInstallMode(string Mode)
        {
            SecureStorage.SetAsync(INSTALL_MODE, Mode);
        }

        public static string ChekInstallMode()
        {
            var Mode = SecureStorage.GetAsync ( INSTALL_MODE );

            if ( String.IsNullOrEmpty ( Mode.Result ) )
                return "None";

            return Mode.Result;
        }
        public static bool CheckFTPDownload()
        {
            var Host = SecureStorage.GetAsync("ftpDownload_Host");
            if (!String.IsNullOrEmpty(Host.Result))
            {
                var data = Mobile.ConfData;
                data.FtpDownload_Host = Host.Result;
                data.FtpDownload_Path = SecureStorage.GetAsync("ftpDownload_Path").Result;
                data.FtpDownload_User = SecureStorage.GetAsync("ftpDownload_User").Result;
                data.FtpDownload_Pass = SecureStorage.GetAsync("ftpDownload_Pass").Result;
                data.FtpDownload_Port = int.Parse(SecureStorage.GetAsync("ftpDownload_Port").Result);
                data.HasFTP = true;
                data.HasIntune = false;
                return true;
            }

            return false;
        }

        private static bool CheckConfigFile(string name)
        {
            string sFile;
            foreach (string file in filesToCheck)
            {
                sFile = file + XML_EXT;
                if (name.Equals(sFile))
                    return true;

            }

            if ( name.Equals ( "debugoptions" + XML_EXT ) )
                return true;

            return false;
        }

        public static string CheckFTPConfigVersion()
        {
            string sVersion = string.Empty;
            try
            {
                Mobile.ConfigData data = Mobile.ConfData;
                using (SftpClient sftp = new SftpClient(data.FtpDownload_Host, data.FtpDownload_Port, data.FtpDownload_User, data.FtpDownload_Pass))
                {
                    sftp.Connect();

                    foreach (SftpFile file in sftp.ListDirectory(data.FtpDownload_Path))
                    {
                        if (file.Name.Contains(FIL_VER))
                        {
                            sVersion = file.Name.Substring(0, file.Name.Length - 4); // delete ".ver"
                            break;
                        }
                    }
                    sftp.Disconnect();
                    if (string.IsNullOrEmpty(sVersion))
                        return "Version_0";
                    return sVersion;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string CheckPubConfigVersion()
        {
            string sVersion = string.Empty;
            try
            {
                DirectoryInfo info = new DirectoryInfo(Mobile.ConfigPublicPath);
                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains(FIL_VER))
                    {
                        //sVersion = file.Name.Substring(0, file.Name.Length - 4); // delete ".ver"
                        sVersion = Path.GetFileNameWithoutExtension(file.Name); // delete ".ver"
                        break;
                    }
                }   
                if (string.IsNullOrEmpty(sVersion))
                    return "Version_0";
                return sVersion;
             
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool DownloadConfigFiles( out string sfileCert)
        {
            bool ok = true;
            sfileCert = String.Empty;
            try
            {
                Mobile.ConfigData data = Mobile.ConfData;
                using (SftpClient sftp = new SftpClient(data.FtpDownload_Host, data.FtpDownload_Port, data.FtpDownload_User, data.FtpDownload_Pass))
                {
                    sftp.Connect();

                    // Remote FTP File directory
                    string configPath = Mobile.ConfigPath;

                    foreach (SftpFile file in sftp.ListDirectory(data.FtpDownload_Path))
                    {
                        string name = file.Name;
                        if (name.ToLower().Contains(XML_CER)) sfileCert = name.ToLower();

                        if (name.Contains(XML_CER) || //name.Contains(FIL_VER) ||
                          ( name.Contains(XML_EXT) && CheckConfigFile(name.ToLower())))
                        {
                            string sfile = Path.Combine(configPath, name.ToLower());
                            if (File.Exists(sfile))
                                File.Delete(sfile);
                            using (Stream stream = File.OpenWrite(sfile))  // keep in low case
                            {
                                sftp.DownloadFile(Path.Combine(data.FtpDownload_Path, name), stream);
                            }
                        }
                    }

                    sftp.Disconnect();
                
                    Console.WriteLine("Download config.files from FTP: OK");

                    return ok;
                }
            }
            catch (Exception )
            {
                return false;
            }
                    
        }

        public static  bool GenerateBase64Certificate (string configPath)
        {
            bool   ok         = true;
           
            string txtPath    = Path.Combine ( configPath, CER_TXT );
        
            try
            {
                
                foreach ( string filePath in Directory.GetFiles ( configPath ) )
                {
                    if ( filePath.Contains ( XML_CER ) )
                    {
                        // Convert certificate to base64 string                                
                        string pathCer   = Path.Combine ( configPath, filePath );  // Path to .cer in Library
                        byte[] bytes     = File.ReadAllBytes ( pathCer );          // Read .cer full bytes
                        string strBase64 = Convert.ToBase64String ( bytes );       // Convert bytes to base64 string
                        File.WriteAllText ( txtPath, strBase64 );                  // Create new {name}.txt file with base64 string and delete .cer
                        File.Delete ( pathCer );
                        
                        Utils.Print ( "Certificate to base64 txt: '" + strBase64 + "'" );
                        
                        break;
                    }
                }
            }
            catch ( Exception )
            {
                ok = false;
            }

            if ( File.Exists ( txtPath ) )
                 Console.WriteLine ( "Is the certificate installed correctly? " + ( ( ok ) ? "OK" : "NO" ) );
            else Console.WriteLine ( "No certificate is being used" );

            return ok;
        }

       

        public static bool HasDeviceAllXmls (string path)
        {
            bool ok;
              
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] filesLocal = info.GetFiles();

            int count = 0;
            foreach ( string fileNeeded in filesToCheck )
                foreach (FileInfo file in filesLocal)
                {
                    string compareStr = fileNeeded + XML_EXT;
              
                    string fileStr = file.Name.ToLower();
                 
                    if ( fileStr.Equals ( compareStr ) )
                    {
                        
                        if (!file.Name.Equals(compareStr) && Data.Get.IsIOS) // lower case only in iOS
                        {
                            file.CopyTo(Path.Combine(path, file.Name.ToLower()), true);
                            file.Delete();
                        }
                        count++;
                        break;
                    }
                }

            ok = ( count == filesToCheck.Length );

            Console.WriteLine ( "Are all config.files installed? " + ( ( ok ) ? "OK" : "NO" ) );
            
            return ok;
        }
               
        public static bool CopyConfigFiles(bool bRemove, string sPathFrom, string sPathTo, out string sFileCert)
        {
            sFileCert = string.Empty;
            string fileCopy;
            try
            {
                Mobile.CreateDirectoryIfNotExist(sPathTo);

                DirectoryInfo info = new DirectoryInfo(sPathFrom);
                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains(XML_CER)) sFileCert = file.Name.ToLower();

                    if (file.Name.Contains(XML_CER) || //name.Contains(FIL_VER) ||
                          (file.Name.Contains(XML_EXT) && CheckConfigFile(file.Name.ToLower())))
                    {
                        fileCopy = Path.Combine(sPathTo, file.Name.ToLower());
                        file.CopyTo(fileCopy, true);
                    }
                   
                    if (bRemove) file.Delete();
                }
                return true;
            }
            catch (Exception)
            {         
                return false;
            }
        }

        public static bool GetTagFromGlobalXml(bool bPublic, string sTag, out dynamic value)
        {
          
            string uri;
            if (bPublic)
                uri = Path.Combine(Mobile.ConfigPublicPath, Configuration.XML_GLOBAL);
            else
                uri = Path.Combine(Mobile.ConfigPath, Configuration.XML_GLOBAL);

            XDocument doc = XDocument.Load(uri);
            foreach (XElement xElement in doc.Root.Elements())
            {
                if (xElement.Name == sTag)
                {
                    value = xElement.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public static bool DeleteConfigFiles(string path)
        {
            bool ok;

            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] filesLocal = info.GetFiles();

            int count = 0;
            foreach (string fileNeeded in filesToCheck)
                foreach (FileInfo file in filesLocal)
                {
                    string compareStr = fileNeeded + XML_EXT;
                  
                    string fileStr = file.Name.ToLower();
               
                    if (fileStr.Equals(compareStr))
                    {
                        file.Delete();
                        count++;
                        break;
                    }
                }

            ok = (count == filesToCheck.Length);

            Console.WriteLine("Are all config.files deleted? " + ((ok) ? "OK" : "NO"));

            return ok;
        }

        public static void BackUpConfigFiles()
        {
            string sPathBackup = Path.Combine(Mobile.ConfigPath, "Backup");

            CopyConfigFiles(true, Mobile.ConfigPath, sPathBackup, out string sFileCert);
        }

        public static void RestoreConfigFiles()
        {
            string sPathBackup = Path.Combine(Mobile.ConfigPath, "Backup");

            CopyConfigFiles(true, sPathBackup, Mobile.ConfigPath, out string sFileCert);

            Directory.Delete(sPathBackup);
        }
    }
}
