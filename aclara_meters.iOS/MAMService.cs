using System;
using Xamarin.Forms;
using Microsoft.Intune.MAM;
using aclara_meters.util;
using Foundation;
using Library;
using MTUComm;
using System.Linq;

[assembly: Dependency(typeof(aclara_meters.iOS.MAMService))]
namespace aclara_meters.iOS
{

    public class MAMService : IMAMService
    {
        
        void IMAMService.UtilMAMService()
        {
            IntuneMAMAppConfigManager appConfig = IntuneMAMAppConfigManager.Instance;
           // IntuneMAMAppConfig conf = IntuneMAMAppConfig.;
          //  NSDictionary paramsGroups1 =IntuneMAMAppConfig_Extensions.

            IntuneMAMPolicyManager value = IntuneMAMPolicyManager.Instance;

            Utils.Print($"------------ Es ManagementEnabled:  {value.IsManagementEnabled.ToString()}");

            NSDictionary dictionary = value.DiagnosticInformation;
            NSString[] keys = { new NSString(Mobile.ID_DIC_INTUNE) };
            NSDictionary paramsGroups = dictionary.GetDictionaryOfValuesFromKeys(keys);
            //NSDictionary paramsGroups    = IntuneMAMAppConfigManager.Instance.GetDictionaryOfValuesFromKeys(keys);

            //IntuneMAMAppConfig_Extensions.GetFullData(AppConfig);
            NSObject paramsGroup = paramsGroups.ElementAt(0).Value;
        }
    }
}
