using System;
using Xamarin.Forms;
using aclara_meters.util;
using Library;
using MTUComm;
using System.Linq;
using Microsoft.Intune.Mam.Policy;
using Microsoft.Intune.Mam.Client.App;
using Microsoft.Intune.Mam.Policy.AppConfig;

using System.Collections.Generic;

[assembly: Dependency(typeof(aclara_meters.Droid.MAMService))]
namespace aclara_meters.Droid
{

    public class MAMService : IMAMService
    {
        public void LoginUserMAM()
        {
            throw new NotImplementedException();
        }

        public void UtilMAMService()
        {
            IMAMAppConfigManager _configManager;
            // private IMAMEnrollmentManager _enrolledMgr;
            IMAMUserInfo _userInfo;
            try
            {
                //_enrolledMgr = MAMComponents.Get<IMAMEnrollmentManager>();
                _userInfo = MAMComponents.Get<IMAMUserInfo>();
                // if (_enrolledMgr.GetRegisteredAccountStatus(_userInfo.PrimaryUser) == MAMEnrollmentManagerResult.EnrollmentSucceeded)
                // {
                _configManager = MAMComponents.Get<IMAMAppConfigManager>();

                _userInfo = MAMComponents.Get<IMAMUserInfo>();

                string identity = _userInfo.PrimaryUser;
                Utils.PrintDeep($"----------------------------------------------------------  va a buscar la configuracion de: {identity}  ");
                //identity = "h.foronda@bizintekinnova.com";
                var stringValues = new List<Dictionary<string, string>>();
                var dict = new Dictionary<string, string>();
                try
                {
                    var items = _configManager.GetAppConfig(identity);
                    if (items != null)
                    {
                        foreach (var item in items.FullData)
                        {
                            //var dict = new Dictionary<string, string>();
                            foreach (var key in item.Keys)
                            {
                                var value = item[key];
                                dict.Add(key, value);
                            }
                            stringValues.Add(dict);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
                var data = Mobile.configData;

                if (dict.ContainsKey(Mobile.ID_FTP_HOST))
                    dict.TryGetValue(Mobile.ID_FTP_HOST, out data.ftpDownload_Host);
                else
                    return;
                // Convert parameters to string and regenerate the certificate
                if (dict.ContainsKey(Mobile.ID_FTP_USER))
                    dict.TryGetValue(Mobile.ID_FTP_USER, out data.ftpDownload_User);
                if (dict.ContainsKey(Mobile.ID_FTP_PORT))
                {
                    dict.TryGetValue(Mobile.ID_FTP_PORT, out string Port);
                    data.ftpDownload_Port = int.Parse(Port);
                }
                if (dict.ContainsKey(Mobile.ID_FTP_PATH))
                    dict.TryGetValue(Mobile.ID_FTP_PATH, out data.ftpDownload_Path);
                if (dict.ContainsKey(Mobile.ID_FTP_PASS))
                    dict.TryGetValue(Mobile.ID_FTP_PASS, out data.ftpDownload_Pass);


                data.HasIntune = true;
                string certificate = string.Empty;
                if (dict.TryGetValue(Mobile.ID_CERTIFICATE, out certificate))
                {
                    data.StoreCertificate(data.CreateCertificate(certificate));  //save the certificate in keychain
                    //data.GenerateCertFromStore();
                    //data.GenerateCert(certificate);
                }

            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
