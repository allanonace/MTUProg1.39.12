// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ble.net.sampleapp.Helpers;
using ble.net.sampleapp.Models;
using ble.net.sampleapp.viewmodel;
using nexus.core.text;
using nexus.protocols.ble;
using Xamarin.Forms;

namespace ble.net.sampleapp.view
{
   public partial class BleGattServicePage
   {
        
      public BleGattServicePage()
      {
          InitializeComponent();
      }



        BleGattServiceViewModel model_saved;

        public List<ReadMTUItem> menuList { get; set; }

        public List<PageItem> menuList2 { get; set; }
         
        private void cargarMTU(){


            menuList = new List<ReadMTUItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
          
            var page1 = new ReadMTUItem() { Title = "MTU Ser No.", Description = "-" };
            var page2 = new ReadMTUItem() { Title = "1 Way Tx Freq.", Description = "-" };
            var page3 = new ReadMTUItem() { Title = "2 Way Tx Freq.", Description = "-" };
            var page4 = new ReadMTUItem() { Title = "2 Way Rx Freq.", Description = "-" };


            // Adding menu items to menuList
            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            listaMTUread.ItemsSource = menuList;




            menuList2 = new List<PageItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1menu = new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" };
            var page2menu = new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "" };
            var page3menu = new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" };
            var page4menu = new PageItem() { Title = "Rep. MTU", Icon = "replaceMTU.png", TargetType = "" };
            var page5menu = new PageItem() { Title = "Rep. Meter", Icon = "replaceMeter.png", TargetType = "" };
            var page6menu = new PageItem() { Title = "Add MTU / Add meter", Icon = "addMTUaddmeter.png", TargetType = "" };
            var page7menu = new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "" };
            var page8menu = new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "" };
            var page9menu = new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "" };


            // Adding menu items to menuList
            menuList2.Add(page1menu);
            menuList2.Add(page2menu);
            menuList2.Add(page3menu);
            menuList2.Add(page4menu);
            menuList2.Add(page5menu);
            menuList2.Add(page6menu);
            menuList2.Add(page7menu);
            menuList2.Add(page8menu);
            menuList2.Add(page9menu);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerListRead.ItemsSource = menuList2;


            logout_button.Tapped += logoutAsync;


        }

       
        IBleGattServerConnection savedServer;
        IUserDialogs dialogsSaved;

        public BleGattServicePage(BleGattServiceViewModel model, IBleGattServerConnection gattServer, IUserDialogs dialogs)
        {
            InitializeComponent();
            BindingContext = model;

            model_saved = model;


            savedServer = gattServer;
            dialogsSaved = dialogs;


            cargarMTU();
           

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            back_button.Tapped += returntomain;

         

            Task.Run(async () =>
            {

                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {
                    try{
                        model_saved.Characteristic.RemoveAt(0);
 




                    Guid ServicioIndicate = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                        Guid CaracterisicoIndicate = new Guid("00000003-0000-1000-8000-00805f9b34fb");

                        BleGattCharacteristicViewModel gattCharacteristicViewModelIndicate = new BleGattCharacteristicViewModel(ServicioIndicate, CaracterisicoIndicate, gattServer, dialogs);

                        var viewModelIndicate = (BleGattCharacteristicViewModel)gattCharacteristicViewModelIndicate;

                        if (viewModelIndicate.EnableNotificationsCommand.CanExecute(null))
                            viewModelIndicate.EnableNotificationsCommand.Execute(null);



                        model_saved.Characteristic.Add(gattCharacteristicViewModelIndicate);

                        BindingContext = model_saved;

                        lista.BeginRefresh();

                        lista.EndRefresh();



                        Task.Run(async () =>
                        {

                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                            {

                                model_saved.Characteristic.RemoveAt(0);

                                Guid ServicioWrite = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                                Guid CaracterisicoWrite = new Guid("00000002-0000-1000-8000-00805f9b34fb");
                                // String valorDato = "000005258000015a";

                                BleGattCharacteristicViewModel gattCharacteristicViewModelWrite = new BleGattCharacteristicViewModel(ServicioWrite, CaracterisicoWrite, gattServer, dialogs);

                                var viewModelWrite = (BleGattCharacteristicViewModel)gattCharacteristicViewModelWrite;
                                if (viewModelWrite.WriteCurrentBytesGUIDCommand.CanExecute(null))
                                    viewModelWrite.WriteCurrentBytesGUIDCommand.Execute(null);



                                model_saved.Characteristic.Add(gattCharacteristicViewModelWrite);

                                BindingContext = model_saved;

                                lista.BeginRefresh();

                                lista.EndRefresh();


                                Task.Run(async () =>
                                {

                                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
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

                                        String listatotla = listTotal.EncodeToBase16String();
                                        valorHEX.Text = listatotla.Substring(0, listTotalLength * 2);

                                        cargarValoresMTU(identificador_valor.ToString(), oneWayTx_valor.ToString(), TwoWayTx_valor.ToString(), TwoWayRx_valor.ToString());

                                    });
                                });
                            });
                        });



                    }catch(Exception e){
                        
                    }



                });
            });



            //Change username textview to Prefs. String
            if (Settings.SavedUserName != null)
            {
                userName.Text = Settings.SavedUserName;
            }

           

        }


        private async void logoutAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            try
            {
                ((BleGattServerViewModel)BindingContext).DisconnectFromDeviceCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
            }

            // Application.Current.MainPage = new LoginMenuPage(bleAdapterSaved, dialogsSaved);
            int contador = Navigation.NavigationStack.Count;

            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            while(contador>0){
                //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
               
                try{
                    Navigation.PopAsync(false);

                }catch(Exception v){
                    
                }
              
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);

            }
            catch (Exception v)
            {

            }
                       

        }


        private void returntomain(object sender, EventArgs e)
        {

            Application.Current.MainPage.Navigation.PopAsync(false); 
           


        }

        private void cargarValoresMTU(string identificador_int, string oneWayTx_int, string twoWayTx_int, string twoWayRx_int)
        {
            menuList = new List<ReadMTUItem>();


 



            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1 = new ReadMTUItem() { Title = "MTU Ser No.", Description = Convert.ToString(identificador_int) };
            var page2 = new ReadMTUItem() { Title = "1 Way Tx Freq.", Description = Convert.ToString(double.Parse(oneWayTx_int) / 1000000) };
            var page3 = new ReadMTUItem() { Title = "2 Way Tx Freq.", Description = Convert.ToString(double.Parse(twoWayTx_int) / 1000000) };
            var page4 = new ReadMTUItem() { Title = "2 Way Rx Freq.", Description = Convert.ToString(double.Parse(twoWayRx_int) / 1000000) };


            // Adding menu items to menuList
            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);


            // Setting our list to be ItemSource for ListView in MainPage.xaml
            listaMTUread.ItemsSource = menuList;
        }

     

      private void OnItemSelected( Object sender, SelectedItemChangedEventArgs e )
      {
         ((ListView)sender).SelectedItem = null;


      }




        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async void OnMenuItemSelectedAsyncRead(object sender, SelectedItemChangedEventArgs e)
        {

            // if (Settings.IsConnectedBLE)
            //  {
            try
            {


                var item = (PageItem)e.SelectedItem;
                String page = item.TargetType;

                switch (page)
                {
                    case "ReadMTU":



                        await Task.Run(async () =>
                        {

                            await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                            {


                                Guid value = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                                BleGattServiceViewModel account = new BleGattServiceViewModel(value, savedServer, dialogsSaved);
                                Application.Current.MainPage.Navigation.PopAsync(false); 
           
                                Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, savedServer, dialogsSaved),false);
                                //    ReadMtuMethod(account, savedServer, dialogsSaved);


                                if (Device.Idiom == TargetIdiom.Phone)
                                {
                                    ContentNav.Opacity = 0;
                                    ContentNav.IsVisible = false;
                                }
                                else
                                {
                                    ContentNav.Opacity = 1;
                                    ContentNav.IsVisible = true;
                                }



                                navigationDrawerListRead.SelectedItem = null;
                                navigationDrawerListRead.BeginRefresh();
                                navigationDrawerListRead.SelectedItem = null;
                                navigationDrawerListRead.EndRefresh();
                            });

                        });

                        //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                        break;



                }

            } catch(Exception w){
                
            }
               




            // }
        }




        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called

            if (width > height) {
                // landscape

                Task.Run(async () =>
                {

                    await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                    {
                        ContentNav.IsVisible = true;
                        background_scan_page.Opacity = 1;
                      
                        close_menu_icon.Opacity = 0;
                        hamburger_icon.IsVisible = false;
                 

                        background_scan_page.Margin = new Thickness(310, 0, 0, 0);

                    

                        //back_button_menu.
                    });
                });

               

            } else {
              // portrait
                Task.Run(async () =>
                {

                    await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                    {
                        //ContentNav.IsVisible = false;
                        background_scan_page.Margin = new Thickness(0, 0, 0, 0);
                      

                        close_menu_icon.Opacity = 1;
                        hamburger_icon.IsVisible = true;
                 



                    });
                });

               
            }
        } 

   }
}
