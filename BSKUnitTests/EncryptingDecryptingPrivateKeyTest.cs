using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSK_Project;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using System.Linq;

namespace BSKUnitTests
{
    [TestClass]
    public class EncryptingDecryptingPrivateKeyTest
    {
        [TestMethod]
        public void EncryptAndDecryptPrivateKetTest()
        {
            string name = "edtest";
            byte[] password = HashUtil.GenerateSha256Hash("password");
            UserCreateService service = new UserCreateService(name, password);

            var keys = service.GetKeyPair(Constants.RsaKeySize);

            //private
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keys.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPrivate = Convert.ToBase64String(serializedPrivateBytes);

            var encryptedBytes = TwoFishUtils.TwoFishPrivateKeyEncryption(CipherMode.CipherModes.Ecb, serializedPrivateBytes, password, null, 0);
            var decryptedBytes = TwoFishUtils.TwoFishPrivateKeyDecryption(CipherMode.CipherModes.Ecb, encryptedBytes, password, null, 0);

            Assert.IsTrue(decryptedBytes.SequenceEqual(serializedPrivateBytes));
        }
    }
}
