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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aclara_meters
{
    public partial class FormsApp : Application
    {
        public static string AppName { get { return "StoreAccountInfoApp"; } }
        public static ICredentialsService CredentialsService { get; private set; }
        public static BleSerial ble_interface;

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

            //Cargar la pantalla principal
            MainPage = new NavigationPage(new AclaraViewLogin(dialogs));
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
