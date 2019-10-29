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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xml;

using aclara_meters.Helpers;
using aclara_meters.util;
using System.Threading;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {

        #region Attributes

        private string DateCheck;

        public static CredentialsService credentialsService { get; private set; }
        public static BleSerial ble_interface { get; private set; }

        private IBluetoothLowEnergyAdapter adapter;
        private IUserDialogs dialogs;
        private string appVersion;
        public static bool StartInteractive;

        #endregion

        #region Properties
               
        public static string AppName { get; private set; } = "Aclara MTU Programmer ";

        public static string DeviceId { get; set; }
        public static string AppVersion_str { get; set; }
        public static Uri DataUrl { get; set; }

        #endregion

        // FormsApp -> CallToInitApp -> LoadConfigurationAndOpenScene -> InitializeConfiguration [ Android: -> HandleUrl ]
        // iOS: Used semaphore to "force" ( waiting FormsApp to finish ) HandleUrl executes in the same order as in Android

        #region Initialization

        public FormsApp ()
        {
            InitializeComponent ();
        }

        public FormsApp (
            IUserDialogs dialogs,
            string appVersion, Uri url=null )
        {
            try
            {
                InitializeComponent();

                MainPage = new ContentPage();

                VersionTracking.Track();

                var platform = DependencyService.Get<IAdapterBluetooth>();
                adapter = platform.GetNativeBleAdapter();

                DataUrl =url;
                           
                Data.Set ( "ActionInitialized", false );
                Data.Set ( "IsIOS",     Device.RuntimePlatform == Device.iOS     );
                Data.Set ( "IsAndroid", Device.RuntimePlatform == Device.Android );

                this.dialogs    = dialogs;
                this.appVersion = appVersion;
                AppName        += ( Data.Get.IsAndroid ) ? "Android" : "iOS";

                // Config path
                ConfigPaths();
                
                CallToInitApp(adapter, dialogs, appVersion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CallToInitApp(
            IBluetoothLowEnergyAdapter adapter,
            IUserDialogs dialogs,
            string appVersion)
        {
            // Catch unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Utils.Print("FormsApp: Interactive [ " + Data.Get.IsFromScripting + " ]");

            AppVersion_str = appVersion;

            DeviceId = CrossDeviceInfo.Current.Id;

            // Profiles manager
            credentialsService = new CredentialsService();

            // Initializes Bluetooth
            ble_interface = new BleSerial(adapter);

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            string Mode = GenericUtilsClass.ChekInstallMode ();
               

            // The first time the app is launched after installation
            if ( VersionTracking.IsFirstLaunchEver ||
                 Mode.Equals ( "None" ) )
            {
                if (Data.Get.IsAndroid)
                {
                    var MamServ = DependencyService.Get<IMAMService>();
                    MamServ.UtilMAMService();

                    if (Mobile.configData.HasIntune)
                    {
                        GenericUtilsClass.SetInstallMode("Intune");
                        Application.Current.MainPage = new NavigationPage(new AclaraViewConfig(dialogs));
                        return;
                    }
                }

                SecureStorage.RemoveAll ();
                Device.BeginInvokeOnMainThread ( () =>
                {
                    Application.Current.MainPage = new NavigationPage ( new AclaraInstallPage () );
                });
            }
            // Is not the first launch
            else
            {
                
                // Load the ftp settings in configData
                if ( Mode.Equals ( "Intune" ) )
                {
                    var MamServ = DependencyService.Get<IMAMService> ();
                    MamServ.UtilMAMService ();
                }

                // Check if FTP settings is in securestorage
                else if ( Mode.Equals ( "FTP" ) && !GenericUtilsClass.CheckFTPDownload())
                {
                    GenericUtilsClass.SetInstallMode("None");
                    ShowErrorAndKill(new FtpCredentialsMissingException());
                    return;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new AclaraViewConfig(dialogs));
                });
              
            }
        }

        #endregion

        #region Configuration files and System

        private void ConfigPaths()
        {
            string sPath = String.Empty;
            string sPathPrivate = String.Empty;
            
            if ( Device.RuntimePlatform == Device.Android )
            {
                sPath = DependencyService.Get<IPathService>().PrivateExternalFolder;
                sPathPrivate = DependencyService.Get<IPathService>().InternalFolder;
            }
            else
            {
                sPath = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );
                sPathPrivate = Path.Combine ( sPath, "..", "Library" );
            }
            Mobile.ConfigPublicPath = sPath;
            Mobile.ConfigPath       = sPathPrivate;
            Mobile.LogPath          = sPath;
 
            #if DEBUG
            Mobile.LogUniPath = sPath;
            #endif
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

        #region Scripting iOS

        public async  void HandleUrl ( Uri url )
        {
            Data.Set ( "IsFromScripting", true );

            Utils.Print ( "FormsApp: Scripting [ " + Data.Get.IsFromScripting + " ]" );
            Utils.Print ("FormsApp: Uri.Query [ " + url.Query.ToString() + " ]");

            DataUrl = url;

            if (StartInteractive) ///   is in interactive mode
            {
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
                catch (Exception ex)
                {
                    Console.WriteLine($"-----  {ex.Message}");
                }
            
            }
            
 
        }

        #endregion

        #region Events

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<pendiente>")]
        protected override void OnStart()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<pendiente>")]
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            DateCheck = SecureStorage.GetAsync("DateCheck").Result;
            if (!String.IsNullOrEmpty(DateCheck) && DateCheck != DateTime.Today.ToShortDateString())  // once per day
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
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
                
                str.AppendLine ( DateTime.Now.ToString () );
                str.AppendLine ( "Unhandled Exception" );
                
                string actionType = "noActionInitialized";
                if ( Singleton.Has<MTUComm.Action> () )
                {
                    // Action info
                    MTUComm.Action action = Singleton.Get.Action;
                    actionType = action.Type.ToString ();
                    str.AppendLine ( "" );
                    str.AppendLine ( "Action" );
                    str.AppendLine ( "------" );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "Type", action.Type ) );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "User", action.User ) );
                    
                    // MTU info
                    Mtu mtu = action.CurrentMtu;
                    
                    str.AppendLine ( "" );
                    str.AppendLine ( "MTU" );
                    str.AppendLine ( "---" );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "MTU",        mtu.Id          ) );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "SpecialSet", mtu.SpecialSet  ) );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "HexNumber",  mtu.HexNum      ) );
                    str.AppendLine ( string.Format ( "{0,-50} : {1}", "Num.Ports",  mtu.Ports.Count ) );
                }
                
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
                
                foreach ( StackTrace trace in capturedTraces )
                    foreach ( StackFrame frame in trace.GetFrames () )
                        str.AppendLine ( frame.GetFileName () + ".." + Environment.NewLine +
                            frame.GetMethod () + " at line " + frame.GetFileLineNumber () + ", column " + frame.GetFileColumnNumber () );
                
                str.AppendLine ( "" );
                str.AppendLine ( "---------" );
                str.AppendLine ( exception.InnerException.ToString () );
                
                string errorFileName = string.Format ( "{0}_{1}_{2}.txt", "Exception", actionType, DateTime.Now.ToString ( "MM-dd-yyyy_HH-mm" ) );
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
       
            Device.BeginInvokeOnMainThread(() =>
            {
                this.MainPage = new NavigationPage ( new ErrorInitView ( e ) );
            });
        }

        public static void DoLogOff()
        {
            Settings.IsLoggedIn = false;
            credentialsService.DeleteCredentials();
            Singleton.Remove<Puck>();
            Mobile.LogPath = Mobile.ConfigPublicPath;
            ble_interface.Close();
        }

  
    }
}
