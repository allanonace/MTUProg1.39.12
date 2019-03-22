using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using aclara_meters.view;
using Acr.UserDialogs;
using ble_library;
using MTUComm;
using MTUComm.Exceptions;
using nexus.protocols.ble;
using nexus.protocols.ble.scan;
using Plugin.DeviceInfo;
using Plugin.Multilingual;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        #region Initial FTP - Default Config data

        //string host = "159.89.29.176";
        //string username = "aclara";
        //string password = "aclara1234";
        //string pathRemoteFile = "/home/aclara";

        #endregion

        #region Constants

        private const bool   DEBUG_MODE_ON = false;

        private const string SO_ANDROID = "Android";
        private const string SO_IOS     = "iOS";
        private const string SO_UNKNOWN = "Unknown";
        private const string XML_EXT    = ".xml";
        private const string XML_CER    = ".cer";
        private const string CER_TXT    = "certificate.txt";
        public static bool ScriptingMode = false;

        private string[] filesToCheck =
        {
            "alarm",
            "demandconf",
            "global",
            "interface",
            "meter",
            "mtu",
            "user",
            "family_31xx32xx",
            "family_33xx",
            "family_342x"
        };

        #endregion

        #region Attributes

        public string appVersion_str;
        public string deviceId;
        
        public static ICredentialsService credentialsService { get; private set; }
        public static BleSerial ble_interface;
        public static Logger logger;
        public static Configuration config;
        public static IBlePeripheral peripheral;

        private IBluetoothLowEnergyAdapter adapter;
        private IUserDialogs dialogs;
        private string appVersion;

        #endregion

        #region Properties

        public static string AppName
        {
            get { return "Aclara MTU Programmer"; }
        }

        #endregion

        #region Initialization

        public FormsApp ()
        {
            InitializeComponent ();
        }

        public FormsApp (
            IBluetoothLowEnergyAdapter adapter,
            IUserDialogs dialogs,
            string appVersion )
        {
            try
            {
                InitializeComponent();

                this.adapter = adapter;
                this.dialogs = dialogs;
                this.appVersion = appVersion;

                if (Device.RuntimePlatform == Device.Android)
                {
                    Task.Run(async () => { await PermisosLocationAsync(); });
                    CallToInitApp(adapter, dialogs, appVersion);
                }
                else
                    Task.Factory.StartNew(ThreadProcedure);
            }
            catch (Exception e)
            {
                
            }
        }

        private void ThreadProcedure ()
        {
            CallToInitApp ( adapter, dialogs, appVersion );
        }

        private void CallToInitApp (
            IBluetoothLowEnergyAdapter adapter,
            IUserDialogs dialogs,
            string appVersion )
        {
            appVersion_str = appVersion;

            deviceId = CrossDeviceInfo.Current.Id;

            // Profiles manager
            credentialsService = new CredentialsService();

            // Initializes Bluetooth
            ble_interface = new BleSerial(adapter);

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            // Force to not download server XML files
            if ( DEBUG_MODE_ON )
                this.LoadConfigurationAndOpenScene ( dialogs );
            else
            {
                /*
                this.DownloadConfigFiles ();
                this.LoadXmlsAndCreateContainer ( dialogs );
                return;
                */
            
                // Downloads, if necesary, and loads configuration from XML files
                /*
                if ( this.HasDeviceAllXmls () )
                     this.LoadConfigurationAndOpenScene ( dialogs );
                else this.DownloadXmlsIfNecessary ( dialogs );
                */
            
                this.LoadConfigurationAndOpenScene ( dialogs );   
            }
        }

        #endregion

        #region Configuration files and System

        private void LoadConfigurationAndOpenScene ( IUserDialogs dialogs )
        {
            // Only download configuration files from FTP when all are not installed
            //if ( Mobile.IsNetAvailable () &&
            //     ! this.HasDeviceAllXmls () )
            //    this.DownloadConfigFiles ();
            
            // Check if all configuration files are available
            if ( ! this.HasDeviceAllXmls () )
            {
                this.ShowErrorAndKill ( new ConfigurationFilesNotFoundException () );

                return;
            }

            // Install certificate if needed ( Convert from .cer to base64 string / .txt )
            if ( ! this.GenerateBase64Certificate () )
            {
                this.ShowErrorAndKill ( new CertificateFileNotValidException () );
            
                return;
            }

            // Load configuration files
            try
            {
                config  = Configuration.GetInstance ();
            }
            catch ( Exception e )
            {
                // Avoid starting the creation of the login window
                this.ShowErrorAndKill ( e );
               
                return;
            }

            logger = new Logger ();

            switch ( Device.RuntimePlatform )
            {
                case Device.Android:
                    config.setPlatform   ( SO_ANDROID );
                    config.setAppName    ( AppName    );
                    config.setVersion    ( appVersion_str);
                    config.setDeviceUUID ( deviceId   );
                    break;
                case Device.iOS:
                    config.setPlatform   ( SO_IOS     );
                    config.setAppName    ( AppName    );
                    config.setVersion    ( appVersion_str);
                    config.setDeviceUUID ( deviceId   );
                    break;
                default:
                    config.setPlatform   ( SO_UNKNOWN );
                    break;
            }

            Configuration.SetInstance ( config );
            
            if ( ! ScriptingMode )
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
                });
        }

        /*
        private void DownloadXmlsIfNecessary (
            IUserDialogs dialogs )
        {
            // Checks network channels
            if ( Mobile.IsNetAvailable () )
            {
                // Donwloads all configuracion XML files
                if ( this.DownloadConfigFiles () )
                    this.LoadConfigurationAndOpenScene ( dialogs );
                else
                    this.ShowErrorAndKill ( new FtpDownloadException () );
            }
            else 
               this.ShowErrorAndKill ( new NoInternetException () );
        }
        */

        private bool DownloadConfigFiles ()
        {
            bool ok = true;
        
            try
            {
                Mobile.ConfigData data = Mobile.configData;
                using (SftpClient sftp = new SftpClient ( data.ftpHost, data.ftpPort, data.ftpUser, data.ftpPass ) )
                {
                    sftp.Connect ();

                    // List all posible files in the documents directory 
                    // Check if file's lastwritetime is the lastest 
                    List<SftpFile> ftp_array_files = new List<SftpFile>();

                    // Remote FTP File directory
                    bool isCertificate;
                    string configPath = Mobile.GetPathConfig ();
                    foreach ( SftpFile file in sftp.ListDirectory ( data.ftpPath ) )
                    {
                        string name = file.Name;
                    
                        if ( ( isCertificate = name.Contains ( XML_CER ) ) ||
                             name.Contains ( XML_EXT ) )
                        {
                            using ( Stream stream = File.OpenWrite ( Path.Combine ( configPath, name ) ) )
                            {
                                sftp.DownloadFile(Path.Combine ( data.ftpPath, name ), stream );
                            }
                        }
                    }

                    sftp.Disconnect ();
                }
            }
            catch ( Exception e )
            {
                ok = false;
            }

            Console.WriteLine ( "Download config.files from FTP: " + ( ( ok ) ? "OK" : "NO" ) );

            return ok;
        }

        private bool HasDeviceAllXmls ()
        {
            bool ok = true;
        
            string path = Mobile.GetPathConfig ();

            // Directory could exist but is empty
            if ( string.IsNullOrEmpty ( path ) )
                ok = false;

            // Directory exists and is not empty
            string[] filesLocal = Directory.GetFiles ( path );

            int count = 0;
            foreach ( string fileNeeded in filesToCheck )
                foreach ( string filePath in filesLocal )
                {
                    string compareStr = fileNeeded + XML_EXT;
                    compareStr = compareStr.Replace ( path, string.Empty ).Replace("/", string.Empty);

                    string fileStr = filePath.ToString ();
                    fileStr = fileStr.Replace ( path, string.Empty ).Replace("/",string.Empty).ToLower ();

                    if ( fileStr.Equals ( compareStr ) )
                    {
                        count++;
                        break;
                    }
                }

            ok = ( count == filesToCheck.Length );

            Console.WriteLine ( "Are all config.files installed? " + ( ( ok ) ? "OK" : "NO" ) );
            
            return ok;
        }

        private bool GenerateBase64Certificate ()
        {
            bool   ok         = true;
            string configPath = Mobile.GetPathConfig ();
            string txtPath    = Path.Combine ( configPath, CER_TXT );
        
            try
            {
                
                foreach ( string filePath in Directory.GetFiles ( configPath ) )
                {
                    if ( filePath.Contains ( XML_CER ) )
                    {
                        // Convert certificate to base64 string                                
                        string pathCer   = Path.Combine ( configPath, filePath );  // Path to .cer in Library
                        byte[] bytes     = File.ReadAllBytes ( pathCer );          // Read .cer full bytes
                        string strBase64 = Convert.ToBase64String ( bytes );       // Convert bytes to base64 string
                        File.WriteAllText ( txtPath, strBase64 );                  // Create new {name}.txt file with base64 string and delete .cer
                        File.Delete ( pathCer );
                        
                        break;
                    }
                }
            }
            catch ( Exception e )
            {
                ok = false;
            }

            if ( File.Exists ( txtPath ) )
                 Console.WriteLine ( "Is the certificate installed correctly? " + ( ( ok ) ? "OK" : "NO" ) );
            else Console.WriteLine ( "No certificate is being used" );

            return ok;
        }

        private async Task PermisosLocationAsync()
        {
            try
            {
                var statusLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var statusStorage = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if ( statusLocation != PermissionStatus.Granted )
                    await CrossPermissions.Current.RequestPermissionsAsync ( Permission.Location );

                if ( statusStorage != PermissionStatus.Granted )
                    await CrossPermissions.Current.RequestPermissionsAsync ( Permission.Storage );
            }
            catch
            {
                this.ShowErrorAndKill ( new AndroidPermissionsException () );
            }
        }

        #endregion

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

        public void HandleUrl ( Uri url , IBluetoothLowEnergyAdapter adapter)
        {
            try
            {
                ScriptingMode = true; 
                ble_interface.Close();

                #region WE HAVE TO DISABLE THE BLUETOOTH ANTENNA, IN ORDER TO DISCONNECT FROM PREVIOUS CONNECTION, IF WE WENT FROM INTERACTIVE TO SCRIPTING MODE

                adapter.DisableAdapter();
                adapter.EnableAdapter(); //Android shows a window to allow bluetooth

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if ( url != null )
            {
                string path = Mobile.GetPathPublic ();
                NameValueCollection query = HttpUtility.ParseQueryString ( url.Query );

                var script_name = query.Get ( "script_name" );
                var script_data = query.Get ( "script_data" );
                var callback    = query.Get ( "callback"    );

                if ( script_name != null )
                    path = Path.Combine ( path, script_name.ToString () );

                if ( script_data != null )
                    File.WriteAllText ( path, Base64Decode ( script_data ) );

                if ( callback != null ) { /* ... */ }

            
                Task.Run(async () =>
                {
                    await Task.Delay(1000); Xamarin.Forms.Device.BeginInvokeOnMainThread ( async () =>
                    {
                        //Settings.IsLoggedIn = false;
                        //credentialsService.DeleteCredentials ();

                        MainPage = new NavigationPage(new AclaraViewScripting ( path, callback, script_name ) );

                        await MainPage.Navigation.PopToRootAsync ( true );
                    });
                });
            }
        }
        
        #endregion

        #region Events

        protected override void OnStart()
        {
            // https://appcenter.ms/users/ma.jimenez/apps/Aclara-MTU-Testing-App
            //AppCenter.Start("ios=cb622ad5-e2ad-469d-b1cd-7461f140b2dc;" + "android=53abfbd5-4a3f-4eb2-9dea-c9f7810394be", typeof(Analytics), typeof(Crashes), typeof(Distribute) );
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
        
        private void ShowErrorAndKill (
            Exception e )
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.MainPage = new NavigationPage ( new ErrorInitView ( e ) );
            });
        }
    }
}
