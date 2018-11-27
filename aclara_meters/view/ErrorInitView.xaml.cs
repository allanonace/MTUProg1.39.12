using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aclara_meters.view
{
    public partial class ErrorInitView : ContentPage
    {
        public ErrorInitView(string error = "")
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    if(!error.Equals(""))
                    {
                        CheckError(error);
                    }else{
                        CheckError();
                    }
                   
                });
            });
        }



        private async void CheckError(string error = "")
        {
            string respstr = "No connection available";

            if(!error.Equals(""))
            {
                respstr = error;
            }
   
            var response = await Application.Current.MainPage.DisplayAlert("Error", respstr, "Ok", "Cancel");

            if (response == false || response == true)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }

        }

    }
}
