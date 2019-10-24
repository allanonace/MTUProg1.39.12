using aclara_meters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static MTUComm.Action;
using aclara_meters.util;
using Library;
using MTUComm;

namespace aclara_meters.view
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : RelativeLayout
    {
     
        private List<PageItem> MenuList { get; set; }
        public MenuView()
        {
            InitializeComponent();


            if (FormsApp.credentialsService.UserName != null)
            {
                userName.Text = FormsApp.credentialsService.UserName;

            }
            LoadMTUData();

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadTabletUI);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadPhoneUI);
                });
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                backmenu.Scale = 1.42;

            }
        }

        public TapGestureRecognizer GetTGRElement(string buttonName)
        {
            TapGestureRecognizer TGR = (TapGestureRecognizer)this.FindByName(buttonName);
            return TGR;
        }
        public Image GetImageElement(string imageName)
        {
            Image element = (Image)this.FindByName(imageName);
            return element;
        }
        public ListView GetListElement(string listName)
        {
            ListView element = (ListView)this.FindByName(listName);
            return element;
        }
        private void LoadMTUData()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", Color = "White", TargetType = ActionType.ReadMtu });

            if (Singleton.Get.Configuration.Global.ShowTurnOff)
                MenuList.Add(new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", Color = "White", TargetType = ActionType.TurnOffMtu });

            if (Singleton.Get.Configuration.Global.ShowAddMTU)
                MenuList.Add(new PageItem() { Title = "Add MTU", Icon = "addMTU.png", Color = "White", TargetType = ActionType.AddMtu });

            if (Singleton.Get.Configuration.Global.ShowReplaceMTU)
                MenuList.Add(new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", Color = "White", TargetType = ActionType.ReplaceMTU });

            if (Singleton.Get.Configuration.Global.ShowReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", Color = "White", TargetType = ActionType.ReplaceMeter });

            if (Singleton.Get.Configuration.Global.ShowAddMTUMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Add Meter", Icon = "addMTUaddmeter.png", Color = "White", TargetType = ActionType.AddMtuAddMeter });

            if (Singleton.Get.Configuration.Global.ShowAddMTUReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", Color = "White", TargetType = ActionType.AddMtuReplaceMeter });

            if (Singleton.Get.Configuration.Global.ShowReplaceMTUMeter)
                MenuList.Add(new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", Color = "White", TargetType = ActionType.ReplaceMtuReplaceMeter });

            if (Singleton.Get.Configuration.Global.ShowInstallConfirmation)
                MenuList.Add(new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", Color = "White", TargetType = ActionType.MtuInstallationConfirmation });

            if (Singleton.Get.Configuration.Global.ShowValvePosition)
                MenuList.Add(new PageItem() { Title = "Valve Operation", Icon = "valve_operation.png", Color = "White", TargetType = ActionType.RemoteDisconnect });

            if (Singleton.Get.Configuration.Global.ShowDataRead)
                MenuList.Add(new PageItem() { Title = "Historical Read", Icon = "data_read.png", Color = "White", TargetType = ActionType.DataRead });
#if DEBUG
            MenuList.Add(new PageItem() { Title = "Read Fabric", Icon = "readmtu_icon.png", Color = "White", TargetType = ActionType.ReadFabric });
#endif


            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Color = "#6aa2b8", Icon = "" });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;

        }

        private void LoadPhoneUI()
        {
            close_menu_icon.Opacity = 1;
            userName.Scale = 1;
        }

        private void LoadTabletUI()
        {
            close_menu_icon.Opacity = 0;
            userName.Scale = 1.2;
        }

    }
}