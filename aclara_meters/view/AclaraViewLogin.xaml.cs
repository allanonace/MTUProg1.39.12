using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using Xamarin.Forms;
using Plugin.Geolocator;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using Renci.SshNet;
using System.IO;

namespace aclara_meters.view
{
    public partial class AclaraViewLogin : ContentPage
    {
        public viewmodel.LoginMenuViewModel viewModel;

        public AclaraViewLogin()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }

        public AclaraViewLogin(IUserDialogs dialogs)
        {
            InitializeComponent();
            Settings.IsNotConnectedInSettings = false;
            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(dialogs);
            viewModel.Navigation = this.Navigation;

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar(this, false); 

            loginpage.IsVisible = false;
            Task.Run(async () =>
            {
                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    loginpage.IsVisible = true;
                    /*
                    if(IsLocationAvailable()){

                        Task.Run(async () => { await StartListening(); });
                        //ListSFTPDataFiles();
                    }
                    */
                });
            });


            this.EmailEntry.Focused += (s, e) => { SetLayoutPosition(true, (int) -20); };
            this.EmailEntry.Unfocused += (s, e) => { SetLayoutPosition(false, (int)-20); };


            this.PasswordEntry.Focused += (s, e) => { SetLayoutPosition(true, (int) -80); };
            this.PasswordEntry.Unfocused += (s, e) => { SetLayoutPosition(false, (int) -80); };
        }


        void SetLayoutPosition(bool onFocus, int value)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.loginpage.TranslateTo(0, value, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.loginpage.TranslateTo(0, value, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.loginpage.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.loginpage.TranslateTo(0, 0, 50);
                }
            }
        }

        private void ListSFTPDataFiles()
        {
            //string host = "192.168.1.39";
            string host = "169.254.130.57";
            string username = "ma.jimenez";
            string password = "Ingen167";
            string pathRemoteFile = "/Users/ma.jimenez/Desktop/xmltest/User.xml";

            // Path where the file should be saved once downloaded (locally)
            string pathLocalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "User.txt");

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    Console.WriteLine("Downloading {0}", pathRemoteFile);

                    //var files = sftp.ListDirectory(remoteDirectory);
                    //  foreach (var file in files)
                    //  {
                    // Console.WriteLine(file.Name);
                    //   }
                    //    sftp.Disconnect();

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                    }

                    sftp.Disconnect();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught " + e.ToString());
                }
            }
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;
            //CrossGeolocator.Current.DesiredAccuracy = 1;
            CrossGeolocator.Current.DesiredAccuracy = 5;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);
            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            Debug.WriteLine(output);
            accuracy.Text = output.ToString();
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
        }

        private async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }
}
