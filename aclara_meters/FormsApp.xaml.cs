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
using Renci.SshNet;
using System.Linq;
using System.Globalization;
using Xamarin.Essentials;
using Renci.SshNet.Sftp;
using Plugin.Multilingual;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        #region Initial FTP - Default Config data

        string host = "159.89.29.176";
        string username = "aclara";
        string password = "aclara1234";
        string pathRemoteFile = "/home/aclara";

        #endregion

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

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;



      #region Init Configuration Files

            if ( CheckForLocalFiles() )
            {
                ItJustWorks(dialogs, data);

            }else{
                
                DownloadConfigurationFiles(dialogs, data);
            }

      #endregion

        }



    #region Init Configuration Files Implementation

        private async void DownloadConfigurationFiles(IUserDialogs dialogs, string data)
        {
            
            #region Check the Network channels

            if (CheckIfNetworkIsAvailable())
            {
                #region Download all the data

                if( DownloadInitialConfigFiles() )
                {
                    ItJustWorks(dialogs, data);
                }else{
                    
                    #region Error Dialog Must be shown on downloading

                    MainPage = new NavigationPage(new ErrorInitView("Error Downloading files"));

                    #endregion
                }

                #endregion

            }
            else
            {
                #region Error Dialog Must be shown on Loading

                MainPage = new NavigationPage(new ErrorInitView());

                #endregion
            }

            #endregion

        }

        #region Initial Config Files Download from SFTP

        private bool DownloadInitialConfigFiles()
        {
            try
            {
                using (SftpClient sftp = new SftpClient(host, username, password))
                {
                    try
                    {
                        sftp.Connect();

                   
                        /*--------------------------------------------------*/
                        // List all posible files in the documents directory 
                        // Check if file's lastwritetime is the lastest 
                        /*--------------------------------------------------*/
                        List<SftpFile> ftp_array_files = new List<SftpFile>();

                        // Remote FTP File directory
                        var ftp_files = sftp.ListDirectory(pathRemoteFile);
                        foreach (var file in ftp_files)
                        {

                            if(file.Name.Contains(".xml")){
                                ftp_array_files.Add(file);
                            }
                           

                        }

                        var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                        {
                            xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
                        }

                        foreach (var file in ftp_array_files)
                        {
                            string remoteFileName = file.Name;
                         

                            using (Stream file1 = File.OpenWrite( Path.Combine (xml_documents, remoteFileName) ))
                            {
                                sftp.DownloadFile( Path.Combine (pathRemoteFile, remoteFileName ), file1);
                            }

                        }

                       
                        sftp.Disconnect();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception has been caught " + e.ToString());
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }

            return false;
        }

        #endregion

        private bool CheckIfNetworkIsAvailable()
        {
            var current = Connectivity.NetworkAccess;

            var profiles = Connectivity.Profiles;

            if (profiles.Contains(ConnectionProfile.WiFi))
            {
                if (current == NetworkAccess.Internet)
                {
                    return true;
                    // Connection to internet is available
                }
            }else 
            if (profiles.Contains(ConnectionProfile.Cellular))
            {
                if (current == NetworkAccess.Internet)
                {
                    return true;
                    // Connection to internet is available
                }
            }

            return false;
        }


        #region Testing Purposes - Not final - Control the connectivity to network in Realtime

        /*
        public ConnectivityTest()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.Profiles;
        }
        */
        #endregion

        private void ItJustWorks(IUserDialogs dialogs, string data)
        {

            LoadConfiguration();
            //Load Login View
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs, data));
        }

        #region Check if Local config files exists

        private bool CheckForLocalFiles()
        {
            var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
          
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
            }

            var filesLocal = System.IO.Directory.GetFiles(xml_documents);

            if (!filesLocal.Any())
                return false;

            string[] filesToCheck = {
                                    "Alarm.xml",
                                    "DemandConf.xml",
                                    "Global.xml",
                                    "Interface.xml",
                                    "Meter.xml",
                                    "Mtu.xml"
                                 };

            int ContOfChecks = 0;

            foreach (var file in filesLocal)
            {
                foreach (string checkStr in filesToCheck)
                {
                    string compareStr = checkStr;
                    compareStr = compareStr.Replace(xml_documents, "");

                    string fileStr = file.ToString();
                    fileStr = fileStr.Replace(xml_documents + "/", "");
                    
                    if (fileStr.Equals(compareStr))
                    {
                        ContOfChecks++;
                    }
                }
            }

            if (ContOfChecks < filesToCheck.Length)
            {
                return false;
            }

            return true;
        }

        #endregion


    #endregion

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

        public void HandleUrl(Uri url)
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


                if (script_name != null)
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
