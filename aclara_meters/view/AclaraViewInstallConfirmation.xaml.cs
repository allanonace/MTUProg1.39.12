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
using Xml;
using MTUComm;
using Plugin.Multilingual;

namespace aclara_meters.view
{
    public partial class AclaraViewInstallConfirmation
    {

        private List<ReadMTUItem> MTUDataListView { get; set; }

        private List<ReadMTUItem> FinalReadListView { get; set; }


        private List<PageItem> MenuList { get; set; }
        private bool _userTapped;
        private IUserDialogs dialogsSaved;

        public AclaraViewInstallConfirmation()
        {
            InitializeComponent();
        }

        private void LoadMTUData()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" });

            if (FormsApp.config.global.ShowTurnOff)
                MenuList.Add(new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "turnOff" });

            if (FormsApp.config.global.ShowAddMTU)
                MenuList.Add(new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" });

            if (FormsApp.config.global.ShowReplaceMTU)
                MenuList.Add(new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", TargetType = "replaceMTU" });

            if (FormsApp.config.global.ShowReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", TargetType = "replaceMeter" });

            if (FormsApp.config.global.ShowAddMTUMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Add Meter", Icon = "addMTUaddmeter.png", TargetType = "AddMTUAddMeter" });

            if (FormsApp.config.global.ShowAddMTUReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "AddMTUReplaceMeter" });

            if (FormsApp.config.global.ShowReplaceMTUMeter)
                MenuList.Add(new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "ReplaceMTUReplaceMeter" });

            if (FormsApp.config.global.ShowInstallConfirmation)
                MenuList.Add(new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "InstallConfirm" });



            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Icon = "", TargetType = "" });

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


                    Task.Factory.StartNew(ThreadProcedureMTUCOMMAction);


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
            //Create Ation when opening Form
            //Action add_mtu = new Action(new Configuration(@"C:\Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\codelog"),  new USBSerial("COM9"), Action.ActionType.AddMtu, "iker");
            MTUComm.Action add_mtu = new MTUComm.Action(
                config: FormsApp.config,
                serial: FormsApp.ble_interface,
                type: MTUComm.Action.ActionType.MtuInstallationConfirmation,
                user: FormsApp.credentialsService.UserName);

            //Define finish and error event handler
            //add_mtu.OnFinish += Add_mtu_OnFinish;
            //add_mtu.OnError += Add_mtu_OnError;
            add_mtu.OnProgress += ((s, e) =>
            {
                string mensaje = e.Message;

                Device.BeginInvokeOnMainThread(() =>
                {
                    label_read.Text = mensaje;
                });
            });

            add_mtu.OnFinish += ((s, e) =>
            {
                Console.WriteLine("Action Succefull");
                Console.WriteLine("Press Key to Exit");
                //Console.WriteLine(s.ToString());

                // MTUDataListView = new List<ReadMTUItem>();  // Saves all the fields data from MTUComm - DEBUG
                FinalReadListView = new List<ReadMTUItem>(); // Saves the data to view
                /*
                for (int i = 0; i < 31; i++){
                    FinalReadListView.Add(new ReadMTUItem()
                    {
                        isDisplayed = "false",
                        Height = "0"
                    });
                }
                */

                Parameter[] paramResult = e.Result.getParameters();

                int mtu_type = 0;

                foreach (Parameter p in paramResult)
                    try
                    {
                        if (p.CustomParameter.Equals("MtuType"))
                            mtu_type = Int32.Parse(p.Value.ToString());
                    }
                    catch (Exception e5)
                    {
                        Console.WriteLine(e5.StackTrace);
                    }

                InterfaceParameters[] interfacesParams = FormsApp.config.getUserInterfaceFields(mtu_type, "ReadMTU");
                foreach (InterfaceParameters parameter in interfacesParams)
                {
                    if (parameter.Name.Equals("Port"))
                    {
                        ActionResult[] ports = e.Result.getPorts(); //parameter.Parameters.ToArray()

                        for (int i = 0; i < ports.Length; i++)
                        {
                            foreach (InterfaceParameters port_parameter in parameter.Parameters)
                            {
                                Parameter param = null;

                                if (port_parameter.Name.Equals("Description"))
                                {
                                    param = ports[i].getParameterByTag(port_parameter.Name);

                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title = "Here lies the Port title...",
                                        isDisplayed = "true",
                                        Height = "40",
                                        isMTU = "false",
                                        isMeter = "true",
                                        Description = "Port " + i + ": " + param.Value //parameter.Value
                                    });
                                }
                                else
                                {
                                    if (port_parameter.Source != null)
                                    {
                                        try
                                        {
                                            param = ports[i].getParameterByTag(port_parameter.Source.Split(new char[] { '.' })[1]);
                                        }
                                        catch (Exception e2)
                                        {
                                            Console.WriteLine(e2.StackTrace);
                                        }

                                    }

                                    if (param == null)
                                        param = ports[i].getParameterByTag(port_parameter.Name);

                                    if (param != null)
                                    {
                                        FinalReadListView.Add(new ReadMTUItem()
                                        {
                                            Title = param.getLogDisplay() + ":",
                                            isDisplayed = "true",
                                            Height = "70",
                                            isMTU = "false",
                                            isDetailMeter = "true",
                                            isMeter = "false",
                                            Description = param.Value //parameter.Value
                                        });
                                    }
                                }
                            }

                            /*
                            paramPort = ports[i].getParameters();


                            if (paramPort != null)
                            {

                                FinalReadListView.Add(new ReadMTUItem()
                                {
                                    Title = "Here lies the Port title...",
                                    isDisplayed = "true",
                                    Height = "40",
                                    isMTU = "false",
                                    isMeter = "true",
                                    Description = "Port " + i + ": " + paramPort[i].getValue() //parameter.Value
                                });


                                for (int i2 = 1; i2 < paramPort.Length; i2++)
                                {


                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title = "\t\t\t\t\t" + paramPort[i2].getLogDisplay() + ":",
                                        isDisplayed = "true",
                                        Height = "64",
                                        isMTU = "true",
                                        isMeter = "false",
                                        Description = "\t\t\t\t\t" + paramPort[i2].getValue() //parameter.Value
                                    });
                                }


                            }*/

                        }
                    }
                    else
                    {
                        Parameter param = null;

                        if (parameter.Source != null)
                        {
                            try
                            {
                                param = e.Result.getParameterByTag(parameter.Source.Split(new char[] { '.' })[1]);
                            }
                            catch (Exception e3)
                            {
                                Console.WriteLine(e3.StackTrace); //{System.IndexOutOfRangeException: Index was outside the bounds of the array.t aclara_meters.view.AclaraViewReadMTU.< ThreadProcedureMTUCOMMAction > b__19_0(System.Object s, MTUComm.Action + ActionFinishArgs e)[0x0031d] in / Users / ma.jimenez / Desktop / Proyectos / proyecto_aclara / aclara_meters / view / AclaraViewReadMTU.xaml.cs:385 }
                            }
                        }

                        if (param == null)
                            param = e.Result.getParameterByTag(parameter.Name);

                        if (param != null)
                            FinalReadListView.Add(new ReadMTUItem()
                            {
                                Title = param.getLogDisplay() + ":",
                                isDisplayed = "true",
                                Height = "64",
                                isMTU = "true",
                                isMeter = "false",
                                Description = param.Value //parameter.Value
                            });
                    }
                }

                //List <Interface> list = FormsApp.config.interfaces.Interfaces;

                //ActionInterface action = FormsApp.config.interfaces.GetInterfaceByMtuIdAndAction(mtu_type,"ReadMTU");

                //List<InterfaceParameters> para = action.Parameters;

                bool ok = false;
                foreach ( Parameter parameter in paramResult )
                {
                    if ( parameter.getLogTag ().Equals ( "InstallationConfirmationStatus" ) )
                    {
                        string name = parameter.getLogTag ();
                        dynamic value = parameter.Value;
                    }

                    if ( parameter.getLogTag ().Equals ( "InstallationConfirmationStatus" ) &&
                         ! string.IsNullOrEmpty ( parameter.Value ) &&
                         ! string.Equals ( parameter.Value.ToUpper (), "NOT CONFIRMED" ) )
                    {
                        ok = true;
                        break;
                    }
                }
               
                string resultMsg = ( ! ok ) ? "Unsuccessful Installation" : "Successful Installation";
                
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
            });

            add_mtu.OnError += ((s, e) =>
            {
                Console.WriteLine("Action Errror");
                Console.WriteLine("Press Key to Exit");
                // Console.WriteLine(s.ToString());

                // String result = e.Message;
                //Console.WriteLine(result.ToString());

               // extension.ProvideValue("Timeout");

                string resultMsg = e.Message;

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

        private void addToListview(string field, string value, int pos)
        {

            FinalReadListView.RemoveAt(pos);
            FinalReadListView.Insert(pos,
                    new ReadMTUItem()
                    {
                        Title = field + ":",
                        Description = value,

                    });
        }

        private bool CheckIfParamIsVisible(string field, string value, string status = "")
        {
            bool isVisible = false;

            switch (field)
            {
                case "MTU Status":
                    isVisible = true;

                    addToListview(field, value, 0);

                    break;
                case "MTU Ser No":
                    isVisible = true;

                    addToListview(field, value, 1);

                    break;
                case "1 Way Tx Freq":
                    isVisible = true;

                    addToListview(field, value, 2);

                    break;
                case "2 Way Tx Freq":
                    isVisible = true;

                    addToListview(field, value, 3);

                    break;
                case "2 Way Rx Freq":
                    isVisible = true;

                    addToListview(field, value, 4);

                    break;
                case "Tilt Tamp":
                    isVisible = true;

                    addToListview(field, value, 5);

                    break;
                case "Magnetic Tamp":
                    isVisible = true;

                    addToListview(field, value, 6);

                    break;
                case "Interface Tamp":
                    isVisible = true;

                    addToListview(field, value, 7);

                    break;

                case "Reg. Cover":
                    isVisible = true;

                    addToListview(field, value, 8);

                    break;
                case "Rev. Fl Tamp":
                    isVisible = true;

                    addToListview(field, value, 9);

                    break;
                case "Daily Snap":
                    isVisible = true;

                    addToListview(field, value, 10);

                    break;
                case "Installation":
                    isVisible = true;

                    addToListview(field, value, 11);

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

                    addToListview(field, value, 12);

                    break;
                case "Read Interval":
                    isVisible = true;

                    addToListview(field, value, 13);

                    break;
                case "Battery":
                    isVisible = true;

                    addToListview(field, value, 14);

                    break;
                case "MTU Type":
                    isVisible = true;

                    addToListview(field, value, 15);


                    break;
                case "MTU Software":
                    isVisible = true;

                    addToListview(field, value, 16);

                    break;
                case "PCB Number":
                    isVisible = true;

                    addToListview(field, value, 17);

                    break;
            }

            if (status.Equals("MTU Status Off"))
            {
                return false;
            }

            return isVisible;
        }

        public AclaraViewInstallConfirmation(IUserDialogs dialogs)
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
            if (FormsApp.credentialsService.UserName != null)
            {
                userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

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
            logout_button.Tapped += LogoutTapped;
            settings_button.Tapped += OpenSettingsView;

            logoff_no.Tapped += LogOffNoTapped;
            logoff_ok.Tapped += LogOffOkTapped;

        }


        private void LogOffOkTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Settings.IsLoggedIn = false;
            FormsApp.credentialsService.DeleteCredentials();
            FormsApp.ble_interface.Close();
            background_scan_page.IsEnabled = true;

            int contador = Navigation.NavigationStack.Count;
            while (contador > 0)
            {
                try
                {
                    Navigation.PopAsync(false);
                }
                catch (Exception v)
                {
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                Navigation.PopToRootAsync(false);
            }
            catch (Exception v1)
            {
                Console.WriteLine(v1.StackTrace);
            }


        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
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
            ////Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMTU(dialogsSaved), false);
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

            Task.Factory.StartNew(TurnOffMethod);
        }

        private void TurnOffMethod()
        {

            MTUComm.Action turnOffAction = new MTUComm.Action(
                config: FormsApp.config,
                serial: FormsApp.ble_interface,
                type: MTUComm.Action.ActionType.TurnOffMtu,
                user: FormsApp.credentialsService.UserName);

            turnOffAction.OnFinish += ((s, args) =>
            {
                ActionResult actionResult = args.Result;

                Task.Delay(2000).ContinueWith(t =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       this.dialog_turnoff_text.Text = "MTU turned off Successfully";

                       dialog_turnoff_two.IsVisible = false;
                       dialog_turnoff_three.IsVisible = true;
                   }));
            });

            turnOffAction.OnError += ((s, args) =>
            {
                Task.Delay(2000).ContinueWith(t =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       this.dialog_turnoff_text.Text = "MTU turned off Unsuccessfully";

                       dialog_turnoff_two.IsVisible = false;
                       dialog_turnoff_three.IsVisible = true;
                   }));
            });

            turnOffAction.Run();
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
            ////Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMeter(dialogsSaved), false);
        }

        private async void LogoutTapped(object sender, EventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                dialog_turnoff_one.IsVisible = false;
                dialog_open_bg.IsVisible = true;
                dialog_meter_replace_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_logoff.IsVisible = true;
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
            });

            /*
            Settings.IsLoggedIn = false;
            FormsApp.credentialsService.DeleteCredentials();

            int contador = Navigation.NavigationStack.Count;
            while (contador > 0)
            {
                try
                {
                    await Navigation.PopAsync(false);
                }
                catch (Exception v)
                {
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);
            }
            catch (Exception v1)
            {
                Console.WriteLine(v1.StackTrace);
            }
            */

        }

        private void ReturnToMainView(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync(false);
        }

        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
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

                        case "InstallConfirm":
                            Application.Current.MainPage.DisplayAlert("Alert", "You are already there", "Ok");
                            break;

                        case "replaceMTU":
                            Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                            //OnCaseReplaceMTU();
                            break;

                        case "replaceMeter":
                            Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                            //OnCaseReplaceMeter();
                            break;

                        case "AddMTUAddMeter":
                            Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                            //OnCaseAddMTUAddMeter();
                            break;

                        case "AddMTUReplaceMeter":
                            Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                            //OnCaseAddMTUReplaceMeter();
                            break;

                        case "ReplaceMTUReplaceMeter":
                            Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                            //OnCaseReplaceMTUReplaceMeter();
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
