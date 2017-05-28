using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
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
            var keys = GetKeyPair(Constants.RsaKeySize);
            var publicKey = keys.Public;

            var publicPath = Constants.PublicKeysFolderPath + _email;
            var privatePath = Constants.PrivateKeysFolderPath + _email;
            File.WriteAllText(publicPath, TwoFishUtils.ConvertKeyToString(publicKey));

            var privateKey = TwoFishUtils.TwoFishPrivateKeyEncryption(CipherModes.Ecb, TwoFishUtils.ConvertKeyToString(keys.Private), _password,null,0);
            _password = null;
            TwoFishUtils.SaveData(privatePath, privateKey);
            privateKey = null;

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
