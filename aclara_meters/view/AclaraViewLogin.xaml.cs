using System;
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
using Library.Exceptions;

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

            EmailEntry.MaxLength = FormsApp.config.Global.UserIdMaxLength;

            PasswordEntry.MaxLength = FormsApp.config.Global.PasswordMaxLength;

        }

        #endregion

        protected override bool OnBackButtonPressed ()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
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

      }
}
