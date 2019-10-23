using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using nexus.protocols.ble;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aclara_meters.view
{

    public partial class AclaraViewConfig
    {
        #region Constants

        private const bool DEBUG_MODE_ON = false;

        private const string SO_ANDROID = "Android";
        private const string SO_IOS = "iOS";
        private const string SO_UNKNOWN = "Unknown";

        #endregion
        private Configuration config;
        public string ConfigVersion;
        public string NewConfigVersion;
        private bool checkConfigFiles = false;
        private string DateCheck;

        private bool formsInitFailed;
        public AclaraViewConfig(IUserDialogs dialogs)
        {
            InitializeComponent();
            indicator.IsVisible = true;
            txtLoading.IsVisible = true;
            Task.Run(() =>
            {
                LoadConfigurationAndOpenScene(dialogs);
            });

        }
 
        private void LoadConfigurationAndOpenScene(IUserDialogs dialogs)
        {
            bool Result = true;

            if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
            {
                Result = InitialConfigProcess();
                if (!Result)
                    return; // The apps will be forced to close / kill

                SecureStorage.SetAsync("ConfigVersion", NewConfigVersion);
                SecureStorage.SetAsync("DateCheck", DateTime.Today.ToShortDateString());
            }
            else
            {
                DateCheck = SecureStorage.GetAsync("DateCheck").Result;
                if (DateCheck != DateTime.Today.ToShortDateString())  // once per day
                {
                    SecureStorage.SetAsync("DateCheck", DateTime.Today.ToShortDateString());
                    ConfigVersion = SecureStorage.GetAsync("ConfigVersion").Result;
                    NewConfigVersion = SecureStorage.GetAsync("ConfigVersion").Result;

                    if (GenericUtilsClass.TagGlobal(false, "CheckConfigFiles", out dynamic value) &&
                         value != null)
                    {
                        bool.TryParse((string)value, out checkConfigFiles);

                        if (checkConfigFiles)
                        {
                            if (Mobile.configData.HasFTP ||
                                 Mobile.configData.HasIntune)
                                NewConfigVersion = GenericUtilsClass.CheckFTPConfigVersion();
                            else NewConfigVersion = GenericUtilsClass.CheckPubConfigVersion();
                            checkConfigFiles = false;

                            if (!string.IsNullOrEmpty(NewConfigVersion) && !NewConfigVersion.Equals(ConfigVersion))
                            {
                                checkConfigFiles = true;
                                // Backup current and update config files
                                GenericUtilsClass.BackUpConfigFiles();
                                if (!(Result = UpdateConfigFiles()))
                                {
                                    GenericUtilsClass.RestoreConfigFiles();
                                    this.ShowErrorAndKill(new ConfigurationFilesNewVersionException());
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (Result)
            {
                ConfigVersion = SecureStorage.GetAsync("ConfigVersion").Result;

                // Loads configuration files
                if (!this.InitializeConfiguration())
                {
                    if (checkConfigFiles)
                        GenericUtilsClass.RestoreConfigFiles();
                    else
                    {
                        GenericUtilsClass.DeleteConfigFiles(Mobile.ConfigPath);
                        GenericUtilsClass.SetInstallMode("None");
                    }

                    // Finishes because the app will be killed
                    return;
                }

                if (!String.IsNullOrEmpty(NewConfigVersion))
                {
                    ConfigVersion = NewConfigVersion;
                    SecureStorage.SetAsync("ConfigVersion", ConfigVersion);
                }

                Utils.Print($"Config version: { ConfigVersion} ");
                if (!Mobile.configData.HasIntune) Utils.Print("Local parameters loaded..");
                else Utils.Print("Intune parameters loaded..");
                if (Mobile.configData.HasIntune || Mobile.configData.HasFTP)
                {
                    Utils.Print("FTP: " + Mobile.configData.ftpDownload_Host + ":" + Mobile.configData.ftpDownload_Port + " - "
                        + Mobile.configData.ftpDownload_User + " [ " + Mobile.configData.ftpDownload_Pass + " ]");
                    if (Mobile.configData.IsCertLoaded)
                    {
                        Utils.Print("Certificate: " + Mobile.configData.certificate.Subject + " [ " + Mobile.configData.certificate.NotAfter + " ]");
                    }
                }

                if (!Data.Get.IsFromScripting)
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
                    });
                else
                {
                    //if (Data.Get.IsIOS)
                    //   // taskSemaphoreIOS.SetResult(true);
                    //else
                    //if (Data.Get.IsAndroid)
                        HandleUrl(FormsApp.dataUrl);
                }
            }
        }
        private bool InitialConfigProcess()
        {

            string Mode = GenericUtilsClass.ChekInstallMode();
            if (Mode.Equals("Intune"))
            {
                if (Mobile.IsNetAvailable())
                {
                    var MamServ = DependencyService.Get<IMAMService>();
                    MamServ.UtilMAMService();
                    if (Mobile.configData.HasIntune)
                    {
                        NewConfigVersion = GenericUtilsClass.CheckFTPConfigVersion();
                        if (!string.IsNullOrEmpty(NewConfigVersion))
                        {
                            if (!GenericUtilsClass.DownloadConfigFiles(out string sFileCert))
                            {
                                this.ShowErrorAndKill(new FtpDownloadException());
                                return false;
                            }
                            else
                            {
                                if (!Mobile.configData.IsCertLoaded && !string.IsNullOrEmpty(sFileCert))
                                    Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));

                            }
                            return true;
                        }


                    }
                    else
                    {
                        GenericUtilsClass.SetInstallMode("None");
                        this.ShowErrorAndKill(new IntuneCredentialsException());
                        return false;
                    }
                }
                else
                    this.ShowErrorAndKill(new NoInternetException());

                return false;
            }
            else if (Mode.Equals("Manual"))
            {
                Mobile.configData.HasFTP = false;

                // Check if all configuration files are available in public folder
                if (GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath))
                {
                    NewConfigVersion = GenericUtilsClass.CheckPubConfigVersion();

                    bool CPD = false;
                    if (GenericUtilsClass.TagGlobal(true, "ConfigPublicDir", out dynamic value))
                    {
                        if (value != null)
                            bool.TryParse((string)value, out CPD);
                    }
                    if (!GenericUtilsClass.CopyConfigFiles(!CPD, Mobile.ConfigPublicPath, Mobile.ConfigPath, out string sFileCert))
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(sFileCert))
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));


                    return true;
                }
                else
                {
                    this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());
                    GenericUtilsClass.SetInstallMode("None");

                    return false;
                }
            }
            return true;
        }

        public bool InitializeConfiguration()
        {
            try
            {            
                config = Configuration.GetInstanceWithParams(string.Empty);   
                Singleton.Set = new Logger();

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        config.setPlatform(SO_ANDROID);
                        config.setAppName(FormsApp.AppName);
                        config.setVersion(FormsApp.appVersion_str);
                        config.setDeviceUUID(FormsApp.deviceId);
                        break;
                    case Device.iOS:
                        config.setPlatform(SO_IOS);
                        config.setAppName(FormsApp.AppName);
                        config.setVersion(FormsApp.appVersion_str);
                        config.setDeviceUUID(FormsApp.deviceId);
                        break;
                    default:
                        config.setPlatform(SO_UNKNOWN);
                        break;
                }
            }
            catch (Exception e)
            {
                if (Errors.IsOwnException(e))
                    ShowErrorAndKill(e);
                else ShowErrorAndKill(new ConfigurationFilesCorruptedException());

                return false;
            }

            return true;
        }

        #region Base64

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #endregion
        #region Scripting

        public async void HandleUrl(Uri url)
        {
            Data.Set("IsFromScripting", true);

            Utils.Print("Config: Scripting Config [ " + Data.Get.IsFromScripting + " ]");
            Utils.Print("Config: Uri.Query [ " + url.Query.ToString() + " ]");

            // Stops logic because initialization has been canceled due to an error / exception
            if (this.formsInitFailed)
                return;

            try
            {
                if (FormsApp.ble_interface != null &&
                    FormsApp.ble_interface.IsOpen())
                    FormsApp.ble_interface.Close();

                #region WE HAVE TO DISABLE THE BLUETOOTH ANTENNA, IN ORDER TO DISCONNECT FROM PREVIOUS CONNECTION, IF WE WENT FROM INTERACTIVE TO SCRIPTING MODE

                // await adapter.DisableAdapter();
                // await adapter.EnableAdapter(); //Android shows a window to allow bluetooth

                #endregion

            }
            catch (Exception e)
            {
                Utils.Print(e.StackTrace);
            }

            if (url != null)
            {
                //string path = Mobile.ConfigPath;
                //ConfigPaths();
                string path = Mobile.ConfigPath;
                NameValueCollection query = HttpUtility.ParseQueryString(url.Query);

                var script_name = query.Get("script_name");
                var script_data = query.Get("script_data");
                var callback = query.Get("callback");

                if (script_name != null)
                    path = Path.Combine(path, "___" + script_name.ToString());

                if (script_data != null)
                    File.WriteAllText(path, Base64Decode(script_data));

                if (callback != null) { /* ... */ }

                try
                {
                    if (Data.Get.IsIOS)
                    {
                        // Scripting
                        //if (Application.Current.MainPage == null)
                        //{
                        //    taskSemaphoreIOS = new TaskCompletionSource<bool>();

                        //    // Wait until HandleUrl finishes
                        //    bool result = await taskSemaphoreIOS.Task;
                        //}

                        await Task.Run(async () =>
                        {
                            await Task.Delay(1000); Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                            {
                                Application.Current.MainPage = new NavigationPage(
                                    new AclaraViewScripting(path, callback, script_name));

                                await Application.Current.MainPage.Navigation.PopToRootAsync(true);
                            });
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new NavigationPage(
                                new AclaraViewScripting(path, callback, script_name));
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"-----  {ex.Message}");
                }
            }
        }

        #endregion


        private void ShowErrorAndKill(
       Exception e)
        {
            // Avoids executing the HandleUrl method
            this.formsInitFailed = true;

            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(new ErrorInitView(e));
            });
        }


        private bool UpdateConfigFiles()
        {
            if (Mobile.configData.HasIntune || Mobile.configData.HasFTP)
            {
                if (Mobile.IsNetAvailable())
                {
                    if (!GenericUtilsClass.DownloadConfigFiles(out string sFileCert))
                    {
                        return false;
                    }
                    if (!Mobile.configData.IsCertLoaded && !string.IsNullOrEmpty(sFileCert))
                    {
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));
                    }

                    return true;
                }
                //this.ShowErrorAndKill(new NoInternetException());
                //MainPage.DisplayAlert("Attention", "There is not connection at this moment, try again later","OK");
                return false;
            }
            else
            {
                Mobile.configData.HasFTP = false;

                // Check if all configuration files are available in public folder
                if (GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath))
                {

                    bool CPD = false;
                    if (GenericUtilsClass.TagGlobal(true, "ConfigPublicDir", out dynamic value))
                    {
                        if (value != null)
                            bool.TryParse((string)value, out CPD);
                    }
                    if (!GenericUtilsClass.CopyConfigFiles(!CPD, Mobile.ConfigPublicPath, Mobile.ConfigPath, out string sFileCert))
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(sFileCert))
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));

                    if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
                        return false;
                    else
                        return true;
                }

                return true;
            }
        }
    }
}