﻿using System;
using System.Windows.Input;
using Xamarin.Forms;
using aclara_meters.Models;
using aclara_meters.Helpers;
using aclara_meters.view;
using nexus.protocols.ble;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace aclara_meters.viewmodel
{
    public class LoginMenuViewModel : ViewModelBase
    {
        #region Commands
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        #endregion

        #region Properties
        private User _user = new User();

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        #endregion


        IUserDialogs dialogs_save;

        public LoginMenuViewModel(IUserDialogs dialogs)
        {
          
            dialogs_save = dialogs;

            LoginCommand = new Command(Login);
            LoadCommand = new Command(load);

            Task.Run(async () =>
            {

                await Task.Delay(550); Device.BeginInvokeOnMainThread(() =>
                {
                    LoadCommand.Execute(null);
                });
            });

        }


        public void load()
        {
            if (FormsApp.CredentialsService.DoCredentialsExist())
            {
                Application.Current.MainPage.Navigation.PushAsync(new BleDeviceScannerPage(dialogs_save),false);
            }   
        }



        bool AreCredentialsCorrect(string username, string password)
        {
            /*
            string testData = @"<Users Encrypted=""true""> <user> <name>INSTALL1</name> <pass>6A6C60B602D435E7</pass> </user> <user> <name>INSTALL2</name> <pass>6A6C60B602D435E7</pass> </user> <user> <name>Bob</name> <pass>9BFA1831B2529666F9DF84C0136247AD</pass> </user> <user> <name>test</name> <pass>test</pass> </user> </Users>";

            XmlSerializer serializer = new XmlSerializer(typeof(XML.XmlElementList.Users));
            // testData is your xml string
            using (TextReader reader = new StringReader(testData))
            {
                //Configuration result = (Configuration)serializer.Deserialize(reader);

                XML.XmlElementList.Users result =  (XML.XmlElementList.Users)serializer.Deserialize(reader);
            }


          */
           // string[] arr = XDocument.Load(@"User.xml").Descendants("Users").Select(element => element.Value).ToArray();

            // Path where the file should be saved once downloaded (locally)
            //string pathLocalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "User.txt");


            //string[] arr = XDocument.Load(pathLocalFile).Descendants("Users").Select(element => element.Value).ToArray();




            /*

            XmlSerializer serializer = new XmlSerializer(typeof(XML.XmlElementList.Users));



            // testData is your xml string
            using (TextReader reader = new StringReader(XDocument.Load(pathLocalFile).ToString()))
            {
                //Configuration result = (Configuration)serializer.Deserialize(reader);
                XDocument xd = XDocument.Load(reader);
                String jsonresp = xd.Root;
                XML.XmlElementList.Users users = JsonConvert.DeserializeObject<XML.XmlElementList.Users>


                XML.XmlElementList.Users result = (XML.XmlElementList.Users)serializer.Deserialize(reader);
            }

*/

            return username == Constants.Username && password == Constants.Password;
        }


        public async void Login()
        {
            
            IsBusy = true;
            Title = string.Empty;
            try
            {
                if (User.Email != null)
                {
                    if (User.Password != null)
                    {

                        string userName = User.Email;
                        string password = User.Password;

                        var isValid = AreCredentialsCorrect(userName, password);

                        if (isValid)
                        {
                            bool doCredentialsExist = FormsApp.CredentialsService.DoCredentialsExist();
                            if (!doCredentialsExist)
                            {
                                FormsApp.CredentialsService.SaveCredentials(userName, password);
                        
                            }

                            Settings.IsLoggedIn = true;


                            Settings.SavedUserName = User.Email;


                            Application.Current.MainPage.Navigation.PushAsync(new BleDeviceScannerPage(dialogs_save), false);

                        }
                        else
                        {
                            Message = "Wrong username or password";
                        }
                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;
                        Message = "Password required";
                    }

                }
                else
                {
                    IsBusy = false;
                    Message = "Email required";
                }

            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Connection error", e.Message, "Ok");
            }
        }


    }
}