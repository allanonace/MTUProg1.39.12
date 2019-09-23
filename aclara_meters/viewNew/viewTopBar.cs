using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aclara_meters.view
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopBar : Grid
    {
        public TopBar()
        {
            InitializeComponent();
            battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

            hamburger_icon.IsVisible = true;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                aclara_logo.Scale = 1.2;
             //   aclara_logo.TranslationX = 42;
            }
        }
        public TapGestureRecognizer GetTGRElement(string buttonName)
        {
            TapGestureRecognizer element = (TapGestureRecognizer)this.FindByName(buttonName);
            return element;
        }
 
        public Image GetImageElement(string imageName)
        {
            Image element = (Image)this.FindByName(imageName);
            return element;
        }
    }
}