using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using Xamarin.Forms;
using Plugin.Geolocator;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using Renci.SshNet;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.Generic;

namespace aclara_meters.view
{
    public partial class AclaraViewLogin : ContentPage
    {
        public viewmodel.LoginMenuViewModel viewModel;

        public AclaraViewLogin()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // This prevents a user from being able to hit the back button and leave the login page.
            return true;
        }

       

        public AclaraViewLogin(IUserDialogs dialogs, string data)
        {
            InitializeComponent();
            Settings.IsNotConnectedInSettings = false;
            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(dialogs);
            viewModel.Navigation = this.Navigation;

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            loginpage.IsVisible = false;
            Task.Run(async () =>
            {
                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    loginpage.IsVisible = true;
                    Console.WriteLine("Data: " + data);
                });
            });


            this.EmailEntry.Focused += (s, e) =>
            {
                SetLayoutPosition(true, (int)-20);
            };

            this.EmailEntry.Unfocused += (s, e) =>
            {
                SetLayoutPosition(false, (int)-20);
                Task.Factory.StartNew(CertsTask);

            };

            this.PasswordEntry.Focused += (s, e) =>
            {
                SetLayoutPosition(true, (int)-80);
            };

            this.PasswordEntry.Unfocused += (s, e) =>
            {
                SetLayoutPosition(false, (int)-80);
            };


        }

        public AclaraViewLogin(IUserDialogs dialogs)
        {
            InitializeComponent();
            Settings.IsNotConnectedInSettings = false;
            BindingContext = viewModel = new viewmodel.LoginMenuViewModel(dialogs);
            viewModel.Navigation = this.Navigation;

            //Turn off the Navigation bar
            NavigationPage.SetHasNavigationBar(this, false); 

            loginpage.IsVisible = false;
            Task.Run(async () =>
            {
                await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                {
                    loginpage.IsVisible = true;
                    /*
                    if(IsLocationAvailable()){

                        Task.Run(async () => { await StartListening(); });
                        //ListSFTPDataFiles();
                    }
                    */
                });
            });


            this.EmailEntry.Focused += (s, e) => 
            { 
                SetLayoutPosition(true, (int) -20); 
            };

            this.EmailEntry.Unfocused += (s, e) => 
            { 
                SetLayoutPosition(false, (int)-20); 
                Task.Factory.StartNew(CertsTask);


            };


            this.PasswordEntry.Focused += (s, e) => 
            { 
                SetLayoutPosition(true, (int) -80);
            };

            this.PasswordEntry.Unfocused += (s, e) => 
            { 
                SetLayoutPosition(false, (int) -80); 
            };






           


        }

        private void CertsTask()
        {
            /* */

            Console.WriteLine("\r\nExists Certs Name and Location");

            Console.WriteLine("------ ----- -------------------------");
            foreach (StoreLocation storeLocation in (StoreLocation[])
                Enum.GetValues(typeof(StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])
                    Enum.GetValues(typeof(StoreName)))
                {
                    X509Store store = new X509Store(storeName, storeLocation);

                    X509Certificate2Collection collection = store.Certificates;

                    Console.WriteLine("\r\n Collection: " + store.Name);

                    foreach(var cert in store.Certificates){
                        Console.WriteLine(cert.Subject);
                    }
                  


                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);



                        Console.WriteLine("Yes    {0,4}  {1}, {2}",
                            store.Certificates.Count, store.Name, store.Location);
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("No           {0}, {1}",
                            store.Name, store.Location);
                    }
                }
                Console.WriteLine();
            }

            /* */

            /*  ---Install---
             * string file; // Contains name of certificate file
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(file)));
                store.Close();
             * */

            var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            if (Device.RuntimePlatform == Device.Android)
            {
                xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
            }

            string text = File.ReadAllText((Path.Combine(xml_documents, "public.cert"))); // relative path




            byte[] certBuffer = GetBytesFromPEM(text, "Certificate");
            byte[] keyBuffer = GetBytesFromPEM(text, "RsaPrivateKey");


            X509Certificate2 certificate = new X509Certificate2(certBuffer);
            RSACryptoServiceProvider prov = DecodeRSAPrivateKey(keyBuffer);
            certificate.PrivateKey = prov;


            string mensaje = "Cifra este mensaje";
            byte[] bytes_mensaje = Encoding.ASCII.GetBytes(mensaje);
            byte[] datos_cifrados = EncryptDataOaepSha1(certificate, bytes_mensaje);
            byte[] datos_descifrados = DecryptDataOaepSha1(certificate, datos_cifrados);

            string value = Encoding.ASCII.GetString(datos_descifrados);


        }

        //------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }


        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)     //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
              if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }


        public static byte[] GetBytesFromPEM(string pemString, string type)
        {
            string header; string footer;
            switch (type)
            {
                case "Certificate":
                    header = "-----BEGIN CERTIFICATE-----";
                    footer = "-----END CERTIFICATE-----";
                    break;
                case "RsaPrivateKey":
                    header = "-----BEGIN RSA PRIVATE KEY-----";
                    footer = "-----END RSA PRIVATE KEY-----";
                    break;
                default:
                    return null;
            }

            int start = pemString.IndexOf(header) + header.Length;
            int end = pemString.IndexOf(footer, start) - start;
            return Convert.FromBase64String(pemString.Substring(start, end));
        }



        private static X509Certificate2 GetSigningCertificate(string subject)
        {
            X509Certificate2 theCert = null;
            foreach (StoreName name in Enum.GetValues(typeof(StoreName)))
            {
                foreach (StoreLocation location in Enum.GetValues(typeof(StoreLocation)))
                {
                    var store = new X509Store(name, location);
                    store.Open(OpenFlags.ReadOnly);
                    foreach (X509Certificate2 cert in store.Certificates)
                    {
                        if (cert.Subject.ToLower().Contains(subject.ToLower()) && cert.HasPrivateKey)
                        {
                            theCert = cert;
                            break;
                        }
                    }
                    store.Close();
                }
            }
            if (theCert == null)
            {
                throw new Exception(
                    String.Format("No certificate found containing a subject '{0}'.",
                                  subject));
            }

            return theCert;
        }



        public static byte[] EncryptDataOaepSha1(X509Certificate2 cert, byte[] data)
        {
            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (RSA rsa = cert.GetRSAPublicKey())
            {
                // OAEP allows for multiple hashing algorithms, what was formermly just "OAEP" is
                // now OAEP-SHA1.
                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }


        public static byte[] DecryptDataOaepSha1(X509Certificate2 cert, byte[] data)
        {
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (RSA rsa = cert.GetRSAPrivateKey())
            {
                return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }

        void SetLayoutPosition(bool onFocus, int value)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.loginpage.TranslateTo(0, value, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.loginpage.TranslateTo(0, value, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.loginpage.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.loginpage.TranslateTo(0, 0, 50);
                }
            }
        }

        private void ListSFTPDataFiles()
        {
            //string host = "192.168.1.39";
            string host = "169.254.130.57";
            string username = "ma.jimenez";
            string password = "Ingen167";
            string pathRemoteFile = "/Users/ma.jimenez/Desktop/xmltest/User.xml";

            // Path where the file should be saved once downloaded (locally)
            string pathLocalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "User.txt");

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    Console.WriteLine("Downloading {0}", pathRemoteFile);

                    //var files = sftp.ListDirectory(remoteDirectory);
                    //  foreach (var file in files)
                    //  {
                    // Console.WriteLine(file.Name);
                    //   }
                    //    sftp.Disconnect();

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                    }

                    sftp.Disconnect();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught " + e.ToString());
                }
            }
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;
            //CrossGeolocator.Current.DesiredAccuracy = 1;
            CrossGeolocator.Current.DesiredAccuracy = 5;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);
            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            Debug.WriteLine(output);
            accuracy.Text = output.ToString();
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
        }

        private async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }
}
