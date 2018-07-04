using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ble.net.sampleapp.view
{
    public partial class LoginMenuPage : ContentPage
    {

        private viewmodel.LoginMenuViewModel viewModel;


        public LoginMenuPage()
        {
            InitializeComponent();
        }


        public LoginMenuPage(NavigationPage m_rootPage)
        {
            InitializeComponent();

            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(m_rootPage);
            viewModel.Navigation = this.Navigation;

        }

        protected override bool OnBackButtonPressed()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }

    }
}
