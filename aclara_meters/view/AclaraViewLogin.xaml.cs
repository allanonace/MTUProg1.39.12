﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using aclara_meters.Helpers;
using Acr.UserDialogs;
using MTUComm;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Renci.SshNet;
using Xamarin.Forms;

namespace aclara_meters.view
{
    public partial class AclaraViewLogin
    {
        #region Attributes

        public viewmodel.LoginMenuViewModel viewModel;

        #endregion

        #region Initialization

        public AclaraViewLogin ()
        {
            InitializeComponent();
        }

        public AclaraViewLogin (
            IUserDialogs dialogs )
            : this ()
        {
            Settings.IsNotConnectedInSettings = false;
            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(dialogs);
            viewModel.Navigation = this.Navigation;

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            loginpage.IsVisible = false;
            Task.Run ( async () =>
            {
                await Task.Delay ( 1000 );
                Device.BeginInvokeOnMainThread ( () =>
                {
                    loginpage.IsVisible = true;

                    if ( Mobile.IsNetAvailable () )
                    {
                        if ( this.UploadingLogFiles () )
                        {
                            //base.DisplayAlert ( "Information", "All Log files uploaded!", "Ok" );

                            //(( AclaraViewMainMenu )Application.Current.MainPage.Navigation.NavigationStack[ 1 ] ).FirstRefreshSearchPucs ();
                        }
                        else base.DisplayAlert ( "Error", "Error Uploading files", "Ok" );
                    }
                    else base.DisplayAlert ( "Warning", "No connection available. Log files will not be uploaded till you get internet connection", "Ok" );

                    // Force to
                    //(( AclaraViewMainMenu )Application.Current.MainPage.Navigation.NavigationStack[ 1 ] ).FirstRefreshSearchPucs ();
                });
            });

            this.EmailEntry.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-20);
            };

            this.EmailEntry.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-20);
            };

            this.PasswordEntry.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-240);
                else
                    SetLayoutPosition(true, (int)-80);
            };

            this.PasswordEntry.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-240);
                else
                    SetLayoutPosition(false, (int)-80);
            };

            EmailEntry.MaxLength = FormsApp.config.global.UserIdMaxLength;

            //EmailEntry.MaxLength = FormsApp.config.global.UserIdMinLength;

            PasswordEntry.MaxLength = FormsApp.config.global.PasswordMaxLength;

        }

        #endregion

        #region Log files



        #endregion

        protected override bool OnBackButtonPressed ()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }
        
        private bool UploadingLogFiles ()
        {
            string ftp_username = FormsApp.config.global.ftpUserName;
            string ftp_password = FormsApp.config.global.ftpPassword;
            string ftp_remoteHost = FormsApp.config.global.ftpRemoteHost;
            string ftp_remotePath = FormsApp.config.global.ftpRemotePath; //For the logs...


            string host = FormsApp.config.global.ftpRemoteHost;
            string username = FormsApp.config.global.ftpUserName;
            string password = FormsApp.config.global.ftpPassword;

            //string pathRemoteFile = "/home/aclara/"; // prueba_archivo.xml";

            //TODO: UUID MOVIL EN PATH REMOTE FILE
            string pathRemoteFile = "/home/aclara"+ FormsApp.config.global.ftpRemotePath + CrossDeviceInfo.Current.Id + "/"; // prueba_archivo.xml";

            // Path where the file should be saved once downloaded (locally)
            string path = Mobile.GetPath ();
            
            //string name = "ReadMtuResult.xml";
            //string filename = Path.Combine(xml_documents, name);
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    if(!sftp.Exists(pathRemoteFile)){
                        sftp.CreateDirectory(pathRemoteFile);
                    }
                    //TODO

                    List<string> saved_array_files = new List<string>();

                    try
                    {
                        var lines = File.ReadAllLines(Path.Combine( path, "SavedLogsList.txt"));
                        foreach (var line in lines)
                        {
                            saved_array_files.Add(line);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }

                    List<FileInfo> local_array_files = new List<FileInfo>();
                    DirectoryInfo info = new DirectoryInfo( path );
                    FileInfo[] files = info.GetFiles().OrderBy(p => p.LastWriteTimeUtc).ToArray();

                    foreach (FileInfo file in files)
                    { 

                        if (file.Name.Contains("Log.xml") || file.Name.Contains("Result") )
                        {
                            Console.WriteLine(file.Name + " Last Write time: " + file.LastWriteTimeUtc.ToString());
                            bool enc = false;
                            foreach (string fileFtp in saved_array_files)
                            {
                                if (fileFtp.Equals(file.Name))
                                {
                                    enc = true;
                                }
                            }

                            if (!enc)
                            {

                                if( file.Name.Contains("Result") )
                                {
                                    local_array_files.Add(file);
                                }
                                else
                                {
                                    string dayfix = file.Name.Split('.')[0].Replace("Log", "");
                                    DateTime date = DateTime.ParseExact(dayfix, "MMddyyyyHH", CultureInfo.InvariantCulture).ToUniversalTime();
                                    TimeSpan diff = date - DateTime.UtcNow;
                                    int hours = (int)diff.TotalHours;
                                    if (hours < 0)
                                    {
                                        local_array_files.Add(file);
                                    }
                                }
                            }
                        }
                    }

                    if (local_array_files.Count > 0)
                    {
                        foreach (FileInfo file in local_array_files)
                        {
                            var fileStream = new FileStream(file.FullName, FileMode.Open);
                            if (fileStream != null)
                            {
                                sftp.UploadFile(fileStream, Path.Combine(pathRemoteFile, file.Name), null);
                            }
                            long cont = fileStream.Length;
                            fileStream.Close();
                            File.Delete(file.FullName);
                        }

                        try
                        {
                            using (TextWriter tw = new StreamWriter(Path.Combine(path, "SavedLogsList.txt")))
                            {
                                foreach (string fileFtp in saved_array_files)
                                {
                                    tw.WriteLine(fileFtp);
                                }
                                foreach (FileInfo s in local_array_files)
                                    tw.WriteLine(s.Name);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    sftp.Disconnect();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught " + e.ToString());
                }
            }

            return false;
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
