using System;
using Security;

namespace aclara_meters.iOS
{
    public class TEST
    {
        public static void Test ()
        {
            var foundKey = GetKeyFromKeyChain ();

            /*
            var query = new SecRecord (SecKind.InternetPassword) {
                Server  = "bugzilla.novell.com",
                Account = "miguel"
            };

            var password = SecKeyChain.QueryAsData (query);
            Console.WriteLine ("The password for the account is: {0}", password);
            */
        }

        public static SecKey GetKeyFromKeyChain ()
        {
            // SecRecord
            // https://docs.microsoft.com/en-us/dotnet/api/security.secrecord?view=xamarin-ios-sdk-12
            // Tracks a set of properties from the keychain

            // SecKeyChain.QueryAsConcreteType
            // https://docs.microsoft.com/en-us/dotnet/api/security.seckeychain.queryasconcretetype?view=xamarin-ios-sdk-12
            // Use this method to query the KeyChain and get back a SecCertificate, a SecKey or a SecIdentity

            // SecCertificate
            // https://docs.microsoft.com/en-us/dotnet/api/security.seccertificate?view=xamarin-ios-sdk-12
            // Represents digital certificates on iOS/OSX

            /*
            using ( SecRecord rec = new SecRecord ( SecKind.Certificate ) )
            {

            }
            */

            /*
            <CertSubject>New-Test-Dev-Aclara</CertSubject>
            <CertPath>Root</CertPath>
            <CertPair>false</CertPair>
            <CertUpdate>6/17/2015</CertUpdate>
            <CertValid>4/22/2025 12:00:00 AM</CertValid>
            <CertRecord>true</CertRecord>
            */

            var foundKey = SecKeyChain.QueryAsConcreteType (
                new SecRecord ( SecKind.Certificate )
                {
                    //Path = "Root"
                    //Account = "New-Test-Dev-Aclara"
                    
                    

                    // Ni con Server ni con Label ha funcionado...
                    // = "New-Test-Dev-Aclara", //"StarSystemHeadEnd-ALP-nonprod.socalgas.com"
                    
                }, out SecStatusCode errCode );

            Console.WriteLine ( "Cert: Null? " + ( foundKey == null ) + " , Error: " + errCode );

            if ( foundKey == null || errCode != SecStatusCode.Success )
                return null;
            return foundKey as SecKey;
        }
    }
}
