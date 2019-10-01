// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MTUComm;
using Library;

using ActionType = MTUComm.Action.ActionType;

namespace aclara_meters.util
{
   public class BasePage : ContentPage
   {
        protected bool DebugMode { private set; get; }

        public BasePage ()
        {
            PageLinker.CurrentPage = this;

            #if DEBUG
            this.DebugMode = true;
            #endif

            // Reset previous main action reference
            Singleton.Remove<MTUComm.Action>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as IBaseViewModel)?.OnAppearing();
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as IBaseViewModel)?.OnDisappearing();
        }

        protected async void AutoFillTextbox ( object sender, EventArgs e )
        {
            StackLayout main = ( StackLayout )this.FindByName ( "ReadMTUChangeView" );

            AutoFillTextbox_Logic ( main );

            await Task.Delay ( 1000 );

            // NOTE: First fill pickrs because some textbox ( e.g. MeterReading ) need it
            AutoFillTextbox_Logic ( main );
        }

        protected void AutoFillTextbox_Logic (
            StackLayout mainElement )
        {
            List<BorderlessEntry>  listTbx = new List<BorderlessEntry> ();
            List<BorderlessPicker> listPck = new List<BorderlessPicker> ();

            GetChildrensTextbox ( mainElement, listTbx, listPck );
            
            foreach ( BorderlessPicker pck in listPck
                .Where ( pck =>
                    pck.IsEnabled &&
                    pck.IsVisible &&
                    ( pck as BorderlessPicker ).SelectedIndex <= -1 ) )
            {
                try
                {
                    pck.SelectedIndex = 0;
                }
                catch ( Exception ex )
                {
                    
                }
            }

            foreach ( BorderlessEntry tbx in listTbx
                .Where ( tbx =>
                    tbx.IsEnabled &&
                    tbx.IsVisible &&
                    string.IsNullOrEmpty ( ( tbx as BorderlessEntry ).Text ) ) )
            {
                try
                {
                    tbx.Text = new string ( '1', tbx.MaxLength );
                }
                catch ( Exception ex )
                {
                    // NOTE: Can fail if the control does not have set a max length value
                }
            }
        }

        private void GetChildrensTextbox (
            Layout element,
            List<BorderlessEntry> listTbx,
            List<BorderlessPicker> listPck )
        {
            element.Children.ToList ().ForEach (
                child =>
                {
                    if ( child is BorderlessEntry )
                        listTbx.Add ( child as BorderlessEntry );

                    else if ( child is BorderlessPicker )
                        listPck.Add ( child as BorderlessPicker );

                    else if ( child is Layout )
                        GetChildrensTextbox ( child as Layout, listTbx, listPck );
                });
        }

        protected async Task<bool> ValidateNavigation (
            ActionType typeTarget )
        {
            try
            {
                MTUComm.Action basicRead = new MTUComm.Action(
                   FormsApp.ble_interface,
                   ActionType.BasicRead);

                return await basicRead.RunNavValidation(typeTarget);
            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}
