using System;
using Android.App;
using Android.Runtime;
using Microsoft.Intune.Mam.Client.App;

namespace aclara_meters.Droid
{
#if DEBUG
    /// <remarks>
    /// Due to an issue with debugging the Xamarin bound MAM SDK the Debuggable = false attribute must be added to the Application in order to enable debugging.
    /// Without this attribute the application will crash when launched in Debug mode. Additional investigation is being performed to identify the root cause.
    /// </remarks>
    [Application(Debuggable = false)]
#else
    [Application(AllowBackup = false, AllowClearUserData = true)]
#endif

    public class MyApplication : MAMApplication
    {
        protected MyApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }


        public override byte[] GetADALSecretKey()
        {
            return null;
        }
    }

}