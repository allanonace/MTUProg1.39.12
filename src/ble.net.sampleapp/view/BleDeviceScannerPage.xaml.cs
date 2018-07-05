// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ble.net.sampleapp.Helpers;
using ble.net.sampleapp.Models;
using ble.net.sampleapp.viewmodel;
using nexus.core.logging;
using nexus.protocols.ble;
using Xamarin.Forms;

namespace ble.net.sampleapp.view
{
   public partial class BleDeviceScannerPage
   {
        
  
       public BleDeviceScannerPage()
       {
          InitializeComponent();
       }

        BleDeviceScannerViewModel bleScanViewModel_login;
        public List<PageItem> menuList { get; set; }

        public BleDeviceScannerPage(BleDeviceScannerViewModel bleScanViewModel )
        {
            
             InitializeComponent();



             bleScanViewModel_login = bleScanViewModel;

             BindingContext = bleScanViewModel_login;


             NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

             Task.Run(async () =>
             {
                 await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                 {
                     bleScanViewModel.ScanForDevicesCommand.Execute(true);

                 });
             });


             back_button.Tapped += hamburgerOpen;
             back_button_menu.Tapped += hamburgerClose;
             logout_button.Tapped += logout;


             ContentNav.IsVisible = false;
             ContentNav.IsEnabled = true;
             background_scan_page.Opacity = 1;

           
             //MENU


             //Change username textview to Prefs. String
             if (Settings.SavedUserName != null)
             {
                userName.Text = Settings.SavedUserName;
             }

             menuList = new List<PageItem>();
             
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1 = new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" };
            var page2 = new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "" };
            var page3 = new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" };
            var page4 = new PageItem() { Title = "Rep. MTU", Icon = "replaceMTU.png", TargetType = "" };
            var page5 = new PageItem() { Title = "Rep. Meter", Icon = "replaceMeter.png", TargetType = "" };
            var page6 = new PageItem() { Title = "Add MTU / Add meter", Icon = "addMTUaddmeter.png", TargetType = "" };
            var page7 = new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "" };
            var page8 = new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "" };
            var page9 = new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "" };


            // Adding menu items to menuList
            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);
            menuList.Add(page5);
            menuList.Add(page6);
            menuList.Add(page7);
            menuList.Add(page8);
            menuList.Add(page9);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
           

       }

        private void logout(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            Application.Current.MainPage = new LoginMenuPage(bleScanViewModel_login);

         


        }



        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

           // if (Settings.IsConnectedBLE)
          //  {
                var item = (PageItem)e.SelectedItem;
                String page = item.TargetType;

                switch (page)
                {
                    case "ReadMTU":
                        //Application.Current.MainPage = new NavigationPage(new TestPage1());

                        break;

                    case "AddMTU":
                        //Application.Current.MainPage = new NavigationPage(new AddMTUPage());

                        break;


                }


                background_scan_page.Opacity = 1;

                ContentNav.Opacity = 0;


                ContentNav.IsVisible = false;
                ContentNav.IsEnabled = true;


           // }
        }




        private void hamburgerOpen(object sender, EventArgs e)
        {
            ContentNav.IsVisible = true;
            ContentNav.IsEnabled = true;
            background_scan_page.Opacity = 0.5;

            ContentNav.Opacity = 1;

           


        }
     

        private void hamburgerClose(object sender, EventArgs e)
        {
    

            background_scan_page.Opacity = 1;
              
            ContentNav.Opacity = 0;
              

            ContentNav.IsVisible = false;
            ContentNav.IsEnabled = true;
            




        }


      private void ListView_OnItemSelected( Object sender, SelectedItemChangedEventArgs e )
      {
         if(e.SelectedItem != null)
         {
            //((BlePeripheralViewModel)e.SelectedItem).IsExpanded = !((BlePeripheralViewModel)e.SelectedItem).IsExpanded;
            ((ListView)sender).SelectedItem = null;
         }
      }

      private void ListView_OnItemTapped( Object sender, ItemTappedEventArgs e )
      {
      }

      private void Switch_OnToggled( Object sender, ToggledEventArgs e )
      {
         var vm = BindingContext as BleDeviceScannerViewModel;
         if(vm == null)
         {
            return;
         }
         if(e.Value)
         {
            if(vm.EnableAdapterCommand.CanExecute( null ))
            {
               vm.EnableAdapterCommand.Execute( null );
            }
         }
         else if(vm.DisableAdapterCommand.CanExecute( null ))
         {
            vm.DisableAdapterCommand.Execute( null );
         }
      }
   }
}
