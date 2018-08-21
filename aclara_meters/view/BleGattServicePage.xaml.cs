﻿// Copyright M. Griffie <nexus@nexussays.com>
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
using aclara_meters.Helpers;
using aclara_meters.Models;
using aclara_meters.viewmodel;
using nexus.core.text;
using nexus.protocols.ble;
using Xamarin.Forms;
using System.Threading;
using ble_library;

namespace aclara_meters.view
{
    public partial class BleGattServicePage
   {
        
        public BleGattServicePage()
      {
          InitializeComponent();
      }


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
            var page2menu = new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "turnOff" };
            var page3menu = new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" };
            var page4menu = new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", TargetType = "replaceMTU" };
            var page5menu = new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", TargetType = "replaceMeter" };
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
            navigationDrawerList.ItemsSource = menuList2;


            logout_button.Tapped += logoutAsync;
            settings_button.Tapped += openSettings;

        }


        private void openSettings(object sender, EventArgs e)
        {

            Task.Run(async () =>
            {

                await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                {


                

                    //Application.Current.MainPage.Navigation.PopAsync(false); 
                    Application.Current.MainPage.Navigation.PushAsync(new BleSettingsPage(dialogsSaved), false);


                });

            });


        }

        private void changeImageColor(bool v)
        {
            if (v)
            {
                bg_read_mtu_button_img.Source = "read_mtu_btn_black.png";
            
            }
            else
            {
                bg_read_mtu_button_img.Source = "read_mtu_btn.png";

            }
        }

        private bool _userTapped;

        private void ReadMTU(object sender, EventArgs e)
        {
  



            if(!_userTapped){
                
                Task.Run(async () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;

                        //ble_library.BleSerial.buffer_interface = new byte[] { };
                       
                        //ble_library.BlePort.Write_Characteristic_ReadMTU();
                       
                        FormsApp.ble_interface.ble_port_serial.CaracterisicoIndicate = new Guid("00000003-0000-1000-8000-00805f9b34fb");
                        FormsApp.ble_interface.ble_port_serial.ServicioIndicate =  new Guid("2cf42000-7992-4d24-b05d-1effd0381208");

                        FormsApp.ble_interface.ble_port_serial.ServicioWrite = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                        FormsApp.ble_interface.ble_port_serial.CaracterisicoWrite = new Guid("00000002-0000-1000-8000-00805f9b34fb");


                        FormsApp.ble_interface.Write(new byte[] { (byte)0x00, (byte)0x00, (byte)0x05, (byte)0x25, (byte)0x80, (byte)0x00, (byte)0xFF, (byte)0x5C },0,8);

                    });
                });

                background_scan_page.IsEnabled = false;
                _userTapped = true;
                changeImageColor(true);
                load_read_mtu();
                label_read.Text = "Reading from MTU ... ";
                Task.Run(async () =>
                {
                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                    {

                        label_read.Text = "Reading from MTU ... 3 sec";
                        //pb_ProgressBar.ProgressTo(0.2, 350, Easing.Linear);


                        Task.Run(async () =>
                        {
                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                            {
                                label_read.Text = "Reading from MTU ... 2 sec";
                                //pb_ProgressBar.ProgressTo(0.5, 650, Easing.Linear);

                                Task.Run(async () =>
                                {
                                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                    {
                                        label_read.Text = "Reading from MTU ... 1 sec";
                                        //pb_ProgressBar.ProgressTo(0.7, 850, Easing.Linear);

                                        Task.Run(async () =>
                                        {
                                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                            {

                                                _userTapped = false;
                                                label_read.Text = "Successful MTU read";
                                                //pb_ProgressBar.ProgressTo(1, 450, Easing.Linear);
                                                bg_read_mtu_button.NumberOfTapsRequired = 1;
                                                changeImageColor(false);

                                                Task.Run(async () =>
                                                {
                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {

                                                        backdark_bg.IsVisible = false;
                                                        indicator.IsVisible = false;
                                                        background_scan_page.IsEnabled = true;

                                                   

                                                    });
                                                });

                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });



            }
           
        }


        IUserDialogs dialogsSaved;


        public BleGattServicePage(IUserDialogs dialogs)
        {
            InitializeComponent();





            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;

                });
            });



            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContentNav.IsVisible = true;
                        background_scan_page.Opacity = 1;

                        close_menu_icon.Opacity = 0;
                        hamburger_icon.IsVisible = false;


                        background_scan_page.Margin = new Thickness(310, 0, 0, 0);



                        tablet_user_view.TranslationY = -22;
                        tablet_user_view.Scale = 1.2;

                        logo_tablet_aclara.Opacity = 0;
                        shadoweffect.IsVisible = true;

                        aclara_logo.Scale = 1.2;
                        aclara_logo.TranslationX = 42;
                        aclara_logo.TranslationX = 42;

                    });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        background_scan_page.Margin = new Thickness(0, 0, 0, 0);


                        close_menu_icon.Opacity = 1;
                        hamburger_icon.IsVisible = true;



                        tablet_user_view.TranslationY = 0;

                        tablet_user_view.Scale = 1;
                        logo_tablet_aclara.Opacity = 1;

                    });
                });
            }


            dialogsSaved = dialogs;


            cargarMTU();
           

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            back_button.Tapped += returntomain;

            //Button READ
            bg_read_mtu_button.Tapped += ReadMTU;

            label_read.Text = "Push Button to START";


            _userTapped = false;



            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName;
            }

            turnoffmtu_ok.Tapped += TurnOffMTU_OK;
            turnoffmtu_no.Tapped += Turnoffmtu_No_Tapped;
            turnoffmtu_ok_close.Tapped += TurnOffMtu_Close;
            replacemeter_ok.Tapped += Replacemeter_Ok_Tapped;
            replacemeter_cancel.Tapped += Replacemeter_Cancel_Tapped;
            meter_ok.Tapped += Meter_Ok_Tapped;
            meter_cancel.Tapped += Meter_Cancel_Tapped;


            //Libreria BLE

            //ble_library.BleMainClass.MostrarBuffer();

          



            /*
            Device.BeginInvokeOnMainThread(() =>
            {
                ble_library.BlePort.Listen_Characteristic_Notification_ReadMTU();
         
            });

*/
           




        }


        private void Replacemeter_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void Replacemeter_Ok_Tapped(object sender, EventArgs e)
        {

            //APP OPEN VIEW
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;


            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMTUPage(dialogsSaved), false);

        }

        private void TurnOffMtu_Close(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void Turnoffmtu_No_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTU_OK(object sender, EventArgs e)
        {

            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = true;

            Task.Run(async () =>
            {
                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {
                    dialog_turnoff_two.IsVisible = false;
                    dialog_turnoff_three.IsVisible = true;
                });
            });


        }



        void Meter_Cancel_Tapped(object sender, EventArgs e)
        {

            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

        }


        void Meter_Ok_Tapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;

            //APP OPEN VIEW
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMeterPage(dialogsSaved), false);


        }



        private void load_read_mtu()
        {
           

    


       

            Task.Run(async () =>
            {

                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        lista.BeginRefresh();

                        lista.EndRefresh();

                        Task.Run(async () =>
                        {

                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                            {

                                lista.BeginRefresh();

                                lista.EndRefresh();

                                Task.Run(async () =>
                                {
                                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                    {
                                        ////////
                                        try{
                                            Queue<byte[]> numbers = new Queue<byte[]>();
                                            numbers = FormsApp.ble_interface.ble_port_serial.getBuffer_ble_data();


                                            // Create an array twice the size of the queue and copy the
                                            // elements of the queue, starting at the middle of the 
                                            // array. 
                                          //  byte[] array2 = new byte[]{};
                                          
                                         //   for (int i = 0; i < numbers.Count; i++){
                                         //       numbers.Peek().CopyTo(array2, numbers.Count);
                                         //   }

                                        

                                            cargarValoresMTU(FormsApp.ble_interface.Read(null, 11, 4).ToString(),
                                                             (Double.Parse(FormsApp.ble_interface.Read(null, 15, 4).ToString()) / 1000000).ToString(),
                                                             (Double.Parse(FormsApp.ble_interface.Read(null, 19, 4).ToString()) / 1000000).ToString(),
                                                             (Double.Parse(FormsApp.ble_interface.Read(null, 23, 4).ToString()) / 1000000).ToString()
                                                            );



                                            //FormsApp.ble_interface.Read(FormsApp.ble_interface.GetBufferElement(), 11, 4);
                                           // FormsApp.ble_interface.Read(FormsApp.ble_interface.GetBufferElement(), 15, 4);
                                            //FormsApp.ble_interface.Read(FormsApp.ble_interface.GetBufferElement(), 19, 4);
                                           // FormsApp.ble_interface.Read(FormsApp.ble_interface.GetBufferElement(), 23, 4);




                                         //   Byte[] value1 = BlePort.buffer_ble_data
                                         //   int longi = interfaceBle.getBufferInterface().Length - 8;

                                          //  Byte[] value2 = new Byte[longi];
                                          //  Array.Copy(value1, 8, value2, 0, longi);

                                          //  value1 = value2;

                                           // ble_library.BleSerial.buffer_interface = value2;

                                          
                                                    
                                            //try{
                                                //int identificador = lexi.Read(value1, 11, 4);
                                                // int oneWayTx = lexi.Read(value1, 15, 4);
                                                // int TwoWayTx = lexi.Read(value1, 19, 4);
                                                // int TwoWayRx = lexi.Read(value1, 23, 4);

                                                //lexi.Write(0, value1);

                                                /*
                                                Console.WriteLine(BitConverter.ToString(lexi.Read(0, 1)));
                                                Console.WriteLine("");
                                                Console.WriteLine(BitConverter.ToString(lexi.Read(1, 9)));
                                                Console.WriteLine("");
                                                Console.WriteLine(BitConverter.ToString(lexi.Read(600, 9)));
                                                Console.WriteLine("");
                                                lexi.Write(64, new byte[] { 0x01 });
                                                */


                                                /*
                                                Console.WriteLine(BitConverter.ToString(interfaceBle.Read(0, 1)));
                                                Console.WriteLine("");
                                                Console.WriteLine(BitConverter.ToString(interfaceBle.Read(1, 9)));
                                                Console.WriteLine("");
                                                Console.WriteLine(BitConverter.ToString(lx.Read(600, 9)));
                                                Console.WriteLine("");
                                                interfaceBle.Write(64, new byte[] { 0x01 });

*/
                                                /*
                                                Console.WriteLine("Valores LEXI:");
                                                Console.WriteLine("");

                                                try
                                                {
                                                    byte [] a1 = lexi.Read(11, 4);
                                                }
                                                catch(Exception w1){
                                                    
                                                }
                                               

                                                Console.WriteLine("Buffer Write MTU Ser: " + ble_library.BleSerial.buffer_interface_write);
                                                Console.WriteLine("");

                                                try
                                                {
                                                    byte[] a2 = lexi.Read(15, 4);
                                                }
                                                catch (Exception w2)
                                                {

                                                }

                                            
                                                Console.WriteLine("Buffer Write 1 Way Tx: " + ble_library.BleSerial.buffer_interface_write);
                                                Console.WriteLine("");

                                           
                                                try
                                                {
                                                    byte[] a3 = lexi.Read(19, 4);
                                                }
                                                catch (Exception w3)
                                                {

                                                }

                                                Console.WriteLine("Buffer Write 2 Way Tx: " + ble_library.BleSerial.buffer_interface_write);
                                                Console.WriteLine("");

                                               
                                                try
                                                {
                                                    byte[] a4 = lexi.Read(23, 4);
                                                }
                                                catch (Exception w4)
                                                {

                                                }

                                                Console.WriteLine("Buffer Write 2 Way Rx: " + ble_library.BleSerial.buffer_interface_write);



                                                Console.WriteLine("");
                                               
                                
                                                //lexi.Write(64, new byte[] { 0x01 });
                                                

                                               
                                            }catch(Exception e){
                                                
                                            }

                                         
                                         
                                            /*

                                                       
                                            int longitud = value1.Length;

                                            int contador = 0;


                                            byte[] listTotal = new byte[1024];
                                            int listTotalLength = 0;
                                            int cuantoDato = 0;

                                            while (contador < longitud)
                                            {
                                                byte[] array2 = new byte[20];

                                                try
                                                {
                                                     Array.Copy(value1, contador, array2, 0, 20);
                                                }
                                                catch (Exception s)
                                                {

                                                }

                                                cuantoDato = array2[2];
                                               
                                                    
                                        
                                                if (cuantoDato > 0)
                                                {
                                                    try
                                                    {
                                                        Array.Copy(array2, 3, listTotal, listTotalLength, cuantoDato);
                                                    }
                                                    catch (Exception v)
                                                    {

                                                    }

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


                                            //cargarValoresMTU("21189225", "456,3375", "456,3375", "468,8375");
                                            cargarValoresMTU(identificador_valor.ToString(),
                                                             (Double.Parse(oneWayTx_valor.ToString()) / 1000000).ToString(),
                                                             (Double.Parse(TwoWayTx_valor.ToString()) / 1000000).ToString(),
                                                             (Double.Parse(TwoWayRx_valor.ToString()) / 1000000).ToString()
                                                            );

*/
                                        }catch(Exception e){
                                            //cargarValoresMTU("21189225", "456,3375", "456,3375", "468,8375");
                                            cargarValoresMTU("0",
                                                            "0",
                                                            "0",
                                                            "0"
                                                            );
                                        }
                                      

                                    });
                                    
                                });
                            });
                        });

                    }
                    catch (Exception e)
                    {

                    }

                });
            });
        }

        private async void logoutAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();

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
            var page2 = new ReadMTUItem() { Title = "1 Way Tx Freq.", Description = oneWayTx_int }; // / 1000000
            var page3 = new ReadMTUItem() { Title = "2 Way Tx Freq.", Description = twoWayTx_int }; // / 1000000
            var page4 = new ReadMTUItem() { Title = "2 Way Rx Freq.", Description = twoWayRx_int }; // / 1000000 


            // Adding menu items to menuList
            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);

            //if( (int.Parse(oneWayTx_int) / 1000000) > 480){
           ///     load_read_mtu();
           // }

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            listaMTUread.ItemsSource = menuList;


        }

     

      private void OnItemSelected( Object sender, SelectedItemChangedEventArgs e )
      {
         ((ListView)sender).SelectedItem = null;


      }



        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async void OnMenuItemSelectedAsync(object sender, ItemTappedEventArgs e)
        {


            if (!ble_library.BlePort.Connection_app)
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            }
                    if (ble_library.BlePort.Connection_app)
            {
                navigationDrawerList.SelectedItem = null;

                try
                {
                    var item = (PageItem)e.Item;
                    String page = item.TargetType;

                    ((ListView)sender).SelectedItem = null;

                    switch (page)
                    {
                        case "ReadMTU":
                            background_scan_page.Opacity = 1;


                            background_scan_page.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }

                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {



                                    navigationDrawerList.SelectedItem = null;


                                    Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(dialogsSaved), false);


                                    background_scan_page.Opacity = 1;


                                    if (Device.Idiom == TargetIdiom.Tablet)
                                    {
                                        ContentNav.Opacity = 1;
                                        ContentNav.IsVisible = true;
                                    }
                                    else
                                    {
                                        ContentNav.Opacity = 0;
                                        ContentNav.IsVisible = false;
                                    }

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }


                                });

                            });


                            break;

                        case "AddMTU":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {


                                    navigationDrawerList.SelectedItem = null;


                                    Application.Current.MainPage.Navigation.PushAsync(new AddMTUPage(dialogsSaved), false);


                                    background_scan_page.Opacity = 1;


                                    if (Device.Idiom == TargetIdiom.Tablet)
                                    {
                                        ContentNav.Opacity = 1;
                                        ContentNav.IsVisible = true;
                                    }
                                    else
                                    {
                                        ContentNav.Opacity = 0;
                                        ContentNav.IsVisible = false;
                                    }
                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }

                                });

                            });


                            break;




                        case "turnOff":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {
                                    dialog_open_bg.IsVisible = true;
                                    turnoff_mtu_background.IsVisible = true;
                                    dialog_meter_replace_one.IsVisible = false;
                                    dialog_turnoff_one.IsVisible = true;
                                    dialog_turnoff_two.IsVisible = false;
                                    dialog_turnoff_three.IsVisible = false;

                                    dialog_replacemeter_one.IsVisible = false;


                                    background_scan_page.Opacity = 1;

                                    if (Device.Idiom == TargetIdiom.Tablet)
                                    {
                                        ContentNav.Opacity = 1;
                                        ContentNav.IsVisible = true;


                                    }
                                    else
                                    {
                                        ContentNav.Opacity = 0;
                                        ContentNav.IsVisible = false;
                                    }
                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }

                                });

                            });


                            break;

                        case "replaceMTU":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {
                                    dialog_open_bg.IsVisible = true;
                                    turnoff_mtu_background.IsVisible = true;

                                    dialog_meter_replace_one.IsVisible = false;
                                    dialog_turnoff_one.IsVisible = false;
                                    dialog_turnoff_two.IsVisible = false;
                                    dialog_turnoff_three.IsVisible = false;

                                    dialog_replacemeter_one.IsVisible = true;

                                    background_scan_page.Opacity = 1;

                                    if (Device.Idiom == TargetIdiom.Tablet)
                                    {
                                        ContentNav.Opacity = 1;
                                        ContentNav.IsVisible = true;
                                    }
                                    else
                                    {
                                        ContentNav.Opacity = 0;
                                        ContentNav.IsVisible = false;
                                    }

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }

                                });

                            });


                            break;


                        case "replaceMeter":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {

                                    dialog_open_bg.IsVisible = true;
                                    turnoff_mtu_background.IsVisible = true;


                                    dialog_turnoff_one.IsVisible = false;
                                    dialog_turnoff_two.IsVisible = false;
                                    dialog_turnoff_three.IsVisible = false;

                                    dialog_replacemeter_one.IsVisible = false;
                                    dialog_meter_replace_one.IsVisible = true;

                                    background_scan_page.Opacity = 1;

                                    if (Device.Idiom == TargetIdiom.Tablet)
                                    {
                                        ContentNav.Opacity = 1;
                                        ContentNav.IsVisible = true;
                                    }
                                    else
                                    {
                                        ContentNav.Opacity = 0;
                                        ContentNav.IsVisible = false;
                                    }

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }


                                });

                            });



                            break;




                    }


                }
                catch (Exception w)
                {

                }
            }

        }

   }
}