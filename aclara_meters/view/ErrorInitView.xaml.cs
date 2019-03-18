using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using MTUComm;

namespace aclara_meters.view
{
    public partial class ErrorInitView
    {
        public ErrorInitView (
            Exception e )
        {
            InitializeComponent();

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar(this, false);
            
            Errors.ShowErrorAndKill ( e );
        }
    }
}
