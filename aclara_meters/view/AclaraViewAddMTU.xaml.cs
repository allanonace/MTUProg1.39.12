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
using System.Collections.ObjectModel;
using Plugin.Settings;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Xml.Serialization;
using System.IO;
using MTUComm;
using Xml;
using System.Threading;
using MTUComm.actions;
using FIELD = MTUComm.actions.AddMtuForm.FIELD;

namespace aclara_meters.view
{
    public partial class AclaraViewAddMTU
    {
        /**         **          **       **         **          **          **          **/

        public enum MTUStatus
        {
            ReadingMtuShortTime,
            ReadingMtuData,
            Autodetect,
            CheckingEnconderLongTime,

            ProgramingMtuShortTime,
            PreparingToProgram,
            TurningOffMtu,
            ReadingMtuAgain,
            ProgramingMtu,
            VerifyingMtuData,
            CheckingEnconderShortTime,
            TurningOnMtu,
            ReadingMtu,
        };


        public void SetUserInterfaceMTUStatus(string statusMsg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                label_read.Text = statusMsg;
            });
        }

        public string GetStringFromMTUStatus(MTUStatus mtuStatus, int time)
        {
            switch(mtuStatus)
            {
                case MTUStatus.ReadingMtuShortTime:
                    return "Reading MTU in " + time + " seconds...";
                   
                case MTUStatus.ReadingMtuData:
                    return "Reading MTU data...";
                   
                case MTUStatus.Autodetect:
                    return "Autodetect...";

                case MTUStatus.CheckingEnconderLongTime:
                    return "Checking Encoder... " + time;
               
                case MTUStatus.ProgramingMtuShortTime:
                    return "Programming MTU in " + time + " seconds...";
               
                case MTUStatus.PreparingToProgram:
                    return "Preparing to program...";
              
                case MTUStatus.TurningOffMtu:
                    return "Turning off MTU...";
               
                case MTUStatus.ReadingMtuAgain:
                    return "Reading MTU again...";
              
                case MTUStatus.ProgramingMtu:
                    return "Programming MTU...";
             
                case MTUStatus.VerifyingMtuData:
                    return "Verifying Mtu Data...";
              
                case MTUStatus.CheckingEnconderShortTime:
                    return "Checking Encoder... "+ time;
              
                case MTUStatus.TurningOnMtu:
                    return "Autodetect...";
                
                case MTUStatus.ReadingMtu:
                    return "Reading MTU...";
                
            }

            return "Error Detected";
        }

        /**         **          **       **         **          **          **          **/


        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        private List<string> picker_List_Vendor_port1;
        private List<string> picker_List_Model_port1;
        private List<string> picker_List_Name_port1;

        private List<string> picker_List_Vendor_port2;
        private List<string> picker_List_Model_port2;
        private List<string> picker_List_Name_port2;

        private double StepValue;
        private Slider SliderMain;

        private double StepValue2;
        private Slider SliderMain2;

        private bool port2enabled;

        private BorderlessPicker MeterVendorPicker;
        private BorderlessPicker MeterModelPicker;
        private BorderlessPicker MeterNamePicker;

        private BorderlessPicker MeterVendorPicker2;
        private BorderlessPicker MeterModelPicker2;
        private BorderlessPicker MeterNamePicker2;

        private enum Names
        {
            Name1 = 0,
        };

        private bool isCancellable;

        private MeterTypes meterTypes;
        private List<Xml.Meter> meters;
        private List<string> vendors;
        private List<string> models;
        private List<Meter> names;

        private string vendor;
        private string model;
        private string name;

        private List<string> vendors2;
        private List<string> models2;
        private List<Meter> names2;


        private string vendor2;
        private string model2;
        private string name2;


        private Global globals;
        private MtuTypes mtuData;

        /*
            Optional 1  WorkOrderRecording      Hides/shows Field Order
            Optional 2  MeterWorkRecording      Shows option to specify if meter is broken when replacing it
            Optional 3  ApptScreen              —— —— —— —— —— —— —— ——
            Optional 4  AccountDualEntry        Enable popup to repeat Service Pt. ID
            Optional 5  WorkOrderDualEntry      Enables popup to repeat Field Order
            Optional 6  OldSerialNumDualEntry   Enables popup to repeat Old meter #
            Optional 7  NewSerialNumDualEntry   Enable popup to repeat New meter #
            Optional 8  ReadingDualEntry        —— —— —— —— —— —— —— ——
            Optional 9  OldReadingDualEntry     Enable popup to repeat Old Reading
            Optional 10 ReverseReading          All Reading Inputs reversed
            Optional 11 IndividualReadInterval  If false, disables Read Interval field (it is visible but readonly)
            Optional 12 RegisterRecording       Shows Replace Meter/Register field, with Meter/Register/Both list
            Optional 13 UseMeterSerialNumber    Shows/hides different meter # fields (Old, New)
            Optional 14 OldReadingRecording     Shows/hides Old Reading field
            Optional 15 ShowAddMtu              Disables Add Mtu button (visible, but not clickable)
            Optional 16 ShowAddMtuReplaceMeter  Disables Add MTU/Replace Meter button (visible, but not clickable)
            Optional 17 ShowAddMtuMeter         Disables Add MTU/Add Meter button (visible, but not clickable)
            Optional 18 ShowReplaceMtu          Disables Replace MTU button (visible, but not clickable)
            Optional 19 ShowReplaceMtuMeter     Disables Replace MTU/Replace Meter button (visible, but not clickable)
            Optional 20 ShowReplaceMeter        Disables Replace Meter button (visible, but not clickable)
         */

       private bool WorkOrderRecording, 
                    MeterWorkRecording, 
                    ApptScreen, 
                    AccountDualEntry, 
                    WorkOrderDualEntry, 
                    OldSerialNumDualEntry, 
                    NewSerialNumDualEntry, 
                    ReadingDualEntry, 
                    OldReadingDualEntry, 
                    ReverseReading, 
                    IndividualReadInterval, 
                    RegisterRecording, 
                    UseMeterSerialNumber, 
                    OldReadingRecording, 
                    ShowAddMtu, 
                    ShowAddMtuReplaceMeter, 
                    ShowAddMtuMeter, 
                    ShowReplaceMtu, 
                    ShowReplaceMtuMeter, 
                    ShowReplaceMeter;

        private Slider MeterSnapReadsPort1Slider;
        private BorderlessPicker MeterTwoWayPort1Picker;
        private BorderlessPicker MeterAlarmSettingsPort1Picker;

        // Current MTU
        private Mtu currentMtu;

        // Snap reads
        private bool mtuDailyReads;
        private bool globalsAllowDailyReads;
        private bool setGlobalSnap;
        private bool globalsChangeDailyReads;
        private double globalsDailyReadsDefault;
        private double dailyReadsMemoryMapValue;

        // 2-Way
        private bool mtuFastMessageConfig;
        private bool globalsFastMessageConfig;

        // Alarms
        private bool mtuRequiresAlarmProfile;


        private List<Alarm> alarmListPort1;
        private List<Alarm> alarmListPort2;

    
        private List<string> demandListPort1;
        private List<string> demandListPort2;


        private List<ReadMTUItem> FinalReadListView { get; set; }


        // Add MTU form
        AddMtuForm addMtuForm;

    
        public AclaraViewAddMTU()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }

        private List<PageItem> MenuList { get; set; }

        private void PickerSelection(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }

        private void PickerSelection2(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }


        private void InitPickerReason()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();
            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>();
            objClassList.Add(new PickerItems { Name = "Complete" });
            objClassList.Add(new PickerItems { Name = "Cancel" });
            objClassList.Add(new PickerItems { Name = "Skip" });
            objClassList.Add(new PickerItems { Name = "Not Home" });
            objClassList.Add(new PickerItems { Name = "Other" });

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */
            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }
            //Now I am given ItemsSorce to the Pickers
            pickerReason.ItemsSource = objStringList;
        }


        private void InitPickerList()
        {
            /* Desconozco completamente cual es el caso de tener varios puertos, o el procedimiento para ello. Dejo la función de cara a su implementación */
            //
            //port2enabled = true;
            port2enabled = true;

            InitPickerReadInterval();
            InitPickerTwoWay();

            if (port2enabled)
            {
                port2label.IsVisible = true; //Las vistas en si no provocan fallos, con controlar el boton, bastaria.
                InitPickerReadInterval2();
                InitPickerTwoWay2();
            }
            else
            {
                port2label.IsVisible = false;
            }

            InitPickerReason();

        }

        private void InitPickerReadInterval()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "24 Hours" },
                new PickerItems { Name = "12 Hours" },
                new PickerItems { Name = "6 Hours" },
                new PickerItems { Name = "4 Hours" },
                new PickerItems { Name = "3 Hours" },
                new PickerItems { Name = "2 Hours" },
                new PickerItems { Name = "1 Hour" },
                new PickerItems { Name = "30 Min" },
                new PickerItems { Name = "20 Min" },
                new PickerItems { Name = "15 Min" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReadInterval.ItemsSource = objStringList;
        }


        private void InitPickerReadInterval2()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "24 Hours" },
                new PickerItems { Name = "12 Hours" },
                new PickerItems { Name = "6 Hours" },
                new PickerItems { Name = "4 Hours" },
                new PickerItems { Name = "3 Hours" },
                new PickerItems { Name = "2 Hours" },
                new PickerItems { Name = "1 Hour" },
                new PickerItems { Name = "30 Min" },
                new PickerItems { Name = "20 Min" },
                new PickerItems { Name = "15 Min" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReadInterval2.ItemsSource = objStringList;
        }


        private void InitPickerTwoWay()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Slow" },
                new PickerItems { Name = "Fast" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerTwoWay.ItemsSource = objStringList;
        }

        private void InitPickerTwoWay2()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Slow" },
                new PickerItems { Name = "Fast" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerTwoWay2.ItemsSource = objStringList;
        }

        private void LoadSideMenuElements()
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


        private void OpenSettingsCallAsync(object sender, EventArgs e)
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
                bg_read_mtu_button_img.Source = "add_mtu_btn_black.png";

            }
            else
            {
                bg_read_mtu_button_img.Source = "add_mtu_btn.png";
            }
        }

        private void AddMtu(object sender, EventArgs e)
        {
            if (!ValidateEmptyFields()){

                DisplayAlert("Error", "Mandatory fields are not filled in","Ok");
                return;
            }

            isCancellable = true;

            if (!_userTapped)
            {

               /*Task.Delay(100).ContinueWith(t =>

               Device.BeginInvokeOnMainThread(() =>
               {
                   backdark_bg.IsVisible = true;
                   indicator.IsVisible = true;
                   _userTapped = true;
                   background_scan_page.IsEnabled = false;
                   ChangeLowerButtonImage(true);

       
                   Task.Delay(1000).ContinueWith(t0 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 5);
                   }));

                   Task.Delay(2000).ContinueWith(t1 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 4);
                   }));

                   Task.Delay(3000).ContinueWith(t2 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 3);
                   }));

                   Task.Delay(4000).ContinueWith(t3 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 2);
                   }));

                   Task.Delay(5000).ContinueWith(t4 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 1);
                   }));

                   Task.Delay(6000).ContinueWith(t5 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.PreparingToProgram, 0);
                   }));

                   Task.Delay(7000).ContinueWith(t6 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.TurningOffMtu, 0);
                   }));

                   Task.Delay(8000).ContinueWith(t7 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuAgain, 0);
                   }));

                   Task.Delay(9000).ContinueWith(t8 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 20);
                   }));

                   Task.Delay(10000).ContinueWith(t9 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 19);
                   }));

                   Task.Delay(11000).ContinueWith(t10 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 18);
                   }));

                   Task.Delay(12000).ContinueWith(t11 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 17);
                   }));

                   Task.Delay(13000).ContinueWith(t12 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtu, 0);
                   }));

                   Task.Delay(14000).ContinueWith(t13 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.VerifyingMtuData, 0);
                   }));


                   Task.Delay(15000).ContinueWith(t14 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 20);
                   }));

                   Task.Delay(16000).ContinueWith(t15 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 19);
                   }));

                   Task.Delay(17000).ContinueWith(t16 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 18);
                   }));

                   Task.Delay(18000).ContinueWith(t17 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 17);
                   }));

                   Task.Delay(19000).ContinueWith(t18 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.TurningOnMtu, 0);
                   }));

                   Task.Delay(20000).ContinueWith(t17 =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                        label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtu, 0);

                        //ThreadProcedureMTUCOMMAction();
                        FinalReadListView = new List<ReadMTUItem>();


                        FinalReadListView.Add(new ReadMTUItem()
                        {
                            Title = "Param Field" + ":",
                            isDisplayed = "true",
                            Height = "64",
                            isMTU = "true",
                            isMeter = "false",
                            Description = "99" //parameter.Value
                        });

                        FinalReadListView.Add(new ReadMTUItem()
                        {
                            Title = "Here lies the Port title...",
                            isDisplayed = "true",
                            Height = "40",
                            isMTU = "false",
                            isMeter = "true",
                            Description = "Port " + "99" + ": " + " PlaceHolder"
                        });

                        FinalReadListView.Add(new ReadMTUItem()
                        {
                            Title = "\t\t" + "Param Field" + ":",
                            isDisplayed = "true",
                            Height = "64",
                            isMTU = "true",
                            isMeter = "false",
                            Description = "\t\t" + "99"//parameter.Value
                        });


                        FinalReadListView.Add(new ReadMTUItem()
                        {
                            Title = "Another Param Field" + ":",
                            isDisplayed = "true",
                            Height = "64",
                            isMTU = "true",
                            isMeter = "false",
                            Description = "99" //parameter.Value
                        });


                        ReadMTUChangeView.IsVisible = false;
                        listaMTUread.IsVisible = true;
                        listaMTUread.ItemsSource = FinalReadListView;

                        Task.Run(() =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                label_read.Opacity = 1;
                                backdark_bg.IsVisible = false;
                                indicator.IsVisible = false;
                                _userTapped = false;
                                ChangeLowerButtonImage(false);
                                background_scan_page.IsEnabled = true;
                            });
                        });
                   }));
               }));*/
                Device.BeginInvokeOnMainThread(() =>
                {
                    ThreadProcedureMTUCOMMAction();
                });
            }
        }

        private bool ValidateEmptyFields()
        {
           
            /** Validation **/
            /** Check if visible fields are filled with the correct values in order to Enable Add MTU Cmd **/
                
            /////// Port 1 ///////
            if (servicePortId.Text.Length < servicePortId.MaxLength)
                return false;

            if (fo_view.IsVisible)
                if (fieldOrder.Text.Length < fieldOrder.MaxLength)
                return false;
            
            if (mn_view.IsVisible)
                if (meterNumber.Text.Length < meterNumber.MaxLength)
                return false;
            
            if (EntriesStackLayout.Children[2].IsVisible)
                if (model != null)
                    if (EntriesStackLayout.Children[1].IsVisible)
                        if (name.Length < 0)
                            return false;

            if (initialRead.Text.Length < initialRead.MaxLength)
                return false;
         
            if(read_view.Opacity>0.8)
                if (pickerReadInterval.SelectedIndex == -1)
                    return false;
            
            /*
            if (SnapReads_Port1.IsVisible)   if(SnapReadsViewPort1.Opacity>0.8)
                if(sliderValue.Text -1)
                    isValid = false;
            */

            if (TwoWay_Port1.IsVisible)
                if (pickerTwoWay.SelectedIndex == -1)
                    return false;
               

            if (Alarms_Port1.IsVisible)
                if (pickerAlarms.SelectedIndex == -1)
                    return false;


            if (Demands_Port1.IsVisible)
                if (pickerDemands.SelectedIndex == -1)
                    return false;
            

            /////// Port 2 ///////
           
            if (servicePortId2.Text.Length < servicePortId2.MaxLength)
                return false;
           
            if (fo_view2.IsVisible)
                if (fieldOrder2.Text.Length < fieldOrder2.MaxLength)
                return false;


            if (mn_view2.IsVisible)
                if (meterNumber2.Text.Length < meterNumber2.MaxLength)
                return false;


            if (EntriesStackLayout2.Children[2].IsVisible)
                if (model2 != null)
                    if (EntriesStackLayout2.Children[1].IsVisible)
                        if (name2.Length < 0)
                            return false;

            if (initialRead2.Text.Length < initialRead2.MaxLength)
                return false;

            if(read_view2.Opacity>0.8)
                if (pickerReadInterval2.SelectedIndex == -1)
                    return false;

            if (TwoWay_Port2.IsVisible)
                if (pickerTwoWay2.SelectedIndex == -1)
                    return false;


            if (Alarms_Port2.IsVisible)
                if (pickerAlarms2.SelectedIndex == -1)
                    return false;


            if (Demands_Port2.IsVisible)
                if (pickerDemands2.SelectedIndex == -1)
                    return false;
            
            /////// Misc ///////

            if (mtuGeolocationLat.Text.Length < 0)
                return false;
            
            if (mtuGeolocationLong.Text.Length < 0)
                return false;

            if (mtuLocation.SelectedIndex == -1)
                return false;

            if (meterLocation.SelectedIndex == -1)
                return false;

            if (construction.SelectedIndex == -1)
                return false;
            
            return true;
        }

        public AclaraViewAddMTU(IUserDialogs dialogs)
        {
            /*// TEST
            byte[] memory = new byte[400];
            dynamic map = new MTUComm.MemoryMap.MemoryMap(memory, "31xx32xx"); // TODO: identify map by mtu type
            map.ReadInterval = "24 Hours";
            map.MessageOverlapCount = 5;
            map.P1Reading = 0;
            map.P1MeterId = "1";
            map.P1MeterType = "1";
            map.EncryptionKey = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            List<dynamic> modifiedRegisters = map.GetModifiedRegisters().GetAllElements();
            // END TEST */


            /* Get detected mtu */
            MTUBasicInfo mtuBasicInfo = MtuForm.mtuBasicInfo;
            int DetectedMtuType = (int)mtuBasicInfo.Type;
            currentMtu = FormsApp.config.GetMtuTypeById(DetectedMtuType);

            /* Instantiate form */
            addMtuForm = new AddMtuForm(currentMtu);
            addMtuForm.SetCondition(AddMtuForm.FIELD_CONDITIONS.MTU_REQUIRES_ALARM_PROFILE, currentMtu.RequiresAlarmProfile);
            addMtuForm.SetCondition(AddMtuForm.FIELD_CONDITIONS.MTU_TWO_PORTS, currentMtu.TwoPorts);

            /* Get meters for detected mtu */
            MeterTypes m = FormsApp.config.GetMeterTypes();
            // TODO: dos puertos
            this.meters = m.FindByPortTypeAndFlow(currentMtu.Ports[0].Type, currentMtu.Flow);

            /* Get alarms for detected mtu */
            alarmListPort1 = FormsApp.config.Alarms.FindByMtuType(DetectedMtuType);
            alarmListPort2 = FormsApp.config.Alarms.FindByMtuType(DetectedMtuType);

            demandListPort1 = new List<string>();
            demandListPort2 = new List<string>();

            //ADD ALARMS TO LIST
            for (int i = 1; i < 4; i++)
            {
                demandListPort1.Add("Demand " + i);
                demandListPort2.Add("Demand " + i);
            }

            InitializeComponent();

            isCancellable = false;

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    label_read.Opacity = 1;
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

            // Load side menu list
            LoadSideMenuElements();

            //Init picker list elements
            InitPickerList();

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

        
            Device.BeginInvokeOnMainThread(() =>
            {
                label_read.Opacity = 1;
            });

            _userTapped = false;

            //Initialize Tap/Clickable element listeners
            TappedListeners();

            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName; //"Kartik";
            }


            battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

            InitializeLowerbarLabel();

            InitializeBlocks();


            TestOptionalFields();

            Validations();

            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            submit_dialog.Clicked += submit_send;

            listaMTUread.IsVisible = false;



            Task.Delay(100).ContinueWith(t =>

              Device.BeginInvokeOnMainThread(() =>
              {
                  backdark_bg.IsVisible = true;
                  indicator.IsVisible = true;
                  background_scan_page.IsEnabled = false;

                  Task.Delay(1000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuShortTime, 5);
                  }));

                  Task.Delay(2000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuShortTime, 4);
                  }));

                  Task.Delay(3000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuShortTime, 3);
                  }));


                  Task.Delay(4000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuShortTime, 2);
                  }));

                  Task.Delay(5000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuShortTime, 1);
                  }));

                  Task.Delay(6000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuData, 1);
                  }));

                  Task.Delay(7000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.Autodetect, 1);
                  }));


                  Task.Delay(8000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderLongTime, 50);
                  }));

                  Task.Delay(9000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderLongTime, 49);
                  }));

                  Task.Delay(10000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderLongTime, 48);
                  }));

                  Task.Delay(11000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderLongTime, 47);
                  }));


                  Task.Delay(12000).ContinueWith(t0 =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderLongTime, 46);
                      Task.Run(() =>
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                              label_read.Opacity = 1;
                              backdark_bg.IsVisible = false;
                              indicator.IsVisible = false;
                              background_scan_page.IsEnabled = true;
                              label_read.Text = "Press Button to Start";
                          });
                      });

                  }));

              }
                                             
            ));


        }

        private void submit_send(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            Task.Run(async () =>
            {
                await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopAsync(false);
                });
            });

        }


        private void ThreadProcedureMTUCOMMAction()
        {
            // Create parameters with form fields content
            addMtuForm.AddParameter(FIELD.SERVICE_PORT_ID, servicePortId.Text);
            addMtuForm.AddParameter(FIELD.FIELD_ORDER, fieldOrder.Text);
            addMtuForm.AddParameter(FIELD.METER_NUMBER, meterNumber.Text);
            addMtuForm.AddParameter(FIELD.SELECTED_METER_ID, (Meter)MeterNamePicker.SelectedItem);

            addMtuForm.AddParameter(FIELD.READ_INTERVAL, pickerReadInterval.SelectedItem.ToString());
            addMtuForm.AddParameter(FIELD.SNAP_READS, SliderMain.Value.ToString());
            addMtuForm.AddParameter(FIELD.TWO_WAY, pickerTwoWay.SelectedItem.ToString());
            addMtuForm.AddParameter(FIELD.ALARM, (Alarm)pickerAlarms.SelectedItem);

            //Create Ation when opening Form
            // TODO: usuario real
            MTUComm.Action add_mtu = new MTUComm.Action ( 
                config    : FormsApp.config,
                serial    : FormsApp.ble_interface,
                actiontype: MTUComm.Action.ActionType.AddMtu,
                user      : "iker");

            // Add parameters to the action
            add_mtu.AddParameter ( addMtuForm );

            add_mtu.OnFinish += ((s, e) => {   
                Console.WriteLine("Action Succefull");
                Console.WriteLine("Press Key to Exit");
                //Console.WriteLine(s.ToString());

                /*foreach(Parameter param in e.Result.getParameters()){
                    Console.WriteLine(param.getLogDisplay() + ":" + param.getValue());
                }*/

                FinalReadListView = new List<ReadMTUItem>(); // Saves the data to view
    
                Parameter[] paramResult = e.Result.getParameters();

                int mtu_type = 0;

                foreach (Parameter p in paramResult)
                {
                    try
                    {
                        if (p.CustomParameter.Equals("MtuType"))
                        {
                            mtu_type = Int32.Parse(p.Value.ToString());
                        }

                    }
                    catch (Exception e5)
                    {
                        Console.WriteLine(e5.StackTrace);
                    }

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
                                        Description = "Port " + i + ": " + param.getValue() //parameter.Value
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
                                    {
                                        param = ports[i].getParameterByTag(port_parameter.Name);
                                    }

                                    if (param != null)
                                    {


                                        FinalReadListView.Add(new ReadMTUItem()
                                        {
                                            Title = "\t\t\t\t\t" + param.getLogDisplay() + ":",
                                            isDisplayed = "true",
                                            Height = "64",
                                            isMTU = "true",
                                            isMeter = "false",
                                            Description = "\t\t\t\t\t" + param.getValue() //parameter.Value
                                        });
                                    }
                                }
                            }
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
                        {
                            param = e.Result.getParameterByTag(parameter.Name);
                        }

                        if (param != null)
                        {

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
                }




                Task.Delay(100).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    
                    _userTapped = false;
                    bg_read_mtu_button.NumberOfTapsRequired = 1;
                    ChangeLowerButtonImage(false);
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    label_read.Text = "Successful MTU write";
                    background_scan_page.IsEnabled = true;

                    /*
                    ReadMTUChangeView.IsVisible = false;
                    listaMTUread.IsVisible = true;
                    listaMTUread.ItemsSource = FinalReadListView;
                    */

                    }));



            });


            add_mtu.OnError  += ((s, e) => {
                 Console.WriteLine("Action Errror");
                 Console.WriteLine("Press Key to Exit");
                // Console.WriteLine(s.ToString());

                 String result = e.Message;

           
                                            
                Task.Delay(100).ContinueWith(t =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      _userTapped = false;
                      bg_read_mtu_button.NumberOfTapsRequired = 1;
                      ChangeLowerButtonImage(false);
                      backdark_bg.IsVisible = false;
                      indicator.IsVisible = false;
                      label_read.Text = result;
                      background_scan_page.IsEnabled = true;
                  }));



                 Console.WriteLine(result.ToString());

             });

            // Launch the action!
            add_mtu.Run ( addMtuForm );
             
        }

        private void TestOptionalFields()
        {
            //globals =  FormsApp.config.GetGlobal();

            //WorkOrderRecording = globals.WorkOrderRecording;
            //AccountDualEntry = globals.AccountDualEntry;
            //WorkOrderDualEntry = globals.WorkOrderDualEntry;
            //IndividualReadInterval = globals.IndividualReadInterval;
            //UseMeterSerialNumber = globals.UseMeterSerialNumber;

            ////mtuData
            //Mtu mtu = FormsApp.config.GetMtuTypeById(138);

            //WorkOrderRecording = globalData.WorkOrderRecording;
            //AccountDualEntry = globalData.AccountDualEntry;
            //WorkOrderDualEntry = globalData.WorkOrderDualEntry;
            //IndividualReadInterval = globalData.IndividualReadInterval;
            //UseMeterSerialNumber = globalData.UseMeterSerialNumber;

            ///******************/
            ///**  Snap Reads  **/
            //mtuDailyReads = mtu.DailyReads;
            //globalsAllowDailyReads = globalData.AllowDailyReads;

            //// Snap Reads
            //mtuDailyReads = currentMtu.DailyReads;
            //globalsAllowDailyReads = globals.AllowDailyReads;
            //setGlobalSnap = true;
            //dailyReadsMemoryMapValue = 15.0;
            //globalsChangeDailyReads = true;
            //globalsDailyReadsDefault =  Convert.ToDouble( globalData.DailyReadsDefault ); 
            ///****    ****    ****/


            ///******************/
            ///**     2-way    **/

            //mtuFastMessageConfig = true;
            //globalsFastMessageConfig = true;

            ///****   ****   ****/


            ///***************************/
            ///**     Alarm Settings    **/

            //mtuRequiresAlarmProfile = true;
            //alarmListPort1 = new List<string>();
            //alarmListPort2 = new List<string>();

            ////ADD ALARMS TO LIST
            //for (int i = 1; i < 4; i++)
            //{
            //    alarmListPort1.Add("Alarm "+i);
            //    alarmListPort2.Add("Alarm "+ i);
            //globalsDailyReadsDefault = Convert.ToDouble(globals.DailyReadsDefault);
            //if (globalsDailyReadsDefault > 23 || globalsDailyReadsDefault < 0)
            //{
            //    globalsDailyReadsDefault = 24.0;
            //}

            ///***************************/
            ///**     Demand Settings    **/

            //demandListPort1 = new List<string>();
            //demandListPort2= new List<string>();

            ////ADD ALARMS TO LIST
            //for (int i = 1; i < 4; i++)
            //{
            //    demandListPort1.Add("Demand " + i);
            //    demandListPort2.Add("Demand " + i);
            //}

            ///****  ****  ****  **** ****/

            //// 2-Way
            //mtuFastMessageConfig = currentMtu.FastMessageConfig;
            //globalsFastMessageConfig = globals.FastMessageConfig || globals.Fast2Way;

            //// Alarm Settings
            //mtuRequiresAlarmProfile = currentMtu.RequiresAlarmProfile;
        }


        private void FormsUILogic()
        {
            /******************/
            /**  Snap Reads  **/

            if( mtuDailyReads && globalsAllowDailyReads )
            {
                SnapReads_Port1.IsVisible = true;
            }else{
                SnapReads_Port1.IsVisible = false;
            }

            if( setGlobalSnap )
            {
                SliderMain.Value = globalsDailyReadsDefault;
            }else{

                SliderMain.Value = dailyReadsMemoryMapValue;
            }
          
            if( !globalsChangeDailyReads )
            {
                SnapReads_Port1.IsVisible = true;
                SnapReads_Port1.IsEnabled = false;
                SnapReadsViewPort1.IsEnabled = false;
                SnapReadsViewPort1.Opacity = 0.8;
            }else{
                SnapReadsViewPort1.Opacity = 1;
            }

            /****    ****    ****/

            /******************/
            /**     2-way    **/

            if ( mtuFastMessageConfig )
            {
                TwoWay_Port1.IsVisible = true;
            }else{
                TwoWay_Port1.IsVisible = false;
            }

            if (globalsFastMessageConfig)
            {
                pickerTwoWay.SelectedIndex = 1;
                //Fast
            }else{
                pickerTwoWay.SelectedIndex = 0;
                //slow
            }

        
            /***************************/
            /**     Alarm Settings    **/

            if ( mtuRequiresAlarmProfile )
            {
                Alarms_Port1.IsVisible = true;
            }else{
                Alarms_Port1.IsVisible = false;
            }

            if( alarmListPort1.Count > 0 )
            {
                pickerAlarms.ItemsSource = alarmListPort1;
                pickerAlarms.ItemDisplayBinding = new Binding("Name");
            } 
           
            if(alarmListPort2.Count > 0)
            {
                pickerAlarms2.ItemsSource = alarmListPort2;
                pickerAlarms2.ItemDisplayBinding = new Binding("Name");
            }

            /****  ****  ****  **** ****/



            /***************************/
            /**     Demand Settings    **/

            if(demandListPort1.Count > 0){
                pickerDemands.ItemsSource = demandListPort1;
            }

            if(demandListPort2.Count > 0){
                pickerDemands2.ItemsSource = demandListPort2;
            }

            /****  ****  ****  **** ****/

        }

        private void Validations()
        {

            FormsUILogic();


            if( WorkOrderRecording )
            {
                fo_view.IsVisible = true;
            }else{
                fo_view.IsVisible = false;
            }

            if( AccountDualEntry )
            {
                servicePortId.Unfocused += (s, e) => { ServicePortId_validate(1); };
                servicePortId2.Unfocused += (s, e) => { ServicePortId_validate(2); };
                servicePortId_ok.Tapped += ServicePortId_Ok_Tapped;
                servicePortId_cancel.Tapped += ServicePortId_Cancel_Tapped;
            }

            if (WorkOrderDualEntry)
            {
                fieldOrder.Unfocused += (s, e) => { FieldOrder_validate(1); };
                fieldOrder2.Unfocused += (s, e) => { FieldOrder_validate(2); };
                fieldOrder_ok.Tapped += FieldOrder_Ok_Tapped;
                fieldOrder_cancel.Tapped += FieldOrder_Cancel_Tapped;
            }

            if(IndividualReadInterval)
            {
                pickerReadInterval.IsEnabled = true;
                read_view.Opacity = 1;

            }else{
                pickerReadInterval.IsEnabled = false;
                read_view.Opacity = 0.8;
            }

            if (UseMeterSerialNumber)
            {
                mn_view.IsVisible = true;
            }else{
                mn_view.IsVisible = false;
            }

        }

        /** Dialogs Tap Detection **/
        private void ServicePortId_Ok_Tapped(object sender, EventArgs e)
        {

            if (servicePortId.Text.Equals(serviceCheckEntry.Text))
            {
                errorServicePort.IsVisible = false;
                dialog_open_bg.IsVisible = false;
                turnoff_mtu_background.IsVisible = false;
                dialog_servicePortId.IsVisible = false;
                serviceCheckEntry.Text = "";
            }else{
                errorServicePort.IsVisible = true;
           }
        }

        private void ServicePortId_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            dialog_servicePortId.IsVisible = false;
            errorServicePort.IsVisible = false;
            servicePortId.Text = "";
        }

        private void FieldOrder_Ok_Tapped(object sender, EventArgs e)
        {

            if (fieldOrder.Text.Equals(fieldOrderCheckEntry.Text))
            {
                errorFieldOrder.IsVisible = false;
                dialog_open_bg.IsVisible = false;
                turnoff_mtu_background.IsVisible = false;
                dialog_fieldOrder.IsVisible = false;
                fieldOrderCheckEntry.Text = "";
            }
            else
            {
                errorFieldOrder.IsVisible = true;
            }
        }

        private void FieldOrder_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            dialog_fieldOrder.IsVisible = false;
            errorFieldOrder.IsVisible = false;
            fieldOrder.Text = "";
        }

        /*********          ****        ************/

        /** Dialogs Validation **/
        private void ServicePortId_validate(int v)
        {
            if(!servicePortId.Text.Equals(""))
            {
                if (v == 1)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_servicePortId.IsVisible = true;

                }
                else if (v == 2)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_servicePortId.IsVisible = true;
                }
            }
        }

        private void FieldOrder_validate(int v)
        {
            if (!fieldOrder.Text.Equals(""))
            {
                   
                if (v == 1)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_fieldOrder.IsVisible = true;

                }
                else if (v == 2)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_fieldOrder.IsVisible = true;
                }
            }

        }

        /*********          ****        ************/

        private void InitializeBlocks()
        {
            ColectionElementsPort1();
            ColectionElementsPort2();
            InitMTULocationPicker();
            InitMeterLocationPicker();
            InitConstructionPicker();
        }

        private void InitConstructionPicker()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Vinyl" },
                new PickerItems { Name = "Wood" },
                new PickerItems { Name = "Brick" },
                new PickerItems { Name = "Aluminium" },
                new PickerItems { Name = "Other" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            construction.ItemsSource = objStringList;
        }

        private void InitMeterLocationPicker()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Outside" },
                new PickerItems { Name = "Inside" },
                new PickerItems { Name = "Basement" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            meterLocation.ItemsSource = objStringList;
        }

        private void InitMTULocationPicker()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Outside" },
                new PickerItems { Name = "Inside" },
                new PickerItems { Name = "Basement" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            mtuLocation.ItemsSource = objStringList;
        }



        private void ColectionElementsPort1()
        {
            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  MARCA [0]            **/

           

            meterTypes = FormsApp.config.GetMeterTypes();


            //int encoderType = 2;
            //int liveDigits = 6;

            //meters = meterTypes.FindByEncoderTypeAndLiveDigits(encoderType, liveDigits);

            vendors = meterTypes.GetVendorsFromMeters(meters);
           

            //Listado de los Selectores
            picker_List_Vendor_port1 = new List<string>();

            for (int i1 = 0; i1 < vendors.Count; i1++)
            {
                picker_List_Vendor_port1.Add(vendors[i1]);
            }

          
            Frame fm1_vendor = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_vendor = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_vendor = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            // Generamos el Selector
            MeterVendorPicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40, 
                FontSize=17,
                ItemsSource = picker_List_Vendor_port1
            };

            //Detectar el Selector clickado
            MeterVendorPicker.SelectedIndexChanged += PickerMarcas_SelectedIndexChanged;


            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloque = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            //Texto del titulo
            Label textoTitulo = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            //Añadimos el titulo y el selector al bloque

            //Texto del titulo
            Label textoTituloCamposGrandes = new Label()
            {
                Text = "Vistas de Colección",
                Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),
                IsVisible = false,
                Margin = new Thickness(0, 8, 0, 0)
                   
            };


            st_vendor.Children.Add(MeterVendorPicker);
            fm2_vendor.Content = st_vendor;
            fm1_vendor.Content = fm2_vendor;

            ElementoBloque.Children.Add(textoTituloCamposGrandes);
            ElementoBloque.Children.Add(textoTitulo);

            // Picker to    fm1_vendor --> fm2_vendor --> st_vendor --> picker
            ElementoBloque.Children.Add(fm1_vendor);

            //Introducimos el bloque en la vista
            EntriesStackLayout.Children.Add(ElementoBloque);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  MODELO  [1]          **/
            //Listado de los Selectores
            picker_List_Model_port1 = new List<string>();

            Frame fm1_model = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_model = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_model = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

  

            // Generamos el Selector
            MeterModelPicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Model_port1,
                StyleId = "pickerModelos"
            };

            //Detectar el Selector clickado
            MeterModelPicker.SelectedIndexChanged += PickerModelos_SelectedIndexChanged;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueModelo = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            //Texto del titulo
            Label textoTituloModelo = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            st_model.Children.Add(MeterModelPicker);
            fm2_model.Content = st_model;
            fm1_model.Content = fm2_model;


            //Añadimos el titulo y el selector al bloque
            ElementoBloqueModelo.Children.Add(textoTituloModelo);
            ElementoBloqueModelo.Children.Add(fm1_model);

            //Introducimos el bloque en la vista
            EntriesStackLayout.Children.Add(ElementoBloqueModelo);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  COLOR  [2]          **/
            //Listado de los Selectores
            picker_List_Name_port1 = new List<string>();

            Frame fm1_name = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_name = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_name = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };



            // Generamos el Selector
            MeterNamePicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Name_port1,
                StyleId = "pickerName"
            };

            //Detectar el Selector clickado
            MeterNamePicker.SelectedIndexChanged += PickerName_SelectedIndexChanged;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueName = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            //Texto del titulo
            Label textoTituloName = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            st_name.Children.Add(MeterNamePicker);
            fm2_name.Content = st_name;
            fm1_name.Content = fm2_name;

            //Añadimos el titulo y el selector al bloque
            ElementoBloqueName.Children.Add(textoTituloName);
            ElementoBloqueName.Children.Add(fm1_name);

            //Introducimos el bloque en la vista
            EntriesStackLayout.Children.Add(ElementoBloqueName);

            ElementoBloqueName.IsVisible = false;
            ElementoBloqueModelo.IsVisible = false;

            StepValue = 1.0;

            SliderMain = new Slider
            {
                Minimum = 0.0f,
                Maximum = 20.0f,
                HeightRequest = 40,
                Value = 10.0f,
                Margin = new Thickness(0, -20, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            SliderMain.ValueChanged += OnSliderValueChanged;

            IntegerSlider.Children.Add(new StackLayout
            {
                Children = { SliderMain },
                Orientation = StackOrientation.Vertical,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });

        }


        private void ColectionElementsPort2()
        {
            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  MARCA [0]            **/


            meterTypes = FormsApp.config.GetMeterTypes();


            //int encoderType = 2;
            //int liveDigits = 6;

            //meters = meterTypes.FindByEncoderTypeAndLiveDigits(encoderType, liveDigits);

            vendors2 = meterTypes.GetVendorsFromMeters(meters);


            //Listado de los Selectores
            picker_List_Vendor_port2 = new List<string>();

            for (int i1 = 0; i1 < vendors2.Count; i1++)
            {
                picker_List_Vendor_port2.Add(vendors2[i1]);
            }


            Frame fm1_vendor = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_vendor = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_vendor = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            // Generamos el Selector
            MeterVendorPicker2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Vendor_port2
            };

            //Detectar el Selector clickado
            MeterVendorPicker2.SelectedIndexChanged += PickerMarcas_SelectedIndexChanged2;


            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloque = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            //Texto del titulo
            Label textoTitulo = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            //Añadimos el titulo y el selector al bloque

            //Texto del titulo
            Label textoTituloCamposGrandes = new Label()
            {
                Text = "Vistas de Colección",
                Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),
                IsVisible = false,
                Margin = new Thickness(0, 8, 0, 0)

            };


            st_vendor.Children.Add(MeterVendorPicker2);
            fm2_vendor.Content = st_vendor;
            fm1_vendor.Content = fm2_vendor;

            ElementoBloque.Children.Add(textoTituloCamposGrandes);
            ElementoBloque.Children.Add(textoTitulo);

            // Picker to    fm1_vendor --> fm2_vendor --> st_vendor --> picker
            ElementoBloque.Children.Add(fm1_vendor);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloque);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  MODELO  [1]          **/
            //Listado de los Selectores
            picker_List_Model_port2 = new List<string>();

            Frame fm1_model = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_model = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_model = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };



            // Generamos el Selector
            MeterModelPicker2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Model_port2,
                StyleId = "pickerModelos2"
            };

            //Detectar el Selector clickado
            MeterModelPicker2.SelectedIndexChanged += PickerModelos_SelectedIndexChanged2;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueModelo = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            //Texto del titulo
            Label textoTituloModelo = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            st_model.Children.Add(MeterModelPicker2);
            fm2_model.Content = st_model;
            fm1_model.Content = fm2_model;


            //Añadimos el titulo y el selector al bloque
            ElementoBloqueModelo.Children.Add(textoTituloModelo);
            ElementoBloqueModelo.Children.Add(fm1_model);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloqueModelo);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  COLOR  [2]          **/
            //Listado de los Selectores
            picker_List_Name_port2 = new List<string>();

            Frame fm1_name = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_name = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_name = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };



            // Generamos el Selector
            MeterNamePicker2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Name_port2,
                StyleId = "pickerName2"
            };

            //Detectar el Selector clickado
            MeterNamePicker2.SelectedIndexChanged += PickerName_SelectedIndexChanged2;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueName = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            //Texto del titulo
            Label textoTituloName = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            st_name.Children.Add(MeterNamePicker2);
            fm2_name.Content = st_name;
            fm1_name.Content = fm2_name;

            //Añadimos el titulo y el selector al bloque
            ElementoBloqueName.Children.Add(textoTituloName);
            ElementoBloqueName.Children.Add(fm1_name);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloqueName);

            ElementoBloqueName.IsVisible = false;
            ElementoBloqueModelo.IsVisible = false;

            StepValue2 = 1.0;

            SliderMain2 = new Slider
            {
                Minimum = 0.0f,
                Maximum = 20.0f,
                HeightRequest = 40,
                Value = 10.0f,
                Margin = new Thickness(0, -20, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            SliderMain2.ValueChanged += OnSliderValueChanged2;

            IntegerSlider2.Children.Add(new StackLayout
            {
                Children = { SliderMain2 },
                Orientation = StackOrientation.Vertical,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });

        }


        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);

            SliderMain.Value = newStep * StepValue;

            sliderValue.Text = SliderMain.Value.ToString();

        }

 

        void OnSliderValueChanged2(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue2);

            SliderMain2.Value = newStep * StepValue2;

            sliderValue2.Text = SliderMain2.Value.ToString();

        }




        private void PickerMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            vendor = vendors[j];
            models = null;

            models = meterTypes.GetModelsByVendorFromMeters(meters, vendor);
            name = "";

            try
            {
                MeterModelPicker.ItemsSource = null;
                MeterModelPicker.ItemsSource = models;



                EntriesStackLayout.Children[1].IsVisible = true;
                EntriesStackLayout.Children[2].IsVisible = false;
            }
            catch (Exception e3)
            {
                MeterModelPicker.ItemsSource = null;
                EntriesStackLayout.Children[1].IsVisible = false;
                EntriesStackLayout.Children[2].IsVisible = false;
                Console.WriteLine(e3.StackTrace);
            }

        }



        private void PickerMarcas_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            vendor2 = vendors2[j];
            models2 = null;

            models2 = meterTypes.GetModelsByVendorFromMeters(meters, vendor2);
            name2 = "";

            try
            {
                MeterModelPicker2.ItemsSource = null;
                MeterModelPicker2.ItemsSource = models2;



                EntriesStackLayout2.Children[1].IsVisible = true;
                EntriesStackLayout2.Children[2].IsVisible = false;
            }
            catch (Exception e3)
            {
                MeterModelPicker2.ItemsSource = null;
                EntriesStackLayout2.Children[1].IsVisible = false;
                EntriesStackLayout2.Children[2].IsVisible = false;
                Console.WriteLine(e3.StackTrace);
            }

        }


		private void PickerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            if (j == -1)
                return;
            
            Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;
            name = selectedMeter.Display;
            try
            {
                Console.WriteLine(name + " Selected");
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2.StackTrace);
            }

        }

        
        private void PickerName_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            if (j == -1)
                return;

            Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;
            name2 = selectedMeter.Display;
            try
            {
                Console.WriteLine(name2 + " Selected");
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2.StackTrace);
            }

        }




        private void PickerModelos_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            int i = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + i);
            if (i == -1)
                return;
            
                
            MeterNamePicker.ItemDisplayBinding = new Binding("Display");

          
            model = models[i];
            List<Meter> meterlist = meterTypes.GetMetersByModelAndVendorFromMeters(meters, vendor, model);

            try
            {

                MeterNamePicker.ItemsSource = null;

                MeterNamePicker.ItemsSource = meterlist;

                EntriesStackLayout.Children[2].IsVisible = true;
                EntriesStackLayout.Children[1].IsVisible = true;
            }
            catch (Exception e3)
            {
                MeterNamePicker.ItemsSource = null;
                MeterNamePicker.ItemsSource = meterlist;
                EntriesStackLayout.Children[1].IsVisible = false;
                EntriesStackLayout.Children[2].IsVisible = false;
                Console.WriteLine(e3.StackTrace);
            }
        }
    


        private void PickerModelos_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int i = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + i);
            if (i == -1)
                return;


            MeterNamePicker2.ItemDisplayBinding = new Binding("Display");


            model2 = models[i];
            List<Meter> meterlist2 = meterTypes.GetMetersByModelAndVendorFromMeters(meters, vendor2, model2);

            try
            {

                MeterNamePicker2.ItemsSource = null;

                MeterNamePicker2.ItemsSource = meterlist2;

                EntriesStackLayout2.Children[2].IsVisible = true;
                EntriesStackLayout2.Children[1].IsVisible = true;
            }
            catch (Exception e3)
            {
                MeterNamePicker2.ItemsSource = null;
                MeterNamePicker2.ItemsSource = meterlist2;
                EntriesStackLayout2.Children[1].IsVisible = false;
                EntriesStackLayout2.Children[2].IsVisible = false;
                Console.WriteLine(e3.StackTrace);
            }
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

        private void InitializeLowerbarLabel()
        {
            label_read.Text = "Push Button to START";
        }

        private void TappedListeners()
        {
            logout_button.Tapped += LogoutCallAsync;
            settings_button.Tapped += OpenSettingsCallAsync;
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += AddMtu;
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMeterOkTapped;
            replacemeter_cancel.Tapped += ReplaceMeterCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;

            port1label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => port1_command()),
            });
            misclabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => misc_command()),
            });
            port2label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => port2_command()),
            });

            gps_icon_button.Tapped += GPSUpdateButton;

        }

        private void GPSUpdateButton(object sender, EventArgs e)
        {
          
            if(IsLocationAvailable())
            {

                Task.Run(async () => { await StartListening(); });
            }
                    
        }


        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;
          
            CrossGeolocator.Current.DesiredAccuracy = 5;


            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);
            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
            await Task.Delay(5000).ContinueWith(t => StopListening());
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            Console.WriteLine(output);
            //accuracy.Text = output.ToString();

            mtuGeolocationLat.Text = position.Latitude.ToString();
            mtuGeolocationLong.Text = position.Longitude.ToString();

        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
        }

        private async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }

        /**********************************/
        /**      Command functions       **/
        private void misc_command()
        {
            port1label.Opacity = 0.5;
            misclabel.Opacity = 1;
            port2label.Opacity = 0.5;

            port1label.FontSize = 19;
            misclabel.FontSize = 22;
            port2label.FontSize = 19;

            port1view.IsVisible = false;
            port2view.IsVisible = false;
            miscview.IsVisible = true;
        }

        private void port2_command()
        {
            port1label.Opacity = 0.5;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 1;

            port1label.FontSize = 19;
            misclabel.FontSize = 19;
            port2label.FontSize = 22;

            port1view.IsVisible = false;
            port2view.IsVisible = true;
            miscview.IsVisible = false;


        }

        private void port1_command()
        {
            port1label.Opacity = 1;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 0.5;

            port1label.FontSize = 22;
            misclabel.FontSize = 19;
            port2label.FontSize = 19;

            port1view.IsVisible = true;
            port2view.IsVisible = false;
            miscview.IsVisible = false;
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

            Task.Delay(2000).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
             {
                dialog_turnoff_two.IsVisible = false;
                 dialog_turnoff_three.IsVisible = true;
             }));


        }

        void MeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void MeterOkTapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMeter(dialogsSaved), false);
        }

        private async void LogoutCallAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();

            int contador = Navigation.NavigationStack.Count;
            while (contador > 0)
            {
                try
                {
                    await Navigation.PopAsync(false);
                } catch (Exception v){
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);
            }catch (Exception v1){
                Console.WriteLine(v1.StackTrace);
            }

        }

        private void ReturnToMainView(object sender, EventArgs e)
        {
            if(isCancellable){
                Application.Current.MainPage.Navigation.PopAsync(false);
            }else{
                //REASON
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            }
            
           
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
                            OnMenuCaseTurnOFF();
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if(Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
             }));

        }

        private void OnMenuCaseTurnOFF()
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
             }));
        }
    }
}
