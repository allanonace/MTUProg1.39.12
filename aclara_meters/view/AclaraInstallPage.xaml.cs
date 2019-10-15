using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aclara_meters.util;
using Library;
using Library.Exceptions;
using MTUComm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aclara_meters.view
{
    public partial class AclaraInstallPage : BasePage 
    {

        public AclaraInstallPage()
        {
            InitializeComponent();

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
                    await DisplayAlert("Attention", "The app will close to apply the configuration", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
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
            await DisplayAlert("Attention", "The app will close, you must copy the config files in the public folder of the app, and then restart", "OK");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }     
    }
}
