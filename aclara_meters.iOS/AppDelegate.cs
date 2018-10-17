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
using Microsoft.Intune.MAM;

namespace aclara_meters.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private FormsApp appSave;
        private string identity = "";

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

            //identity = IntuneMAMEnrollmentManager.Instance.EnrolledAccount;
            // IntuneMAMEnrollmentManager.Instance.LoginAndEnrollAccount("ma.jimenez@bizintekinnova.com");

            var keys = new[]
            {
                new NSString("ftp_url"),
                new NSString("ftp_username")
            };

            /// IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;
            /*
            NSDictionary dictionary = value.DiagnosticInformation;

            int count = (int) dictionary.Count;

            Console.WriteLine("Elementos diccionario: ");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Key: "+ dictionary.ElementAt(i).Key  + " Value: "+ dictionary.ElementAt(i).Value);

            }

            IntuneMAMPolicySource IntuneMAMPolicySource2 = value.MamPolicySource;

       */
            /*
            try{

                string object1 = NSBundle.MainBundle.ObjectForInfoDictionary("ftp_username").ToString();
                string object2 = NSBundle.MainBundle.ObjectForInfoDictionary("ftp_url").ToString();
                string object3 = NSBundle.MainBundle.ObjectForInfoDictionary("userprincipalname").ToString();


                Console.WriteLine("ftp_username: " + object1 + " ftp_url: " + object2 + "userprincipalname: " + object3);

            }catch (Exception c){
                Console.WriteLine(c.StackTrace);
            }
*/

            IntuneMAMAppConfigManager appConfigManager = IntuneMAMAppConfigManager.Instance;

            IntuneMAMEnrollmentManager intuneMAMEnrollmentManager = IntuneMAMEnrollmentManager.Instance;




            identity = "ma.jimenez@bizintekinnova.com";


           

            IntuneMAMAppConfig intuneMAMAppConfig = appConfigManager.AppConfigForIdentity(identity);

          
            string[] field1 = intuneMAMAppConfig.AllStringsForKey("ftp_username");

            string[] field2 = intuneMAMAppConfig.AllStringsForKey("ftp_url");

            string[] field3 = intuneMAMAppConfig.AllStringsForKey("userprincipalname");

         
            List <string> listaDatos = new List<string>();

            try{
                if(field1.Length != 0 || field1 != null )
                    listaDatos.Add("ftp_username: " + field1[0]);
                
            }catch (Exception c1){

            }

            try
            {

                if (field2.Length !=  0 || field2 != null)
                    listaDatos.Add("ftp_url: " + field2[0]);
                
             }catch (Exception c1){

             }

            try
            {
                
            if (field3.Length != 0 || field3 != null)
                listaDatos.Add("userprincipalname: " + field3[0]);
                
            }
            catch (Exception c1)
            {

            }

            appSave = new FormsApp(BluetoothLowEnergyAdapter.ObtainDefaultAdapter(), UserDialogs.Instance, listaDatos);
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
