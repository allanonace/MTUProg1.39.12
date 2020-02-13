using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;
 
namespace aclara_meters.view
{

    public partial class AclaraViewConfig
    {
        #region Constants

        private const string VAR_VERSION   = "ConfigVersion";
        private const string VAR_DATECHECK = "DateCheck";
        private const string INTUNE        = "Intune";
        private const string SO_ANDROID    = "Android";
        private const string SO_IOS        = "iOS";
        private const string SO_UNKNOWN    = "Unknown";

        #endregion

        private Configuration config;

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
 
        private void LoadConfigurationAndOpenScene (
            IUserDialogs dialogs )
        {
            bool   configFilesBackuped  = false;
            string currentConfigVersion = SecureStorage.GetAsync ( VAR_VERSION ).Result;
            string newConfigVersion     = currentConfigVersion;

            #region Copy Files

            // Not all necessary configuration files are installed
            // NOTE: Only the first launch of the app AclaraInstallPage is used
            if ( ! GenericUtilsClass.HasDeviceAllXmls ( Mobile.ConfigPath ) )
            {
                // Try to copy/install the configuration files
                if ( ! CopyConfigFiles ( ref newConfigVersion ) )
                {
                    GenericUtilsClass.SetInstallMode ( "None" );

                    this.ShowErrorAndKill ( new ConfigFilesNotFoundException () );
                    return;
                }
            }
            // All necessary configuration files are installed
            else
            {
                // Check once per day if there is a new version of the configuration files
                string dateCheck = SecureStorage.GetAsync ( VAR_DATECHECK ).Result;
                if ( dateCheck != DateTime.Today.ToShortDateString () )
                {
                    if ( GenericUtilsClass.GetTagFromGlobalXml ( false, "CheckConfigFiles", out dynamic value ) &&
                         value != null )
                    {
                        bool.TryParse ( ( string )value, out bool checkConfigFiles );
                        if ( checkConfigFiles )
                        {
                            if ( Mobile.ConfData.HasFTP ||
                                 Mobile.ConfData.HasIntune )
                                 newConfigVersion = GenericUtilsClass.CheckFTPConfigVersion ();
                            else newConfigVersion = GenericUtilsClass.CheckPubConfigVersion ();
                            
                            // The current version of the configuration files is different from the "new" version available
                            if ( ! newConfigVersion.Equals ( currentConfigVersion ) )
                            {
                                configFilesBackuped = true;

                                // Backup current version of the config files
                                GenericUtilsClass.BackUpConfigFiles ();

                                // Update config files with the "new" version detected
                                if ( !this.UpdateConfigFiles () )
                                {
                                    GenericUtilsClass.RestoreConfigFiles ();

                                    this.ShowErrorAndKill ( new ConfigFilesNewVersionException () );
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            // Update the value of the current version with the ( maybe )
            // new version, which starts with the same value as the current one
            currentConfigVersion = newConfigVersion;

            #endregion

            #region Load Configuration

            try
            {
                // Verify the configuration files and preload important information for the hardware
                // [ Configuration.cs ] ConfigurationFilesNotFoundException
                // [ Configuration.cs ] ConfigurationFilesCorruptedException
                // [ Configuration.cs ] DeviceMinDateAllowedException
                this.InitializeConfig ();

                #if DEBUG

                // Force some error cases in debug mode
                DebugOptions debug = config.Debug;
                if ( debug != null )
                {
                    if ( debug.ForceErrorConfig_Init_Date )
                        throw new DeviceMinDateAllowedException ();
                    else if ( debug.ForceErrorConfig_Init_Files )
                        throw new Exception ();
                }

                #endif
            }
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                if ( ! ( e is DeviceMinDateAllowedException ) )
                {
                    // Error updating
                    if ( configFilesBackuped )
                    {
                        GenericUtilsClass.RestoreConfigFiles ();

                        e = new ConfigFilesNewVersionException ();
                    }
                    // Error in a new installation
                    else
                    {
                        GenericUtilsClass.SetInstallMode ( "None" );
                        GenericUtilsClass.DeleteConfigFiles ( Mobile.ConfigPath );

                        e = new ConfigFilesCorruptedException ();
                    }
                }
                
                this.ShowErrorAndKill ( e );
                return;
            }
            
            // Information to be used to verify if there is a new version of the configuration files
            SecureStorage.SetAsync ( VAR_VERSION, newConfigVersion );
            SecureStorage.SetAsync ( VAR_DATECHECK, DateTime.Today.ToShortDateString () );

            // Output of some informational data to improve debugging
            Utils.Print ( $"Config version: { currentConfigVersion } " );
            
            if ( ! Mobile.ConfData.HasIntune )
                 Utils.Print ( "Local parameters loaded.." );
            else Utils.Print ( "Intune parameters loaded.." );

            if ( Mobile.ConfData.HasIntune ||
                 Mobile.ConfData.HasFTP )
            {
                Utils.Print ( "FTP: " + Mobile.ConfData.FtpDownload_Host + ":" +
                    Mobile.ConfData.FtpDownload_Port + " - "
                    + Mobile.ConfData.FtpDownload_User +
                    " [ " + Mobile.ConfData.FtpDownload_Pass + " ]");

                if ( Mobile.ConfData.IsCertLoaded )
                    Utils.Print ( "Certificate: " + Mobile.ConfData.certificate.Subject +
                        " [ " + Mobile.ConfData.certificate.NotAfter + " ]" );
            }

            #endregion

            #region Go to MainMenu

            // Load the main menu scene/window
            // Interactive mode
            if ( FormsApp.StartInteractive = ! Data.Get.IsFromScripting )
                Device.BeginInvokeOnMainThread ( () =>
                    Application.Current.MainPage = new NavigationPage ( new AclaraViewLogin ( dialogs ) ) );
            // Scripted mode
            else HandleUrl ( FormsApp.DataUrl );

            #endregion
        }

        private bool CopyConfigFiles (
            ref string newConfigVersion )
        {
            string Mode = GenericUtilsClass.ChekInstallMode();
            if ( Mode.Equals ( INTUNE ) )
            {
                if (Mobile.IsNetAvailable())
                {
                    var MamServ = DependencyService.Get<IMAMService>();
                    MamServ.UtilMAMService();
                    if (Mobile.ConfData.HasIntune)
                    {
                        newConfigVersion = GenericUtilsClass.CheckFTPConfigVersion ();
                        if ( ! string.IsNullOrEmpty ( newConfigVersion ) )
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
                // FIXME: PARECE QUE SOLO EN MANUAL SE ESTA COMPROBANDO SI ESTAN DISPONIBLES TODOS LOS FICHEROS DE CONFIGURACION
                if (GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath))
                {
                    newConfigVersion = GenericUtilsClass.CheckPubConfigVersion ();

                    bool CPD = false;
                    if (GenericUtilsClass.GetTagFromGlobalXml(true, "ConfigPublicDir", out dynamic value))
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
                    this.ShowErrorAndKill(new ConfigFilesNotFoundException());                    
                    return false;
                }
            }
            return false; // mode FTP without config files 
        }

        public void InitializeConfig ()
        {
            config = Configuration.GetInstance ();
            Singleton.Set = new Logger ();

            switch ( Device.RuntimePlatform )
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

        private void ShowErrorAndKill (
            Exception e)
        {
            // Avoids executing the HandleUrl method
            this.formsInitFailed = true;

            base.ShowErrorAndKill ( e );
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
                if (GenericUtilsClass.GetTagFromGlobalXml(true, "ConfigPublicDir", out dynamic value))
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
