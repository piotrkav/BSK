using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace BSK_Project
{
    internal class FileEncryptionService
    {

        public byte[] GenerateKey(int size)
        {
            var sessionKeyGenerator = new RNGCryptoServiceProvider();
            //in bits
            var key = new byte[size / Constants.ByteSize];
            sessionKeyGenerator.GetBytes(key);
            return key;
        }

        public AsymmetricKeyParameter GetPublicKey(string email)
        {
            var fileStream = File.OpenText(Constants.PublicKeysFolderPath + email);
            var pemReader = new PemReader(fileStream);
            var keyParameter = (AsymmetricKeyParameter)pemReader.ReadObject();
            return keyParameter;
        }

        public AsymmetricKeyParameter GetPublicKey2(string email)
        {

            byte[] publicKey = File.ReadAllBytes(Constants.PublicKeysFolderPath + email);

            var deserializedKey = PublicKeyFactory.CreateKey(publicKey);

            return deserializedKey;

        }

        public AsymmetricKeyParameter GetPrivateKey(string email)
        {
            var fileStream = File.OpenText(Constants.PrivateKeysFolderPath + email);
            var pemReader = new PemReader(fileStream);
            var keyParameter = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            return keyParameter.Private;
        }

        public AsymmetricKeyParameter GetPrivateKey2(string email)
        {
            byte[] privateKey = File.ReadAllBytes(Constants.PrivateKeysFolderPath + email);
            var deserializedKey = PrivateKeyFactory.CreateKey(privateKey);
            return deserializedKey;

        }

        public byte[] GetEncryptedByRsaSessionKey(AsymmetricKeyParameter keyParameter, byte[] sessionKey)
        {
            var encryptEngine = new RsaEngine();
            encryptEngine.Init(true, keyParameter);
            var encrypted = encryptEngine.ProcessBlock(sessionKey, 0, sessionKey.Length);


            sessionKey = null;

            return encrypted;
        }

        public byte[] GetDecryptedByRsaSessionKey(AsymmetricKeyParameter keyParameter, byte[] key)
        {
            var encryptEngine = new RsaEngine();
            encryptEngine.Init(false, keyParameter);
            var decrypted = encryptEngine.ProcessBlock(key, 0, key.Length);
            key = null;
            return decrypted;
        }


    }

}
