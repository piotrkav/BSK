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
using Org.BouncyCastle.Crypto.Parameters;

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

        public byte[] GetEncryptedByRsaSessionKey(AsymmetricKeyParameter keyParameter, byte[] sessionKey)
        {
            var encryptEngine = new RsaEngine();
            encryptEngine.Init(true, keyParameter);
            var encrypted = encryptEngine.ProcessBlock(sessionKey, 0, sessionKey.Length);


            sessionKey = null;

            return encrypted;
        }

        public byte[] GetDecryptedByRsaSessionKey(AsymmetricKeyParameter keyParameter, byte[] sessionKey)
        {
            var encryptEngine = new RsaEngine();
            encryptEngine.Init(false, keyParameter);
            var decrypted = encryptEngine.ProcessBlock(sessionKey, 0, sessionKey.Length);
            sessionKey = null;
            return decrypted;
        }


    }

}
