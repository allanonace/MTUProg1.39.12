using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Library;
using Microsoft.Intune.MAM;
using MTUComm;
using System.IO;
using Library.Exceptions;
using Xamarin.Essentials;

namespace aclara_meters.iOS
{
    public sealed class Parameters
    {
        public static bool PrepareFromIntune ()
        {
            List <string> listaDatos = new List<string> ();

            try
            {
                // Recover configuration parameters from Intune
                IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;
                NSDictionary dictionary      = value.DiagnosticInformation;
                NSString[]   keys            = new NSString[] { new NSString ( Mobile.ID_DIC_INTUNE ) };
                NSDictionary paramsGroups    = dictionary.GetDictionaryOfValuesFromKeys ( keys );
                NSObject     paramsGroup     = paramsGroups.ElementAt ( 0 ).Value;

                var data = Mobile.ConfData;

                // Convert parameters to string and regenerate the certificate
                data.FtpDownload_User =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_USER    ) ).ToString ();
                data.FtpDownload_Pass =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PASS    ) ).ToString ();
                data.FtpDownload_Host =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_HOST    ) ).ToString ();
                data.FtpDownload_Port = int.Parse ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PORT    ) ).ToString () );
                data.FtpDownload_Path =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PATH    ) ).ToString ();
                data.GenerateCert        ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_CERTIFICATE ) ).ToString () );
                data.HasIntune = true;
                
                Utils.Print ( "Intune parameters loaded.." );
                Utils.Print ( "FTP: " + data.FtpDownload_Host + ":" + data.FtpDownload_Port + " - " + data.FtpDownload_User + " [ " + data.FtpDownload_Pass + " ]" );
                Utils.Print ( "Certificate: " + data.certificate.FriendlyName + " [ " + data.certificate.NotAfter + " ]" );
                
                // Free memory
                paramsGroup .Dispose ();
                paramsGroups.Dispose ();
                dictionary  .Dispose ();
                value       .Dispose ();
                paramsGroup  = null;
                paramsGroups = null;
                keys         = null;
                dictionary   = null;
                value        = null;
            }
            catch ( Exception e )
            {
                return false;
            }
            return true;
        }
    }
}
