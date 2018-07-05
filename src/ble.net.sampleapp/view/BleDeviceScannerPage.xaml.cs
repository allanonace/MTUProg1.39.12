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
using Newtonsoft.Json;
using nexus.core.logging;
using nexus.protocols.ble;
using Xamarin.Forms;

namespace ble.net.sampleapp.view
{
   public partial class BleDeviceScannerPage
   {
       // Desconectar device
       // ((BleGattServerViewModel) BindingContext).DisconnectFromDeviceCommand.Execute( null );

      public BleDeviceScannerPage()
      {
         InitializeComponent();
      }

        private BleGattServiceViewModel m_bleServiceSelected;

        private void OnServiceSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                m_bleServiceSelected = ((BleGattServiceViewModel)e.SelectedItem);
                ((ListView)sender).SelectedItem = null;

                bleScanViewModel.StopScan();
                   

                string json = JsonConvert.SerializeObject(m_bleServiceSelected, Formatting.None);

                //BleGattServiceViewModel account = JsonConvert.DeserializeObject<BleGattServiceViewModel>(json);

                Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(m_bleServiceSelected));
                   
           
              


            }
        }



        private void connection()
        {

            //var logsVm = new LogsViewModel();
            //SystemLog.Instance.AddSink(logsVm);

            var bleAssembly = bleAdapterSaved.GetType().GetTypeInfo().Assembly.GetName();
            Log.Info(bleAssembly.Name + "@" + bleAssembly.Version);

            var bleGattServerViewModel = new BleGattServerViewModel(dialogsSaved, bleAdapterSaved);

            bleScanViewModel = new BleDeviceScannerViewModel(
                bleAdapter: bleAdapterSaved,
                dialogs: dialogsSaved,
               onSelectDevice:



                async p =>
                {
                    await bleGattServerViewModel.Update(p);

                    background_scan_page_detail.IsEnabled = true;
                    background_scan_page_detail.IsVisible = true;


                    background_scan_page.IsEnabled = false;
                    background_scan_page.IsVisible = false;


                    BindingContext = bleGattServerViewModel;




                    // m_bleServiceSelected;

                    /*
                        await Application.Current.MainPage.Navigation.PushAsync(
                               new BleGattServerPage(
                                  model: bleGattServerViewModel,
                                    bleServiceSelected: async s =>
                                    {

                                        await Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(s));
                                        // LOGICA DE LA VISTA DETALLADA

                                    }
                                )
                            );

                    */


                    await Task.Run(async () =>
                     {

                         await Task.Delay(1000); Device.BeginInvokeOnMainThread(async () =>
                         {
                             await bleGattServerViewModel.OpenConnection();

                             navigationDrawerList.IsEnabled = true;
                       
                             navigationDrawerList.Opacity = 1;



                         });
                     });
                  
                }


            );

            BindingContext = bleScanViewModel;

        


            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            Task.Run(async () =>
            {

                await Task.Delay(1500); Device.BeginInvokeOnMainThread(() =>
                {
                    bleScanViewModel.ScanForDevicesCommand.Execute(true);
                   
                });
            });
        }


      public List<PageItem> menuList { get; set; }


        IBluetoothLowEnergyAdapter bleAdapterSaved;
        IUserDialogs dialogsSaved;
        BleDeviceScannerViewModel bleScanViewModel;

        public BleDeviceScannerPage(IBluetoothLowEnergyAdapter bleAdapter, IUserDialogs dialogs )
        {

            InitializeComponent();


            bleAdapterSaved = bleAdapter;
            dialogsSaved = dialogs;

            connection();

            back_button.Tapped += hamburgerOpen;
            back_button_menu.Tapped += hamburgerClose;
            logout_button.Tapped += logout;


            back_button_detail.Tapped += hamburgerOpen;


            navigationDrawerList.Opacity = 0.65;

            navigationDrawerList.IsEnabled = false;

            ContentNav.IsVisible = false;
            ContentNav.IsEnabled = true;
            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;


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
            Application.Current.MainPage = new LoginMenuPage(bleAdapterSaved,dialogsSaved );

         


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
            background_scan_page_detail.Opacity = 0.5;
            ContentNav.Opacity = 1;

           


        }
     

        private void hamburgerClose(object sender, EventArgs e)
        {
    

            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;
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
