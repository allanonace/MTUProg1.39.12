using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Library;
using Microsoft.Intune.MAM;
using MTUComm;

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

                var data = Mobile.configData;

                // Convert parameters to string and regenerate the certificate
                data.ftpDownload_User =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_USER    ) ).ToString ();
                data.ftpDownload_Pass =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PASS    ) ).ToString ();
                data.ftpDownload_Host =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_HOST    ) ).ToString ();
                data.ftpDownload_Port = int.Parse ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PORT    ) ).ToString () );
                data.ftpDownload_Path =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PATH    ) ).ToString ();
                //data.GenerateCert        ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_CERTIFICATE ) ).ToString () );
                
                Utils.Print ( "Intune parameters loaded.." );
                Utils.Print ( "FTP: " + data.ftpDownload_Host + ":" + data.ftpDownload_Port + " - " + data.ftpDownload_User + " [ " + data.ftpDownload_Pass + " ]" );
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
