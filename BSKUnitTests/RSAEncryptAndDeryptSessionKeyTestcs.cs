using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSK_Project;
using System.Linq;
using BSK_Project.Utils;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;

namespace BSKUnitTests
{
    [TestClass]
    public class RSAEncryptAndDeryptSessionKeyTestcs
    {
        [TestMethod]
        public void RSAEncDecTest()
        {
            FileEncryptionService service = new FileEncryptionService();
            var sessionKey = service.GenerateKey(128);

            string name = "rsatest";
            var password = HashUtil.GenerateSha256Hash("password");
            UserCreateService userCreateservice = new UserCreateService(name, password);
            userCreateservice.AddUser();

            UserDeleteService deleteUserService = new UserDeleteService(null, name);

            var publicKey = service.GetPublicKey2(name);
            var encryptedSessionKey = service.GetEncryptedByRsaSessionKey(publicKey, sessionKey);

            byte[] privateKey = XmlUtils.GetKey(Constants.PrivateKeysFolderPath + name);
            var privateKeyDecrypted = TwoFishUtils.TwoFishFileDecryption(CipherMode.CipherModes.Ecb, privateKey, password, null, 0);
            AsymmetricKeyParameter keyParameter = null;

            keyParameter = PrivateKeyFactory.CreateKey(privateKeyDecrypted);

            var decryptedSessionKey = service.GetDecryptedByRsaSessionKey(keyParameter, encryptedSessionKey);
            Assert.IsTrue(decryptedSessionKey.SequenceEqual(sessionKey));
            deleteUserService.DeleteUserKeysForTest(Constants.PrivateKeysFolderPath + name, Constants.PublicKeysFolderPath + name);
        }
    }
}
