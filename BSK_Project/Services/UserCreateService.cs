using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using static BSK_Project.CipherMode;

namespace BSK_Project
{

    internal class UserCreateService
    {
        private readonly string _email;
        private byte[] _password;


        public UserCreateService(string email, byte[] password)
        {
            this._email = email;
            this._password = password;
        }

        public void AddUser()
        {
            //keys
            var keys = GetKeyPair(Constants.RsaKeySize);
            
            //private
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keys.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPrivate = Convert.ToBase64String(serializedPrivateBytes);
            //public
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keys.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPublic = Convert.ToBase64String(serializedPublicBytes);

            
            //paths
            var publicPath = Constants.PublicKeysFolderPath + _email;
            var privatePath = Constants.PrivateKeysFolderPath + _email;
            //save keys to files

            File.WriteAllBytes(publicPath, serializedPublicBytes);

            File.WriteAllBytes(privatePath, serializedPrivateBytes);


            
            //var privateKey = TwoFishUtils.TwoFishPrivateKeyEncryption(CipherModes.Ecb, serializedPrivateBytes, _password, null, 0
            // Console.WriteLine(" privateKey 1");
            // Console.WriteLine(Convert.ToBase64String(privateKey));
            // var key22 =  PrivateKeyFactory.CreateKey(privateKey);
            //privateKey = null;
            // Console.WriteLine("private key" + serializedPrivate);
            // privateKey = null;
            //TwoFishUtils.ConvertKeyToString(publicKey));
            // 


            //CHECK OF READING KEYS FROM FILES

            //var puk = File.ReadAllBytes(publicPath);
            //AsymmetricKeyParameter deserializedKey1 = PublicKeyFactory.CreateKey(puk);
            //         var puk2 = File.ReadAllBytes(privatePath);
            //       FileEncryptionService service = new FileEncryptionService();
            //  var x =  service.GetPrivateKey2(_email);

            //     AsymmetricKeyParameter deserializedKey2 = PrivateKeyFactory.CreateKey(puk2);
            //if (keys.Public.Equals(deserializedKey1))
            //{
            //    Console.WriteLine("TRUE");
            //}
            //else
            //{
            //    Console.WriteLine("FALSE");
            //}
            //if (keys.Private.Equals(deserializedKey2))
            //{
            //    Console.WriteLine("true");
            //}
            //else
            //{
            //    Console.WriteLine("false");
            //}


        }





        public AsymmetricCipherKeyPair GetKeyPair(int rsaKeySize)
        {

            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            SecureRandom secureRandom = new SecureRandom(randomGenerator);
            //RsaKeySize represents strength given in bits
            var keyGenerationParameters = new KeyGenerationParameters(secureRandom, rsaKeySize);

            var keyPairGenerator = new RsaKeyPairGenerator();

            keyPairGenerator.Init(keyGenerationParameters);
            var keys = keyPairGenerator.GenerateKeyPair();
            return keys;
        }



    }
}
