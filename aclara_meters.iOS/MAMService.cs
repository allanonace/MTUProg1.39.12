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
        public void LoginUserMAM()
        {
            try
            {

                string user = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
                Console.WriteLine($"=========== User: {user}");
                //IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;
                if (string.IsNullOrEmpty(user))
                {
                    IntuneMAMEnrollmentManager.Instance.Delegate = new MainControllerEnrollmentDelegate(); 
                    IntuneMAMEnrollmentManager.Instance.LoginAndEnrollAccount(null);
                    user = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
                    Console.WriteLine($"=========== Login User: {user}");
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
                Console.WriteLine($"=========== Enrrolled User: {user}");
                //user= null;              
                var stringValues = new Dictionary<string,string> ();
                var numberValues = new Dictionary<string, int>();

                IntuneMAMAppConfig appConfig = IntuneMAMAppConfigManager.Instance.AppConfigForIdentity(user);
               
                var fullData = appConfig.FullData;
                foreach (var i in fullData)
                {
#pragma warning disable S3217 // "Explicit" conversions of "foreach" loops should not be used
                    foreach (NSString key in i.Keys)
#pragma warning restore S3217 // "Explicit" conversions of "foreach" loops should not be used
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
                {
                    stringValues.TryGetValue(Mobile.ID_FTP_HOST, out string dataValue);
                    data.FtpDownload_Host = dataValue;
                }
                else return;
                if (stringValues.ContainsKey(Mobile.ID_FTP_PATH))
                {
                    stringValues.TryGetValue(Mobile.ID_FTP_PATH, out string dataValue);
                    data.FtpDownload_Path = dataValue;
                }
                if (stringValues.ContainsKey(Mobile.ID_FTP_PASS))
                {
                    stringValues.TryGetValue(Mobile.ID_FTP_PASS, out string dataValue);
                    data.FtpDownload_Pass = dataValue;
                }
                if (stringValues.ContainsKey(Mobile.ID_FTP_USER))
                {
                    stringValues.TryGetValue(Mobile.ID_FTP_USER, out string dataValue);
                    data.FtpDownload_User = dataValue;
                }
                if (numberValues.ContainsKey(Mobile.ID_FTP_PORT))
                {
                    numberValues.TryGetValue(Mobile.ID_FTP_PORT, out int dataValue);
                    data.FtpDownload_Port = dataValue;
                }
                else if (stringValues.ContainsKey(Mobile.ID_FTP_PORT))
                {
                    stringValues.TryGetValue(Mobile.ID_FTP_PORT, out string dataValue);
                    if (int.TryParse(dataValue, out int value))
                        data.FtpDownload_Port = value;
                }

                data.HasIntune = true;

                string certificate = string.Empty;
                if (stringValues.ContainsKey(Mobile.ID_CERTIFICATE))
                {
                    stringValues.TryGetValue(Mobile.ID_CERTIFICATE, out certificate);
                    data.StoreCertificate(data.CreateCertificate(certificate));  //save the certificate in keychain
                    //data.GenerateCertFromStore();
                    //data.GenerateCert(certificate);
                }
               
            }
            catch (Exception e )
            {
                return;
            }
        }
    }
    public class MainControllerEnrollmentDelegate : IntuneMAMEnrollmentDelegate
    {

        public override void EnrollmentRequestWithStatus(IntuneMAMEnrollmentStatus status)
        {
            if (status.DidSucceed)
            {
                Console.WriteLine($"Enrollment Ok: {status.Identity}");
                //this.ViewController.HideLogInButton();
            }
            else if (IntuneMAMEnrollmentStatusCode.MAMEnrollmentStatusLoginCanceled != status.StatusCode)
            {
                //this.ViewController.ShowAlert("Enrollment Failed", status.ErrorString);
                Console.WriteLine($"Enrollment Failed: {status.ErrorString}");
            }
        }

        public override void UnenrollRequestWithStatus(IntuneMAMEnrollmentStatus status)
        {
            if (status.DidSucceed)
            {
               // this.ViewController.HideLogOutButton();
            }
            else
            {
                //this.ViewController.ShowAlert("Unenroll Failed", status.ErrorString);
            }
        }
    }
}
