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
        TaskCompletionSource<bool> tcs1;
        public FtpDownloadSettings(TaskCompletionSource<bool> tcs)
        {
            InitializeComponent();

            tcs1 = tcs;  //new TaskCompletionSource<bool>();
            //tcs1 = tcs;

            BindingContext = this;

            if (Device.Idiom == TargetIdiom.Tablet)
                ScaleFrame = 1;
            else
                ScaleFrame = 1;

            Executing(false);

            FocusEntryFields();

            if (config.HasFTP)
            {
                tbx_remote_host.Text = config.ftpDownload_Host;
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
                backgroundWait.IsEnabled = bExec;
                Botones.IsVisible = !bExec;
     
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
                await Navigation.PopAsync();
                tcs1.SetResult(true);

            }
            catch (Exception exc)
            {
                tcs1.SetResult(false);//await Application.Current.MainPage.Navigation.PopAsync();
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
            tcs1.SetResult(false);
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
        private double _scaleFrame;
        public double ScaleFrame
        {
            get => _scaleFrame;
            set
            {
                _scaleFrame = value;
                OnPropertyChanged();
            }
        }

        private void FocusEntryFields()
        {
            this.tbx_user_name.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-150);
            };

            this.tbx_user_name.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-150);
            };

            this.tbx_user_pass.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-150);
            };

            this.tbx_user_pass.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-150);
            };

        }

        void SetLayoutPosition(bool onFocus, int value)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.frm_FTP.TranslateTo(0, value, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.frm_FTP.TranslateTo(0, value, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.frm_FTP.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.frm_FTP.TranslateTo(0, 0, 50);
                }
            }
        }
    }
}
