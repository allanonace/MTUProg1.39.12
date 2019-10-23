﻿using System;
using System.Threading;
using Acr.UserDialogs;
using Foundation;
using Library;
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
        
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and make window visible
        // NOTE: You have 17 seconds to return from this method or iOS will terminate application
        public override bool FinishedLaunching (
            UIApplication uiApplication,
            NSDictionary  launchOptions )
        {
            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            // Core Foundation Keys:
            // https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Articles/CoreFoundationKeys.html
            var appVersion = NSBundle.MainBundle.InfoDictionary[ "CFBundleShortVersionString" ];
            var appBuild   = NSBundle.MainBundle.InfoDictionary[ "CFBundleVersion" ];

          
            IUserDialogs userDialogs = UserDialogs.Instance;
            string appversion = appVersion.Description + " ( " + appBuild.Description + " )";

            Data.Set("IsFromScripting", false);
           
            appSave = new FormsApp(userDialogs, appversion);

            base.LoadApplication ( appSave );

            return base.FinishedLaunching (uiApplication, launchOptions );
        }

        public override bool OpenUrl (
            UIApplication app,
            NSUrl         url,
            NSDictionary  options )
        {
            appSave.HandleUrl ( ( Uri )url );
            return true;
        }
    }
}
