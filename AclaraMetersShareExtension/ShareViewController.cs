using System;
using System.Diagnostics;
using Foundation;
using MobileCoreServices;
using Social;
using UIKit;

namespace AclaraMetersShareExtension
{
    public partial class ShareViewController : SLComposeServiceViewController
    {
        protected ShareViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.

            UIApplication.SharedApplication.BeginInvokeOnMainThread(() => { DidSelectPost(); });

        }

        //public override bool IsContentValid()
        //  {
            // Do validation of contentText and/or NSExtensionContext attachments here
        //      return true;
        //   }

        public override void DidSelectPost()
        {
            // This is called after the user selects Post. Do the upload of contentText and/or NSExtensionContext attachments.

            // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
            ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);

            var item = ExtensionContext.InputItems[0];

            NSItemProvider prov = null;
            if (item != null) prov = item.Attachments[0];
            if (prov != null)
            {
                prov.LoadItem(UTType.URL, null, (NSObject url, NSError error) =>
                {
                    if (url == null) return;
                    NSUrl newUrl = (NSUrl)url;

                    InvokeOnMainThread(() =>
                    {
                        //String encode1 = System.Web.HttpUtility.UrlEncode("thirdpartyappurlscheme://?param=" + rece);

                        NSUrl request = new NSUrl("aclara-mtu-programmer://?script_path=" + newUrl);

                        try
                        {
                            bool isOpened = UIApplication.SharedApplication.OpenUrl(request);

                            if (isOpened == false)
                                UIApplication.SharedApplication.OpenUrl(new NSUrl("aclara-mtu-programmer://?script_path=" + newUrl));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Cannot open url: {0}, Error: {1}", request.AbsoluteString, ex.Message);
                            var alertView = new UIAlertView("Error", ex.Message, null, "OK", null);

                            alertView.Show();
                        }
                    });
                });
            }
        }

        public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
        {
            // To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
            return new SLComposeSheetConfigurationItem[0];
        }
    }
}
