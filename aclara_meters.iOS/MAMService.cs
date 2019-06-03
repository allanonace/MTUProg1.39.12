using System;
using Xamarin.Forms;
using Microsoft.Intune.MAM;
using aclara_meters.util;
using Foundation;
using Library;
using MTUComm;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

[assembly: Dependency(typeof(aclara_meters.iOS.MAMService))]
namespace aclara_meters.iOS
{
    public class MAMService : IMAMService
    {
        public static void LoginUserMAM()
        {
            try
            {

                string user = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
                //IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;
                if (string.IsNullOrEmpty(user))
                {
                     IntuneMAMEnrollmentManager.Instance.LoginAndEnrollAccount(null);
                }
              
            }
            catch (Exception e)
            {
                Utils.Print($"Enrollment exceptions: {e.ToString()}");
            }
        }

        public void UtilMAMService()
        {
            try
            {
               
                string user = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
                //user= null;              
                var stringValues = new Dictionary<string,string> ();
                var numberValues = new Dictionary<string, int>();

                IntuneMAMAppConfig appConfig = IntuneMAMAppConfigManager.Instance.AppConfigForIdentity(user);
               
                var fullData = appConfig.FullData;
                foreach (var i in fullData)
                {
                    foreach (NSString key in i.Keys)
                    {
                        var val = i.ValueForKey(key);
                        if (val is NSString)
                            stringValues.Add(key, (string)(val as NSString));
                        else if (val is NSNumber)
                            numberValues.Add(key, (int)(val as NSNumber));
                    }
                }

                var data = Mobile.configData;

                // Convert parameters to string and regenerate the certificate
                if (stringValues.ContainsKey(Mobile.ID_FTP_HOST))
                    stringValues.TryGetValue(Mobile.ID_FTP_HOST, out data.ftpDownload_Host);
                else return;
                if (stringValues.ContainsKey(Mobile.ID_FTP_PATH))
                    stringValues.TryGetValue(Mobile.ID_FTP_PATH, out data.ftpDownload_Path);
                if (stringValues.ContainsKey(Mobile.ID_FTP_PASS))
                    stringValues.TryGetValue(Mobile.ID_FTP_PASS, out data.ftpDownload_Pass);
                if (stringValues.ContainsKey(Mobile.ID_FTP_USER))
                    stringValues.TryGetValue(Mobile.ID_FTP_USER, out data.ftpDownload_User);
                if (numberValues.ContainsKey(Mobile.ID_FTP_PORT))
                    numberValues.TryGetValue(Mobile.ID_FTP_PORT, out data.ftpDownload_Port);

                data.HasIntune = true;

                string certificate = string.Empty;
                if (stringValues.ContainsKey(Mobile.ID_CERTIFICATE))
                {
                    stringValues.TryGetValue(Mobile.ID_CERTIFICATE, out certificate);
                    data.StoreCertificate(data.CreateCertificate(certificate));  //save the certificate in keychain
                    data.GenerateCertFromStore();
                    //data.GenerateCert(certificate);
                }
               
            }
            catch (Exception e )
            {
                return;
            }
        }
    }
}
