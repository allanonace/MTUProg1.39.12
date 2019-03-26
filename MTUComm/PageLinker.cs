using Xamarin.Forms;
using Xml;
using System.Threading.Tasks;

namespace MTUComm
{
    public class PageLinker
    {
        private const string BTN_TXT = "Ok";
    
        private static PageLinker instance;
        private static Page currentPage;
        public static Page mainPage;
    
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

        private async void _ShowAlert (
            string title,
            string message,
            string btnText,
            bool   kill = false )
        {
            if ( ! kill )
                return;
        
            if ( currentPage != null )
            {
                Device.BeginInvokeOnMainThread ( async () =>
                {
                    await currentPage.DisplayAlert ( title, message, btnText );
                    
                    if ( kill )
                        System.Diagnostics.Process.GetCurrentProcess ().Kill ();
                });
            }
        }

        public static void ShowAlert (
            string title,
            string message,
            string btnText,
            bool   kill = false )
        {
            GetInstance ()._ShowAlert ( title, message, btnText );
        }
        
        public static void ShowAlert (
            string title,
            Error  error,
            bool   kill    = false,
            string btnText = BTN_TXT )
        {
            if ( error.Id > -1 )
                GetInstance ()._ShowAlert (
                    title, "Error " + error.Id + ": " + error.Message, btnText, kill );
            else
                GetInstance ()._ShowAlert (
                    title, error.Message, btnText, kill );
        }
    }
}
