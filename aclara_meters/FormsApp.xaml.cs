using System;
using Acr.UserDialogs;
using aclara_meters.view;
using nexus.protocols.ble;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using ble_library;
using Plugin.Settings;
using Lexi;
using System.Threading.Tasks;
using System.Web;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        public static string AppName { get { return "StoreAccountInfoApp"; } }
        public static ICredentialsService CredentialsService { get; private set; }
        public static BleSerial ble_interface;
        public static LexiComm lexi;

        public FormsApp()
        {
            InitializeComponent();
            CredentialsService = new CredentialsService();
        }

        public FormsApp(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs)
        {
            InitializeComponent();

            //Gestor de cuentas
            CredentialsService = new CredentialsService();

            //Inicializar libreria personalizada
            ble_interface = new BleSerial(adapter);
            lexi = new LexiComm(ble_interface, 10000);

          
            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
        }



        public async void HandleUrl(Uri url)
        {
            if (url == null)
            {
                
            }
            else
            {
                string url_str = url.LocalPath;
                string url_str2 = url.AbsolutePath;
                string url_str3 = url.Scheme;

            
                var response = await Application.Current.MainPage.DisplayAlert("Alert", "Work in Progress deep linking", "ok","cancel");  
  
                if (response)  
                {

					String decode1 = System.Web.HttpUtility.UrlDecode(url.ToString());
                    var uri = new Uri(decode1);
                    var query = HttpUtility.ParseQueryString(uri.Query);

                  
                    try
                    {
                        var var1 = query.Get("script_path");
                        if (var1 != null)
                        {
                            Console.WriteLine("Var1:" + var1.ToString());
                            var response3 = await Application.Current.MainPage.DisplayAlert("Alert", "script_path: " + var1.ToString(), "ok", "cancel");
                        }
                        var var2 = query.Get("callback");
                        if (var2 != null)
                        {
                            Console.WriteLine("Var2:" + var2.ToString());
                            var response2 = await Application.Current.MainPage.DisplayAlert("Alert", "callback: " + var2.ToString(), "ok", "cancel");
                            /**/
                            var uri2 = new Uri(var2);
                            var query2 = HttpUtility.ParseQueryString(uri2.Query);
                            var var12 = query2.Get("param");
                            String cabecera = var2.Replace(var12, "");
                            String datos = System.Web.HttpUtility.UrlEncode(var12);
                            Xamarin.Forms.Device.OpenUri(new Uri(cabecera + datos));

                        }
                       


                    }catch(Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }

                }  
      
                else  
                {  
                   //user click cancel  
                
                }  

            }
        }

        protected override void OnStart()
        {
            AppCenter.Start("ios=cb622ad5-e2ad-469d-b1cd-7461f140b2dc;" + "android=53abfbd5-4a3f-4eb2-9dea-c9f7810394be", typeof(Analytics), typeof(Crashes), typeof(Distribute) );
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
