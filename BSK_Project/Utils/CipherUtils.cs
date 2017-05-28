using Org.BouncyCastle.Crypto;
using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using static BSK_Project.CipherMode;

namespace BSK_Project.Utils
{
    class CipherUtils
    {
        public static IBufferedCipher CreateTwofishCipher(bool forEncrypt, CipherModes mode, byte[] key, byte[] iv = null, int subLength = 64)
        {

            if (key == null)
                throw new Exception("Key must not be null.");
            if (mode != CipherModes.Ecb && iv == null)
                throw new Exception("This mode requires initial value.");
            if ((mode == CipherModes.Cfb || mode == CipherModes.Ofb) && subLength == 0)
                subLength = 16;

            IBufferedCipher cipher;
            var keyParameter = new KeyParameter(key);
            if (iv == null)
            {
                var fileEncryptionService =new FileEncryptionService();
                iv = fileEncryptionService.GenerateKey(128);
            }
            var keyWithIv = new ParametersWithIV(keyParameter, iv);

            switch (mode)
            {
                   case CipherModes.Ecb:
                    cipher = new PaddedBufferedBlockCipher(new TwofishEngine());
                    cipher.Init(forEncrypt, keyParameter);
                    return cipher;
                case CipherModes.Cbc:
                    cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new TwofishEngine()));
                    cipher.Init(forEncrypt, keyWithIv);
                    return cipher;
                case CipherModes.Cfb:
                    cipher = new BufferedBlockCipher(new CfbBlockCipher(new TwofishEngine(), subLength));
                    cipher.Init(forEncrypt, keyWithIv);
                    return cipher;
                case CipherModes.Ofb:
                    cipher = new BufferedBlockCipher(new OfbBlockCipher(new TwofishEngine(), subLength));
                    cipher.Init(forEncrypt, keyWithIv);
                    return cipher;
                default:
                    throw new Exception("Wrong block cipher mode.");
            }
        }

    }
}

