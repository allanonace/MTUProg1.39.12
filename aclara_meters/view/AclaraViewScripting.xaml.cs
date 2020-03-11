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

using Error = Library.Error;

namespace aclara_meters.view
{
    public partial class AclaraViewScripting
    {
        
        private Thread connectionThread;
        private ObservableCollection<DeviceItem> listPucks;
       

        private string resultCallback;
        private string resultScriptName;

        private string scriptToLaunchData;
        private bool autoConnect;
        private bool bAlertBatt = true;
        private bool bAlertBatt10 = true;

        private bool conectarDevice;

        private int peripheralConnected = ble_library.BlePort.NO_CONNECTED;
        private Boolean peripheralManualDisconnection = false;

        public AclaraViewScripting()
        {
            InitializeComponent();
        }

        public AclaraViewScripting(string url, string callback, string script_name)
        {
            Utils.PrintDeep($"-------------------------------       AclaraViewScripting, thread: { Thread.CurrentThread.ManagedThreadId}");
            InitializeComponent();

            resultCallback = callback;
            resultScriptName = script_name;
      
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
            // If action is UploadXML it is not necesary the connection to puck
            if (scriptToLaunchData.Contains("UploadXML") && Mobile.IsNetAvailable())
            {
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

        public async Task UpdateFiles()
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
                Utils.PrintDeep($"----------------------REFRESH command dispositivos encontrados : {FormsApp.ble_interface.GetBlePeripheralList().Count}");
                Utils.PrintDeep($"-------------------------------        REFRESH command, thread: { Thread.CurrentThread.ManagedThreadId}");

                if (!GetAutoConnectStatus())
                {

                    Esperando();

                    if (connectionThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                    {
                        try
                        {
                            connectionThread.Interrupt();
                            //printer.Resume();
                        }
                        catch (Exception e11)
                        {
                            Utils.Print(e11.StackTrace);
                        }
                    }
                    
                    listPucks = new ObservableCollection<DeviceItem>();
                    Utils.PrintDeep("comienza el Escaneo de dispositivos - Interface_background_scan_page");
                  
                    //bucle infinito hasy a que encuentre algun puck
                    await FormsApp.ble_interface.Scan();
                    while (FormsApp.ble_interface.GetBlePeripheralList().Count == 0)
                    {
                        await FormsApp.ble_interface.Scan();
                    }
                    
                    
                    if (FormsApp.ble_interface.GetBlePeripheralList().Count > 0)
                    {

                        ChangeListViewData();

                        if (listPucks.Count != 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DeviceList.ItemsSource = listPucks;
                                txtBuscando.IsVisible = false;
                            });
                        }
                        if (conectarDevice)
                        {
                            PairWithKnowDevice();
                        }
                        else
                        {
                            ContentView_DeviceList.IsVisible = true;
                            Terminado();

                        }                        
                    }                    
                }
            });

        }

        private void PairWithKnowDevice()
        {

            autoConnect = false;
            conectarDevice = false;
            #region Autoconnect to stored device 
            NewOpenConnectionWithDevice();
            #endregion

        }

        private void Esperando()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region New Circular Progress bar Animations    
                DeviceList.IsRefreshing = false;
                backdark_bg.IsVisible = true;
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

                #endregion
            });
        }

        private void Interface_ContentView_DeviceList()
        {
            Utils.PrintDeep($"-------------------------------    Interface_ContentView_DeviceList, thread: { Thread.CurrentThread.ManagedThreadId}");

            connectionThread = new Thread(new ThreadStart(ConnectionMethod))
            {
                Name = "ConnectionThreadScript"
            };
            connectionThread.Start();

            DeviceList.RefreshCommand.Execute(true);
              
        }
        private void RefreshPuckData(bool Firtstime = false)
        {
            if (!Singleton.Has<Puck>()) return;

            Puck puck = Singleton.Get.Puck;
            int battery;
            
            if (Firtstime)
                 battery = puck.BatteryLevelFix;
            else
                 battery = puck.BatteryLevel;
                       
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


        private void ConnectionMethod()
        {
            Utils.PrintDeep("dentro del metodo - InvokeMethod");

            int timeout_connecting = 0;
            int cont = 0;
            int refresh = 0;

            Utils.PrintDeep("se va a ejecutar un bucle (WHILE TRUE) - InvokeMethod");
         
            while (true)
            {
                try
                {
                    Utils.PrintDeep("dentro del bucle (WHILE TRUE) - InvokeMethod");

                    Utils.PrintDeep("buscamos el estado de la conexion - InvokeMethod");

                    int status = FormsApp.ble_interface.GetConnectionStatus();

                    Utils.PrintDeep("se obtiene el estado de la conexion - InvokeMethod");

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
                        Utils.PrintDeep($"---------------------------------Invoke method ----estado : {status} , Perifericoconnected: {peripheralConnected}");
                        Utils.PrintDeep($"---------------------------------Invoke method ---- Thread: {Thread.CurrentThread.ManagedThreadId}");
                        Utils.PrintDeep("buscamos el estado de la conexion - InvokeMethod");

                        Utils.PrintDeep("¿ES NO_CONNECTED? - InvokeMethod");

                        if (peripheralConnected == ble_library.BlePort.NO_CONNECTED)
                        {
                            peripheralConnected = status;
                            timeout_connecting = 0;
                        }
                        else if (peripheralConnected == ble_library.BlePort.CONNECTING)
                        {
                            Utils.PrintDeep("Nop, es CONNECTING - InvokeMethod");

                            if (status == ble_library.BlePort.NO_CONNECTED)
                            {

                                Utils.PrintDeep("Se va a ejecutar algo en la UI - InvokeMethod");

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Utils.PrintDeep("Se va a detectar el estado de la conexion - InvokeMethod");

                                    switch (FormsApp.ble_interface.GetConnectionError())
                                    {
                                        case ble_library.BlePort.NO_ERROR:
                                            Utils.PrintDeep("Estado conexion: NO_ERROR - InvokeMethod");
                                            break;
                                        case ble_library.BlePort.CONECTION_ERRROR:
                                            Utils.PrintDeep("Estado conexion: CONECTION_ERRROR - InvokeMethod");


                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                                backdark_bg.IsVisible = false;
                                                indicator.IsVisible = false;
                                                ContentView_DeviceList.IsEnabled = true;

                                            #endregion
                                        });

                                            Utils.PrintDeep("Desactivar barra de progreso - InvokeMethod");

                                            Application.Current.MainPage.DisplayAlert("Alert", "Connection error. Please, retry", "Ok");
                                            break;
                                        case ble_library.BlePort.DYNAMIC_KEY_ERROR:
                                            Utils.PrintDeep("Estado conexion: DYNAMIC_KEY_ERROR - InvokeMethod");

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                                backdark_bg.IsVisible = false;
                                                indicator.IsVisible = false;
                                                ContentView_DeviceList.IsEnabled = true;

                                            #endregion
                                        });

                                            Utils.PrintDeep("Desactivar barra de progreso - InvokeMethod");
                                            Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                            break;
                                        case ble_library.BlePort.NO_DYNAMIC_KEY_ERROR:
                                            Utils.PrintDeep("Estado conexion: NO_DYNAMIC_KEY_ERROR - InvokeMethod");

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                                backdark_bg.IsVisible = false;
                                                indicator.IsVisible = false;
                                                ContentView_DeviceList.IsEnabled = true;

                                            #endregion

                                        });
                                            Utils.PrintDeep("Desactivar barra de progreso - InvokeMethod");
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
                                Utils.PrintDeep($"---------------------------------Invoke method ----estado : {status} , Conectado");

                                Utils.PrintDeep("Estas Conectado - InvokeMethod");

                                DeviceList.IsEnabled = true;

                                peripheralConnected = status;
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    fondo.Opacity = 1;
                                    ContentView_DeviceList.Opacity = 1;
                                    ContentView_DeviceList.IsEnabled = true;

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

                                    Utils.PrintDeep("Se va a ejecutar el Script - InvokeMethod");

                                    try
                                    {
                                        connectionThread.Abort();
                                    }
                                    catch (Exception e5)
                                    {
                                        Utils.Print(e5.StackTrace);
                                    }

                                Terminado();
                                //Connection Method
                               
                                runScript();

                                });
                            }
                        }
                        else if (peripheralConnected == ble_library.BlePort.CONNECTED)
                        {
                            Utils.PrintDeep("Nop, es CONNECTED - InvokeMethod");

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

                                Utils.Print("========================================================================================= NOT CONNECTED");
                                ContentView_Scripting.IsVisible = false;
                                ContentView_DeviceList.IsVisible = true;
                                backdark_bg.IsVisible = false;
                                indicator.IsVisible = false;

                            });

                        }

                    }

                    Utils.PrintDeep("¿Está en CONNECTING? - InvokeMethod");

                    if (peripheralConnected == ble_library.BlePort.CONNECTING)
                    {
                        Utils.PrintDeep("Si, es CONNECTING - InvokeMethod");
                        timeout_connecting++;
                        if (timeout_connecting >= 2 * 10) // 10 seconds
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Utils.PrintDeep("Un Timeout que te llevas - InvokeMethod");
                                Application.Current.MainPage.DisplayAlert("Timeout", "Connection Timeout", "Ok");
                                listPucks = new ObservableCollection<DeviceItem>();
                                DeviceList.ItemsSource = listPucks;
                                DeviceList.IsEnabled = true;
                                fondo.Opacity = 1;
                                ContentView_DeviceList.Opacity = 1;

                                autoConnect = false;

                                #region Disable Circular Progress bar Animations when done

                            backdark_bg.IsVisible = false;
                                indicator.IsVisible = false;
                                ContentView_DeviceList.IsEnabled = true;

                            #endregion

                                peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                                timeout_connecting = 0;
                                FormsApp.ble_interface.Close();
                                CrossSettings.Current.AddOrUpdateValue("session_dynamicpass", string.Empty);
                            });

                            Utils.PrintDeep("Cerrar Conexion - InvokeMethod");

                            Thread.Sleep(Timeout.Infinite);
                            //printer.Suspend();

                        }
                    }
                    else
                    {
                        Utils.PrintDeep("Nop, no es CONNECTING - InvokeMethod");
                    }

                    Utils.PrintDeep("Esperamos 300 ms - InvokeMethod");
                    Thread.Sleep(300); // 0.5 Second

                    Utils.PrintDeep("¿Se va a realizar reconexion? - InvokeMethod");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread '{0}' awoken.",
                                  Thread.CurrentThread.Name);
                }

            }

        }

        private void runScript()
        {
            Task.Run(async () =>
            {
                Utils.PrintDeep("runScript Tarea Asincrona - InvokeMethod");
                await Task.Delay(150); 
                Device.BeginInvokeOnMainThread ( async () =>
                {
                    try
                    {
                        Utils.PrintDeep("Mostrar barra de progreso - InvokeMethod");

                        ContentView_Scripting_label_read.Text = "Preparing the script ... ";
                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;
                        ContentView_DeviceList.IsEnabled = false;
                        ContentView_Scripting.IsEnabled = false;
                        Thread.Sleep(2000);
                        ContentView_Scripting_label_read.Text = "Executing Script ... ";
                        await Task.Factory.StartNew(scriptFunction);
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

        private async Task scriptFunction()
        {
            ScriptRunner runner = new ScriptRunner ();

            //Define finish and error event handler
            runner.OnFinish     += OnFinish;
            runner.OnProgress   += OnProgress;
            runner.onStepFinish += onStepFinish;
            runner.OnError      += OnError;

            await runner.ParseScriptAndRun ( FormsApp.ble_interface, scriptToLaunchData, scriptToLaunchData.Length );
        }

        //private async Task ChangeListViewData()
        private void ChangeListViewData()
        {
     
            try
            {
                List<IBlePeripheral> blePeripherals;
                blePeripherals = FormsApp.ble_interface.GetBlePeripheralList();

                byte[] bytes = System.Convert.FromBase64String(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty));

                byte[] byte_now;

                int sizeList = blePeripherals.Count;

                for (int i = 0; i < sizeList; i++)
                {
                    try
                    {
                        if (blePeripherals[i] != null)
                        {
                            Puck puck = new Puck ( blePeripherals[ i ] );
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


                            if (!enc)
                            {
                                DeviceItem device = new DeviceItem
                                {
                                    deviceMacAddress = puck.SerialNumber,
                                    deviceName = puck.Name,
                                    deviceBattery = puck.BatteryLevel + "%",
                                    deviceRssi = puck.RSSI + " dBm",
                                    deviceBatteryIcon = puck.BatteryLevelIcon,
                                    deviceRssiIcon = puck.RSSIIcon,
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
    
                                            Singleton.Set = new Puck ( blePeripherals[ i ], FormsApp.ble_interface );

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
                            }
                        }
                    }
                    catch (Exception er)
                    {

                        Utils.Print(er.StackTrace); 
                    }
                }
                if (!autoConnect)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region Disable Circular Progress bar Animations when done

                        DeviceList.IsRefreshing = false;
                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        ContentView_DeviceList.IsEnabled = true;

                        #endregion
                    });
                }
            }
            catch (Exception e)
            {
                Utils.Print(e);
            }
        }

        #region We want to connect to the device if there is not scanning running
        
        private void NewOpenConnectionWithDevice()
        {

            Utils.PrintDeep("Se va a entrar en un bucle mientras esté Escaneando bluetooth - NewOpenConnectionWithDevice");

            while (FormsApp.ble_interface.IsScanning())
            {
                Utils.PrintDeep("A esperar 100 ms mientras escanea... - NewOpenConnectionWithDevice");
                Thread.Sleep(100);
            }

            Utils.PrintDeep("Se va a ejecutar algo en el UI - NewOpenConnectionWithDevice");

            Device.BeginInvokeOnMainThread(() =>
            {
                var seconds = TimeSpan.FromSeconds(1); // Don't execute it asap!

                Device.StartTimer(seconds, () =>
                {
                    Utils.PrintDeep("Cada 1 segundo, se ejectua lo siguinete en el UI - NewOpenConnectionWithDevice");
                    Device.BeginInvokeOnMainThread(() =>
                       {
                       Utils.PrintDeep("¿Esta la conexion abierta ? - NewOpenConnectionWithDevice");

                       if (!FormsApp.ble_interface.IsOpen())
                       {
                            Utils.PrintDeep("¿Esta escaneando perifericos ? - NewOpenConnectionWithDevice");
                            while (FormsApp.ble_interface.IsScanning())
                            {
                                Utils.PrintDeep("A esperar 100 ms en bucle - NewOpenConnectionWithDevice");
                                Thread.Sleep(100);
                            }

                            // call your method to check for notifications here
                            FormsApp.ble_interface.Open(Singleton.Get.Puck.Device, true);
                            
                       }
                       else
                       {
                           Utils.PrintDeep("NOPE, no lo esta - NewOpenConnectionWithDevice");
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
                        ContentView_Scripting_label_read.Text = "Script Execution Error";

                        Lexi.Lexi.ShowPopupAttemps ();

                        if ( ! Singleton.Get.Configuration.Global.NoKillInScripted )
                        {
                            Device.OpenUri ( new Uri ( resultCallback + "?" +
                                                    "status=error" +
                                                    Compression.GetUriParameter () +
                                                    "&message=" + Compression.CompressToUrlUsingGlobal ( "Error code: " + error.Id + "\n" + error.MessagePopup ) +
                                                    "&output_filename=" + resultScriptName +
                                                    "&output_data=" + Compression.CompressToUrlUsingGlobal ( Errors.lastErrorLogGenerated ) ) );

                            FormsApp.ble_interface.Close ();

                            System.Diagnostics.Process.GetCurrentProcess ().Kill ();
                        }
                    });
                });
            });
        }

  
        private  async Task onStepFinish ( object sender, MTUComm.Delegates.ActionFinishArgs args )
        {
            
            Utils.PrintDeep("HI FINISH RUNNER");
            
        }

        private void OnProgress ( object sender, MTUComm.Delegates.ProgressArgs e )
        {
            string mensaje = e.Message;

            Device.BeginInvokeOnMainThread(() =>
            {
                if ( ! string.IsNullOrEmpty ( mensaje ) )
                    ContentView_Scripting_label_read.Text = mensaje;
            });
        }

        private async Task OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        ContentView_Scripting_textScript.Text = "";
                        Parameter[] allParams = args.Result.getParameters();
    
                        for (int k = 0; k < allParams.Length; k++)
                        {
                            String res = allParams[k].getLogDisplay() + ": " + allParams[k].Value;
                            String val = ContentView_Scripting_textScript.Text;
                            ContentView_Scripting_textScript.Text = val + res + "\r\n";
                        }
    
                        ActionResult[] allports = args.Result.getPorts();
    
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

                        string xmlResultTocallback = ((MTUComm.Action)sender).GetResultXML (); // args.Result);
    
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            ContentView_Scripting.IsEnabled = true;
                            ContentView_Scripting_label_read.Text = "Successful Script Execution";

                            Lexi.Lexi.ShowPopupAttemps ();

                            if ( ! Singleton.Get.Configuration.Global.NoKillInScripted )
                            {
                                Device.OpenUri ( new Uri ( resultCallback + "?" +
                                                    "status=success" +
                                                    Compression.GetUriParameter () +
                                                    "&output_filename=" + resultScriptName +
                                                    "&output_data=" + Compression.CompressToUrlUsingGlobal ( xmlResultTocallback ) ) );
                            
                                FormsApp.ble_interface.Close ();
                            
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                            }
                        });
    
                    }
                    catch ( Exception )
                    {
                        OnError();
                    }
                });
            });
        }

        private void LoadPhoneUI()
        {
            ContentView_Scripting.Margin = new Thickness(0, 0, 0, 0);
            ContentView_Scripting_hamburger_icon.IsVisible = true;
            indicator.Scale = 1;
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


        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelectedListDevices(object sender, ItemTappedEventArgs e)
        {
            Utils.PrintDeep($"-------------------------------       OnMenuItemSelectedListDevices, thread: { Thread.CurrentThread.ManagedThreadId}");
            try
            {
                connectionThread.Interrupt();
                //printer.Resume();
            }
            catch (Exception e5)
            {
                Utils.Print(e5.StackTrace);
            }

            var item = (DeviceItem)e.Item;
            
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
                Singleton.Set = new Puck(item.Peripheral, FormsApp.ble_interface);

                FormsApp.ble_interface.Open(Singleton.Get.Puck.Device, reassociate);
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
    }
}
