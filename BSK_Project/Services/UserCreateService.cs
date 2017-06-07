using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BSK_Project.Utils;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using static BSK_Project.CipherMode;

namespace BSK_Project
{

    public class UserCreateService
    {
        private readonly string _email;
        private readonly byte[] _password;

        public string GetEmail() => _email;
        public byte[] GetPassword() => _password;

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

            if (File.Exists(publicPath))
            {
                MessageBox.Show("Użytkownik o takiej nazwie już istnieje", "Tworzenie użytkownika", MessageBoxButton.OK, MessageBoxImage.Exclamation); return;
            }
            //save keys to files
            KeyDetails publicKeyDetails = new KeyDetails(Constants.Rsa, Constants.PublicType,
                serializedPublicBytes);
            XmlUtils.CreateXmlKey(publicPath, publicKeyDetails);

            var privateKey = TwoFishUtils.TwoFishPrivateKeyEncryption(CipherModes.Ecb, serializedPrivateBytes, _password, null, 0);
            KeyDetails privateKeyDetails = new KeyDetails(Constants.Rsa, Constants.PrivateType, privateKey);
            XmlUtils.CreateXmlKey(privatePath, privateKeyDetails);

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
