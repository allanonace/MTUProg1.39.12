using System;
using System.Windows.Input;
using Xamarin.Forms;
using ble.net.sampleapp.Models;
using ble.net.sampleapp.Helpers;

namespace ble.net.sampleapp.viewmodel
{
    public class LoginMenuViewModel : ViewModelBase
    {
        #region Commands
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }
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

        NavigationPage m_rootPage_login;

        public LoginMenuViewModel(NavigationPage m_rootPage)
        {
            m_rootPage_login = m_rootPage;
            LoginCommand = new Command(Login);



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
                        if (User.Email == "test@email.com" && User.Password == "123456")
                        {
                            Settings.IsLoggedIn = true;


                            Settings.SavedUserName = User.Email;

                            Application.Current.MainPage = m_rootPage_login;

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