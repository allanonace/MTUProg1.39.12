using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aclara_meters.view
{
    public partial class AclaraInstallPage : BasePage 
    {
        IUserDialogs dialogs;

        public AclaraInstallPage(IUserDialogs dialogs)
        {
            InitializeComponent();

            this.dialogs = dialogs;
            btn_Cancel.Clicked += Btn_Cancel_Clicked;
            btn_FTP.Clicked += Btn_FTP_Clicked;
            btn_Intune.Clicked += Btn_Intune_Clicked;
            btn_Manual.Clicked += Btn_Manual_Clicked;

            if (Data.Get.IsAndroid)
                btn_Intune.IsVisible = false;
        }

        public async void Btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Attention", "The app will close, you must decide the installation mode for the app", "OK");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public async void Btn_FTP_Clicked(object sender, EventArgs e)
        {
            string result;
            if (!Mobile.IsNetAvailable())
            {
                await DisplayAlert("Attention", "No internet connection, try later", "OK");
                return;
            }
            GenericUtilsClass.SetInstallMode("FTP");
            TaskCompletionSource<string> taskSemaphoreDownload = new TaskCompletionSource<string>();
            await Navigation.PushAsync(new FtpDownloadSettings(taskSemaphoreDownload));
            result = await taskSemaphoreDownload.Task;
            switch(result)
            {
                case "OK":
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    Application.Current.MainPage = new NavigationPage(new AclaraViewConfig(dialogs));
                    //});
                    if (Configuration.LoadAndVerifyXMLs())
                    {
                        await DisplayAlert("Attention", "The app will close to apply the configuration", "OK");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    else
                    {
                        GenericUtilsClass.SetInstallMode("None");
                        GenericUtilsClass.DeleteConfigFiles(Mobile.ConfigPath);
                        await DisplayAlert("Attention",
                            "There is a problem with the configuration files downloaded from SFTP, "+ Environment.NewLine +
                            "some of them are corrupted or maybe some MTUs don't have port type defined. Contact your IT administrator", "OK");                        
                    }
                    return;

                case "ERROR":
                    GenericUtilsClass.SetInstallMode("None");

                    await DisplayAlert("Attention", "The app will close, try configuration again later", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                case "CANCEL":
                    GenericUtilsClass.SetInstallMode("None");
                    return;
            }
       
        }
        public async void Btn_Intune_Clicked(object sender, EventArgs e)
        {
            if (!Mobile.IsNetAvailable())
            {
                await DisplayAlert("Attention", "No internet connection, try later", "OK");
                return;
            }
            GenericUtilsClass.SetInstallMode("Intune");
            var MamServ = DependencyService.Get<IMAMService>();
            MamServ.LoginUserMAM();
            await DisplayAlert("Attention", "The app will close to apply the configuration", "OK");
           
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public async void Btn_Manual_Clicked(object sender, EventArgs e)
        {
            GenericUtilsClass.SetInstallMode("Manual");
            await DisplayAlert("Attention", "Now you must copy the config files in the public folder of the app, then restart the app", "OK");
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    Application.Current.MainPage = new NavigationPage(new AclaraViewConfig(dialogs));
            //});
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }     
    }
}
