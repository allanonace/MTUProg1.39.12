// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Reflection;
using Acr.UserDialogs;
using ble.net.sampleapp.view;
using ble.net.sampleapp.viewmodel;
using nexus.core.logging;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;
using nexus.protocols.ble;
using ble.net.sampleapp.Helpers;
#if RELEASE
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
#endif

namespace ble.net.sampleapp
{
   public partial class FormsApp
   {

      public FormsApp()
      {
         InitializeComponent();
      }

 
      public FormsApp( IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs )
      {
            InitializeComponent();

            var bleAssembly = adapter.GetType().GetTypeInfo().Assembly.GetName();
            Log.Info(bleAssembly.Name + "@" + bleAssembly.Version);

            var bleGattServerViewModel = new BleGattServerViewModel(dialogs, adapter);
            var bleScanViewModel = new BleDeviceScannerViewModel(
               bleAdapter: adapter,
               dialogs: dialogs,
               onSelectDevice: async p =>
               {
                   await bleGattServerViewModel.Update(p);
                   await Application.Current.MainPage.Navigation.PushAsync(
                   new BleGattServerPage(
                      model: bleGattServerViewModel,
                        bleServiceSelected: async s => { await Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(s)); }));
                   await bleGattServerViewModel.OpenConnection();
               });

           // NavigationPage  m_rootPage = new NavigationPage(new BleDeviceScannerPage(bleScanViewModel));



            if (Settings.IsLoggedIn)
            {
                MainPage = new NavigationPage(new BleDeviceScannerPage(bleScanViewModel));

            }else{
                MainPage = new LoginMenuPage(bleScanViewModel);
            }

      }


      protected override void OnStart()
      {
         base.OnStart();
      }

   }
}
