using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MTUComm;
using MTUComm.Exceptions;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;

namespace aclara_meters
{
    public class GenericUtilsClass
    {
        public static int NumFilesUploaded;

        public async static Task<bool> UploadFiles (Boolean UploadPrompt = true, Boolean AllLogs = true )
        {
            Global global = FormsApp.config.global;
        
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
  //          else upload = false;
            
            
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
                                        remotePath = global.ftpRemotePath + file.Directory.Name; // Logs + User folder
                                        
                                        if ( ! sftp.Exists ( remotePath ) )
                                            sftp.CreateDirectory ( remotePath );
    
                                        // File path
                                        string sTick = DateTime.Now.Ticks.ToString();
                                        string sName = file.Name.Substring(0, 10) + "-" + sTick + "Log.xml";
                                        remotePath = Path.Combine ( remotePath, sName );
                                    
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
                                        if ( ! MTUComm.Action.IsFromScripting )
                                        {
                                            string url_to_copy = Mobile.LogUserBackupPath;// Path.Combine ( file.Directory.FullName, Mobile.PATH_BACKUP );
                                            if ( ! Directory.Exists ( url_to_copy ) )
                                                Directory.CreateDirectory ( url_to_copy );
                    
                                            File.Copy ( file.FullName, Path.Combine ( url_to_copy, file.Name ), true );
                                        }
                                        File.Delete ( file.FullName );
                                        
                                        NumFilesUploaded += 1;
                                        
                                        progress.PercentComplete = ( int )( NumFilesUploaded * 100 / filesToUpload.Count );
                                        
                                        Console.WriteLine ( "- " + file.Directory.Name + " uploaded" );
                                    }
                                }
                            }
        
                            sftp.Disconnect ();
                        }
                    }
                    catch ( Exception e )
                    {
                        // Catch all exceptions and then always show the number of
                        // files uploaded using the exception FtpUpdateLogsException
                    }
                    finally
                    {
                        if ( sftp != null )
                            sftp.Dispose ();
                        
                        sftp = null;
                        
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

        public static List<FileInfo> LogFilesToUpload(string path, bool AllFiles = false )
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
                
                if ( file.Name.ToLower ().Contains ("result") )
                    local_array_files.Add ( file );
                else
                {

	                if (!AllFiles)
                    {
                        string dayfix = file.Name.ToLower().Substring(0, 10);
                        DateTime date = DateTime.ParseExact(dayfix, "MMddyyyyHH", CultureInfo.InvariantCulture).ToUniversalTime();
                        TimeSpan diff = date - DateTime.UtcNow;
                    
                        int hours = ( int )diff.TotalHours;
                        if ( hours < 0 )
                           local_array_files.Add(file);
                    }
                    else local_array_files.Add(file);
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

            //Console.WriteLine("Download config.files from FTP: " + ((ok) ? "OK" : "NO"));

            return ok;
        }

        public static bool CheckFTPDownload()
        {
            var Host = SecureStorage.GetAsync("ftpDownload_Host");
            if (!String.IsNullOrEmpty(Host.Result))
            {
                var data = Mobile.configData;
                data.ftpDownload_Host = Host.Result;
                data.ftpDownload_Path = SecureStorage.GetAsync("ftpDownload_Path").Result;
                data.ftpDownload_User = SecureStorage.GetAsync("ftpDownload_User").Result;
                data.ftpDownload_Port = int.Parse(SecureStorage.GetAsync("ftpDownload_Port").Result);
                data.HasFTP = true;
                return true;
            }

            return false;
        }

        public static bool DownloadConfigFiles()
        {
            bool ok = true;

            try
            {
                Mobile.ConfigData data = Mobile.configData;
                using (SftpClient sftp = new SftpClient(data.ftpDownload_Host, data.ftpDownload_Port, data.ftpDownload_User, data.ftpDownload_Pass))
                {
                    sftp.Connect();

                    // List all posible files in the documents directory 
                    // Check if file's lastwritetime is the lastest 
                    List<SftpFile> ftp_array_files = new List<SftpFile>();

                    // Remote FTP File directory
                    bool isCertificate;
                    string configPath = Mobile.ConfigPath;

                    foreach (SftpFile file in sftp.ListDirectory(data.ftpDownload_Path))
                    {
                        string name = file.Name;

                        if ((isCertificate = name.Contains(FormsApp.XML_CER)) ||
                             name.Contains(FormsApp.XML_EXT))
                        {
                            using (Stream stream = File.OpenWrite(Path.Combine(configPath, name)))
                            {
                               sftp.DownloadFile(Path.Combine(data.ftpDownload_Path, name), stream);
                            }
                        }
                    }

                    sftp.Disconnect();
                }
            }
            catch (Exception e)
            {
                ok = false;
            }

            Console.WriteLine("Download config.files from FTP: " + ((ok) ? "OK" : "NO"));

            return ok;
        }
    }
}
