using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using Acr.UserDialogs;
using ble.net.sampleapp.viewmodel;
using nexus.protocols.ble;
using Xamarin.Forms;

namespace ble.net.sampleapp.view
{
    public partial class LoginMenuPage : ContentPage
    {

        public viewmodel.LoginMenuViewModel viewModel;


        public LoginMenuPage()
        {
            InitializeComponent();
        }


        public LoginMenuPage(IBluetoothLowEnergyAdapter bleAdapter, IUserDialogs dialogs)
        {
            InitializeComponent();

            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(bleAdapter, dialogs);
            viewModel.Navigation = this.Navigation;
        }


        protected override bool OnBackButtonPressed()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }

    }
}
