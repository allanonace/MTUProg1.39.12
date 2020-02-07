using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MTUComm;
using Library;
using Library.Exceptions;
using aclara_meters.view;

using ActionType       = MTUComm.Action.ActionType;
using ValidationResult = MTUComm.MTUComm.ValidationResult;

namespace aclara_meters.util
{
    public class BasePage : ContentPage
    {
        protected const string COLOR_FONT             = "#000000";
        protected const string COLOR_BACKGROUND       = "#FFF";
        protected const string COLOR_BG_ND_EXCELENT   = "#0F0";
        protected const string COLOR_BG_ND_GOOD       = "#FF0";
        protected const string COLOR_BG_ND_FAIL       = "#F00";
        protected const string COLOR_FONT_ND_EXCELENT = COLOR_FONT;
        protected const string COLOR_FONT_ND_GOOD     = COLOR_FONT;
        protected const string COLOR_FONT_ND_FAIL     = "#FFFFFF";

        public bool DebugMode { private set; get; }

        public BasePage ()
        {
            PageLinker.CurrentPage = this;

            #if DEBUG
            this.DebugMode = true;
            #endif

            // Reset previous main action reference
            Singleton.Remove<MTUComm.Action> ();
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();
            ( BindingContext as IBaseViewModel )?.OnAppearing ();

            #if DEBUG

            Lexi.Lexi.ResetAttemptsCounter ();

            #endif
        }
        
        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();
            ( BindingContext as IBaseViewModel )?.OnDisappearing ();
        }

        protected async void AutoFillTextbox ( object sender, EventArgs e )
        {
            StackLayout main = ( StackLayout )this.FindByName ( "ReadMTUChangeView" );

            var random = new Random();
            var list = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int index = random.Next(list.Count);
            char cTextWr = list[index];
            index = random.Next(list.Count);
            char cTextOld = list[index];

            AutoFillTextbox_Logic ( main, cTextWr, cTextOld );

            await Task.Delay ( 1000 );

            // NOTE: First fill pickrs because some textbox ( e.g. MeterReading ) need it
            AutoFillTextbox_Logic ( main, cTextWr, cTextOld );
        }

        protected void AutoFillTextbox_Logic (
            StackLayout mainElement, char cTextWr, char cTextOld )
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
                catch ( Exception )
                {
                    // only in debug
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
                    tbx.Text = new string(cTextWr, tbx.MaxLength);
                    if (tbx.Display.ToLower().Contains("old"))
                        tbx.Text = new string(cTextOld, tbx.MaxLength);
                    
                }
                catch ( Exception )
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

        protected async Task<ValidationResult> ValidateNavigation (
            ActionType typeTarget )
        {
            try
            {
                MTUComm.Action basicRead = new MTUComm.Action (
                   FormsApp.ble_interface,
                   ActionType.BasicRead );

                return await basicRead.RunNavValidation ( typeTarget );
            }
            catch ( Exception e )
            {
                if ( e is MtuDoesNotBelongToAnyFamilyException )
                    return ValidationResult.FAMILY_NOT_SUPPORTED;
                return ValidationResult.EXCEPTION;
            }
        }
    
        protected string LogAttempts (
            string message )
        {
            return
                "E."  + Lexi.Lexi.NumErrors  .ToString ( "000" ) +
                " A." + Lexi.Lexi.NumAttempts.ToString ( "000" ) +
                ( ( ! string.IsNullOrEmpty ( message ) ) ? " | " + message : string.Empty );
        }
    
        protected void ShowErrorAndKill (
            Exception e )
        {
            Device.BeginInvokeOnMainThread ( () =>
            {
                Application.Current.MainPage = new NavigationPage ( new ErrorInitView ( e ) );
            });
        }

        protected void ShowAlert (
            Exception e )
        {
            Device.BeginInvokeOnMainThread ( () =>
            {
                Errors.LogErrorNowAndContinue ( e );
            });
        }
    }
}
