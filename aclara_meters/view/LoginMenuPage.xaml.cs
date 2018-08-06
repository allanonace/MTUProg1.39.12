using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using aclara_meters.viewmodel;
using nexus.protocols.ble;
using Xamarin.Forms;

using Plugin.Geolocator;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using Renci.SshNet;
using System.IO;


namespace aclara_meters.view
{
    public partial class LoginMenuPage : ContentPage
    {



        public viewmodel.LoginMenuViewModel viewModel;


        public LoginMenuPage()
        {
            InitializeComponent();
        }


        public LoginMenuPage(IUserDialogs dialogs)
        {
            InitializeComponent();
            Settings.IsNotConnectedInSettings = false;
           
            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(dialogs);
            viewModel.Navigation = this.Navigation;

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            loginpage.IsVisible = false;

            Task.Run(async () =>
            {

                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    loginpage.IsVisible = true;

           
                    if(IsLocationAvailable()){
                        Task.Run(async () => { await StartListening(); });
                        //listFiles();
                    }
                });
            });

       

        }



        private void listFiles()
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

        protected override bool OnBackButtonPressed()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;
            //CrossGeolocator.Current.DesiredAccuracy = 1;
            CrossGeolocator.Current.DesiredAccuracy = 5;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }


        /*
        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            GetCurrentLocationAsync();
          

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }


        public static async void GetCurrentLocationAsync()
        {

            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return;
        }

*/



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
            //Handle event here for errors
        }

        async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;
            
            CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }



    }
}
