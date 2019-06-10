using System;
using System.Threading;
using Acr.UserDialogs;
using Foundation;
using Library;
using Microsoft.Intune.MAM;
using nexus.protocols.ble;
using UIKit;
using Xamarin.Essentials;

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
            //var Mode = GenericUtilsClass.ChekInstallMode();
            //if (Mode.Equals("None"))
            //{
            //    MAMService.LoginUserMAM();
            //    Thread.Sleep(2000);
            //}

            //Online.DownloadIntuneParameters ();
            //Parameters.PrepareFromIntune();

            // Core Foundation Keys:
            // https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Articles/CoreFoundationKeys.html
            var appVersion = NSBundle.MainBundle.InfoDictionary[ "CFBundleShortVersionString" ];
            var appBuild   = NSBundle.MainBundle.InfoDictionary[ "CFBundleVersion" ];

            IBluetoothLowEnergyAdapter bluetoothLowEnergyAdapter = BluetoothLowEnergyAdapter.ObtainDefaultAdapter();
            IUserDialogs userDialogs = UserDialogs.Instance;
            string appversion = appVersion.Description + " ( " + appBuild.Description + " )";


            appSave = new FormsApp ( bluetoothLowEnergyAdapter, userDialogs, appversion);
                 


            //string user = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
            //TEST.Test ();

            base.LoadApplication ( appSave );

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
