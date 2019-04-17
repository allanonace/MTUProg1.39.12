using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
using Rg.Plugins.Popup.Services;

using System.Reflection;

using System.Text;
using Xml;
using System.Xml.Linq;
using aclara_meters.Helpers;
using System.Threading;

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
        public const string XML_EXT    = ".xml";
        public const string XML_CER    = ".cer";
        private const string CER_TXT    = "certificate.txt";
        public static bool ScriptingMode = false;

        private string[] filesToCheck =
        {
            "alarm",
            "demandconf",
            "global",
            "meter",
            "mtu",
            "user",
        };

        #endregion

        #region Attributes

        public string appVersion_str;
        public string deviceId;
        
        public static ICredentialsService credentialsService { get; private set; }
        public static BleSerial ble_interface;
        public static Logger logger;
        public static Configuration config;

        private IBluetoothLowEnergyAdapter adapter;
        private IUserDialogs dialogs;
        private string appVersion;

        private bool abortMission;

        public static TaskCompletionSource<bool> tcs;
        public static TaskCompletionSource<bool> tcs1;
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
                
                //Singleton.Set = new Puck ();

                this.adapter = adapter;
                this.dialogs = dialogs;
                this.appVersion = appVersion;

                if (Device.RuntimePlatform == Device.Android)
                {
                    Task.Run(async () =>
                    {
                        await PermisosLocationAsync();
                    });
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

        private void CallToInitApp(
            IBluetoothLowEnergyAdapter adapter,
            IUserDialogs dialogs,
            string appVersion)
        {
            // Catch unhandled exceptions
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            //TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Console.WriteLine("FormsApp: Interactive [ " + MTUComm.Action.IsFromScripting + " ]");

            appVersion_str = appVersion;

            deviceId = CrossDeviceInfo.Current.Id;

            // Profiles manager
            credentialsService = new CredentialsService();

            // Initializes Bluetooth
            ble_interface = new BleSerial(adapter);

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            // Force to not download server XML files
  
            this.LoadConfigurationAndOpenScene(dialogs);
  
        }

        #endregion

        #region Configuration files and System

        private void ConfigPaths()
        {
            string sPath = String.Empty;
            string sPathPrivate = String.Empty;
            if (Device.RuntimePlatform == Device.Android)
            {
                sPath = DependencyService.Get<IPathService>().PrivateExternalFolder;
                sPathPrivate = DependencyService.Get<IPathService>().InternalFolder;
            }
            else
            {
                sPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sPathPrivate = Path.Combine(sPath, "..", "Library");
            }
            Mobile.ConfigPublicPath = sPath;
            Mobile.ConfigPath = sPathPrivate;
            Mobile.LogPath = sPath;
            #if DEBUG
            Mobile.LogUniPath = sPath;
            #endif


        }

        private bool InitialConfigProcess()
        {
            if (Mobile.configData.HasIntune)
            {
                if (Mobile.IsNetAvailable())
                    GenericUtilsClass.DownloadConfigFiles();
                else
                    MainPage.DisplayAlert("Attention", "There is not connection at this moment, try again later","OK");
                return true;
            }
            else
            {
                // Check if all configuration files are available in public folder
                bool HasPublicFiles = this.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                //this.abortMission = !this.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                if (HasPublicFiles)
                {
                    // Install certificate if needed ( Convert from .cer to base64 string / .txt )
                    if (!this.GenerateBase64Certificate(Mobile.ConfigPublicPath))
                    {
                        this.ShowErrorAndKill(new CertificateFileNotValidException());

                        return false;
                    }
                    //File.Copy(file.FullName, Path.Combine(url_to_copy, file.Name), true);
                    bool CPD = false;
                    if (TagGlobal("ConfigPublicDir", out dynamic value))
                    {
                        if (value != null) CPD = (bool)value;
                    }
                    CopyConfigFilesToPrivate(!CPD);

                }
                else
                {

                    //Mobile.configData.HasFTP = false;
                    //Configure download FTP
                    if (Mobile.IsNetAvailable())
                    {
                         bool result =false;
                         tcs = new TaskCompletionSource<bool>();
                        // Console.WriteLine($"------------------------------------FTP  Thread: {Thread.CurrentThread.ManagedThreadId}");
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //await MainPage.DisplayAlert("Attention", "Desea utilizar FTP?", "OK");
                            Console.WriteLine($"------------------------------------Beginmain  Thread: {Thread.CurrentThread.ManagedThreadId}");
                           
                            //await MainPage.Navigation.PushModalAsync(new FtpDownloadSettings());
                            MainPage = new NavigationPage(new FtpDownloadSettings());
                            //PopupNavigation.Instance.PushAsync(new FtpDownloadSettings());

                            result = await tcs.Task;

                            if (!this.InitializeConfiguration())
                                return;

                            if (this.abortMission)
                            {
                                this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());

                                return;
                            }
                            if (!ScriptingMode)
                            {
                                Console.WriteLine($"------------------------------------Login  Thread: {Thread.CurrentThread.ManagedThreadId}");
                                Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
                            }
                            else
                                tcs1.SetResult(true);
                        });
                            //result = tcs.Task;
                        return false;
                    }
                    else
                    {
                        this.abortMission = true;
                        return false;
                    }
                }
                return true;
            }
        }

        private void  LoadConfigurationAndOpenScene(IUserDialogs dialogs)
        {
            ConfigPaths();

            // Only download configuration files from FTP when all are not installed
            //if (Mobile.IsNetAvailable() &&
            //     !this.HasDeviceAllXmls())
            //    this.DownloadConfigFiles();
            bool Result = true;
            if (!this.HasDeviceAllXmls(Mobile.ConfigPath))
            {
                Result = InitialConfigProcess();
            }

            if (Result)
            {
                // Load configuration files
                // If some configuration file is not present, Configuration.cs initialization should avoid
                // launch exception when try to parse xmls, to be able to use generating the log error
                if (!this.InitializeConfiguration())
                    return;

                if (this.abortMission)
                {
                    this.ShowErrorAndKill(new ConfigurationFilesNotFoundException());

                    return;
                }
                if (!ScriptingMode)
                    Device.BeginInvokeOnMainThread(async() =>
                    {
                       Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
                       // await MainPage.Navigation.PopToRootAsync(true);
                    });
                else
                    tcs1.SetResult(true);
            }
                  
        }

        private void CopyConfigFilesToPrivate(bool bRemove)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(Mobile.ConfigPublicPath);
                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                {
                    string fileCopy = Path.Combine(Mobile.ConfigPath, file.Name);
                    file.CopyTo(fileCopy);
                    if (bRemove) file.Delete();
                }
            }
            catch (Exception e)
            {
                this.ShowErrorAndKill(e);
                return;
            }
        }

        private bool TagGlobal(string sTag , out dynamic value) 
        {
            string sVal = String.Empty;
            string uri = Path.Combine(Mobile.ConfigPublicPath, "Global.xml");

            XDocument doc = XDocument.Load(uri);
            foreach (XElement xElement in doc.Root.Elements())
            {
                if (xElement.Name == sTag)
                {
                    value = xElement.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public bool InitializeConfiguration ()
        {
            try
            {
                config = Configuration.GetInstance ( "", this.abortMission );
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
            }
            catch ( Exception e )
            {
                // Avoid starting the creation of the login window
                this.ShowErrorAndKill ( e );
               
                return false;
            }
            
            return true;
        }

        private bool HasDeviceAllXmls (string path)
        {
            bool ok = true;

            //string path = Mobile.ConfigPath;
        

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

        private bool GenerateBase64Certificate (string configPath)
        {
            bool   ok         = true;
            //string configPath = Mobile.ConfigPath;
  
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

        public async void HandleUrl ( Uri url , IBluetoothLowEnergyAdapter adapter)
        {
           
            MTUComm.Action.IsFromScripting = true;
            
            Console.WriteLine ( "FormsApp: Scripting [ " + MTUComm.Action.IsFromScripting + " ]" );
        
            if ( this.abortMission )
                return;
        
            try
            {
                ScriptingMode = true; 
                ble_interface.Close();

                #region WE HAVE TO DISABLE THE BLUETOOTH ANTENNA, IN ORDER TO DISCONNECT FROM PREVIOUS CONNECTION, IF WE WENT FROM INTERACTIVE TO SCRIPTING MODE

                await adapter.DisableAdapter();
                await adapter.EnableAdapter(); //Android shows a window to allow bluetooth

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if ( url != null )
            {
                //string path = Mobile.ConfigPath;
                //ConfigPaths();
                string path = Mobile.ConfigPath;
                NameValueCollection query = HttpUtility.ParseQueryString ( url.Query );

                var script_name = query.Get ( "script_name" );
                var script_data = query.Get ( "script_data" );
                var callback    = query.Get ( "callback"    );

                if ( script_name != null )
                    path = Path.Combine ( path, "___" + script_name.ToString () );

                if ( script_data != null )
                    File.WriteAllText ( path, Base64Decode ( script_data ) );

                if ( callback != null ) { /* ... */ }
                tcs1 = new TaskCompletionSource<bool>(); 
                bool result = await tcs1.Task;

                await Task.Run(async () =>
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

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var e = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException( e );
        }
    
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var e = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException( e );
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                StringBuilder str = new StringBuilder ();

                // MTU info
                Mtu mtu = MTUComm.Action.currentMtu;
                
                str.AppendLine ( DateTime.Now.ToString () );
                str.AppendLine ( "Unhandled Exception" );
                
                str.AppendLine ( "" );
                str.AppendLine ( "MTU" );
                str.AppendLine ( "---" );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "MTU",        mtu.Id          ) );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "SpecialSet", mtu.SpecialSet  ) );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "HexNumber",  mtu.HexNum      ) );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "Num.Ports",  mtu.Ports.Count ) );
                
                // Action info
                MTUComm.Action action = MTUComm.Action.currentAction;
                str.AppendLine ( "" );
                str.AppendLine ( "Action" );
                str.AppendLine ( "------" );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "Type", action.type ) );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "User", action.user ) );
                
                // Add current values in Global.xml
                str.AppendLine ( "" );
                str.AppendLine ( "Global.XML" );
                str.AppendLine ( "----------" );
                Global global = Configuration.GetInstance ().global;
                foreach ( var property in global.GetType ().GetProperties () )
                {
                    if ( ! property.GetType ().IsArray &&
                         property.CanRead )
                    {
                        var value = property.GetValue ( global );
                        if ( ! ( value is null ) )
                            str.AppendLine ( string.Format ( "{0,-50} : {1}", property.Name, value ) );
                    }
                }

                str.AppendLine ( "" );
                str.AppendLine ( "Exception" );
                str.AppendLine ( "---------" );
                
                StackTrace traces = new StackTrace ( exception.InnerException, true );
                
                var capturedTraces = typeof ( StackTrace ).GetField ( "captured_traces", BindingFlags.Instance | BindingFlags.NonPublic)
                  .GetValue ( traces ) as StackTrace[];
                
                string traces2 = exception.InnerException.StackTrace;
                foreach ( StackTrace trace in capturedTraces )
                    foreach ( StackFrame frame in trace.GetFrames () )
                        str.AppendLine ( frame.GetFileName () + ".." + Environment.NewLine +
                            frame.GetMethod () + " at line " + frame.GetFileLineNumber () + ", column " + frame.GetFileColumnNumber () );
                
                str.AppendLine ( "" );
                str.AppendLine ( "---------" );
                str.AppendLine ( exception.InnerException.ToString () );
                
                string errorFileName = string.Format ( "{0}_{1}_{2}.txt", "Exception", action.type, DateTime.Now.ToString ( "MM-dd-yyyy_HH-mm" ) );
                var libraryPath      = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );
                var errorFilePath    = Path.Combine ( libraryPath, errorFileName );
                File.WriteAllText ( errorFilePath, str.ToString () );
        
                str.Clear ();
                str = null;
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        #endregion
        
        private void ShowErrorAndKill (
            Exception e )
        {
            this.abortMission = true;
        
            Device.BeginInvokeOnMainThread(() =>
            {
                this.MainPage = new NavigationPage ( new ErrorInitView ( e ) );
            });
        }

        public static void DoLogOff()
        {
            Settings.IsLoggedIn = false;
            FormsApp.credentialsService.DeleteCredentials();
            Singleton.Remove<Puck>();
            Mobile.LogPath = Mobile.ConfigPublicPath;
            FormsApp.ble_interface.Close();
        }
    }
}
