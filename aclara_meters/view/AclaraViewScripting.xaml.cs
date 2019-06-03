// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using aclara_meters.Models;
using Library;
using MTUComm;
using nexus.protocols.ble.scan;
using Plugin.Settings;
using Xamarin.Forms;

using Error = Xml.Error;

namespace aclara_meters.view
{
    public partial class AclaraViewScripting
    {
        private const bool DEBUG_MODE_ON = false;

        private List<ReadMTUItem> MTUDataListView { get; set; }
        private List<PageItem> MenuList { get; set; }
        private bool _userTapped;
     //   private IUserDialogs dialogsSaved;
        private Thread printer;
        private ObservableCollection<DeviceItem> listPucks;
        //private String username;

        private string resultCallback;
        private string resultScriptName;

        private string scriptToLaunchData;
        private bool autoConnect;
        private bool bAlertBatt = true;
        private bool bAlertBatt10 = true;

        private bool conectarDevice;

        public AclaraViewScripting()
        {
            InitializeComponent();
        }

        public AclaraViewScripting(string url, string callback, string script_name)
        {
            PrintToConsole($"-------------------------------       AclaraViewScripting, thread: { Thread.CurrentThread.ManagedThreadId}");
            InitializeComponent();

            //CrossSettings.Current.AddOrUpdateValue("session_dynamicpass", string.Empty);

            resultCallback = callback;
            resultScriptName = script_name;

            //FormsApp.credentialsService.SaveCredentials("test", "test");
           
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

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            ContentView_Scripting_label_read.Text = "";

            _userTapped = false;

            TappedListeners();

            //Change username textview to Prefs. String
            //if (FormsApp.credentialsService.UserName != null)
            //{
            //    username = FormsApp.credentialsService.UserName; //"Kartik";
            //}

            ContentView_Scripting_battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            ContentView_Scripting_rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hamburger_icon.Scale = 0;
                    hamburger_icon.IsEnabled = false;
                    ContentView_Scripting_hamburger_icon.Scale = 0;
                    ContentView_Scripting_hamburger_icon.IsEnabled = false;
                });
            });

            scriptToLaunchData = File.ReadAllText(url);

            // Remove temporary script file on root of the public folder
            File.Delete ( url );

            ContentView_Scripting_textScript.Text = "Processing ...";
            ContentView_Scripting_fieldpath_script.Text = url;

            #region New Scripting method is called

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    PrintToConsole("Se va a empezar el flujo");

            //    PrintToConsole("Se va a lanzar una Tarea. Task.Factory.StartNew(Init_Scripting_Method)");

            //    Task.Factory.StartNew(Interface_ContentView_DeviceList);

            //});
            if (scriptToLaunchData.Contains("UploadXML") & Mobile.IsNetAvailable())
            {
                // GenericUtilsClass.UploadFilesTaskSettings();
                this.txtBuscando.Text = "Uploading files...";

                this.UpdateFiles();
               
                return;
            }
            else
            {
                InitRefreshCommand();

                Interface_ContentView_DeviceList();
            }
            #endregion
        }

        public async void UpdateFiles()
        {
            String sMessage;
            // Upload log files
            if (GenericUtilsClass.NumLogFilesToUpload(Mobile.LogPath) > 0)
            {
                bool bUpload = await GenericUtilsClass.UploadFiles(false);
                int numFiles = GenericUtilsClass.NumFilesUploaded;

                sMessage = (bUpload) ? $" ** {numFiles.ToString()} Files uploaded successfully ** " : "Error while uploading files to the FTP server";
            }
            else
                sMessage = "There are not log files to upload";

            Device.OpenUri(new Uri(resultCallback + "?" +
                                           "status=success" +
                                           Compression.GetUriParameter() +
                                           "&output_filename=UploadingFiles" +
                                           "&output_data=" + Compression.CompressToUrlUsingGlobal(sMessage)));

            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
        }
            /*--------------------------------------------------*/
            /*          Device List Interface Contenview
            /---------------------------------------------------*/
        private void InitRefreshCommand()
        {
            DeviceList.RefreshCommand = new Command(async () =>
            {
                PrintToConsole($"----------------------REFRESH command dispositivos encontrados : {FormsApp.ble_interface.GetBlePeripheralList().Count}");
                PrintToConsole($"-------------------------------        REFRESH command, thread: { Thread.CurrentThread.ManagedThreadId}");

                if (!GetAutoConnectStatus())
                {

                    Esperando();

                    if (printer.ThreadState == System.Threading.ThreadState.Suspended)
                    {
                        try
                        {

                            printer.Resume();
                        }
                        catch (Exception e11)
                        {
                            Utils.Print(e11.StackTrace);
                        }
                    }
                    //DeviceList.IsRefreshing = true;
                    listPucks = new ObservableCollection<DeviceItem>();
                    PrintToConsole("comienza el Escaneo de dispositivos - Interface_background_scan_page");
                    //FormsApp.ble_interface.SetTimeOutSeconds(TimeOutSeconds);
                    //bucle infinito hasy a que encuentre al gun puck
                    await FormsApp.ble_interface.Scan();
                    while (FormsApp.ble_interface.GetBlePeripheralList().Count == 0)
                    {
                        await FormsApp.ble_interface.Scan();
                    }
                    //TimeOutSeconds = 3; // los siguientes escaneos son de 5 sec
                    DeviceList.ItemsSource = null;
                    if (FormsApp.ble_interface.GetBlePeripheralList().Count > 0)
                    {

                        //await ChangeListViewData();
                        ChangeListViewData();

                        //DeviceList.IsRefreshing = false;
                        if (listPucks.Count != 0)
                        {
                            DeviceList.ItemsSource = listPucks;
                        }
                        if (conectarDevice)
                        {
                            PairWithKnowDevice();
                        }
                        else
                        {
                            ContentView_DeviceList.IsVisible = true;

                        }
                        
                    }
                    //else
                    //{
                    //    DeviceList.ItemsSource = null;
                    //    Application.Current.MainPage.DisplayAlert("Alert", "No device found, please, press the button to turn on the device and refresh", "Ok");
                    //    Terminado();
                    //}
                    Terminado();
                }
            });

        }

        private void PairWithKnowDevice()
        {

            autoConnect = false;
            conectarDevice = false;
            #region Autoconnect to stored device 

            //PrintToConsole($"-----------------------------------va a conectar con : {Singleton.Get.Puck.Name}");
            //Task.Factory.StartNew(NewOpenConnectionWithDevice);
            NewOpenConnectionWithDevice();
            #endregion

        }

        private void Esperando()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region New Circular Progress bar Animations    
                DeviceList.IsRefreshing = false;
                backdark_bg.IsVisible = false;
                indicator.IsVisible = true;
                txtBuscando.IsVisible = true;
                ContentView_DeviceList.IsEnabled = false;
                ContentView_DeviceList.IsVisible = true;
                #endregion
            });
        }

        private void Terminado()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region Disable Circular Progress bar Animations when done

                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                txtBuscando.IsVisible = false;


                //DeviceList.IsEnabled = true;
                //fondo.Opacity = 1;
                //background_scan_page.Opacity = 1;
                //background_scan_page.IsEnabled = true;
                #endregion
            });
        }

        private void Interface_ContentView_DeviceList()
        {
            PrintToConsole($"-------------------------------    Interface_ContentView_DeviceList, thread: { Thread.CurrentThread.ManagedThreadId}");

            printer = new Thread(new ThreadStart(InvokeMethod));

            printer.Start();

            DeviceList.RefreshCommand.Execute(true);
              
        }
        private void RefreshPuckData(bool Firtstime = false)
        {
            if (!Singleton.Has<Puck>()) return;

            Puck puck = Singleton.Get.Puck;
            int battery;
            string batteryIcon;

            if (Firtstime)
            {
                battery = puck.BatteryLevelFix;
                batteryIcon = puck.BatteryLevelIconFix;
            }
            else
            {
                battery = puck.BatteryLevel;
                batteryIcon = puck.BatteryLevelIcon;
            }

            int rssi = puck.RSSI;
            string rssiIcon = puck.RSSIIcon;


            Device.BeginInvokeOnMainThread(async () =>
            {

                ContentView_Scripting_battery_level.Source = Singleton.Get.Puck.BatteryLevelIconFix + "_white";
                ContentView_Scripting_rssi_level.Source = Singleton.Get.Puck.RSSIIcon + "_white";


                if (battery == 20 && bAlertBatt)
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "The battery level is at 20%", "OK");
                    bAlertBatt = false;
                }
                if (battery == 10 && bAlertBatt10)
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "The battery level is at 10%, soon the puck will turn off", "OK");
                    bAlertBatt10 = false;
                }
            });
        }


        private void InvokeMethod()
        {
            PrintToConsole("dentro del metodo - InvokeMethod");

            int timeout_connecting = 0;
            int cont = 0;
            int refresh = 0;

            PrintToConsole("se va a ejecutar un bucle (WHILE TRUE) - InvokeMethod");
         
            while (true)
            {
                PrintToConsole("dentro del bucle (WHILE TRUE) - InvokeMethod");

                PrintToConsole("buscamos el estado de la conexion - InvokeMethod");

                int status = FormsApp.ble_interface.GetConnectionStatus();

                PrintToConsole("se obtiene el estado de la conexion - InvokeMethod");

                if (cont == 2000)
                {
                    if (refresh == 4)
                    {
                        refresh = 0;
                        bAlertBatt = true;
                        bAlertBatt10 = true;
                    }
                    else refresh += 1;

                    RefreshPuckData();
                    cont = 0;
                }
                else cont += 1;


                if (status != peripheralConnected)
                {
                    Utils.Print($"---------------------------------Invoke method ----estado : {status} , Perifericoconnected: {peripheralConnected}");
                    Utils.Print($"---------------------------------Invoke method ---- Thread: {Thread.CurrentThread.ManagedThreadId}");
                    PrintToConsole("buscamos el estado de la conexion - InvokeMethod");

                    PrintToConsole("¿ES NO_CONNECTED? - InvokeMethod");

                    if (peripheralConnected == ble_library.BlePort.NO_CONNECTED)
                    {
                        peripheralConnected = status;
                        timeout_connecting = 0;
                    }
                    else if (peripheralConnected == ble_library.BlePort.CONNECTING)
                    {
                        PrintToConsole("Nop, es CONNECTING - InvokeMethod");

                        if (status == ble_library.BlePort.NO_CONNECTED)
                        {

                            PrintToConsole("Se va a ejecutar algo en la UI - InvokeMethod");

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PrintToConsole("Se va a detectar el estado de la conexion - InvokeMethod");

                                switch (FormsApp.ble_interface.GetConnectionError())
                                {
                                    case ble_library.BlePort.NO_ERROR:
                                        PrintToConsole("Estado conexion: NO_ERROR - InvokeMethod");
                                        break;
                                    case ble_library.BlePort.CONECTION_ERRROR:
                                        PrintToConsole("Estado conexion: CONECTION_ERRROR - InvokeMethod");


                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            ContentView_DeviceList.IsEnabled = true;

                                            #endregion
                                        });

                                        PrintToConsole("Desactivar barra de progreso - InvokeMethod");

                                        Application.Current.MainPage.DisplayAlert("Alert", "Connection error. Please, retry", "Ok");
                                        break;
                                    case ble_library.BlePort.DYNAMIC_KEY_ERROR:
                                        PrintToConsole("Estado conexion: DYNAMIC_KEY_ERROR - InvokeMethod");

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            ContentView_DeviceList.IsEnabled = true;

                                            #endregion
                                        });

                                        PrintToConsole("Desactivar barra de progreso - InvokeMethod");
                                        Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                        break;
                                    case ble_library.BlePort.NO_DYNAMIC_KEY_ERROR:
                                        PrintToConsole("Estado conexion: NO_DYNAMIC_KEY_ERROR - InvokeMethod");

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            ContentView_DeviceList.IsEnabled = true;

                                            #endregion

                                        });
                                        PrintToConsole("Desactivar barra de progreso - InvokeMethod");
                                        Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                        break;
                                }
                                DeviceList.IsEnabled = true;
                                fondo.Opacity = 1;
                                ContentView_DeviceList.Opacity = 1;
                                ContentView_DeviceList.IsEnabled = true;

                               

                            });
                            peripheralConnected = status;
                            Singleton.Remove<Puck>();

                            bAlertBatt = true;
                            bAlertBatt10 = true;
                        }
                        else
                        {
                            Utils.Print($"---------------------------------Invoke method ----estado : {status} , Conectado");

                            PrintToConsole("Estas Conectado - InvokeMethod");

                            DeviceList.IsEnabled = true;

                            peripheralConnected = status;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                fondo.Opacity = 1;
                                ContentView_DeviceList.Opacity = 1;
                                ContentView_DeviceList.IsEnabled = true;

                                //Utils.Print("CONNECTED");

                                ContentView_Scripting.IsVisible = true;
                                ContentView_DeviceList.IsVisible = false;


                                try
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        // Minor bug fix in case of missing battery data
                                        try
                                        {
                                            String icono_bateria;
                                            string rssiIcono;
                                            Puck puck = Singleton.Get.Puck;
                                            icono_bateria = puck.BatteryLevelIconFix;
                                            rssiIcono = puck.RSSIIcon;
                                                                                   
                                            ContentView_Scripting_battery_level.Source = icono_bateria + "_white";
                                            ContentView_Scripting_rssi_level.Source = rssiIcono + "_white";

                                        }
                                        catch (Exception e)
                                        {
                                            Utils.Print(e.StackTrace);
                                        }
                                                                            

                                    });

                                }
                                catch (Exception e)
                                {
                                    Utils.Print(e.StackTrace);
                                }

                                PrintToConsole("Se va a ejecutar el Script - InvokeMethod");

                                try
                                {
                                    printer.Abort();
                                }
                                catch (Exception e5)
                                {
                                    Utils.Print(e5.StackTrace);
                                }

                                //Connection Method
                                runScript();

                            });
                        }
                    }
                    else if (peripheralConnected == ble_library.BlePort.CONNECTED)
                    {
                        PrintToConsole("Nop, es CONNECTED - InvokeMethod");

                        DeviceList.IsEnabled = true;

                        peripheralConnected = status;
                        Singleton.Remove<Puck>();
                        bAlertBatt = true;
                        bAlertBatt10 = true;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            fondo.Opacity = 1;
                            ContentView_DeviceList.Opacity = 1;
                            ContentView_DeviceList.IsEnabled = true;

                            //Utils.Print("NOT CONNECTED");
                            ContentView_Scripting.IsVisible = false;
                            ContentView_DeviceList.IsVisible = true;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                        });

                        //Application.Current.MainPage.DisplayAlert("Alert", "The puck has disconnected", "Ok");
                        //System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }

                }

                PrintToConsole("¿Está en CONNECTING? - InvokeMethod");

                if (peripheralConnected == ble_library.BlePort.CONNECTING)
                {
                    PrintToConsole("Si, es CONNECTING - InvokeMethod");
                    timeout_connecting++;
                    if (timeout_connecting >= 2 * 10) // 10 seconds
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PrintToConsole("Un Timeout que te llevas - InvokeMethod");
                            Application.Current.MainPage.DisplayAlert("Timeout", "Connection Timeout", "Ok");
                            DeviceList.IsEnabled = true;
                            fondo.Opacity = 1;
                            ContentView_DeviceList.Opacity = 1;
                            ContentView_DeviceList.IsEnabled = true;

                            autoConnect = false;

                            #region Disable Circular Progress bar Animations when done

                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            ContentView_DeviceList.IsEnabled = true;

                            #endregion


                            try
                            {
                                printer.Suspend();
                            }
                            catch (Exception e5)
                            {
                                Utils.Print(e5.StackTrace);
                            }

                        });
                        peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                        timeout_connecting = 0;

                        PrintToConsole("Cerrar Conexion - InvokeMethod");
                        //FormsApp.ble_interface.Close();
                    }
                }
                else
                {
                    PrintToConsole("Nop, no es CONNECTING - InvokeMethod");
                }

                PrintToConsole("Esperamos 300 ms - InvokeMethod");
                Thread.Sleep(300); // 0.5 Second

                PrintToConsole("¿Se va a realizar reconexion? - InvokeMethod");

            }

        }

        private void runScript()
        {
            Task.Run(async () =>
            {

                PrintToConsole("runScript Tarea Asincrona - InvokeMethod");
                await Task.Delay(150); 
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PrintToConsole("Mostrar barra de progreso - InvokeMethod");

                            backdark_bg.IsVisible = true;
                            indicator.IsVisible = true;
                            ContentView_DeviceList.IsEnabled = false;
                            ContentView_Scripting.IsEnabled = false;
                            _userTapped = true;
                            ContentView_Scripting_label_read.Text = "Executing Script ... ";
                        });


                        Device.BeginInvokeOnMainThread(() =>
                        {

                            Task.Factory.StartNew(scriptFunction);
                        });

                    }
                    catch (Exception e2)
                    {
                        Utils.Print(e2.StackTrace);

                    }

                });
            });

        }

        private bool GetAutoConnectStatus()
        {
            return autoConnect;
        }

        private void scriptFunction()
        {
            ScriptRunner runner = new ScriptRunner ();

            //Define finish and error event handler
            runner.OnFinish     += OnFinish;
            runner.OnProgress   += OnProgress;
            runner.onStepFinish += onStepFinish;
            runner.OnError      += OnError;

            runner.ParseScriptAndRun ( FormsApp.ble_interface, scriptToLaunchData, scriptToLaunchData.Length );
        }

        //private async Task ChangeListViewData()
        private void ChangeListViewData()
        {
           // await Task.Factory.StartNew(() =>
           // {
                // wait until scan finish
              //  while (FormsApp.ble_interface.IsScanning())
              //  {
                    try
                    {
                        List<IBlePeripheral> blePeripherals;
                        blePeripherals = FormsApp.ble_interface.GetBlePeripheralList();


                        byte[] bytes = System.Convert.FromBase64String(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty));

                        byte[] byte_now = new byte[] { };

                        int sizeList = blePeripherals.Count;

                        for (int i = 0; i < sizeList; i++)
                        {
                            try
                            {
                                if (blePeripherals[i] != null)
                                {
                                    Puck puck = new Puck(blePeripherals[i]);
                                    //puck.BlInterfaz= FormsApp.ble_interface;

                                    byte_now = puck.ManofacturerData;

                                    bool enc = false;
                                    int sizeListTemp = listPucks.Count;

                                    for (int j = 0; j < sizeListTemp; j++)
                                    {
                                        if (listPucks[j].Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()
                                            .SequenceEqual(puck.ManofacturerData))
                                        {
                                            enc = true;
                                        }
                                    }

                                    string icono_bateria;

                                    if (!enc)
                                    {
                                        int bateria = puck.BatteryLevel;
                                        string iconBattery = puck.BatteryLevelIcon;

                                        int rssi = puck.RSSI;
                                        string iconRSSI = puck.RSSIIcon;

                                        DeviceItem device = new DeviceItem
                                        {
                                            deviceMacAddress = puck.SerialNumber,
                                            deviceName = puck.Name,
                                            deviceBattery = bateria + "%",
                                            deviceRssi = rssi + " dBm",
                                            deviceBatteryIcon = iconBattery,
                                            deviceRssiIcon = iconRSSI,
                                            Peripheral = puck.Device
                                        };


                                        listPucks.Add(device);

                                      

                                        if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                                            bytes.Take(4).ToArray().SequenceEqual(byte_now) &&
                                            blePeripherals[i].Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)) &&
                                            !peripheralManualDisconnection &&
                                            !Singleton.Has<Puck>())

                                        {
                                            if (!FormsApp.ble_interface.IsOpen())
                                            {
                                                try
                                                {

                                                    #region Autoconnect to stored device 
    
                                                    Singleton.Set = new Puck();
                                                    Singleton.Get.Puck.Device = blePeripherals[i];
                                                    Singleton.Get.Puck.BlInterfaz = FormsApp.ble_interface;
                                                    peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                                                    peripheralManualDisconnection = false;

                                                    conectarDevice = true;

                                                    autoConnect = true;

                                                    #endregion

                                                }
                                                catch (Exception e)
                                                {
                                                    Utils.Print(e.StackTrace);
                                                }

                                            }
                                            else
                                            {

                                                if (autoConnect)
                                                {

                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        #region Disable Circular Progress bar Animations when done

                                                        backdark_bg.IsVisible = false;
                                                        indicator.IsVisible = false;
                                                        ContentView_DeviceList.IsEnabled = true;

                                                        #endregion
                                                    });

                                                }

                                            }
                                        }
                                        else
                                        {

                                            // if (autoConnect)
                                            //  {

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                #region Disable Circular Progress bar Animations when done

                                                DeviceList.IsRefreshing = false;
                                                backdark_bg.IsVisible = false;
                                                indicator.IsVisible = false;
                                                ContentView_DeviceList.IsEnabled = true;

                                                #endregion

                                            });

                                            //  }

                                        }


                                    }
                                }
                            }
                            catch (Exception er)
                            {

                                Utils.Print(er.StackTrace); //2018-09-21 13:08:25.918 aclara_meters.iOS[505:190980] System.NullReferenceException: Object reference not set to an instance of an object
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Utils.Print(e);
                    }
             //   }
            //});
        }

        #region We want to connect to the device if there is not scanning running
        
        private void NewOpenConnectionWithDevice()
        {

            PrintToConsole("Se va a entrar en un bucle mientras esté Escaneando bluetooth - NewOpenConnectionWithDevice");

            while (FormsApp.ble_interface.IsScanning())
            {
                PrintToConsole("A esperar 100 ms mientras escanea... - NewOpenConnectionWithDevice");
                Thread.Sleep(100);
            }

            PrintToConsole("Se va a ejecutar algo en el UI - NewOpenConnectionWithDevice");

            Device.BeginInvokeOnMainThread(() =>
            {
                var seconds = TimeSpan.FromSeconds(1); // Don't execute it asap!

                Device.StartTimer(seconds, () =>
                {
                    PrintToConsole("Cada 1 segundo, se ejectua lo siguinete en el UI - NewOpenConnectionWithDevice");
                    Device.BeginInvokeOnMainThread(() =>
                       {

                       PrintToConsole("¿Esta la conexion abierta ? - NewOpenConnectionWithDevice");


                       if (!FormsApp.ble_interface.IsOpen())
                       {
                            PrintToConsole("¿Esta escaneando perifericos ? - NewOpenConnectionWithDevice");
                            while (FormsApp.ble_interface.IsScanning())
                            {
                                PrintToConsole("A esperar 100 ms en bucle - NewOpenConnectionWithDevice");
                                Thread.Sleep(100);
                            }

                            // call your method to check for notifications here
                            FormsApp.ble_interface.Open(Singleton.Get.Puck.Device, true);
                       }
                       else
                       {
                           PrintToConsole("NOPE, no lo esta - NewOpenConnectionWithDevice");
                       }
                   });

                    return false;
                });
            });
        }

        #endregion
        
        private void OnError ()
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Error error = Errors.LastError;
                
                    ContentView_Scripting_textScript.Text = "Error code: " + error.Id + "\n" + error.Message;

                    Utils.Print ( "[ Scripting ] " + ContentView_Scripting_textScript.Text + ( ( error.HasMessagePopup ) ? " | " + error.MessagePopup : string.Empty ) );

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        ContentView_Scripting.IsEnabled = true;
                        _userTapped = false;
                        ContentView_Scripting_label_read.Text = "Script Execution Error";
                                            
                        Device.OpenUri ( new Uri ( resultCallback + "?" +
                                                   "status=error" +
                                                   Compression.GetUriParameter () +
                                                   "&message=" + Compression.CompressToUrlUsingGlobal ( "Error code: " + error.Id + "\n" + error.MessagePopup ) +
                                                   "&output_filename=" + resultScriptName +
                                                   "&output_data=" + Compression.CompressToUrlUsingGlobal ( Errors.lastErrorLogGenerated ) ) );

                        FormsApp.ble_interface.Close ();

                        // Close the app
                        System.Diagnostics.Process.GetCurrentProcess ().Kill ();
                    });
                });
            });
        }

        public string GZipCompress ( string input )
        {
            using ( var outStream = new MemoryStream () )
            {
                using ( var gzipStream = new GZipStream ( outStream, CompressionMode.Compress ) )
                {
                    using (var ms = new MemoryStream ( Encoding.UTF8.GetBytes ( input ) ) )
                    {
                        ms.CopyTo ( gzipStream );
                    }
                }
    
                return Encoding.UTF8.GetString ( outStream.ToArray() );
            }
        }

        private void onStepFinish(object sender, int step, MTUComm.Action.ActionFinishArgs e)
        {
            Utils.Print("HI FINISH RUNNER");
            //throw new NotImplementedException();
        }

        private void OnProgress ( object sender, MTUComm.Action.ActionProgressArgs e )
        {
            string mensaje = e.Message;

            Device.BeginInvokeOnMainThread(() =>
            {
                if ( ! string.IsNullOrEmpty ( mensaje ) )
                    ContentView_Scripting_label_read.Text = mensaje;
            });
        }

        private void OnFinish(object sender, MTUComm.Action.ActionFinishArgs e)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        ContentView_Scripting_textScript.Text = "";
                        Parameter[] allParams = e.Result.getParameters();
    
                        for (int k = 0; k < allParams.Length; k++)
                        {
                            String res = allParams[k].getLogDisplay() + ": " + allParams[k].Value;
                            String val = ContentView_Scripting_textScript.Text;
                            ContentView_Scripting_textScript.Text = val + res + "\r\n";
                        }
    
                        ActionResult[] allports = e.Result.getPorts();
    
                        for (int i = 0; i < allports.Length; i++)
                        {
                            ActionResult actionResult = allports[i];
                            Parameter[] portParams = actionResult.getParameters();
    
                            for (int j = 0; j < portParams.Length; j++)
                            {
                                String res = portParams[j].getLogDisplay() + ": " + portParams[j].Value;
                                String val = ContentView_Scripting_textScript.Text;
                                ContentView_Scripting_textScript.Text = val + res + "\r\n";
                            }
                        }
    
                        string xmlResultTocallback = ((MTUComm.Action)sender).GetResultXML(e.Result);
    
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            ContentView_Scripting.IsEnabled = true;
                            _userTapped = false;
                            ContentView_Scripting_label_read.Text = "Successful Script Execution";
    
                            Device.OpenUri ( new Uri ( resultCallback + "?" +
                                                       "status=success" +
                                                       Compression.GetUriParameter () +
                                                       "&output_filename=" + resultScriptName +
                                                       "&output_data=" + Compression.CompressToUrlUsingGlobal ( xmlResultTocallback ) ) );
                            
                            FormsApp.ble_interface.Close();
                            
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                        });
    
                    }
                    catch ( Exception ex )
                    {
                        
                    }
                });
            });
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private void TappedListeners()
        {
            ContentView_Scripting_bg_read_mtu_button_img.Scale = 0;
        }

        //private async void PickFilesCommandHandler()
        //{
        //    string resultMsg = "Successful MTU read";


        //    Task.Delay(100).ContinueWith(t =>
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        ContentView_Scripting_label_read.Text = resultMsg;
        //        _userTapped = false;
        //        ContentView_Scripting_bg_read_mtu_button.NumberOfTapsRequired = 1;
        //        backdark_bg.IsVisible = false;
        //        indicator.IsVisible = false;
        //        ContentView_Scripting.IsEnabled = true;


        //    }));


        //}

        private void LoadPhoneUI()
        {
            ContentView_Scripting.Margin = new Thickness(0, 0, 0, 0);
            ContentView_Scripting_hamburger_icon.IsVisible = true;

        }

        private void LoadTabletUI()
        {

            ContentView_Scripting.Opacity = 1;

            ContentView_Scripting_hamburger_icon.IsVisible = false;
            ContentView_Scripting.Margin = new Thickness(0, 0, 0, 0);



            ContentView_Scripting_aclara_logo.Scale = 1.2;
            ContentView_Scripting_aclara_logo.TranslationX = 42;
            ContentView_Scripting_hamburger_icon.TranslationX = 42;
        }
         
 
        //private IBlePeripheral peripheral = null;
        private int peripheralConnected = ble_library.BlePort.NO_CONNECTED;
        private Boolean peripheralManualDisconnection = false;

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelectedListDevices(object sender, ItemTappedEventArgs e)
        {
            PrintToConsole($"-------------------------------       OnMenuItemSelectedListDevices, thread: { Thread.CurrentThread.ManagedThreadId}");
            try
            {
                printer.Resume();
            }
            catch (Exception e5)
            {
                Utils.Print(e5.StackTrace);
            }
           

            var item = (DeviceItem)e.Item;
            //fondo.Opacity = 0;
            ContentView_DeviceList.Opacity = 0.5;
            ContentView_DeviceList.IsEnabled = false;

            Device.BeginInvokeOnMainThread(() =>
            {

                #region New Circular Progress bar Animations    

                DeviceList.IsRefreshing = false;
                backdark_bg.IsVisible = true;
                indicator.IsVisible = true;
                ContentView_DeviceList.IsEnabled = false;
                #endregion
            });

            bool reassociate = false;
            if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
              //  FormsApp.credentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                System.Convert.ToBase64String(item.Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray())
                .Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty)) &&
                item.Peripheral.Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)))
            {
                reassociate = true;
            }


            try
            {
                Singleton.Set = new Puck();
                Singleton.Get.Puck.Device = item.Peripheral;
                FormsApp.ble_interface.Open(Singleton.Get.Puck.Device, reassociate);
                Singleton.Get.Puck.BlInterfaz = FormsApp.ble_interface;
                bAlertBatt = true;
                bAlertBatt10 = true;

                Device.BeginInvokeOnMainThread(() =>
                {
                    ContentView_Scripting_battery_level.Source = Singleton.Get.Puck.BatteryLevelIconFix + "_white";
                    ContentView_Scripting_rssi_level.Source = Singleton.Get.Puck.RSSIIcon + "_white";
                });
            }
            catch (Exception e22)
            {
                Utils.Print(e22.StackTrace);
            }


        }

        public void PrintToConsole(string printConsole)
        {
            if(DEBUG_MODE_ON)
                Utils.Print("DEBUG_ACL: " + printConsole);
        }
    }
}
