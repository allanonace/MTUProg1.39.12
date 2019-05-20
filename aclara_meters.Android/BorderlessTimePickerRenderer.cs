using System;
using aclara_meters;
using aclara_meters.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(BorderlessTimePicker), typeof(BorderlessTimePickerRenderer))]
namespace aclara_meters.Droid
{
    public class BorderlessTimePickerRenderer : TimePickerRenderer
    {
        public BorderlessTimePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}
