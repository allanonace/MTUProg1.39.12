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

        private const string SO_ANDROID = "Android";
        private const string SO_IOS = "iOS";
        private const string SO_UNKNOWN = "Unknown";

        #endregion
        private Configuration config;
        private string ConfigVersion;
        private string NewConfigVersion;
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
                {
                    GenericUtilsClass.SetInstallMode("None");
                    this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());
                    return; // The apps will be forced to close / kill
                }
                if (!Configuration.CheckLoadXML())
                {
                    GenericUtilsClass.SetInstallMode("None");
                    GenericUtilsClass.DeleteConfigFiles(Mobile.ConfigPath);
                    this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());
                    return;
                }
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
                            if (Mobile.ConfData.HasFTP ||
                                 Mobile.ConfData.HasIntune)
                                NewConfigVersion = GenericUtilsClass.CheckFTPConfigVersion();
                            else NewConfigVersion = GenericUtilsClass.CheckPubConfigVersion();
                            checkConfigFiles = false;

                            if (!string.IsNullOrEmpty(NewConfigVersion) && !NewConfigVersion.Equals(ConfigVersion))
                            {
                                checkConfigFiles = true;
                                // Backup current and update config files
                                GenericUtilsClass.BackUpConfigFiles();
                                Result = UpdateConfigFiles();
                                if (!Result)
                                {
                                    GenericUtilsClass.RestoreConfigFiles();
                                    this.ShowErrorAndKill(new ConfigurationFilesNewVersionException());
                                    return;
                                }
                                else
                                {
                                    if(!Configuration.CheckLoadXML())
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
                if (!Mobile.ConfData.HasIntune) Utils.Print("Local parameters loaded..");
                else Utils.Print("Intune parameters loaded..");
                if (Mobile.ConfData.HasIntune || Mobile.ConfData.HasFTP)
                {
                    Utils.Print("FTP: " + Mobile.ConfData.FtpDownload_Host + ":" + Mobile.ConfData.FtpDownload_Port + " - "
                        + Mobile.ConfData.FtpDownload_User + " [ " + Mobile.ConfData.FtpDownload_Pass + " ]");
                    if (Mobile.ConfData.IsCertLoaded)
                    {
                        Utils.Print("Certificate: " + Mobile.ConfData.certificate.Subject + " [ " + Mobile.ConfData.certificate.NotAfter + " ]");
                    }
                }

                if (!Data.Get.IsFromScripting)
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FormsApp.StartInteractive = true;
                        Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
                    });
                else
                {
                    HandleUrl(FormsApp.DataUrl);
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
                    if (Mobile.ConfData.HasIntune)
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
                                if (!Mobile.ConfData.IsCertLoaded && !string.IsNullOrEmpty(sFileCert))
                                    Mobile.ConfData.StoreCertificate(Mobile.ConfData.CreateCertificate(null, sFileCert));

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
                Mobile.ConfData.HasFTP = false;

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
                        Mobile.ConfData.StoreCertificate(Mobile.ConfData.CreateCertificate(null, sFileCert));


                    return true;
                }
                else
                {
                    GenericUtilsClass.SetInstallMode("None");
                    this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());                    
                    return false;
                }
            }
            return false; // mode FTP without config files 
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
                        config.setVersion(FormsApp.AppVersion_str);
                        config.setDeviceUUID(FormsApp.DeviceId);
                        break;
                    case Device.iOS:
                        config.setPlatform(SO_IOS);
                        config.setAppName(FormsApp.AppName);
                        config.setVersion(FormsApp.AppVersion_str);
                        config.setDeviceUUID(FormsApp.DeviceId);
                        break;
                    default:
                        config.setPlatform(SO_UNKNOWN);
                        break;
                }
            }
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                if ( Errors.IsOwnException ( e ) )
                     ShowErrorAndKill ( e );
                else ShowErrorAndKill ( new ConfigurationFilesCorruptedException () );

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

        public async Task HandleUrl(Uri url)
        {
            if (url == null)
                return;
            
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
            }
            catch (Exception e)
            {
                Utils.Print(e.StackTrace);
            }                    

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
            catch (Exception e)
            {
                Console.WriteLine($"-----  {e.Message}");
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
            string Mode = GenericUtilsClass.ChekInstallMode();
            if (Mobile.IsNetAvailable() && (Mode == "Intune" || Mode == "FTP"))
            {
                if (!GenericUtilsClass.DownloadConfigFiles(out string sFileCert))
                {
                    return false;
                }
                if (!Mobile.ConfData.IsCertLoaded && !string.IsNullOrEmpty(sFileCert))
                {
                    Mobile.ConfData.StoreCertificate(Mobile.ConfData.CreateCertificate(null, sFileCert));
                }
                return true;
            }
            else if (Mode=="Manual" && GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath))
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
                    Mobile.ConfData.StoreCertificate(Mobile.ConfData.CreateCertificate(null, sFileCert));

                if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
                    return false;
                else
                    return true;
            }
            else return false;
                                   
        }
    }
}