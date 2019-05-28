using System;
using Xamarin.Forms;
using Microsoft.Intune.MAM;
using aclara_meters.util;
using Foundation;
using Library;
using MTUComm;
using System.Linq;

[assembly: Dependency(typeof(aclara_meters.iOS.MAMService))]
namespace aclara_meters.iOS
{

    public class MAMService : IMAMService
    {
        
        void IMAMService.UtilMAMService()
        {
            try
            {
                //IntuneMAMAppConfigManager appConfig = IntuneMAMAppConfigManager.Instance;
                // IntuneMAMAppConfig conf = IntuneMAMAppConfig.;
                //  NSDictionary paramsGroups1 =IntuneMAMAppConfig_Extensions.

                IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;

                Utils.Print($"------ Es ManagementEnabled:  {value.IsManagementEnabled.ToString()}");

                NSDictionary dictionary = value.DiagnosticInformation;
                NSString[] keys = { new NSString(Mobile.ID_DIC_INTUNE) };
                NSDictionary paramsGroups = dictionary.GetDictionaryOfValuesFromKeys(keys);
                //NSDictionary paramsGroups    = IntuneMAMAppConfigManager.Instance.GetDictionaryOfValuesFromKeys(keys);

                //IntuneMAMAppConfig_Extensions.GetFullData(AppConfig);
                NSObject paramsGroup = paramsGroups.ElementAt(0).Value;

                var data = Mobile.configData;

                // Convert parameters to string and regenerate the certificate
                data.ftpDownload_User = paramsGroup.ValueForKey(new NSString(Mobile.ID_FTP_USER)).ToString();
                data.ftpDownload_Pass = paramsGroup.ValueForKey(new NSString(Mobile.ID_FTP_PASS)).ToString();
                data.ftpDownload_Host = paramsGroup.ValueForKey(new NSString(Mobile.ID_FTP_HOST)).ToString();
                data.ftpDownload_Port = int.Parse(paramsGroup.ValueForKey(new NSString(Mobile.ID_FTP_PORT)).ToString());
                data.ftpDownload_Path = paramsGroup.ValueForKey(new NSString(Mobile.ID_FTP_PATH)).ToString();
                data.HasIntune = true;
                data.GenerateCert(paramsGroup.ValueForKey(new NSString(Mobile.ID_CERTIFICATE)).ToString());

                Utils.Print("Intune parameters loaded..");
                Utils.Print("FTP: " + data.ftpDownload_Host + ":" + data.ftpDownload_Port + " - " + data.ftpDownload_User + " [ " + data.ftpDownload_Pass + " ]");
                //Utils.Print("Certificate: " + data.certificate.FriendlyName + " [ " + data.certificate.NotAfter + " ]");

                // Free memory
                paramsGroup .Dispose();
                paramsGroups.Dispose();
                dictionary  .Dispose();
                value       .Dispose();
                paramsGroup  = null;
                paramsGroups = null;
                keys         = null;
                dictionary   = null;
                value        = null;
            }
            catch (Exception e )
            {
                return;
            }
        }
    }
}
