using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Foundation;
using nexus.protocols.ble;
using UIKit;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;

namespace aclara_meters.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private FormsApp appSave;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            Distribute.DontCheckForUpdatesInDebug();
            appSave = new FormsApp(BluetoothLowEnergyAdapter.ObtainDefaultAdapter(), UserDialogs.Instance);
            LoadApplication( appSave );
          
            return base.FinishedLaunching(app, options);
        }


        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            appSave.HandleUrl( (Uri) url);
        
            return true;
        }

    }



}
