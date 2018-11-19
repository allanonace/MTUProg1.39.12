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

namespace aclara_meters.view
{
    public partial class AclaraViewReplaceMTUReplaceMeter
    {
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


        private enum Names
        {
            Name1 = 0,
            Name2 = 1,
            Name3 = 2,
            Name4 = 3,
            Name5 = 4,
            Name6 = 5,
            Name7 = 6,
            Name8 = 7,
            Name9 = 8
        };

        public AclaraViewReplaceMTUReplaceMeter()
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

                InitPickerOldMeterWorking2();

                InitPickerReplaceMeterRegister2();
            }
            else
            {
                port2label.IsVisible = false;
            }

            //
            InitPickerOldMeterWorking();
            InitPickerReplaceMeterRegister();

        }





        private void InitPickerOldMeterWorking2()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Yes" },
                new PickerItems { Name = "No" },
                new PickerItems { Name = "Broken" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerOldMeterWorking2.ItemsSource = objStringList;
        }

        private void InitPickerReplaceMeterRegister2()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Meter" },
                new PickerItems { Name = "Register" },
                new PickerItems { Name = "Both" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReplaceMeterRegister2.ItemsSource = objStringList;
        }









        private void InitPickerOldMeterWorking()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Yes" },
                new PickerItems { Name = "No" },
                new PickerItems { Name = "Broken" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerOldMeterWorking.ItemsSource = objStringList;
        }

        private void InitPickerReplaceMeterRegister()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Meter" },
                new PickerItems { Name = "Register" },
                new PickerItems { Name = "Both" }
            };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReplaceMeterRegister.ItemsSource = objStringList;
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
                new PickerItems { Name = "Moderate" },
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
                new PickerItems { Name = "Moderate" },
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
                bg_read_mtu_button_img.Source = "rep_mtu_rep_meter_btn_black.png";

            }
            else
            {
                bg_read_mtu_button_img.Source = "rep_mtu_rep_meter_btn.png";
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
                    label_read.Text = "Writing to MTU ... ";
                    _userTapped = true;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);
                });

                Task.Delay(1000).ContinueWith(t =>
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     label_read.Text = "Writing to MTU ... 3 sec";
                 }));

                Task.Delay(2000).ContinueWith(t =>
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     label_read.Text = "Writing to MTU ... 2 sec";
                 }));


                Task.Delay(3000).ContinueWith(t =>
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     label_read.Text = "Writing to MTU ... 1 sec";
                 }));


                Task.Delay(4000).ContinueWith(t =>
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     _userTapped = false;
                     bg_read_mtu_button.NumberOfTapsRequired = 1;
                     ChangeLowerButtonImage(false);
                     backdark_bg.IsVisible = false;
                     indicator.IsVisible = false;
                     label_read.Text = "Successful MTU write";
                     background_scan_page.IsEnabled = true;
                 }));
            }
        }



        public AclaraViewReplaceMTUReplaceMeter(IUserDialogs dialogs)
        {
            InitializeComponent();

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
        }

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

            //Listado de los Selectores
            picker_List_Vendor_port1 = new List<string>();

            picker_List_Vendor_port1.Add("Vendor 1");
            picker_List_Vendor_port1.Add("Vendor 2");
            picker_List_Vendor_port1.Add("Vendor 3");

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
            BorderlessPicker picker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Vendor_port1
            };

            //Detectar el Selector clickado
            picker.SelectedIndexChanged += PickerMarcas_SelectedIndexChanged;


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


            st_vendor.Children.Add(picker);
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

            picker_List_Model_port1.Add("Vendor 1 Model 1");
            picker_List_Model_port1.Add("Vendor 1 Model 2");
            picker_List_Model_port1.Add("Vendor 1 Model 3");
            picker_List_Model_port1.Add("Vendor 1 Model 4");

            picker_List_Model_port1.Add("Vendor 2 Model 1");
            picker_List_Model_port1.Add("Vendor 2 Model 2");
            picker_List_Model_port1.Add("Vendor 2 Model 3");
            picker_List_Model_port1.Add("Vendor 2 Model 4");

            picker_List_Model_port1.Add("Vendor 3 Model 1");
            picker_List_Model_port1.Add("Vendor 3 Model 2");
            picker_List_Model_port1.Add("Vendor 3 Model 3");
            picker_List_Model_port1.Add("Vendor 3 Model 4");



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
            BorderlessPicker pickerModelos = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Model_port1,
                StyleId = "pickerModelos"
            };

            //Detectar el Selector clickado
            pickerModelos.SelectedIndexChanged += PickerModelos_SelectedIndexChanged;

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


            st_model.Children.Add(pickerModelos);
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

            picker_List_Name_port1.Add("Name 1");
            picker_List_Name_port1.Add("Name 2");
            picker_List_Name_port1.Add("Name 3");
            picker_List_Name_port1.Add("Name 4");
            picker_List_Name_port1.Add("Name 5");
            picker_List_Name_port1.Add("Name 6");
            picker_List_Name_port1.Add("Name 7");
            picker_List_Name_port1.Add("Name 8");
            picker_List_Name_port1.Add("Name 9");



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
            BorderlessPicker pickerColor = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Name_port1,
                StyleId = "pickerColor"
            };

            //Detectar el Selector clickado
            pickerColor.SelectedIndexChanged += PickerColor_SelectedIndexChanged;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueColor = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            //Texto del titulo
            Label textoTituloColor = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };




            st_name.Children.Add(pickerColor);
            fm2_name.Content = st_name;
            fm1_name.Content = fm2_name;



            //Añadimos el titulo y el selector al bloque
            ElementoBloqueColor.Children.Add(textoTituloColor);
            ElementoBloqueColor.Children.Add(fm1_name);

            //Introducimos el bloque en la vista
            EntriesStackLayout.Children.Add(ElementoBloqueColor);

            ElementoBloqueColor.IsVisible = false;
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

            //Listado de los Selectores
            picker_List_Vendor_port2 = new List<string>();

            picker_List_Vendor_port2.Add("Vendor 1");
            picker_List_Vendor_port2.Add("Vendor 2");
            picker_List_Vendor_port2.Add("Vendor 3");

            Frame fm1_vendor2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_vendor2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_vendor2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            // Generamos el Selector
            BorderlessPicker picker2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Vendor_port2
            };

            //Detectar el Selector clickado
            picker2.SelectedIndexChanged += PickerMarcas_SelectedIndexChanged2;


            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloque2 = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            //Texto del titulo
            Label textoTitulo2 = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            //Añadimos el titulo y el selector al bloque

            //Texto del titulo
            Label textoTituloCamposGrandes2 = new Label()
            {
                Text = "Vistas de Colección",
                Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),
                IsVisible = false,
                Margin = new Thickness(0, 8, 0, 0)

            };


            st_vendor2.Children.Add(picker2);
            fm2_vendor2.Content = st_vendor2;
            fm1_vendor2.Content = fm2_vendor2;

            ElementoBloque2.Children.Add(textoTituloCamposGrandes2);
            ElementoBloque2.Children.Add(textoTitulo2);

            // Picker to    fm1_vendor --> fm2_vendor --> st_vendor --> picker
            ElementoBloque2.Children.Add(fm1_vendor2);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloque2);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  MODELO  [1]          **/
            //Listado de los Selectores
            picker_List_Model_port2 = new List<string>();

            picker_List_Model_port2.Add("Vendor 1 Model 1");
            picker_List_Model_port2.Add("Vendor 1 Model 2");
            picker_List_Model_port2.Add("Vendor 1 Model 3");
            picker_List_Model_port2.Add("Vendor 1 Model 4");

            picker_List_Model_port2.Add("Vendor 2 Model 1");
            picker_List_Model_port2.Add("Vendor 2 Model 2");
            picker_List_Model_port2.Add("Vendor 2 Model 3");
            picker_List_Model_port2.Add("Vendor 2 Model 4");

            picker_List_Model_port2.Add("Vendor 3 Model 1");
            picker_List_Model_port2.Add("Vendor 3 Model 2");
            picker_List_Model_port2.Add("Vendor 3 Model 3");
            picker_List_Model_port2.Add("Vendor 3 Model 4");



            Frame fm1_model2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_model2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_model2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };



            // Generamos el Selector
            BorderlessPicker pickerModelos2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Model_port2,
                StyleId = "pickerModelos"
            };

            //Detectar el Selector clickado
            pickerModelos2.SelectedIndexChanged += PickerModelos_SelectedIndexChanged2;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueModelo2 = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            //Texto del titulo
            Label textoTituloModelo2 = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            st_model2.Children.Add(pickerModelos2);
            fm2_model2.Content = st_model2;
            fm1_model2.Content = fm2_model2;


            //Añadimos el titulo y el selector al bloque
            ElementoBloqueModelo2.Children.Add(textoTituloModelo2);
            ElementoBloqueModelo2.Children.Add(fm1_model2);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloqueModelo2);

            /*******************************************//*******************************************//*******************************************/
            /*******************************************/
            /**                  COLOR  [2]          **/
            //Listado de los Selectores
            picker_List_Name_port2 = new List<string>();

            picker_List_Name_port2.Add("Name 1");
            picker_List_Name_port2.Add("Name 2");
            picker_List_Name_port2.Add("Name 3");
            picker_List_Name_port2.Add("Name 4");
            picker_List_Name_port2.Add("Name 5");
            picker_List_Name_port2.Add("Name 6");
            picker_List_Name_port2.Add("Name 7");
            picker_List_Name_port2.Add("Name 8");
            picker_List_Name_port2.Add("Name 9");



            Frame fm1_name2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };


            Frame fm2_name2 = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout st_name2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };



            // Generamos el Selector
            BorderlessPicker pickerColor2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = picker_List_Name_port2,
                StyleId = "pickerColor"
            };

            //Detectar el Selector clickado
            pickerColor2.SelectedIndexChanged += PickerColor_SelectedIndexChanged2;

            //Creamos el Bloque con toda la informacion
            StackLayout ElementoBloqueColor2 = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            //Texto del titulo
            Label textoTituloColor2 = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };




            st_name2.Children.Add(pickerColor2);
            fm2_name2.Content = st_name2;
            fm1_name2.Content = fm2_name2;



            //Añadimos el titulo y el selector al bloque
            ElementoBloqueColor2.Children.Add(textoTituloColor2);
            ElementoBloqueColor2.Children.Add(fm1_name2);

            //Introducimos el bloque en la vista
            EntriesStackLayout2.Children.Add(ElementoBloqueColor2);

            ElementoBloqueColor2.IsVisible = false;
            ElementoBloqueModelo2.IsVisible = false;



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

            List<string> filter_result = new List<string>();

            StackLayout bloque2 = (StackLayout)EntriesStackLayout.Children[1];


            Frame tempframeMarca = (Frame)bloque2.Children[1];
            Frame tempFrame2Marca = (Frame)tempframeMarca.Content;
            StackLayout tempStackMarca = (StackLayout)tempFrame2Marca.Content;


            BorderlessPicker PickerToModify = (BorderlessPicker)tempStackMarca.Children[0];

            if (j != -1)
            {
                int i = 0;

                int cuantosProcesar = picker_List_Model_port1.Count - 1;
                switch (j)
                {
                    case 0:
                        Console.WriteLine("Vendor 1 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port1[i].Contains("Vendor 1"))
                            {
                                filter_result.Add(picker_List_Model_port1[i]);
                            }
                        }

                        break;

                    case 1:
                        Console.WriteLine("Vendor 2 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port1[i].Contains("Vendor 2"))
                            {
                                filter_result.Add(picker_List_Model_port1[i]);
                            }
                        }

                        break;

                    case 2:
                        Console.WriteLine("Vendor 3 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port1[i].Contains("Vendor 3"))
                            {
                                filter_result.Add(picker_List_Model_port1[i]);
                            }
                        }

                        break;

                }

                try
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout.Children[1].IsVisible = true;
                    EntriesStackLayout.Children[2].IsVisible = false;
                }
                catch (Exception e3)
                {
                    EntriesStackLayout.Children[1].IsVisible = false;
                    EntriesStackLayout.Children[2].IsVisible = false;
                    Console.WriteLine(e3.StackTrace);
                }
            }
        }



        private void PickerMarcas_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            List<string> filter_result = new List<string>();

            StackLayout bloque2 = (StackLayout)EntriesStackLayout2.Children[1];


            Frame tempframeMarca = (Frame)bloque2.Children[1];
            Frame tempFrame2Marca = (Frame)tempframeMarca.Content;
            StackLayout tempStackMarca = (StackLayout)tempFrame2Marca.Content;


            BorderlessPicker PickerToModify = (BorderlessPicker)tempStackMarca.Children[0];

            if (j != -1)
            {
                int i = 0;

                int cuantosProcesar = picker_List_Model_port2.Count - 1;
                switch (j)
                {
                    case 0:
                        Console.WriteLine("Vendor 1 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port2[i].Contains("Vendor 1"))
                            {
                                filter_result.Add(picker_List_Model_port2[i]);
                            }
                        }

                        break;

                    case 1:
                        Console.WriteLine("Vendor 2 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port2[i].Contains("Vendor 2"))
                            {
                                filter_result.Add(picker_List_Model_port2[i]);
                            }
                        }

                        break;

                    case 2:
                        Console.WriteLine("Vendor 3 Selected");
                        for (i = 0; i < cuantosProcesar; i++)
                        {
                            if (picker_List_Model_port2[i].Contains("Vendor 3"))
                            {
                                filter_result.Add(picker_List_Model_port2[i]);
                            }
                        }

                        break;

                }

                try
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout2.Children[1].IsVisible = true;
                    EntriesStackLayout2.Children[2].IsVisible = false;
                }
                catch (Exception e3)
                {
                    EntriesStackLayout2.Children[1].IsVisible = false;
                    EntriesStackLayout2.Children[2].IsVisible = false;
                    Console.WriteLine(e3.StackTrace);
                }
            }
        }





        private void PickerColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            List<string> itemsColores = (List<string>)((BorderlessPicker)sender).ItemsSource;



            try
            {
                Console.WriteLine(itemsColores[j] + " Selected");
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2.StackTrace);
            }

        }


        private void PickerColor_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + j);

            List<string> itemsColores = (List<string>)((BorderlessPicker)sender).ItemsSource;



            try
            {
                Console.WriteLine(itemsColores[j] + " Selected");
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

            List<string> filter_result = new List<string>();

            StackLayout bloque2 = (StackLayout)EntriesStackLayout.Children[2];

            Frame tempframeMarca = (Frame)bloque2.Children[1];
            Frame tempFrame2Marca = (Frame)tempframeMarca.Content;
            StackLayout tempStackMarca = (StackLayout)tempFrame2Marca.Content;


            BorderlessPicker PickerToModify = (BorderlessPicker)tempStackMarca.Children[0];

            List<string> valores = (List<string>)((BorderlessPicker)sender).ItemsSource;

            if (i != -1)
            {
                if (valores[0].Contains("Vendor 1"))
                {
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name1]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name2]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name3]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name4]);
                }

                if (valores[0].Contains("Vendor 2"))
                {
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name3]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name4]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name5]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name6]);
                }

                if (valores[0].Contains("Vendor 3"))
                {
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name6]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name7]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name8]);
                    filter_result.Add(picker_List_Name_port1[(int)Names.Name9]);
                }

                try
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout.Children[2].IsVisible = true;
                    EntriesStackLayout.Children[1].IsVisible = true;
                }
                catch (Exception e3)
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout.Children[1].IsVisible = false;
                    EntriesStackLayout.Children[2].IsVisible = false;
                    Console.WriteLine(e3.StackTrace);
                }
            }

        }





        private void PickerModelos_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int i = ((BorderlessPicker)sender).SelectedIndex;
            Console.WriteLine("Elemento Picker : " + i);

            List<string> filter_result = new List<string>();

            StackLayout bloque2 = (StackLayout)EntriesStackLayout2.Children[2];

            Frame tempframeMarca = (Frame)bloque2.Children[1];
            Frame tempFrame2Marca = (Frame)tempframeMarca.Content;
            StackLayout tempStackMarca = (StackLayout)tempFrame2Marca.Content;


            BorderlessPicker PickerToModify = (BorderlessPicker)tempStackMarca.Children[0];

            List<string> valores = (List<string>)((BorderlessPicker)sender).ItemsSource;

            if (i != -1)
            {
                if (valores[0].Contains("Vendor 1"))
                {
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name1]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name2]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name3]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name4]);
                }

                if (valores[0].Contains("Vendor 2"))
                {
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name3]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name4]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name5]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name6]);
                }

                if (valores[0].Contains("Vendor 3"))
                {
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name6]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name7]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name8]);
                    filter_result.Add(picker_List_Name_port2[(int)Names.Name9]);
                }

                try
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout2.Children[2].IsVisible = true;
                    EntriesStackLayout2.Children[1].IsVisible = true;
                }
                catch (Exception e3)
                {
                    PickerToModify.ItemsSource = filter_result;
                    EntriesStackLayout2.Children[1].IsVisible = false;
                    EntriesStackLayout2.Children[2].IsVisible = false;
                    Console.WriteLine(e3.StackTrace);
                }
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
            bg_read_mtu_button.Tapped += ReadMTU;
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

            if (IsLocationAvailable())
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
