using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace aclara_meters
{
    // NOTE: Xamarin.IOS 12.2.1.16+ does not support Xamarin.Auth AccountStore ( is deprecated ) -> Moved to Xamarin.Essentials SecureStorage
    public class CredentialsService
    {
        private const string USER = "user";
        private const string PASS = "pass";

        private string userName;
        private string password;

        public string UserName
        {
            private set { userName = value; }
            get { return userName; }
        }
        
        public string Password
        {
            private set { password = value; }
            get { return password; }
        }

        public async Task SaveCredentials (
            string user,
            string pass )
        {
            try
            {
                UserName = user;
                Password = pass;
                await SecureStorage.SetAsync ( USER, UserName );
                await SecureStorage.SetAsync ( PASS,  Password );
            }
            catch ( Exception )
            {
                //...
            }
        }
        
        private async Task<string> GetUserName ()
        {
            string name = string.Empty;
        
            try
            {
                name = await SecureStorage.GetAsync ( USER );
            }
            catch ( Exception )
            {
                //...
            }
            
            return name;
        }
        
        private async Task<string> GetPassword ()
        {
            string pass = string.Empty;
        
            try
            {
                pass = await SecureStorage.GetAsync ( PASS );
            }
            catch ( Exception )
            {
                //...
            }
            
            return pass;
        }

        public void DeleteCredentials ()
        {
            SecureStorage.Remove ( USER );
            SecureStorage.Remove ( PASS );
        }

        public async Task<bool> CredentialsExist ()
        {
            string user = await this.GetUserName ();
        
            if ( ! string.IsNullOrEmpty ( user ) )
            {
                UserName = user;
                Password = await this.GetPassword ();
                
                return true;
            }
            return false;
        }
    }
}
