using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
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

            UserDeleteService deleteService2 = new UserDeleteService(null, name);
            deleteService2.DeleteUserKeysForTest(Constants.PublicKeysFolderPath + name, Constants.PrivateKeysFolderPath + name);

            UserCreateService service = new UserCreateService(name, password);
            service.AddUser();

            FileEncryptionService fileEncryptionService = new FileEncryptionService();

            var iv = fileEncryptionService.GenerateKey(128);

            const string testPath = "C:\\Users\\Piotr\\Desktop\\WWT15.pdf";
            var bytesToEncrypt = File.ReadAllBytes(testPath);

            foreach (var mode in Enum.GetValues(typeof(CipherMode.CipherModes)).Cast<CipherMode.CipherModes>())
            {
                foreach (var keyLength in Constants.KeyLengths)
                {
                    foreach (var subBlockLength in Constants.SubBlockSizes)
                    {
                        var sessionKey = fileEncryptionService.GenerateKey(keyLength);
                        byte[] encryptedFile;
                        if (mode == CipherMode.CipherModes.Cfb || mode == CipherMode.CipherModes.Ofb)
                        {
                            encryptedFile = TwoFishUtils.TwoFishEncryption(mode, bytesToEncrypt, sessionKey, iv, subBlockLength);
                            Trace.WriteLine("mode " + mode.ToString() + " " + keyLength + " " + subBlockLength);
                        }
                        else
                        {
                            encryptedFile = TwoFishUtils.TwoFishEncryption(mode, bytesToEncrypt, sessionKey, iv, 0);
                            Trace.WriteLine("mode " + mode.ToString() + " " + keyLength);
                        }
                     
                      
                       


                        //  var encryptedFile = TwoFishUtils.TwoFishEncryption(CipherMode.CipherModes.Ecb, bytesToEncrypt, sessionKey, null, 0);

                        var publicKey = fileEncryptionService.GetPublicKey2(name);
                        var encryptedSessionkey =
                            fileEncryptionService.GetEncryptedByRsaSessionKey(publicKey, sessionKey);

                        byte[] privateKey = XmlUtils.GetKey(Constants.PrivateKeysFolderPath + name);
                        var privateKeyDecrypted =
                            TwoFishUtils.TwoFishFileDecryption(CipherMode.CipherModes.Ecb, privateKey, password, null,
                                0);

                        AsymmetricKeyParameter keyParameter = null;

                        keyParameter = PrivateKeyFactory.CreateKey(privateKeyDecrypted);

                        var decryptedSessionKey =
                            fileEncryptionService.GetDecryptedByRsaSessionKey(keyParameter, encryptedSessionkey);
                        var fileDone = TwoFishUtils.TwoFishFileDecryption(mode,
                            encryptedFile, decryptedSessionKey, iv, subBlockLength);

                        Assert.IsTrue(bytesToEncrypt.SequenceEqual(fileDone));
                    }
                }
            }
            UserDeleteService deleteService = new UserDeleteService(null, name);
            deleteService.DeleteUserKeysForTest(Constants.PublicKeysFolderPath + name, Constants.PrivateKeysFolderPath + name);

        }
    }
}
