using System;
using System.Collections.Generic;
using Xamarin.Forms;

using Xamarin.Essentials;
using Acr.UserDialogs;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MTUComm;
using System.Diagnostics;

namespace aclara_meters.view
{
    public partial class FtpDownloadSettings: INotifyPropertyChanged//: Rg.Plugins.Popup.Pages.PopupPage
    {
        private MTUComm.Mobile.ConfigData config = MTUComm.Mobile.configData;
        private TaskCompletionSource<string> taskSemaphoreDownload;
        const int smallWidthResolution = 768;
        const int smallHeightResolution = 1280;
        public FtpDownloadSettings (
            TaskCompletionSource<string> taskSemaphore )
        {
            InitializeComponent ();

            this.taskSemaphoreDownload = taskSemaphore;

            BindingContext = this;

            if (DeviceDisplay.MainDisplayInfo.Width < smallWidthResolution)
                this.ScaleFrame = 0.9;
            else this.ScaleFrame = 1;

            this.Executing ( false );

            this.FocusEntryFields ();

            if ( config.HasFTP )
            {
                tbx_remote_host.Text = config.ftpDownload_Host;
                //tbx_user_pass.Text = MTUComm.Mobile.configData.ftpDownload_Pass;
                tbx_user_name  .Text = config.ftpDownload_User;
                tbx_remote_path.Text = config.ftpDownload_Path;
                tbx_remote_port.Text = config.ftpDownload_Port.ToString ();
            }

            #if DEBUG
            tbx_remote_host.Text = "159.89.29.176";
            tbx_user_pass  .Text = "aclara1234";
            tbx_user_name  .Text = "aclara";
            tbx_remote_path.Text = "/home/aclara/prod";
            tbx_remote_port.Text = "22";
            #endif

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

            if ( ! res )
            {
                lb_Error.Text = "Error dowloading configuration files," + Environment.NewLine + "please check connection data or try it later";
                Executing(false);

                return;
            }

            try
            {
                await Navigation.PopAsync ();

                // Configuration files downloaded correctly
                taskSemaphoreDownload.SetResult ( "OK" );
            }
            catch ( Exception exc )
            {
                // Error downloading configuration files
                taskSemaphoreDownload.SetResult ( "ERROR" );
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
                config.ftpDownload_Port = iPort;

                SecureStorage.SetAsync("ftpDownload_Host", tbx_remote_host.Text);
                SecureStorage.SetAsync("ftpDownload_Port", iPort.ToString());
                SecureStorage.SetAsync("ftpDownload_Pass", tbx_user_pass.Text);
                SecureStorage.SetAsync("ftpDownload_User", tbx_user_name.Text);
                SecureStorage.SetAsync("ftpDownload_Path", tbx_remote_path.Text);


                if (GenericUtilsClass.DownloadConfigFiles(out string sFileCert))
                {
                    NewConfigVersion = GenericUtilsClass.CheckFTPConfigVersion();
                    SecureStorage.SetAsync("ConfigVersion", NewConfigVersion);
                    if (!string.IsNullOrEmpty(sFileCert))
                    {
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));
                    }
                    return true;
                }
                else
                {
                    Debug.WriteLine("-----------  Fallo bajando los ficheros---------------------");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("-----------  Fallo en la conexion con el FTP------------------");
                return false;
            }

        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync ();

            // Canceled downloading action
            taskSemaphoreDownload.SetResult ( "CANCEL" );
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
        private string NewConfigVersion;

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
            this.tbx_remote_path.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-150);
            };

            this.tbx_remote_path.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-150);
            };
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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
