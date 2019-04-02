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

namespace aclara_meters
{
    public class GenericUtilsClass
    {
        public static int NumFilesUploaded;
        public static int NumFiles;

        public static async Task UploadFilesTask(Boolean UploadPrompt)
        {
            var resp = false;

            if (UploadPrompt)
                resp = await Application.Current.MainPage.DisplayAlert("Alert", "Detected pending log files. Do you want to Upload them?", "Ok", "Cancel");
            else
                resp = !UploadPrompt;

            if (resp)
            {
                if (GenericUtilsClass.UploadLogFiles(true))
                {
                    //await Application.Current.MainPage.DisplayAlert("Alert", "Log files successfully uploaded", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Log files could not be uploaded", "Ok");
                }
            }
        }



        public static async Task<Boolean> UploadFilesTaskScripting()
        {
            return GenericUtilsClass.UploadLogFiles(true);
        }

        public static bool UploadLogFiles(Boolean AllLogs)
        {

            string host = FormsApp.config.global.ftpRemoteHost;
            string username = FormsApp.config.global.ftpUserName;
            string password = FormsApp.config.global.ftpPassword;
            string remotepath = FormsApp.config.global.ftpRemotePath;

            if (String.IsNullOrEmpty(host) || string.IsNullOrEmpty(username)
                    || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(remotepath))
            {
                Errors.LogErrorNowAndKill(new FtpCredentialsMissingException());
                return false;
            }
            //string pathRemoteFile = "/home/aclara/"; // prueba_archivo.xml";
            NumFilesUploaded = 0;
            NumFiles = 0;
            //TODO: UUID MOVIL EN PATH REMOTE FILE
            //string pathRemoteFile = FormsApp.config.global.ftpRemotePath + CrossDeviceInfo.Current.Id + "/"; // prueba_archivo.xml";
            string pathRemoteFile = FormsApp.config.global.ftpRemotePath;// + FormsApp.credentialsService.UserName + "/";

            // Path where the file should be saved once downloaded (locally)
            string path;
            if (AllLogs)
                path = Mobile.LogPath;
            else
                path = Mobile.LogUserPath;

            //string name = "ReadMtuResult.xml";
            //string filename = Path.Combine(xml_documents, name);
            List<FileInfo> local_array_files = new List<FileInfo>();

            local_array_files = LogFilesToUpload(path);
            NumFiles = local_array_files.Count;
            if (NumFiles == 0) return true;

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    // if not exist create the remote path from global.xml
                    if (!sftp.Exists(pathRemoteFile))
                    {
                        sftp.CreateDirectory(pathRemoteFile);
                    }

                    foreach (FileInfo file in local_array_files)
                    {
                        var fileStream = new FileStream(file.FullName, FileMode.Open);

                        string sDir = file.Directory.Name;
                        pathRemoteFile = FormsApp.config.global.ftpRemotePath + sDir; //+ "/";
                        if (!sftp.Exists(pathRemoteFile))
                        {
                            sftp.CreateDirectory(pathRemoteFile);
                        }

                        if (fileStream != null)
                        {
                            NumFilesUploaded += 1;
                            sftp.UploadFile(fileStream, Path.Combine(pathRemoteFile, file.Name), null);
                        }
                        long cont = fileStream.Length;
                        fileStream.Close();

                        #region Create copy of deleted files to another dir

                        string url_to_copy = Path.Combine(file.Directory.FullName, "backup");
                        if (!Directory.Exists(url_to_copy))
                            Directory.CreateDirectory(url_to_copy);

                        File.Copy(file.FullName, Path.Combine(url_to_copy, file.Name), true);


                        #endregion

                        File.Delete(file.FullName);
                    }

                    sftp.Disconnect();

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught " + e.ToString());
                }
            }

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

            NumFiles = files.Length;
            foreach (FileInfo file in files)
            {
                if (file.Directory.Name == "backup" || file.Directory.Name == "Logs") continue;
                if (file.Name.Contains("Result"))
                {
                    local_array_files.Add(file);
                }
                else
                {
                    string dayfix = file.Name.Split('.')[0].Replace("Log", "");
                    DateTime date = DateTime.ParseExact(dayfix, "MMddyyyyHH", CultureInfo.InvariantCulture).ToUniversalTime();
                    TimeSpan diff = date - DateTime.UtcNow;
                    int hours = (int)diff.TotalHours;
                    if (hours < 0)
                    {
                        local_array_files.Add(file);
                    }

                }
            }
            return local_array_files;
        }

        public static List<FileInfo> BackupFiles()
        {
            List<FileInfo> local_array_files = new List<FileInfo>();

            DirectoryInfo info = new DirectoryInfo(Mobile.LogPath);

            FileInfo[] files = info.GetFiles("*.xml", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTimeUtc).ToArray();

            NumFiles = files.Length;
            foreach (FileInfo file in files)
            {
                if (file.Directory.Name != "backup") continue;

                local_array_files.Add(file);

            }
            return local_array_files;
        }
    }
}
