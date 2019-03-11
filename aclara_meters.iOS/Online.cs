using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Microsoft.Intune.MAM;
using MTUComm;

namespace aclara_meters.iOS
{
    public sealed class Online
    {
        public static bool DownloadIntuneParameters ()
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

                var data = Mobile.configData = new Mobile.ConfigData ();

                // Convert parameters to string and regenerate the certificate
                data.ftpUser =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_USER    ) ).ToString ();
                data.ftpPass =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PASS    ) ).ToString ();
                data.ftpHost =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_HOST    ) ).ToString ();
                data.ftpPort = int.Parse ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PORT    ) ).ToString () );
                data.ftpPath =             paramsGroup.ValueForKey ( new NSString ( Mobile.ID_FTP_PATH    ) ).ToString ();
                data.GenerateCert        ( paramsGroup.ValueForKey ( new NSString ( Mobile.ID_CERTIFICATE ) ).ToString () );
                
                Console.WriteLine ( "FTP: " + data.ftpHost + ":" + data.ftpPort + " - " + data.ftpUser + " [ " + data.ftpPass + " ]" );
                Console.WriteLine ( "Certificate: " + data.certificate.FriendlyName + " [ " + data.certificate.NotAfter + " ]" );
                
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
