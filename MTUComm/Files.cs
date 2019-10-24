using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Renci.SshNet.Sftp;
using Renci.SshNet;
using Library.Exceptions;
using System.Collections.Generic;

namespace MTUComm
{
    public class Files
    {
        public static int CheckCorrectUpload (
            string localBasePath,  // Logs and inside a folder per user
            string remoteBasePath, // The same structure but in the FTP
            out List<string> filesUploadedOk ) 
        {
            byte[] md5Local;
            byte[] md5Remote;
            int    filesNotUploaded = 0;
            SftpClient sftp = null;
            filesUploadedOk = new List<string> ();
        
            try
            {
                Mobile.ConfigData data = Mobile.configData;
                using ( sftp = new SftpClient ( data.ftpDownload_Host, data.ftpDownload_Port, data.ftpDownload_User, data.ftpDownload_Pass ) )
                {
                    sftp.Connect ();
            
                    using ( MD5 md5Hash = MD5.Create () )
                    {
                        string dirPath = localBasePath;
                    
                        // Iterate all users local folders and ask for the same files in the remote ( FTP )
                        //foreach ( string dirPath in Directory.GetDirectories ( localBasePath ) )
                        {
                            // Iterate all files for the current user
                            foreach ( string filePath in Directory.GetFiles ( dirPath ) )
                            {
                                int    lastIndex = ( filePath.LastIndexOf ( '\\' ) > -1 ) ? filePath.LastIndexOf ( '\\' ) : filePath.LastIndexOf ( '/' );
                                string fileName = filePath.Substring ( lastIndex + 1 );
                                
                                string remoteFullPath = Path.Combine ( data.ftpDownload_Path, remoteBasePath, fileName );
                                
                                // File is present in the FTP
                                if ( sftp.Exists ( remoteFullPath ) )
                                {
                                    using ( StreamReader stream = new StreamReader ( filePath ) )
                                        md5Local = md5Hash.ComputeHash ( Encoding.UTF8.GetBytes ( stream.ReadToEnd () ) );
                                    
                                    md5Remote = md5Hash.ComputeHash ( Encoding.UTF8.GetBytes ( sftp.ReadAllText ( remoteFullPath ) ) );
                                    
                                    // Compare local and remote files
                                    if ( Enumerable.SequenceEqual ( md5Local, md5Remote ) )
                                        filesUploadedOk.Add ( fileName );
                                    else
                                        filesNotUploaded++; // Remote file is corrupted/modified
                                }
                                else filesNotUploaded++; // File not uploaded to the FTP
                            }
                        }
                    }
                }
            }
            catch ( Exception )
            {
                throw new FtpConnectionException ();
            }
            finally
            {
                if ( sftp != null )
                    sftp.Dispose ();
                
                sftp = null;
            }

            return filesNotUploaded;
        }
    }
}
