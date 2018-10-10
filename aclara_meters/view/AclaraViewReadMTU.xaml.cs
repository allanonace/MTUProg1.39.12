// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Xamarin.Forms;
using Plugin.Settings;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Globalization;
using Xml;
using MTUComm;

namespace aclara_meters.view
{
    public partial class AclaraViewReadMTU
    {

        private List<ReadMTUItem> MTUDataListView { get; set; }

        private List<ReadMTUItem> FinalReadListView { get; set; }


        private List<PageItem> MenuList { get; set; }
        private bool _userTapped;
        private IUserDialogs dialogsSaved;

        public AclaraViewReadMTU()
        {
            InitializeComponent();
        }

        private void LoadMTUData()
        {
            MenuList = new List<PageItem>
            {
                // Creating our pages for menu navigation
                // Here you can define title for item, 
                // icon on the left side, and page that you want to open after selection

                // Adding menu items to MenuList
                new PageItem()
                {
                    Title = "Read MTU",
                    Icon = "readmtu_icon.png",
                    TargetType = "ReadMTU"
                },

                new PageItem()
                {
                    Title = "Turn Off MTU",
                    Icon = "turnoff_icon.png",
                    TargetType = "turnOff"
                },

                new PageItem()
                {
                    Title = "Add MTU",
                    Icon = "addMTU.png",
                    TargetType = "AddMTU"
                },

                new PageItem()
                {
                    Title = "Replace MTU",
                    Icon = "replaceMTU2.png",
                    TargetType = "replaceMTU"
                },

                new PageItem()
                {
                    Title = "Replace Meter",
                    Icon = "replaceMeter.png",
                    TargetType = "replaceMeter"
                },

                new PageItem()
                {
                    Title = "Add MTU / Add meter",
                    Icon = "addMTUaddmeter.png",
                    TargetType = "AddMTUAddMeter"
                },

                new PageItem()
                {
                    Title = "Add MTU / Rep. Meter",
                    Icon = "addMTUrepmeter.png",
                    TargetType = "AddMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Rep.MTU / Rep. Meter",
                    Icon = "repMTUrepmeter.png",
                    TargetType = "ReplaceMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Install Confirmation",
                    Icon = "installConfirm.png",
                    TargetType = "InstallConfirm"
                }
            };

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;
          
        }

        private void OpenSettingsView(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (FormsApp.ble_interface.IsOpen())
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(dialogsSaved), false);
                        return;
                    }
                    else
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(true), false);
                    }
                }
                catch (Exception i2)
                {
                    Console.WriteLine(i2.StackTrace);
                }
            });
        }

        private void ChangeLowerButtonImage(bool v)
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

        private void ReadMTU(object sender, EventArgs e)
        {
            if (!_userTapped)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);
                    _userTapped = true;
                    label_read.Text = "Reading from MTU ... ";


                    ThreadProcedureMTUCOMMAction();


                });






                /*long timeout_ms = 20000; // 10s
                bool timeout_error = false;

                FormsApp.ble_interface.Write(new byte[] { (byte)0x25, (byte)0x80, (byte)0x00, (byte)0xFF, (byte)0x5C }, 0, 5);

                long timeout_limit = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeout_ms;
                while (FormsApp.ble_interface.BytesToRead() < 262)
                {
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit)
                    {
                        timeout_error = true;
                        break;
                    }
                }
                if (timeout_error)
                {
                    resultMsg = "Timeout";
                }
                else
                {
                    byte[] rxbuffer = new byte[262];
                    FormsApp.ble_interface.Read(rxbuffer, 0, 262);
                }*/

                //LoadMTUValuesToListView("","","","");

               
            }
        }



        private void ThreadProcedureMTUCOMMAction()
        {


            var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Create Ation when opening Form
            //Action add_mtu = new Action(new Configuration(@"C:\Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\codelog"),  new USBSerial("COM9"), Action.ActionType.AddMtu, "iker");
            MTUComm.Action add_mtu = new MTUComm.Action(config: new Configuration(xml_documents), serial: FormsApp.ble_interface, actiontype: MTUComm.Action.ActionType.ReadMtu, user: "iker");

            //Define finish and error event handler
            //add_mtu.OnFinish += Add_mtu_OnFinish;
            //add_mtu.OnError += Add_mtu_OnError;


            add_mtu.OnFinish += ((s, e) => {
                Console.WriteLine("Action Succefull");
                Console.WriteLine("Press Key to Exit");
                //Console.WriteLine(s.ToString());

                MTUDataListView = new List<ReadMTUItem>();
                FinalReadListView = new List<ReadMTUItem>();

                for (int i = 0; i < 31; i++){
                    FinalReadListView.Add(new ReadMTUItem()
                    {
                        isDisplayed = "false",
                        Height = "0"
                    });
                }

                foreach (Parameter param in e.Result.getParameters())
                {
                    Console.WriteLine(param.getLogDisplay() + ":" + param.getValue());


                    if(e.Result.getParameters()[0].getLogDisplay().Equals("MTU Status"))
                    {
                        if(e.Result.getParameters()[0].getValue().Equals("ON"))
                        {
                            if (!CheckIfParamIsVisible(param.getLogDisplay(), param.getValue()   ))
                            {
                                MTUDataListView.Add(new ReadMTUItem() 
                                { 
                                    Title = param.getLogDisplay() + ":", 
                                    isDisplayed = "false", 
                                    Height = "0", 
                                    Description = param.getValue() 
                                });
                            }
                            else
                            {
                                MTUDataListView.Add(new ReadMTUItem() 
                                { 
                                    Title = param.getLogDisplay() + ":", 
                                    Description = param.getValue() 
                                });
                            }
                        }else{
                            if (!CheckIfParamIsVisible(param.getLogDisplay(), param.getValue() , "MTU Status Off"))
                            {
                                MTUDataListView.Add(new ReadMTUItem() 
                                { 
                                    Title = param.getLogDisplay() + ":", 
                                    isDisplayed = "false", 
                                    Height = "0", 
                                    Description = param.getValue() 
                                });
                            }
                            else
                            {
                                MTUDataListView.Add(new ReadMTUItem() 
                                { 
                                    Title = param.getLogDisplay() + ":", 
                                    Description = param.getValue() 
                                });
                            }
                        }
                    }
                }

                ReadResult[] portResult = e.Result.getPorts();

                int cont = 1;
                foreach (MTUComm.ReadResult portval in portResult)
                {
                    

                    int index = FinalReadListView.FindIndex(
                        a => 
                        a.Title.ToString().Contains("Xmit Interval") 
                    );

              
                   

                    FinalReadListView.Insert(index, new ReadMTUItem()
                    {
                        Description = "Port " + cont + ": " + portval.getParameters()[0].getValue(),
                        Height = "40",
                        isMTU = "false",
                        isMeter = "true"

                    });

                    /*
                     
                        // PORT FIELDS -->

                        case "Meter Type":
                            isVisible = true;

                            break;
                        case "Service Pt. ID":
                            isVisible = true;
                            break;
                        case "Meter Reading":
                            isVisible = true;
                            break;
                        // <-- END PORT FIELDS

                     */

                    MTUDataListView.Add(new ReadMTUItem()
                    {
                        Description = "Port " + cont + ": " + portval.getParameters()[0].getValue(),
                        Height = "40",
                        isMTU = "false",
                        isMeter = "true"

                    });

                    for (int i = 0; i < portval.getParameters().Length; i++)
                    {
                        MTUComm.Parameter paramval = portval.getParameters()[i];

                        Console.WriteLine(paramval.getLogDisplay() + ":" + paramval.getValue());

                        if(i == portval.getParameters().Length)
                        {
                            if (!CheckIfParamIsVisible(paramval.getLogDisplay(), paramval.getValue()))
                            {

                                FinalReadListView.Insert(index+1+i,new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    Height = "0",
                                    isDisplayed = "false",
                                    Description = paramval.getValue()
                                });


                                MTUDataListView.Add(new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    Height = "0",
                                    isDisplayed = "false",
                                    Description = paramval.getValue()
                                });
                            }else{

                               FinalReadListView.Insert(index+1+i,new ReadMTUItem()
                               {
                                   Title = paramval.getLogDisplay() + ":",
                                   Height = "80",
                                   Description = paramval.getValue()
                               });


                                MTUDataListView.Add(new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    Height = "80",
                                    Description = paramval.getValue()
                                });
                            }
                        }else{
                            if (!CheckIfParamIsVisible(paramval.getLogDisplay(), paramval.getValue()))
                            {




                                FinalReadListView.Insert(index+1+i,new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    isDisplayed = "false",
                                    Height = "0",
                                    Description = paramval.getValue()
                                });


                                MTUDataListView.Add(new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    isDisplayed = "false",
                                    Height = "0",
                                    Description = paramval.getValue()
                                });
                            }
                            else
                            {

                               
                                FinalReadListView.Insert(index+1+i,new ReadMTUItem()
                                  {
                                      Title = paramval.getLogDisplay() + ":",
                                      Description = paramval.getValue()
                                  });

                                MTUDataListView.Add(new ReadMTUItem()
                                {
                                    Title = paramval.getLogDisplay() + ":",
                                    Description = paramval.getValue()
                                });
                            }
                        }
                    }
                }

                /*
                FinalReadListView.ForEach(a =>
                {
                   
                    if(a.Title.Contains("-"))
                    {
                        
                        FinalReadListView.RemoveAt(FinalReadListView.IndexOf(a));
                    }
                 
                });
*/




                string resultMsg = "Successful MTU read";
                byte[] readData;

                Task.Delay(100).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    listaMTUread.ItemsSource = FinalReadListView;
                    label_read.Text = resultMsg;
                    _userTapped = false;
                    bg_read_mtu_button.NumberOfTapsRequired = 1;
                    ChangeLowerButtonImage(false);
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    background_scan_page.IsEnabled = true;


                }));



                /*
                 * 
                 *   
                 MTUDataListView = new List<ReadMTUItem>
            {
            
                   new ReadMTUItem() { Title = "MTU Status:", Description = "On" },
                new ReadMTUItem() { Title = "MTU Ser No:", Description = "63004810"},
                new ReadMTUItem() { Title = "Interface Tamp:", Description = "Triggered" },
                new ReadMTUItem() { Title = "Last Gasp:", Description = "Enabled" },
                new ReadMTUItem() { Title = "Insf. Mem:", Description = "Enabled" },
                new ReadMTUItem() { Title = "Daily Snap:", Description = "2 PM" },
                new ReadMTUItem() { Title = "Encrypted:", Description = "Yes"},

                new ReadMTUItem() { Description = "Port 1: MTU Water Single Port On-Demand Encoder ER" , Height = "310", isMTU = "false", isMeter = "true",
                        
                        Title1 = "Meter Type ID:", Description1 = "126" ,
                        Title2 = "Service Pt. ID:", Description2 = "012345678" ,
                        Title3 = "Meter Reading:", Description3 = "0188XX"
                },

                new ReadMTUItem() { Title = "Xmit Interval:", Description = "72 Hrs" },
                new ReadMTUItem() { Title = "Read Interval:", Description = "12 Hrs" },
                new ReadMTUItem() { Title = "Battery:", Description = "3,66 V" },
                new ReadMTUItem() { Title = "2-Way:", Description = "Slow" },
                new ReadMTUItem() { Title = "On Demand Cnt:", Description = "0" },
                new ReadMTUItem() { Title = "Data Req Cnt:", Description = "0"  },
                new ReadMTUItem() { Title = "FOTA Cnt:", Description = "0"  },
                new ReadMTUItem() { Title = "FOTC Cnt:", Description = "0"  },
                new ReadMTUItem() { Title = "MTU Type:", Description = "171"},
                new ReadMTUItem() { Title = "MTU Software:", Description = "Version 01.04.0008"},
                new ReadMTUItem() { Title = "PCB Number", Description = "0"},
 
                };
                 */




               
           

            });


            add_mtu.OnError += ((s, e) => {
                Console.WriteLine("Action Errror");
                Console.WriteLine("Press Key to Exit");
                // Console.WriteLine(s.ToString());

               // String result = e.Message;
                //Console.WriteLine(result.ToString());


                string resultMsg = "Timeout";
                Task.Delay(100).ContinueWith(t =>
                     Device.BeginInvokeOnMainThread(() =>
                     {

                         MTUDataListView = new List<ReadMTUItem> { };

                         FinalReadListView = new List<ReadMTUItem> { };

                         listaMTUread.ItemsSource = FinalReadListView;

                         label_read.Text = resultMsg;
                         _userTapped = false;
                         bg_read_mtu_button.NumberOfTapsRequired = 1;
                         ChangeLowerButtonImage(false);
                         backdark_bg.IsVisible = false;
                         indicator.IsVisible = false;
                         background_scan_page.IsEnabled = true;

                     }));
            });

            add_mtu.Run();



        }

        private void addToListview(string field, string value,int pos)
        {

            FinalReadListView.RemoveAt(pos);
            FinalReadListView.Insert(pos,
                    new ReadMTUItem()
                    {
                        Title = field + ":",
                        Description = value
                    });
        }

        private bool CheckIfParamIsVisible(string field, string value, string status = "")
        {
            bool isVisible = false;

            switch (field)
            {
                case "MTU Status":
                    isVisible = true;

                    addToListview(field, value,0);
           
                    break;
                case "MTU Ser No":
                    isVisible = true;

                    addToListview(field, value,1);

                    break;
                case "1 Way Tx Freq":
                    isVisible = true;

                    addToListview(field, value,2);

                    break;
                case "2 Way Tx Freq":
                    isVisible = true;

                    addToListview(field, value,3);

                    break;
                case "2 Way Rx Freq":
                    isVisible = true;

                    addToListview(field, value,4);

                    break;
                case "Tilt Tamp":
                    isVisible = true;

                    addToListview(field, value,5);

                    break;
                case "Magnetic Tamp":
                    isVisible = true;

                    addToListview(field, value,6);

                    break;
                case "Interface Tamp":
                    isVisible = true;

                    addToListview(field, value,7);

                    break;
                case "Reg. Cover":
                    isVisible = true;

                    addToListview(field, value,8);

                    break;
                case "Rev. Fl Tamp":
                    isVisible = true;

                    addToListview(field, value,9);

                    break;
                case "Daily Snap":
                    isVisible = true;

                    addToListview(field, value,10);

                    break;
                case "Installation":
                    isVisible = true;

                    addToListview(field, value,11);

                    break;


                // PORT FIELDS -->

                case "Meter Type":
                    isVisible = true;

                    break;
                case "Service Pt. ID":
                    isVisible = true;
                    break;
                case "Meter Reading":
                    isVisible = true;
                    break;
                // <-- END PORT FIELDS

                case "Xmit Interval":
                    isVisible = true;

                    addToListview(field, value,12);

                    break;
                case "Read Interval":
                    isVisible = true;

                    addToListview(field, value,13);

                    break;
                case "Battery":
                    isVisible = true;

                    addToListview(field, value, 14);

                    break;
                case "MTU Type":
                    isVisible = true;

                    addToListview(field, value,15);


                    break;
                case "MTU Software":
                    isVisible = true;

                    addToListview(field, value,16);

                    break;
                case "PCB Number":
                    isVisible = true;

                    addToListview(field, value,17);

                    break;
            }

            if (status.Equals("MTU Status Off"))
            {
                return false;
            }

            return isVisible;
        }

        public AclaraViewReadMTU(IUserDialogs dialogs)
        {
            InitializeComponent();

            Task.Run(() =>
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
                    Device.BeginInvokeOnMainThread(LoadTabletUI);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadPhoneUI);
                });
            }

            dialogsSaved = dialogs;
            LoadMTUData();
         
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar


            label_read.Text = "Push Button to START";

            _userTapped = false;


            TappedListeners();
      
            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName; //"Kartik";
              
            }

            battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

        }

        private void TappedListeners()
        {
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += ReadMTU;
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMeterOkTapped;
            replacemeter_cancel.Tapped += ReplaceMeterCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;
            logout_button.Tapped += LogoutAsync;
            settings_button.Tapped += OpenSettingsView;
        }

        private void LoadPhoneUI()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
            close_menu_icon.Opacity = 1;
            hamburger_icon.IsVisible = true;
            tablet_user_view.TranslationY = 0;
            tablet_user_view.Scale = 1;
            logo_tablet_aclara.Opacity = 1;
        }

        private void LoadTabletUI()
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
        }

        private void ReplaceMeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void ReplaceMeterOkTapped(object sender, EventArgs e)
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMTU(dialogsSaved), false);
        }

        private void TurnOffMTUCloseTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUNoTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUOkTapped(object sender, EventArgs e)
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

        private void MeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void MeterOkTapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMeter(dialogsSaved), false);
        }

        private async void LogoutAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();
            int contador = Navigation.NavigationStack.Count;
            while(contador>0)
            {
                try
                {
                    await Navigation.PopAsync(false);
                }catch(Exception v){
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);

            }
            catch (Exception v)
            {
                Console.WriteLine(v.StackTrace);
            }     
        }

        private void ReturnToMainView(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync(false); 
        }

  
        private void OnItemSelected( Object sender, SelectedItemChangedEventArgs e )
        {
            ((ListView)sender).SelectedItem = null;
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!FormsApp.ble_interface.IsOpen())
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            }

            if (FormsApp.ble_interface.IsOpen())
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
                            OnMenuCaseReadMTU();
                            break;

                        case "AddMTU":
                            OnMenuCaseAddMTU();
                            break;

                        case "turnOff":
                            OnMenuCaseTurnOff();
                            break;

                        case "replaceMTU":
                            OnMenuCaseReplaceMTU();
                            break;

                        case "replaceMeter":
                            OnMenuCaseReplaceMeter();
                            break;
                    }
                }
                catch (Exception w1)
                {
                    Console.WriteLine(w1.StackTrace);
                }
            }
        }

        private void OnMenuCaseReplaceMeter()
        {

            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            Task.Delay(200).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
             }));
        }

        private void OnMenuCaseReplaceMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            Task.Delay(200).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
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
                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
              }));
                        
        }

        private void OnMenuCaseTurnOff()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            Task.Delay(200).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
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
              
                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //      if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
             }));

        }

        private void OnMenuCaseAddMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            Task.Delay(200).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
             {
                 navigationDrawerList.SelectedItem = null;
                 Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved), false);
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //      if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
            }));
        }

        private void OnMenuCaseReadMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }


            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                navigationDrawerList.SelectedItem = null;
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved), false);
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
                shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //  if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
            }));
        }
    }
}
