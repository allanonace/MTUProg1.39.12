using System;
using System.Threading;
using Library;
using Microsoft.Intune.MAM;

namespace aclara_meters.iOS
{
    public static class MAMLogin
    {
       
        public static void LoginUserMAM()
        {
            try
            {
                IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;
                if (string.IsNullOrEmpty(value.PrimaryUser))
                    IntuneMAMEnrollmentManager.Instance.LoginAndEnrollAccount(null);

                Thread.Sleep(5000);
            }
            catch(Exception e)
            {
                Utils.Print($"Enrollment exceptions: {e.ToString()}");
            }
        }
    }
}
