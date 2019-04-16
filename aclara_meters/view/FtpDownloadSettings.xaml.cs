using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Acr.UserDialogs;
using System.Threading;

namespace aclara_meters.view
{
    public partial class FtpDownloadSettings //: Rg.Plugins.Popup.Pages.PopupPage
    {
        MTUComm.Mobile.ConfigData config = MTUComm.Mobile.configData;
        public FtpDownloadSettings()
        {
            InitializeComponent();

            //CloseWhenBackgroundIsClicked = false;
            //Findicator.IsVisible = false;
            // dialog_FTP.IsVisible = true;
            indicator.IsVisible = false;
            indicator1.IsVisible = true;
          
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
        private void OK_Clicked(object sender, EventArgs e)
        {
            lb_Error.Text = "";

            indicator.IsVisible = true;
            indicator1.IsRunning = true;
            dialog_FTP.IsVisible = false;
            

            if (!ProcessFtp())
            {
                indicator.IsVisible = false;
                dialog_FTP.IsVisible = true;

                lb_Error.Text = "Error downloading configuration files," + Environment.NewLine + "please check connection data or try it later";

                return;
            }
            try
            {
                Navigation.PopAsync();
               //await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception exc)
            {

            }
            FormsApp.tcs.SetResult(true);
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

    }
}
