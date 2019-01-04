using aclara_meters.Helpers;
using aclara_meters.view;
using System.Collections.Specialized;
using Acr.UserDialogs;
using ble_library;
using MTUComm;
using nexus.protocols.ble;
using Plugin.DeviceInfo;
using Plugin.Multilingual;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        #region Initial FTP - Default Config data

        string host = "159.89.29.176";
        string username = "aclara";
        string password = "aclara1234";
        string pathRemoteFile = "/home/aclara";

        #endregion

        #region Constants

        private const string SO_ANDROID = "Android";
        private const string SO_IOS     = "iOS";
        private const string SO_UNKNOWN = "Unknown";
        private const string XML_EXT    = ".xml";

        private string[] filesToCheck =
        {
            "Alarm",
            "DemandConf",
            "Global",
            "Interface",
            "Meter",
            "Mtu"
        };

        #endregion

        #region Attributes

        public string appVersion;
        public string deviceId;
        
        public static ICredentialsService credentialsService { get; private set; }
        public static BleSerial ble_interface;
        public static Logger loggger;
        public static Configuration config;

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
            List<string> listaDatos,
            string appVersion)
            : this ()
        {
            this.appVersion = appVersion;
            this.deviceId   = CrossDeviceInfo.Current.Id;

            // Profiles manager
            credentialsService = new CredentialsService ();

            // Initializes Bluetooth
            ble_interface = new BleSerial ( adapter );

            string data = string.Empty;
            if ( listaDatos.Count != 0 ||
                 listaDatos != null )
                for ( int i = 0; i < listaDatos.Count; i++ )
                    data = data + listaDatos[ i ] + "\r\n";

            string base64CertificateString = "";

            try
            {
                base64CertificateString = listaDatos[2].Replace("cert_file: ", "");
                byte[] bytes = Convert.FromBase64String(base64CertificateString);
                X509Certificate2 x509certificate = new X509Certificate2(bytes);
            }
            catch ( Exception e )
            {
                Console.WriteLine ( e.StackTrace );
            }

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            // TEST
            //Mobile.GetPath ();
            //this.LoadXmlsAndCreateContainer ( dialogs, data );

            // Downloads, if necesary, and loads configuration from XML files
            if ( this.HasDeviceAllXmls () )
                 this.LoadXmlsAndCreateContainer ( dialogs, data );
            else this.DownloadXmlsIfNecessary ( dialogs, data );
        }

        #endregion

        #region Configuration XMLs

        private bool HasDeviceAllXmls ()
        {
            string path = Mobile.GetPath ();

            // Directory could exist but is empty
            if ( string.IsNullOrEmpty ( path ) )
                return false;

            // Directory exists and is not empty
            string[] filesLocal = Directory.GetFiles ( path );

            //if ( ! filesLocal.Any () )
            //    return false;
            
            int count = 0;
            foreach ( string filePath in filesLocal )
            {
                foreach ( string fileNeeded in filesToCheck )
                {
                    string compareStr = fileNeeded + XML_EXT;
                    compareStr = compareStr.Replace ( path, "" );

                    string fileStr = filePath.ToString ();
                    fileStr = fileStr.Replace ( path, "" );
                    
                    if ( fileStr.Equals ( compareStr ) &&
                         ++count >= filesToCheck.Length )
                        return true;
                }
            }

            return false;
        }

        private void DownloadXmlsIfNecessary (
            IUserDialogs dialogs,
            string data )
        {
            // Checks network channels
            if ( Mobile.IsNetAvailable () )
            {
                // Donwloads all configuracion XML files
                if ( this.DownloadXmls () )
                     this.LoadXmlsAndCreateContainer ( dialogs, data );
                else this.MainPage = new NavigationPage ( new ErrorInitView ( "Error Downloading files" ) );
            }
            else this.MainPage = new NavigationPage ( new ErrorInitView () );
        }

        private bool DownloadXmls ()
        {
            try
            {
                using (SftpClient sftp = new SftpClient(host, username, password))
                {
                    try
                    {
                        sftp.Connect();


                        /*--------------------------------------------------*/
                        // List all posible files in the documents directory 
                        // Check if file's lastwritetime is the lastest 
                        /*--------------------------------------------------*/
                        List<SftpFile> ftp_array_files = new List<SftpFile>();

                        // Remote FTP File directory
                        var ftp_files = sftp.ListDirectory(pathRemoteFile);
                        foreach (var file in ftp_files)
                        {

                            if (file.Name.Contains(".xml"))
                            {
                                ftp_array_files.Add(file);
                            }


                        }

                        string path = Mobile.GetPath ();

                        foreach ( var file in ftp_array_files )
                        {
                            string remoteFileName = file.Name;

                            using (Stream file1 = File.OpenWrite(Path.Combine( path, remoteFileName)))
                            {
                                sftp.DownloadFile(Path.Combine(pathRemoteFile, remoteFileName), file1);
                            }
                        }

                        sftp.Disconnect ();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception has been caught " + e.ToString());
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }

            return false;
        }

        private void LoadXmlsAndCreateContainer ( IUserDialogs dialogs, string data )
        {
            // Load configuration from XML files
            this.LoadXmls ();

            // Load pages container ( ContentPage )
            this.MainPage = new NavigationPage ( new AclaraViewLogin ( dialogs, data ) );
        }

        private void LoadXmls ()
        {
            config  = Configuration.GetInstance ();
            loggger = new Logger ( config );

            switch ( Device.RuntimePlatform )
            {
                case Device.Android:
                    config.setPlatform   ( SO_ANDROID );
                    config.setAppName    ( AppName    );
                    config.setVersion    ( appVersion );
                    config.setDeviceUUID ( deviceId   );
                    break;
                case Device.iOS:
                    config.setPlatform   ( SO_IOS     );
                    config.setAppName    ( AppName    );
                    config.setVersion    ( appVersion );
                    config.setDeviceUUID ( deviceId   );
                    break;
                default:
                    config.setPlatform   ( SO_UNKNOWN );
                    break;
            }

            Configuration.SetInstance ( config );
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

        public void HandleUrl ( Uri url )
        {
            if ( url != null )
            {
                string path = Mobile.pathCache;
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
                        Settings.IsLoggedIn = false;
                        credentialsService.DeleteCredentials ();

                        MainPage = new NavigationPage(new AclaraViewScripting ( path, callback, script_name ) );
                        await MainPage.Navigation.PopToRootAsync ( true );
                    });
                });
            }
        }

        #region OnEvent

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
    }
}
