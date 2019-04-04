using Xamarin.Forms;
using Xml;
using System.Threading.Tasks;
using System;

using Acr.UserDialogs;

namespace MTUComm
{
    public class PageLinker
    {
        private const string BTN_OK     = "Ok";
        private const string BTN_CANCEL = "Cancel";
    
        private static PageLinker instance;
        private static Page currentPage;
        public static Page mainPage;
        //private IDisposable popup;
    
        public static Page CurrentPage
        {
            get { return currentPage;  }
            set { currentPage = value; }
        }
    
        private PageLinker () {}
        
        private static PageLinker GetInstance ()
        {
            if ( instance == null )
                instance = new PageLinker ();
                
            return instance;
        }

        private async Task _ShowAlert (
            string title,
            string message,
            string btnText,
            bool   kill )
        {
            //if ( currentPage != null )
            //{
                //Device.BeginInvokeOnMainThread ( async () =>
                //{
                    // NOTE: Xamarin DisplayAlert dialog cannot be closed/disposed from code
                    //await currentPage.DisplayAlert ( title, message, btnText );
                    
                    await UserDialogs.Instance.AlertAsync ( message, title, btnText );
                    
                    if ( kill )
                    {
                        // Wait four seconds and kill the popup
                        //await Task.Delay ( TimeSpan.FromSeconds ( 6 ) );
                        //popup.Dispose ();
                        
                        // Close the app
                        System.Diagnostics.Process.GetCurrentProcess ().Kill ();
                    }
                //});
            //}
        }

        public async static Task ShowAlert (
            string title,
            string message,
            string btnText = BTN_OK,
            bool   kill    = false )
        {
            await GetInstance ()._ShowAlert ( title, message, btnText, kill );
        }

        public async static Task ShowAlert (
            string title,
            Error  error,
            bool   kill    = false,
            string btnText = BTN_OK )
        {
            if ( error.Id > -1 )
                await GetInstance ()._ShowAlert (
                    title, "Error " + error.Id + ": " + error.Message, btnText, kill );
            else
                await GetInstance ()._ShowAlert (
                    title, error.Message, btnText, kill );
        }
    }
}
