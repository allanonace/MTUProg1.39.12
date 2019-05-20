using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using aclara_meters.view;
using Acr.UserDialogs;
using ble_library;
using Library;
using MTUComm;
using Library.Exceptions;
using nexus.protocols.ble;
using Plugin.DeviceInfo;
using Plugin.Multilingual;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xml;
using System.Xml.Linq;
using aclara_meters.Helpers;
using aclara_meters.util;
using System.Threading;
using Xamarin.Essentials;

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
       
        public static bool ScriptingMode = false;

        #endregion

        #region Attributes

        public string appVersion_str;
        public string deviceId;
        
        public static CredentialsService credentialsService { get; private set; }
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

        private static string appName = "Aclara MTU Programmer ";

        public static string AppName
        {
            get { return appName; }
        }

        #endregion

        #region Initialization

        public FormsApp ()
        {
            InitializeComponent ();
        }

        public FormsApp (
            IBluetoothLowEnergyAdapter badapter,
            IUserDialogs dialogs,
            string appVersion )
        {
            try
            {
                InitializeComponent();
                
                Data.Set ( "IsIOS",     Device.RuntimePlatform == Device.iOS     );
                Data.Set ( "IsAndroid", Device.RuntimePlatform == Device.Android );

                this.adapter    = badapter;
                this.dialogs    = dialogs;
                this.appVersion = appVersion;
                appName        += ( Data.Get.IsAndroid ) ? "Android" : "iOS";

                if ( Data.Get.IsAndroid )
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        
            Utils.Print ( "FormsApp: Interactive [ " + MTUComm.Action.IsFromScripting + " ]" );
        
            appVersion_str = appVersion;

            deviceId = CrossDeviceInfo.Current.Id;

            // Profiles manager
            credentialsService = new CredentialsService();

            // Initializes Bluetooth
            ble_interface = new BleSerial(adapter);

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            // Config path
            ConfigPaths();

            if (Device.RuntimePlatform == Device.iOS)
            {
                var MamServ = DependencyService.Get<IMAMService>();
                MamServ.UtilMAMService();
            }

            this.LoadConfigurationAndOpenScene ( dialogs );   
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
                {
                    GenericUtilsClass.DownloadConfigFiles();
                    return true;
                }
                this.ShowErrorAndKill(new NoInternetException());
                //MainPage.DisplayAlert("Attention", "There is not connection at this moment, try again later","OK");
                return false;
            }
            else
            {
                Mobile.configData.HasFTP = false;
                SecureStorage.RemoveAll();
                // Check if all configuration files are available in public folder
                bool HasPublicFiles = GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                //this.abortMission = !this.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                if (HasPublicFiles)
                {
                    //// Install certificate if needed ( Convert from .cer to base64 string / .txt )
                    //if (!GenericUtilsClass.GenerateBase64Certificate(Mobile.ConfigPublicPath))
                    //{
                    //    this.ShowErrorAndKill(new CertificateFileNotValidException());

                    //    return false;
                    //}
                    //File.Copy(file.FullName, Path.Combine(url_to_copy, file.Name), true);
                    bool CPD = false;
                    if (GenericUtilsClass.TagGlobal("ConfigPublicDir", out dynamic value))
                    {
                        if (value != null)
                            bool.TryParse((string)value,out CPD);
                    }
                    GenericUtilsClass.CopyConfigFilesToPrivate(!CPD);

                    if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
                        return true;

                }
                else
                {
                    //Configure download FTP
                    if (Mobile.IsNetAvailable())
                    {
                         bool result =false;
                         tcs = new TaskCompletionSource<bool>();
                        // Console.WriteLine($"------------------------------------FTP  Thread: {Thread.CurrentThread.ManagedThreadId}");
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                           
                            MainPage = new NavigationPage(new FtpDownloadSettings(tcs));
                            //PopupNavigation.Instance.PushAsync(new FtpDownloadSettings());

                            result = await tcs.Task;
                           
                            // Install certificate if needed ( Convert from .cer to base64 string / .txt )
                            if (!GenericUtilsClass.GenerateBase64Certificate(Mobile.ConfigPath))
                            {
                                this.ShowErrorAndKill(new CertificateFileNotValidException());

                                return;
                            }

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
                            
                        return false;
                    }
                    else
                    {
                        this.ShowErrorAndKill(new NoInternetException());
                        this.abortMission = true;
                        return false;
                    }
                }
                return true;
            }
        }

        private void  LoadConfigurationAndOpenScene(IUserDialogs dialogs)
        {
            //ConfigPaths();

            // Only download configuration files from FTP when all are not installed
           
            bool Result = true;
            if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
            {
                Result = InitialConfigProcess();
                //Install certificate if needed(Convert from.cer to base64 string / .txt)
                if (!GenericUtilsClass.GenerateBase64Certificate(Mobile.ConfigPath))
                {
                    this.ShowErrorAndKill(new CertificateFileNotValidException());

                    return;
                }
            }

            if (Result)
            {
                // Load configuration files
                // If some configuration file is not present, Configuration.cs initialization should avoid
                // launch exception when try to parse xmls, to be able to use generating the log error
                if (!this.InitializeConfiguration())
                {
                    GenericUtilsClass.DeleteConfigFiles(Mobile.ConfigPath);
                    return;
                }

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

        public bool InitializeConfiguration ()
        {
            try
            {
                config = Configuration.GetInstanceWithParams ( string.Empty, this.abortMission );
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
            }
            catch ( Exception e )
            {
                // Avoid starting the creation of the login window
                this.ShowErrorAndKill ( e );
               
                return false;
            }
            
            return true;
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
            
            Utils.Print ( "FormsApp: Scripting [ " + MTUComm.Action.IsFromScripting + " ]" );
        
            if ( this.abortMission )
                return;
        
            try
            {
                ScriptingMode = true; 
                if (ble_interface.IsOpen()) ble_interface.Close();

                #region WE HAVE TO DISABLE THE BLUETOOTH ANTENNA, IN ORDER TO DISCONNECT FROM PREVIOUS CONNECTION, IF WE WENT FROM INTERACTIVE TO SCRIPTING MODE

                await adapter.DisableAdapter();
                await adapter.EnableAdapter(); //Android shows a window to allow bluetooth

                #endregion

            }
            catch (Exception e)
            {
                Utils.Print(e.StackTrace);
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


                if (MainPage == null)  // no interactive 
                {
                    tcs1 = new TaskCompletionSource<bool>(); 
                    bool result = await tcs1.Task;
                }

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
                Mtu mtu = Singleton.Get.Action.CurrentMtu;
                
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
                MTUComm.Action action = Singleton.Get.Action;
                str.AppendLine ( "" );
                str.AppendLine ( "Action" );
                str.AppendLine ( "------" );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "Type", action.type ) );
                str.AppendLine ( string.Format ( "{0,-50} : {1}", "User", action.user ) );
                
                // Add current values in Global.xml
                str.AppendLine ( "" );
                str.AppendLine ( "Global.XML" );
                str.AppendLine ( "----------" );
                Global global = Singleton.Get.Configuration.Global;
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
