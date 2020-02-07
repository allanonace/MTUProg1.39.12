using System;
using System.IO;
using System.Threading.Tasks;
using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using Xamarin.Forms;
using Xml;

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

        public async void Btn_Cancel_Clicked(object sender, EventArgs args )
        {
            await DisplayAlert("Attention", "The app will close, you must decide the installation mode for the app", "OK");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public async void Btn_FTP_Clicked(object sender, EventArgs args )
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
                    try
                    {
                        // Verify the configuration files and preload important information for the hardware
                        // [ Configuration.cs ] ConfigurationFilesNotFoundException
                        // [ Configuration.cs ] ConfigurationFilesCorruptedException
                        // [ Configuration.cs ] DeviceMinDateAllowedException
                        Configuration config = Configuration.GetInstance ();

                        #if DEBUG

                        // Force some error cases in debug mode
                        DebugOptions debug = config.Debug;
                        if ( debug != null )
                        {
                            if ( debug.ForceErrorConfig_New_Date )
                                throw new DeviceMinDateAllowedException ();
                            else if ( debug.ForceErrorConfig_New_Files )
                                throw new Exception ();
                        }

                        #endif

                        await DisplayAlert ( "Attention", "The app will close to apply the new configuration", "OK" );
                        System.Diagnostics.Process.GetCurrentProcess ().Kill ();
                    }
                    catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
                    {
                        GenericUtilsClass.SetInstallMode("None");
                        GenericUtilsClass.DeleteConfigFiles(Mobile.ConfigPath);
                        
                        if ( ! Errors.IsOwnException ( e ) )
                            e = new ConfigFilesCorruptedException ();

                        base.ShowErrorAndKill ( e );                        
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
        
        public async void Btn_Intune_Clicked(object sender, EventArgs args )
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
        public async void Btn_Manual_Clicked(object sender, EventArgs args )
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
