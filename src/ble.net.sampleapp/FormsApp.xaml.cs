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
#if RELEASE
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
#endif

namespace ble.net.sampleapp
{
   public partial class FormsApp
   {
      private readonly NavigationPage m_rootPage;

      private readonly IBluetoothLowEnergyAdapter adapterAclara;
      private readonly IUserDialogs dialogsAclara;

      public FormsApp()
      {
         InitializeComponent();
      }

      public FormsApp( IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs )
      {
         InitializeComponent();

     
         dialogsAclara = dialogs;
         adapterAclara = adapter;

         var logsVm = new LogsViewModel();
         SystemLog.Instance.AddSink( logsVm );

         var bleAssembly = adapter.GetType().GetTypeInfo().Assembly.GetName();
         Log.Info( bleAssembly.Name + "@" + bleAssembly.Version );

         var bleGattServerViewModel = new BleGattServerViewModel( dialogsAclara, adapterAclara );

         var bleScanViewModel = new BleDeviceScannerViewModel(
            bleAdapter: adapterAclara,
            dialogs: dialogsAclara,
            onSelectDevice: async p =>
            {
               await bleGattServerViewModel.Update( p );

               await m_rootPage.PushAsync(
                  new BleGattServerPage(
                     model: bleGattServerViewModel,
                     bleServiceSelected: async s => { await m_rootPage.PushAsync( new BleGattServicePage( s ) ); } ) );
                
               await bleGattServerViewModel.OpenConnection();
            } );

          m_rootPage = new NavigationPage(new BleDeviceScannerPage(bleScanViewModel));

          MainPage = new LoginMenuPage(m_rootPage);
      }

      protected override void OnStart()
      {
         base.OnStart();
      }

   }
}
