﻿using System;
using Xamarin.Forms;
using aclara_meters.util;
#pragma warning disable S125 // Sections of code should not be "commented out"
                            //using Library;
                            //using MTUComm;
                            //using Microsoft.Intune.Mam.Policy;
                            //using Microsoft.Intune.Mam.Client.App;
                            //using Microsoft.Intune.Mam.Policy.AppConfig;
                            //using System.Collections.Generic;
#pragma warning restore S125 // Sections of code should not be "commented out"

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
            #region Only for MAM application
            /*
            IMAMAppConfigManager _configManager;
            IMAMUserInfo _userInfo;
            try
            {

                _userInfo = MAMComponents.Get<IMAMUserInfo>();

                _configManager = MAMComponents.Get<IMAMAppConfigManager>();

                _userInfo = MAMComponents.Get<IMAMUserInfo>();

                string identity = _userInfo.PrimaryUser;
                Utils.Print($"----------------------------------------------------------  va a buscar la configuracion de: {identity}  ");

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
                catch (Exception )
                {
                    return;
                }
                var data = Mobile.configData;

                if (dict.ContainsKey(Mobile.ID_FTP_HOST))
                {
                    dict.TryGetValue(Mobile.ID_FTP_HOST, out string dataValue);
                    data.FtpDownload_Host = dataValue;
                }
                else
                    return;
                // Convert parameters to string and regenerate the certificate
                if (dict.ContainsKey(Mobile.ID_FTP_USER))
                {
                    dict.TryGetValue(Mobile.ID_FTP_USER, out string dataValue);
                    data.FtpDownload_User = dataValue;
                }
                if (dict.ContainsKey(Mobile.ID_FTP_PATH))
                {
                    dict.TryGetValue(Mobile.ID_FTP_PATH, out string dataValue);
                    data.FtpDownload_Path = dataValue;
                }
                if (dict.ContainsKey(Mobile.ID_FTP_PASS))
                {
                    dict.TryGetValue(Mobile.ID_FTP_PASS, out string dataValue);
                    data.FtpDownload_Pass = dataValue;
                }
                if (dict.ContainsKey(Mobile.ID_FTP_PORT))
                {
                    dict.TryGetValue(Mobile.ID_FTP_PORT, out string dataValue);
                    if (int.TryParse(dataValue,out int value))
                        data.FtpDownload_Port = value;
                }

                data.HasIntune = true;
                if (dict.TryGetValue(Mobile.ID_CERTIFICATE, out string certificate))
                {
                    data.StoreCertificate(data.CreateCertificate(certificate));  //save the certificate in keychain
                }

            }
            catch (Exception)
            {
                return;
            } 
            */
            #endregion
        }
    }
}
