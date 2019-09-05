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
    public partial class DialogsView : Frame
    {
        public DialogsView()
        {
            InitializeComponent();
        }
        
        public void OpenCloseDialog(string dialogName, bool visible)
        {
            StackLayout dialog = (StackLayout)this.FindByName(dialogName);
            dialog.IsVisible = visible;
        }
            
        public StackLayout GetStackLayoutElement(string name)
        {
            return (StackLayout)this.FindByName(name);
        }
        public TapGestureRecognizer GetTGRElement(string buttonName)
        {
            TapGestureRecognizer TGR = (TapGestureRecognizer)this.FindByName(buttonName);
            return TGR;
        }
        public void CloseDialogs()
        {
            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = false;
            dialog_turnoff_three.IsVisible = false;
            dialog_replacemeter_one.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_AddMTU.IsVisible = false;
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_logoff.IsVisible = false;
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            
        }
    }
}