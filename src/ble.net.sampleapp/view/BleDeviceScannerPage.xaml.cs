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
        public  BleGattServiceViewModel accountSaved;
        //public  IBleGattServerConnection bleGattServerViewModelSaved;


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

             

               // String json = JsonConvert.SerializeObject(m_bleServiceSelected);

              //  String value = "\\";

              //  String tempjson = json.Replace(value, "");


              //  BleGattServiceViewModel account = JsonConvert.DeserializeObject<BleGattServiceViewModel>(tempjson);

               // BleGattServiceViewModel account = new BleGattServiceViewModel("2cf42000-7992-4d24-b05d-1effd0381208");


              //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(m_bleServiceSelected));
                   
          
            }
        }

        BleGattServerViewModel savedServer;

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

                    bleScanViewModel.StopScan();


                    macAddress.Text = p.Address;
                    deviceID.Text = p.Name;
                    rssiLevel.Text = p.Rssi.ToString() + " db";


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

                         await Task.Delay(500); Device.BeginInvokeOnMainThread(async () =>
                         {
                        
                             
                             await bleGattServerViewModel.OpenConnection();

                             navigationDrawerList.IsEnabled = true;

                             navigationDrawerList.Opacity = 1;

                             bleGattServerViewModel.returnConnect();
                        /*
                        await Task.Run(async () =>
                        {
                            await Task.Delay(1500); Device.BeginInvokeOnMainThread(() =>
                            {

                                Guid value = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                                BleGattServiceViewModel account = new BleGattServiceViewModel(value, bleGattServerViewModel.returnConnect(), dialogsSaved);
                                Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));
                            });
                        });*/
                             savedServer = bleGattServerViewModel;
                         
                        // public BleGattServiceViewModel( Guid service, IBleGattServerConnection gattServer, IUserDialogs dialogManager )
     

              //  
                   





                         });
                     });
                  
                }


            );

            BindingContext = bleScanViewModel;

        


            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            Task.Run(async () =>
            {

                await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                {
                    bleScanViewModel.ScanForDevicesCommand.Execute(true);
                   
                });
            });
        }


      public List<PageItem> menuList { get; set; }


        IBluetoothLowEnergyAdapter bleAdapterSaved;
        IUserDialogs dialogsSaved;
        BleDeviceScannerViewModel bleScanViewModel;

        public List<ReadMTUItem> menuListReadMTU { get; set; }
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







            menuListReadMTU = new List<ReadMTUItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            var page1read = new ReadMTUItem() { Title = "MTU Ser No.", Description = "-" };
            var page2read = new ReadMTUItem() { Title = "1 Way Tx Freq.", Description = "-" };
            var page3read = new ReadMTUItem() { Title = "2 Way Tx Freq.", Description = "-" };
            var page4read = new ReadMTUItem() { Title = "2 Way Rx Freq.", Description = "-" };


            // Adding menu items to menuList
            menuListReadMTU.Add(page1read);
            menuListReadMTU.Add(page2read);
            menuListReadMTU.Add(page3read);
            menuListReadMTU.Add(page4read);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            listREADMTU.ItemsSource = menuListReadMTU;




        }



        private void logout(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            Application.Current.MainPage = new LoginMenuPage(bleAdapterSaved,dialogsSaved );

         


        }



        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async Task OnMenuItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {

           // if (Settings.IsConnectedBLE)
          //  {
                var item = (PageItem)e.SelectedItem;
                String page = item.TargetType;

                switch (page)
                {
                    case "ReadMTU":
     
                       

                        await Task.Run(async () =>
                        {

                             await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                             {
                                 
                            Guid value = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                                 BleGattServiceViewModel account = new BleGattServiceViewModel(value, savedServer.returnConnect(), dialogsSaved);
                            Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, savedServer.returnConnect(), dialogsSaved));
                            //    ReadMtuMethod(account, savedServer, dialogsSaved);


                                background_scan_page.Opacity = 1;
                                background_scan_page_detail.Opacity = 1;
                                ContentNav.Opacity = 0;
              

                                 ContentNav.IsVisible = false;
                                 ContentNav.IsEnabled = true;

                                 navigationDrawerList.SelectedItem = null;
                                 navigationDrawerList.BeginRefresh();
                                 navigationDrawerList.SelectedItem = null;
                                 navigationDrawerList.EndRefresh();
                             });

                         });

                    //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                    break;
                    


                }


                background_scan_page.Opacity = 1;

                ContentNav.Opacity = 0;

                ContentNav.IsEnabled = true;
                ContentNav.IsVisible = false;
                


           // }
        }

        BleGattServiceViewModel model_saved;


        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;


        }

        private void ReadMtuMethod(BleGattServiceViewModel model, BleGattServerViewModel gattServer, IUserDialogs dialogs)
        {
            BindingContext = model;

            model_saved = model;



            Task.Run(async () =>
            {

                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {

                    model_saved.Characteristic.RemoveAt(0);




                    Guid ServicioIndicate = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                    Guid CaracterisicoIndicate = new Guid("00000003-0000-1000-8000-00805f9b34fb");

                    BleGattCharacteristicViewModel gattCharacteristicViewModelIndicate = new BleGattCharacteristicViewModel(ServicioIndicate, CaracterisicoIndicate, gattServer.returnConnect(), dialogs);

                    var viewModelIndicate = (BleGattCharacteristicViewModel)gattCharacteristicViewModelIndicate;

                    if (viewModelIndicate.EnableNotificationsCommand.CanExecute(null))
                        viewModelIndicate.EnableNotificationsCommand.Execute(null);



                    model_saved.Characteristic.Add(gattCharacteristicViewModelIndicate);

                    BindingContext = model_saved;

                    listREADMTU.BeginRefresh();

                    listREADMTU.EndRefresh();



                    Task.Run(async () =>
                    {

                        await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                        {

                            model_saved.Characteristic.RemoveAt(0);

                            Guid ServicioWrite = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                            Guid CaracterisicoWrite = new Guid("00000002-0000-1000-8000-00805f9b34fb");
                            // String valorDato = "000005258000015a";

                            BleGattCharacteristicViewModel gattCharacteristicViewModelWrite = new BleGattCharacteristicViewModel(ServicioWrite, CaracterisicoWrite, gattServer.returnConnect(), dialogs);

                            var viewModelWrite = (BleGattCharacteristicViewModel)gattCharacteristicViewModelWrite;
                            if (viewModelWrite.WriteCurrentBytesGUIDCommand.CanExecute(null))
                                viewModelWrite.WriteCurrentBytesGUIDCommand.Execute(null);



                            model_saved.Characteristic.Add(gattCharacteristicViewModelWrite);

                            BindingContext = model_saved;

                            listREADMTU.BeginRefresh();

                            listREADMTU.EndRefresh();


                            Task.Run(async () =>
                            {

                                await Task.Delay(1500); Device.BeginInvokeOnMainThread(() =>
                                {

                                    BleGattCharacteristicViewModel[] array = new BleGattCharacteristicViewModel[10];
                                    model_saved.Characteristic.CopyTo(array, 0);



                                    Byte[] value1 = array[0].ValueAsHexBytes;

                                    int longitud = value1.Length;

                                    int contador = 0;


                                    byte[] listTotal = new byte[1024];
                                    int listTotalLength = 0;
                                    int cuantoDato = 0;

                                    while (contador < longitud)
                                    {
                                        byte[] array2 = new byte[20];

                                        Array.Copy(value1, contador, array2, 0, 20);

                                        cuantoDato = array2[2];
                                        if (cuantoDato > 0)
                                        {
                                            Array.Copy(array2, 3, listTotal, listTotalLength, cuantoDato);
                                            listTotalLength += cuantoDato;
                                        }

                                        contador = contador + 20;
                                    }


                                    //Identificador
                                    byte[] identificador = new byte[4];
                                    Array.Copy(listTotal, 6 + 5, identificador, 0, 4);

                                    long identificador_valor = (long)(identificador[3] * Math.Pow(2, 24)
                                                                       + identificador[2] * Math.Pow(2, 16)
                                                                       + identificador[1] * Math.Pow(2, 8)
                                                                       + identificador[0] * Math.Pow(2, 0));


                                    //oneWayTx
                                    byte[] oneWayTx = new byte[4];
                                    Array.Copy(listTotal, 10 + 5, oneWayTx, 0, 4);


                                    long oneWayTx_valor = (long)(oneWayTx[3] * Math.Pow(2, 24)
                                                               + oneWayTx[2] * Math.Pow(2, 16)
                                                               + oneWayTx[1] * Math.Pow(2, 8)
                                                               + oneWayTx[0] * Math.Pow(2, 0));


                                    //TwoWayTx
                                    byte[] TwoWayTx = new byte[4];
                                    Array.Copy(listTotal, 14 + 5, TwoWayTx, 0, 4);


                                    long TwoWayTx_valor = (long)(TwoWayTx[3] * Math.Pow(2, 24)
                                                                 + TwoWayTx[2] * Math.Pow(2, 16)
                                                                 + TwoWayTx[1] * Math.Pow(2, 8)
                                                                 + TwoWayTx[0] * Math.Pow(2, 0));

                                    //TwoWayRx
                                    byte[] TwoWayRx = new byte[4];
                                    Array.Copy(listTotal, 18 + 5, TwoWayRx, 0, 4);


                                    long TwoWayRx_valor = (long)(TwoWayRx[3] * Math.Pow(2, 24)
                                                                  + TwoWayRx[2] * Math.Pow(2, 16)
                                                                  + TwoWayRx[1] * Math.Pow(2, 8)
                                                                  + TwoWayRx[0] * Math.Pow(2, 0));
                                    
                                    cargarValoresMTU(identificador_valor.ToString(), oneWayTx_valor.ToString(), TwoWayTx_valor.ToString(), TwoWayRx_valor.ToString());

                                });
                            });
                        });
                    });

                });
            });
        }


        private void cargarValoresMTU(string identificador_int, string oneWayTx_int, string twoWayTx_int, string twoWayRx_int)
        {
            menuListReadMTU = new List<ReadMTUItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1read = new ReadMTUItem() { Title = "MTU Ser No.", Description = Convert.ToString(identificador_int) };
            var page2read = new ReadMTUItem() { Title = "1 Way Tx Freq.", Description = Convert.ToString(oneWayTx_int) };
            var page3read = new ReadMTUItem() { Title = "2 Way Tx Freq.", Description = Convert.ToString(twoWayTx_int) };
            var page4read = new ReadMTUItem() { Title = "2 Way Rx Freq.", Description = Convert.ToString(twoWayRx_int) };


            // Adding menu items to menuList
            menuListReadMTU.Add(page1read);
            menuListReadMTU.Add(page2read);
            menuListReadMTU.Add(page3read);
            menuListReadMTU.Add(page4read);


            // Setting our list to be ItemSource for ListView in MainPage.xaml
            listREADMTU.ItemsSource = menuListReadMTU;
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
