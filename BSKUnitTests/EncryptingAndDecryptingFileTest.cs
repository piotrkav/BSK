using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSK_Project;
using System.IO;
using BSK_Project.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Linq;

namespace BSKUnitTests
{
    [TestClass]
    public class EncryptingAndDecryptingFileTest
    {


        [TestMethod]
        public void EncryptAndDecryptFile()
        {
            var name = "piotr";
            var password = HashUtil.GenerateSha256Hash("password");

            UserCreateService service = new UserCreateService(name, password);
            service.AddUser();

            FileEncryptionService fileEncryptionService = new FileEncryptionService();
            var sessionKey = fileEncryptionService.GenerateKey(256);

            File.Create("testSzyfr").Dispose();
            var path = Path.GetFullPath("testSzyfr");
            string message = "test message";
            File.WriteAllBytes(path, Encoding.ASCII.GetBytes(message));
            var bytesToEncrypt = File.ReadAllBytes(path);

            var encryptedFile = TwoFishUtils.TwoFishEncryption(CipherMode.CipherModes.Ecb, bytesToEncrypt, sessionKey, null, 0);

            var publicKey = fileEncryptionService.GetPublicKey2(name);
            var encryptedSessionkey = fileEncryptionService.GetEncryptedByRsaSessionKey(publicKey, sessionKey);

            byte[] privateKey = XmlUtils.GetKey(Constants.PrivateKeysFolderPath + name);
            var privateKeyDecrypted = TwoFishUtils.TwoFishFileDecryption(CipherMode.CipherModes.Ecb, privateKey, password, null, 0);

            AsymmetricKeyParameter keyParameter = null;

            keyParameter = PrivateKeyFactory.CreateKey(privateKeyDecrypted);

            var decryptedSessionKey = fileEncryptionService.GetDecryptedByRsaSessionKey(keyParameter, encryptedSessionkey);
            var fileDone = TwoFishUtils.TwoFishFileDecryption(CipherMode.CipherModes.Ecb,
                encryptedFile, decryptedSessionKey, null, 0);

            Assert.IsTrue(bytesToEncrypt.SequenceEqual(fileDone));
            UserDeleteService deleteService = new UserDeleteService(null, name);
            deleteService.DeleteUserKeysForTest(Constants.PublicKeysFolderPath + name, Constants.PrivateKeysFolderPath + name);

        }
    }
}
