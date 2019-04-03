using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MTUComm;
using MTUComm.Exceptions;
using Plugin.DeviceInfo;
using Renci.SshNet;
using Xamarin.Forms;
using Xml;
using System.Security.Cryptography;
using System.Text;

namespace aclara_meters
{
    public class GenericUtilsClass
    {
        public static int NumFilesUploaded;

        public static async Task<bool> UploadFilesTask ()
        {
            return await UploadLogFiles ( true );
        }

        public static async Task<bool> UploadFilesTaskScripting ()
        {
            return await UploadLogFiles ( true );
        }

        public async static Task<bool> UploadLogFiles ( Boolean AllLogs )
        {
            Global global = FormsApp.config.global;
        
            // Path where the file should be saved once downloaded (locally)
            string path = ( AllLogs ) ? Mobile.LogPath : Mobile.LogUserPath;
            
            // Only upload if there are files available
            List<FileInfo> filesToUpload = LogFilesToUpload ( path );
            
            var upload = false;
            if ( global.UploadPrompt &&
                 filesToUpload.Count > 0 )
                upload = await Application.Current.MainPage.DisplayAlert (
                        "Pending log files",
                        "Do you want to Upload them?",
                        "Ok", "Cancel" );
        
            if ( ! upload )
                return false;
        
            // The FTP credentiales are not present in Global.xml
            if ( ! global.IsFtpUploadSet )
            {
                Errors.LogErrorNowAndKill ( new FtpCredentialsMissingException () );
                return false;
            }
            
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
                            // Upload local file to the FTP server
                            using ( FileStream fileStream = new FileStream ( file.FullName, FileMode.Open ) )
                            {
                                // Folder path
                                remotePath = global.ftpRemotePath + file.Directory.Name; // Logs + User folder
                                
                                if ( ! sftp.Exists ( remotePath ) )
                                    sftp.CreateDirectory ( remotePath );
                                
                                // File path
                                remotePath = Path.Combine ( remotePath, file.Name );
                            
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

            if ( NumFilesUploaded == filesToUpload.Count )
                return true;
            
            Errors.LogErrorNow ( new FtpUpdateLogsException ( NumFilesUploaded + "/" + filesToUpload.Count ) );
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

        public static List<FileInfo> LogFilesToUpload(string path)
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
                    string dayfix = file.Name.ToLower ().Split('.')[0].Replace ( "log", "" );
                    DateTime date = DateTime.ParseExact(dayfix, "MMddyyyyHH", CultureInfo.InvariantCulture).ToUniversalTime();
                    TimeSpan diff = date - DateTime.UtcNow;
                    
                    int hours = ( int )diff.TotalHours;
                    if ( hours < 0 )
                        local_array_files.Add(file);
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
