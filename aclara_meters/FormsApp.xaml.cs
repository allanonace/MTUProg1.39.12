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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        public static string AppName { get { return "StoreAccountInfoApp"; } }
        public static ICredentialsService CredentialsService { get; private set; }
        public static BleSerial ble_interface;
        //public static Lexi.Lexi lexi;

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



            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
        }



        public FormsApp(IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs, List<string> listaDatos)
        {
            InitializeComponent();

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

           

            string base64CertificateString = listaDatos[2].Replace("cert_file: ", "");
   
            byte[] bytes = Convert.FromBase64String(base64CertificateString);
            X509Certificate2 x509certificate = new X509Certificate2(bytes);



            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs, data));


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


                            List<string> listaObjetos = new List<string>();

                            XDocument doc = XDocument.Parse(var1);

                            foreach (var item in doc.Descendants("note"))  
                            {  
                                string to = item.Element("to").Value.ToString();  
                                string from = item.Element("from").Value.ToString();  
                                string heading = item.Element("heading").Value.ToString();  
                                string body = item.Element("body").Value.ToString();  
                                listaObjetos.Add("To: "+to+" From: "+from+" Heading: "+heading+" Body: "+body);  
                            }  

                            await Application.Current.MainPage.DisplayAlert("Objetos XML", listaObjetos[0], "ok", "cancel");

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
