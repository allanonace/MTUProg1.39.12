using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Acr.UserDialogs;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace aclara_meters.view
{
    public partial class FtpDownloadSettings: INotifyPropertyChanged//: Rg.Plugins.Popup.Pages.PopupPage
    {
        MTUComm.Mobile.ConfigData config = MTUComm.Mobile.configData;
        public FtpDownloadSettings()
        {
            InitializeComponent();

            BindingContext = this;
            //CloseWhenBackgroundIsClicked = false;

            //Findicator.IsVisible = false;
            // dialog_FTP.IsVisible = true;
            //indicator.IsVisible = false;
            Executing(false);
                      
          
            if (config.HasFTP)
            {
                tbx_remote_host.Text = config.ftpDownload_Host ;
                //tbx_user_pass.Text = MTUComm.Mobile.configData.ftpDownload_Pass;
                tbx_user_name.Text = config.ftpDownload_User;
                tbx_remote_path.Text = config.ftpDownload_Path;
                tbx_remote_port.Text = config.ftpDownload_Port.ToString();
            }
            return;

        }
        private void Executing (bool bExec)
        {
            
                lb_Error.Text = bExec ? "" : lb_Error.Text;

                Loading = bExec;
     
        }
        private async void OK_Clicked(object sender, EventArgs e)
        {
        
            Executing(true);

            bool res=false;
            await Task.Run(async () => { res = ProcessFtp(); });

            if (!res)
            {
                lb_Error.Text = "Error dowloading configuration files," + Environment.NewLine + "please check connection data or try it later";
                Executing(false);

                return;
            }
            try
            {
                FormsApp.tcs.SetResult(true);
                await Navigation.PopAsync();
                //await PopupNavigation.Instance.PopAsync();

            }
            catch (Exception exc)
            {

            }
          
            return;

        }

        private bool ProcessFtp()
        {

            if (!int.TryParse(tbx_remote_port.Text, out int iPort))
                iPort = 22;

            if (GenericUtilsClass.TestFtpCredentials(tbx_remote_host.Text, tbx_user_name.Text, tbx_user_pass.Text, tbx_remote_path.Text, iPort))
            {
                config.ftpDownload_Host = tbx_remote_host.Text;
                config.ftpDownload_Pass = tbx_user_pass.Text;
                config.ftpDownload_User = tbx_user_name.Text;
                config.ftpDownload_Path = tbx_remote_path.Text;
                config.HasFTP = true;
                MTUComm.Mobile.configData.ftpDownload_Port = iPort;

                SecureStorage.SetAsync("ftpDownload_Host", tbx_remote_host.Text);
                SecureStorage.SetAsync("ftpDownload_Port", iPort.ToString());
                // await SecureStorage.SetAsync("ftpDownload_Pass", tbx_user_pass.Text);
                SecureStorage.SetAsync("ftpDownload_User", tbx_user_name.Text);
                SecureStorage.SetAsync("ftpDownload_Path", tbx_remote_path.Text);


                if (!GenericUtilsClass.DownloadConfigFiles())
                {

                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //await PopupNavigation.Instance.PopAsync();
            FormsApp.tcs.SetResult(false);
        }

        private bool isLoading;
        public bool Loading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

             

    }
}
