using System;
using System.ComponentModel;
using aclara_meters;
using aclara_meters.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace aclara_meters.iOS
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool alignRight = ( ( BorderlessEntry )sender ).FlowDirection == FlowDirection.RightToLeft;
            
            if ( alignRight &&
                 e.PropertyName.Equals ( "Text" ) )
            {
                BorderlessEntry tbx = ( BorderlessEntry )sender;
                
                if ( ! tbx.Flag )
                {
                    string text = tbx.Text;
                    
                    // When the string has only one character, doesn't perform
                    // additional invocation after set Text property
                    tbx.Flag = ( text.Length > 1 );

                    // Add
                    if ( tbx.PrevValue.Length < text.Length )
                    {
                        // Move last character from right to left ( e.g. 2341 -> 1234 )
                        text = text.Substring ( text.Length - 1, 1 ) +
                               text.Substring ( 0, text.Length - 1 );
                    }
                    // Remove
                    else
                    {
                        // Remove from the left of the string ( e.g. 1234 -> 234 )
                        text = tbx.PrevValue.Substring ( tbx.PrevValue.Length - text.Length );
                    }
                    
                    tbx.Text      = text;
                    tbx.PrevValue = text;
                }
                // Reset flag
                else tbx.Flag = false;
            }
        
            base.OnElementPropertyChanged ( sender, e );

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle       = UITextBorderStyle.None;
            //Control.TextAlignment     = ( alignRight ) ? UITextAlignment.Right : UITextAlignment.Left;
        }
    }
}
