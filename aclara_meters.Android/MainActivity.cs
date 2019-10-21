// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aclara_meters.view;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Library;
using nexus.protocols.ble;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//using Application = Android.App.Application;

namespace aclara_meters.Droid
{
    [Activity(Theme = "@style/MainTheme",  MainLauncher = true, NoHistory = true, Name = "com.aclara.mtu.programmer.urlentryclass")]
    public class urlentryclass : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <remarks>
        /// This must be implemented if you want to Subscribe() to IBluetoothLowEnergyAdapter.State to be notified when the
        /// bluetooth adapter state changes (i.e., it is enabled or disabled). If you don't care about that in your use-case, then
        /// you don't need to implement this -- you can still query the state of the adapter, the observable just won't work. See
        /// <see cref="IBluetoothLowEnergyAdapter.State" />
        /// </remarks>
        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            BluetoothLowEnergyAdapter.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            // ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            try
            {
                // If you want to enable/disable the Bluetooth adapter from code, you must call this.
                BluetoothLowEnergyAdapter.Init(this);
            }
            catch (Exception e)
            {
                Utils.Print(e.StackTrace);
            }

            // Obtain the bluetooth adapter so we can pass it into our (shared-code) Xamarin Forms app. There are
            // additional Obtain() methods on BluetoothLowEnergyAdapter if you have more specific needs (e.g. if you
            // need to support devices with multiple Bluetooth adapters)
            //var bluetooth = BluetoothLowEnergyAdapter.ObtainDefaultAdapter(ApplicationContext);

            if ( Xamarin.Forms.Device.Idiom == TargetIdiom.Phone )
                 RequestedOrientation = ScreenOrientation.Portrait;
            else RequestedOrientation = ScreenOrientation.Landscape;

           // CrossCurrentActivity.Current.Init ( this, bundle );

            var data = Intent.Data;

            // check if this intent is started via custom scheme link
            if (data != null)
            {
                if ( data.Scheme == "aclara-mtu-programmer" )
                {
                    //accessCodeTextbox.Text = data.Host;
                }
            }

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.SplashScreen);

            Context     context    = Android.App.Application.Context;
            PackageInfo info       = context.PackageManager.GetPackageInfo ( context.PackageName, 0 );
            string      appversion = info.VersionName + " ( " + info.VersionCode + " )";

            Data.Set ( "IsFromScripting",   true);
            FormsApp app = new FormsApp (UserDialogs.Instance, appversion,new System.Uri(data.ToString()));
                     
            LoadApplication(app);
            //app.HandleUrl(new System.Uri(data.ToString()), bluetooth); 
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    //[Application(AllowBackup = true, AllowClearUserData = true)]
    //public class MyApplication : Android.App.Application
    //{
    //    protected MyApplication(IntPtr javaReference, JniHandleOwnership transfer)
    //    : base(javaReference, transfer)
    //    {
    //    }

    //    public override void OnCreate()
    //    {
            
    //    }
    //}

    [Activity(
        Label = "aclara_meters", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Unspecified
    )]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        /// <remarks>
        /// This must be implemented if you want to Subscribe() to IBluetoothLowEnergyAdapter.State to be notified when the
        /// bluetooth adapter state changes (i.e., it is enabled or disabled). If you don't care about that in your use-case, then
        /// you don't need to implement this -- you can still query the state of the adapter, the observable just won't work. See
        /// <see cref="IBluetoothLowEnergyAdapter.State" />
        /// </remarks>
        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            BluetoothLowEnergyAdapter.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
           // ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            CrossCurrentActivity.Current.Activity = this;
            try
            {
                // If you want to enable/disable the Bluetooth adapter from code, you must call this.
                BluetoothLowEnergyAdapter.Init(this);
            }catch(Exception e){
                Utils.Print(e.StackTrace);
            }

            // Obtain the bluetooth adapter so we can pass it into our (shared-code) Xamarin Forms app. There are
            // additional Obtain() methods on BluetoothLowEnergyAdapter if you have more specific needs (e.g. if you
            // need to support devices with multiple Bluetooth adapters)
            //var bluetooth = BluetoothLowEnergyAdapter.ObtainDefaultAdapter(ApplicationContext);
            

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
            else
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            }

            var context = Android.App.Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);

            string value = info.VersionName + " ( " + info.VersionCode + " )";


            // Check if FTP settings is in securestorage
            // GenericUtilsClass.CheckFTPDownload();

            Data.Set ( "IsFromScripting",   false );
            LoadApplication(new FormsApp (UserDialogs.Instance, value));

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            var ListPerm = new List<String>();

            if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                ListPerm.Add(Manifest.Permission.AccessCoarseLocation);
                ListPerm.Add(Manifest.Permission.AccessFineLocation);
            }

            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ListPerm.Add(Manifest.Permission.WriteExternalStorage);
                ListPerm.Add(Manifest.Permission.ReadExternalStorage);

            }
            if (CheckSelfPermission(Manifest.Permission.GetAccounts) != Permission.Granted)
            {
                ListPerm.Add(Manifest.Permission.GetAccounts);
                ListPerm.Add(Manifest.Permission.ManageAccounts);
                ListPerm.Add(Manifest.Permission.UseCredentials);

            }

            if (CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted)
            {
                ListPerm.Add(Manifest.Permission.Camera);
                ListPerm.Add(Manifest.Permission.Flashlight);

            }

            if (ListPerm.Count > 0)
                RequestPermissions(ListPerm.ToArray(), 0);

            base.OnStart();
        }

    }

    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }

}

