using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace MTUComm
{
    public class Files
    {
        public static bool CheckCorrectUpload (
            string localBasePath,   // Logs and inside a folder per user
            string remoteBasePath ) // The same structure but in the FTP
        {
            byte[] md5Local;
            byte[] md5Remote;
            bool   ok = true;
            SftpClient sftp = null;
        
            try
            {
                Mobile.ConfigData data = Mobile.configData;
                using ( sftp = new SftpClient ( data.ftpHost, data.ftpPort, data.ftpUser, data.ftpPass ) )
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
                                
                                if ( sftp.Exists ( Path.Combine ( data.ftpPath, remoteBasePath, fileName ) ) )
                                {
                                    using ( StreamReader stream = new StreamReader ( localBasePath ) )
                                        md5Local = md5Hash.ComputeHash ( Encoding.UTF8.GetBytes ( stream.ReadToEnd () ) );
                                        
                                    md5Remote = sftp.ReadAllBytes ( remoteBasePath );
                                    
                                    if ( ! ( ok = Enumerable.SequenceEqual ( md5Local, md5Remote ) ) )
                                        break;
                                }
                                // File not present in the FTP/not uploaded to the FTP
                                else
                                {
        
                                }
                            }
                        }
                    }
                    // Some file is not the same in remote than in local/device
                    if ( ! ok )
                    {
                        
                    }
                }
            }
            catch ( Exception e )
            {
                ok = false;
            }
            finally
            {
                if ( sftp != null )
                    sftp.Disconnect ();
            }

            return ok;
        }
    }
}
