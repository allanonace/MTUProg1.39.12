using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using System.Security.Cryptography.X509Certificates;

namespace MTUComm
{
    public class MtuSha256
    {
        static UInt32 [] SHA_K = 
        {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        UInt32 [] SHA_H = new UInt32[8];
        UInt32 [] SHA_M = new UInt32[16];

        #region SHA Ulitity Functions       
        
        /* Two of six logical functions used in SHA-256, SHA-384, and SHA-512: */
        private UInt32 Ch(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y)^((~x) & z);
        }
        private UInt32 Maj(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        /* 32-bit Rotate-right (used in SHA-256): */
        private UInt32 ROTR(int n, UInt32 x)
        {
            return (x >> n) | (x << (32 - n));
        }

        /* Shift-right (used in SHA-256, SHA-384, and SHA-512): */
        private UInt32 SHR(int n, UInt32 x)
        {
            return (x >> n);
        }

        /* Four of six logical functions used in SHA-256: */
        private UInt32 BigSig0(UInt32 x)
        {
            return ROTR(2, x) ^ ROTR(13, x) ^ ROTR(22, x);
        }

        private UInt32 BigSig1(UInt32 x)
        {
            return ROTR(6, x) ^ ROTR(11, x) ^ ROTR(25, x);
        }

        private UInt32 SmaSig0(UInt32 x)
        {
            return ROTR(7, x) ^ ROTR(18, x) ^ SHR(3, x);
        }
        private UInt32 SmaSig1(UInt32 x)
        {
            return ROTR(17, x) ^ ROTR(19, x) ^ SHR(10, x);
        }
        
        #endregion

        /// <summary>
        /// Perform a SHA 256 hash on a message of variable length. This function will pad the message 
        /// to the desired 512 bit block and then perform the SHA algorithm on the block. 
        /// Displaying the result in the console window. 
        /// </summary>
        /// <param name="buff"> byte array to perform the SHA 256 hash on</param>
        public void GenerateSHAHash(byte [] buff, out byte [] shaHash)
        {
            int i, k, bufsize;
            UInt16 c;
            //byte [] buff1;
            
            // default values for the padding
            k=0;
            c=0x80;

            // detemerine the size of the buffer in bytes
            bufsize = buff.Length;

            // add the padding to the buffer to generate the 16 32 byte message
            // a value of 1 will be added to the last byte of the message and 
            // the length of the message in bytes will be the
            for (i = 0; i <64;i++ )
            {
                // clear every new dword
                if((i & 3)==0) SHA_M[i>>2]=0;

                // load the message into 32 byte values
                if(i<bufsize)
                {
                    // shift the value to the left to add the next value
                    SHA_M[i>>2] = (SHA_M[i>>2] << 8) + buff[i];
                    // increase the bit counter by 8 to account for the new byte 
                    k += 8;
                }
                else
                {
                    // load a '1' at the end of the message
                    if (i<62)
                    {
                        SHA_M[i>>2] = (SHA_M[i>>2] << 8) + c;
                        c=0;
                    }
                    else if (i<63)
                    {
                        // load the high byte of the bit counter into the second to last message
                        SHA_M[i>>2] = ((UInt32)(SHA_M[i>>2] << 8)) + ((UInt32)(k >> 8));
                    }
                    else
                    {
                        // load the low byte of the bit counter into the last byte of the message
                        SHA_M[i>>2] = ((UInt32)(SHA_M[i>>2] << 8)) + ((UInt32)(k & 0xff));
                    }
                }
            }

            // perform the hash after the message has been padded
            SHA256_HASH();

            List<byte> tempsha = new List<byte>();
            // convert the sha to byte array
            foreach (UInt32 val in SHA_H)
            {
                tempsha.AddRange(BitConverter.GetBytes(val));
            }

            shaHash = tempsha.ToArray();
        }

        /// <summary>
        /// SHA 256 algorithm derived from the NIST approved aglorithm
        /// </summary>
        public void SHA256_HASH()
        {
            UInt32 [] SHA_W = new UInt32[16];
            UInt32 a, b, c, d, e, f, g, h;
            UInt32 T1, T2;

            int t, t1;

            // default the hash values
            a = SHA_H[0] = 0x6a09e667;
            b = SHA_H[1] = 0xbb67ae85;
            c = SHA_H[2] = 0x3c6ef372;
            d = SHA_H[3] = 0xa54ff53a;
            e = SHA_H[4] = 0x510e527f;
            f = SHA_H[5] = 0x9b05688c;
            g = SHA_H[6] = 0x1f83d9ab;
            h = SHA_H[7] = 0x5be0cd19;

            // perform the round 64 times
            for (t1=0; t1<4; t1++)
            {
                for (t = 0; t < 16; t++)
                {
                    if (t1 == 0)
                    {
                        SHA_W[t] = SHA_M[t];
                    }
                    else
                    {
        //                SHA_W[t] = SmaSig1(SHA_W[(t - 2) & 0x0f]) + SHA_W[(t - 7) & 0x0f] + SmaSig0(SHA_W[(t - 15) & 0x0f]) + SHA_W[t];
                        SHA_W[t] = SmaSig1(SHA_W[(t + 14) & 0x0f]) + SHA_W[(t + 9) & 0x0f] + SmaSig0(SHA_W[(t + 1) & 0x0f]) + SHA_W[t];
                    }
                    T1 = h + BigSig1(e) + Ch(e, f, g) + SHA_K[t+t1*16] + SHA_W[t];
                    T2 = BigSig0(a) + Maj(a, b, c);
                    h = g;
                    g = f;
                    f = e;
                    e = d + T1;
                    d = c;
                    c = b;
                    b = a;
                    a = T1 + T2;
                }
            }

            SHA_H[0] += a;
            SHA_H[1] += b;
            SHA_H[2] += c;
            SHA_H[3] += d;
            SHA_H[4] += e;
            SHA_H[5] += f;
            SHA_H[6] += g;
            SHA_H[7] += h;  
        }

        private static string kCertificatePassword = "pass4y0u";//"@CL@r@";
        
        // test ->
        public string GetKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;
            
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);
            return rsa.ToString();
        }
        // test <-

        public byte[] RandomBytes ( int bytes )
        {
            // Create a byte array to hold the random value
            byte[] randomNumber = new byte[ bytes ];

            // Create a new instance of the RNGCryptoServiceProvider
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider ();

            // Fill the array with a random value
            Gen.GetBytes ( randomNumber );

            return randomNumber;
        }

        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }
        }
        
        /// <summary>
        /// Encrypt Bytes using only the Public Key bytes
        /// </summary>
        /// <param name="bytesToEncrypt">Bytes to encrypt</param>
        /// <param name="publicKey">Public key bytes</param>
        /// <returns></returns>
        public /*static*/ byte[] encryptBytes(byte[] bytesToEncrypt, byte[] publicKeyInfo)
        {
            // Create RSA Provider
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();

            // Import Public Key
            rsaProvider.ImportCspBlob(publicKeyInfo);
            
            // Encrypt using PKCS#1 v1.5 padding
            return rsaProvider.Encrypt(bytesToEncrypt, false);
        }
        
        public byte[] encryptBytes(byte[] bytesToEncrypt, X509Certificate2 cert )
        {
            // Create RSA Provider
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();

            // Import Public Key
            //rsaProvider.ImportCspBlob(publicKeyInfo);
            
            rsaProvider = cert.GetRSAPublicKey () as RSACryptoServiceProvider;
            
            // Encrypt using PKCS#1 v1.5 padding
            return rsaProvider.Encrypt(bytesToEncrypt, false);
        }
    }
}
