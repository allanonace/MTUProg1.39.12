using System;
using MTUComm;
using Xamarin.Forms;

namespace aclara_meters.view
{
    public partial class ErrorInitView
    {
        public ErrorInitView (
            Exception e )
        {
            InitializeComponent ();

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar ( this, false );
            
            Errors.LogErrorNowAndKill ( e );
        }
    }
}
