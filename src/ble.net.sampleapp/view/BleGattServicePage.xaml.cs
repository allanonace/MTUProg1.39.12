// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ble.net.sampleapp.viewmodel;
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



        public BleGattServicePage(BleGattServiceViewModel model, IBleGattServerConnection gattServer, IUserDialogs dialogs)
        {
            InitializeComponent();
            BindingContext = model;

            model_saved = model;

           
           

            Task.Run(async () =>
            {

                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {

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

                        await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
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

                             await Task.Delay(1500); Device.BeginInvokeOnMainThread(() =>
                             {

                                 BleGattCharacteristicViewModel[] array = new BleGattCharacteristicViewModel[10];
                                 model_saved.Characteristic.CopyTo(array, 0);

                                 /*
                                 for (int i = 0; i < array.Length; i++){
                                     if(array[i].Id.Equals("00000003-0000-1000-8000-00805f9b34fb")){
                                         String valueReaded = array[i].ValueAsHex;


                                     }
                                 }
                                 */



                                 String value1 = array[0].ValueAsHex;

                                 int longitud = value1.Length ;

                                 int contador = 0;

                                 String valortemp = "";

                                 List<String> listofstrings = new List<string>();

                              
                                 while(contador<longitud){

                                      valortemp = value1.Substring(contador, 40 );

                                      listofstrings.Add(valortemp);

                                      contador = contador+40;

                                 }

                               // String listado =  listofstrings.ToString();


                                String listadoDatos = "";


                                    for (int i = 0; i < listofstrings.Count; i++)
                                {
                                     String valorPosicion = listofstrings[i].ToString();

                                     valorPosicion = valorPosicion.Substring(4, 36);

                                     int cuantoDato = 0;

                                     cuantoDato = Int32.Parse(valorPosicion.Substring(0, 2));

                                     cuantoDato = cuantoDato * 2;

                                     valorPosicion = valorPosicion.Substring(2, 34);

                                     valorPosicion = valorPosicion.Substring(0, cuantoDato);

                                     listadoDatos = listadoDatos + valorPosicion;

                                }


                                String pruebavalor = listadoDatos.ToString();

                                    /*
                                 for (int i = 0; i < longitud; i++){
                                        
                                        if(contador<21){
                                            contador++;

                                            valortemp = value1.Substring(0, contador);

                                        }else{
                                            
                                            contador = 0;
                                        }

                                       


                                 }
*/

                               



                                 //value1

                                 //String value2 = array[1].ValueAsHex; //El que me interesa

                                    valorHEX.Text = pruebavalor;

                                 //valorHEX.Text = value2;

                             });
                         });



                           
                           

                        });
                    });


                });
            });


        }
                   




            /*
            Guid ServicioRead = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
            Guid CaracterisicoRead = new Guid("00000003-0000-1000-8000-00805f9b34fb");




            // String valorDato = "000005258000015a";

            BleGattCharacteristicViewModel gattCharacteristicViewModelRead = new BleGattCharacteristicViewModel(ServicioRead, CaracterisicoRead, gattServer, dialogs);

            var viewModelRead = (BleGattCharacteristicViewModel)gattCharacteristicViewModelRead;

            if (viewModelRead.ToggleNotificationsCommand.CanExecute(null))
                viewModelRead.ToggleNotificationsCommand.Execute(null);


            gattServer.NotifyCharacteristicValue(ServicioRead, CaracterisicoRead, bytes => {
                String value = bytes.ToString();
            });




            Task.Run(async () =>
            {

                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {


                    Guid Servicio = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                    Guid Caracterisico = new Guid("00000002-0000-1000-8000-00805f9b34fb");
                    // String valorDato = "000005258000015a";

                    BleGattCharacteristicViewModel gattCharacteristicViewModel = new BleGattCharacteristicViewModel(Servicio, Caracterisico, gattServer, dialogs);

                    var viewModel = (BleGattCharacteristicViewModel)gattCharacteristicViewModel;
                    if (viewModel.WriteCurrentBytesGUIDCommand.CanExecute(null))
                        viewModel.WriteCurrentBytesGUIDCommand.Execute(null);


                    Task.Run(async () =>
                    {

                        await Task.Delay(1500); Device.BeginInvokeOnMainThread(() =>
                        {
                            Guid ServicioRead2 = new Guid("2cf42000-7992-4d24-b05d-1effd0381208");
                            Guid CaracterisicoRead2 = new Guid("00000003-0000-1000-8000-00805f9b34fb");

                            // String valorDato = "000005258000015a";

                            BleGattCharacteristicViewModel gattCharacteristicViewModelRead2 = new BleGattCharacteristicViewModel(ServicioRead2, CaracterisicoRead2, gattServer, dialogs);

                            var viewModelRead2 = (BleGattCharacteristicViewModel)gattCharacteristicViewModelRead2;

                     

                            String valueRead2 = viewModelRead2.getValuehex();

                        });
                    });

                   




                });
            });



            /*
            Task.Run(async () =>
            {

                await Task.Delay(2500); Device.BeginInvokeOnMainThread(() =>
                {
                    BleGattCharacteristicViewModel[] array = new BleGattCharacteristicViewModel[10];

                    //model_saved.Characteristic.Add.SetValue("000005258000015a");

                    model_saved.Characteristic.CopyTo(array, 0);

                    String value1 = array[0].ValueAsHex;
                    String value2 = array[1].ValueAsHex;

                });
            });

     */
      

       // WriteCurrentBytesGUID(Guid Servicio, Guid Caracterisico, String valorDato)


    

      private void OnItemSelected( Object sender, SelectedItemChangedEventArgs e )
      {
         ((ListView)sender).SelectedItem = null;


      }
   }
}
