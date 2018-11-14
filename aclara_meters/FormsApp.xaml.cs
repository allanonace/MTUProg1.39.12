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
using System.Web;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Plugin.DeviceInfo;
using MTUComm;
using aclara_meters.Helpers;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        public static string AppName { get { return "Aclara MTU Programmer"; } }
        public static ICredentialsService CredentialsService { get; private set; }
        public static BleSerial ble_interface;
        //public static Lexi.Lexi lexi;

        public string Version;
        public string deviceId;


        public static Configuration config;

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
            //lexi = new Lexi.Lexi(ble_interface, 10000);

            // XML FILE FTP CREATION
            //var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //var filename_meter = Path.Combine(xml_documents, "Meter.xml");
            // var filename_mtu = Path.Combine(xml_documents, "Mtu.xml");

            // File.WriteAllText(filename_meter, aclara_meters.Resources.XmlStrings.GetMeterString());
            //  File.WriteAllText(filename_mtu, aclara_meters.Resources.XmlStrings.GetMTUString());

            LoadConfiguration();

            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
        }

        private void LoadConfiguration()
        {
            var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
            }

            config = new Configuration(xml_documents);

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                config.setPlatform("Android"); config.setAppName(AppName); config.setVersion(Version); config.setDeviceUUID(deviceId);

            }
            else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                config.setPlatform("iOS"); config.setAppName(AppName); config.setVersion(Version); config.setDeviceUUID(deviceId);
            }
            else
            {
                config.setPlatform("Unknown");
            }
        }


        /*--------------------------------------------------*/
        /*      Extensions Requests are handled here        */

        public async void HandleUrl(string url, string path)
        {
            Uri newUri = null;

            try{
                newUri = new Uri(url);

                HandleUrl(newUri);
            }catch (Exception j){
                Console.WriteLine(j.StackTrace);
            }

           


          
        }

        public FormsApp(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs, List<string> listaDatos, string AppVersion)
        {
            InitializeComponent();

            Version = AppVersion;

            deviceId = CrossDeviceInfo.Current.Id;

            //Gestor de cuentas
            CredentialsService = new CredentialsService();

            //Inicializar libreria personalizada
            ble_interface = new BleSerial(adapter);
           


            string data = "";

            if (listaDatos.Count != 0 || listaDatos != null)
            {
                
 
                for (int i = 0; i < listaDatos.Count; i++)
                {
                    data = data + listaDatos[i] + "\r\n";
                }

              
            }

            string base64CertificateString = "";

            try
            {
                base64CertificateString = listaDatos[2].Replace("cert_file: ", "");
                byte[] bytes = Convert.FromBase64String(base64CertificateString);
                X509Certificate2 x509certificate = new X509Certificate2(bytes);

            }catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
           
            LoadConfiguration();

            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs, data));


        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async void HandleUrl(Uri url)
        {
            if (url == null)
            {

            }
            else
            {
                var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filename = "";

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                {
                    xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
                }

                //string decode1 = System.Web.HttpUtility.UrlDecode(url.ToString());
                //var uri = new Uri(decode1);
                var query = HttpUtility.ParseQueryString(url.Query);

                var script_name = query.Get("script_name");
                var script_data = query.Get("script_data");
                var callback = query.Get("callback");


                if(script_name!=null)
                {
                    filename = Path.Combine(xml_documents, script_name.ToString());
                }

                if (script_data != null)
                {
                    File.WriteAllText(filename, Base64Decode(script_data));
                }

                if (callback != null)
                {
                  
                }
            
                Task.Run(async () =>
                {
                    await Task.Delay(1000); Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        Settings.IsLoggedIn = false;
                        CredentialsService.DeleteCredentials();
                        NavigationPage page = new NavigationPage(new AclaraViewScripting(filename, callback, script_name));
                        MainPage = page;
                        await MainPage.Navigation.PopToRootAsync(true);
                    });
                });

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
