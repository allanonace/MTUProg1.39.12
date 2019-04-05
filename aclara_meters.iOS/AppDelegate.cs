using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Foundation;
using Microsoft.Intune.MAM;
using nexus.protocols.ble;
using UIKit;

using System.Threading.Tasks;
using System.IO;

namespace aclara_meters.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private FormsApp appSave;
        private string identity = "";

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and make window visible
        // NOTE: You have 17 seconds to return from this method or iOS will terminate application
        public override bool FinishedLaunching (
            UIApplication app,
            NSDictionary  options )
        {
            global::Xamarin.Forms.Forms.Init();
            //Distribute.DontCheckForUpdatesInDebug();

            // Get Intun Parameters
            //Online.DownloadIntuneParameters ();
            
            var AppVersion = NSBundle.MainBundle.InfoDictionary[ "CFBundleVersion" ];

            IBluetoothLowEnergyAdapter bluetoothLowEnergyAdapter = BluetoothLowEnergyAdapter.ObtainDefaultAdapter();
            IUserDialogs userDialogs = UserDialogs.Instance;
            NSString appversion = (Foundation.NSString) AppVersion.Description;

            appSave = new FormsApp ( bluetoothLowEnergyAdapter, userDialogs, appversion);

            //appSave = new FormsApp(bluetoothLowEnergyAdapter);

            base.LoadApplication ( appSave );

            /*
            try
            {
            AppDomain.CurrentDomain.UnhandledException += async (sender, e) =>
            {
            
            };
            TaskScheduler.UnobservedTaskException += async (sender, e) =>
            {
            
            };
            }
            catch ( Exception e )
            {

            }
            */

            return base.FinishedLaunching ( app, options );
        }

        public override bool OpenUrl (
            UIApplication app,
            NSUrl         url,
            NSDictionary  options )
        {
            appSave.HandleUrl ( ( Uri )url, BluetoothLowEnergyAdapter.ObtainDefaultAdapter() );
            return true;
        }
    }
}
