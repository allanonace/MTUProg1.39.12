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
using Xamarin.Forms;
using Xml;

namespace aclara_meters
{
    public class GenericUtilsClass
    {
        public static int NumFilesUploaded;

        public async static Task<bool> UploadFiles ( Boolean AllLogs = true )
        {
            Global global = FormsApp.config.global;
        
            // Path where the file should be saved once downloaded (locally)
            string path = ( AllLogs ) ? Mobile.LogPath : Mobile.LogUserPath;
            
            // Only upload if there are files available
            List<FileInfo> filesToUpload = LogFilesToUpload ( path );
            
            var upload = false;

            if (global.UploadPrompt &&
                 filesToUpload.Count > 0)
                upload = await Application.Current.MainPage.DisplayAlert(
                        "Pending log files",
                        "Do you want to Upload them?",
                        "Ok", "Cancel");
            else if (filesToUpload.Count > 0) upload = true;
            
            if ( ! upload )
                return false;
        
            // The FTP credentiales are not present in Global.xml
            if ( ! global.IsFtpUploadSet )
            {
                Errors.LogErrorNowAndContinue( new FtpCredentialsMissingException () );
                return false;
            }
            
            // Has the devices internet connection
            if ( ! Mobile.IsNetAvailable () )
            {
                Errors.LogErrorNowAndContinue( new NoInternetException () );
                return false;
            }
            
            // Cancel action
            bool cancelled = false;
            System.Action OnCancel = () =>
            {
                cancelled = true;
            };

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
                                    string url_to_copy = Path.Combine ( file.Directory.FullName, Mobile.PATH_BACKUP );
                                    if ( ! Directory.Exists ( url_to_copy ) )
                                        Directory.CreateDirectory ( url_to_copy );
            
                                    File.Copy ( file.FullName, Path.Combine ( url_to_copy, file.Name ), true );
                                    File.Delete ( file.FullName );
                                    
                                    NumFilesUploaded += 1;
                                    
                                    progress.PercentComplete = ( int )( NumFilesUploaded * 100 / filesToUpload.Count );
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
                }
            }

            if ( NumFilesUploaded == filesToUpload.Count )
                return true;
            
            if ( ! cancelled )
                 Errors.LogErrorNowAndContinue( new FtpUpdateLogsException ( NumFilesUploaded + "/" + filesToUpload.Count ) );
            else PageLinker.ShowAlert ( "Uploading canceled", "Only uploaded " + NumFilesUploaded + " / " + filesToUpload.Count + " log files" );
            
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
                        TimeSpan diff = date - DateTime.Now;
                    
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

            DirectoryInfo info = new DirectoryInfo(Mobile.LogPath);

            FileInfo[] files = info.GetFiles("*.xml", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTimeUtc).ToArray();

            foreach (FileInfo file in files)
            {
                if ( file.Directory.Name.ToLower () != Mobile.PATH_BACKUP.ToLower () )
                    continue;

                local_array_files.Add(file);
            }
            return local_array_files;
        }
    }
}
