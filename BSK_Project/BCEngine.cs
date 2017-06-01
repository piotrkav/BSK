using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Org.BouncyCastle;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using static BSK_Project.CipherMode;
using BSK_Project.Utils;
using Org.BouncyCastle.Crypto.Encodings;

namespace BSK_Project
{
    public class BCEngine
    {
        private readonly Encoding _encoding;

        public BCEngine(Encoding encoding)
        {

            _encoding = encoding;
        }

        private byte[] BouncyCastleCrypto(bool forEncrypt, CipherModes mode, byte[] input, byte[] key, byte[] iv, int subLength)
        {
            byte[] result;
            var cipher = CipherUtils.CreateTwofishCipher(forEncrypt, mode, key, iv, subLength);
            try
            {
                if (forEncrypt)
                {
                    byte[] _in = input;
                    byte[] _out = new byte[cipher.GetOutputSize(_in.Length)];
                    int len1 = cipher.ProcessBytes(_in, 0, _in.Length, _out, 0);
                    cipher.DoFinal(_out, len1);
                    result = _out;
                }
                else
                {
                    byte[] _in = input;
                    byte[] temp = new byte[cipher.GetOutputSize(_in.Length)];
                    int len = cipher.ProcessBytes(_in, 0, _in.Length, temp, 0);
                    len += cipher.DoFinal(temp, len);

                    result = TransferBytes(len, temp);
                    //cipher.ProcessBytes(input);
                    //return cipher.DoFinal(input);

                }
                return result;
            }
            catch (Exception e)
            {
                return null;

                // return BouncyCastleCrypto(for);
                //FileEncryptionService service = new FileEncryptionService();
                //var sk = service.GenerateKey(key.Length*Constants.ByteSize);

                //return BouncyCastleCrypto(false, mode, input, sk, iv, subLength);
            }


        }



        public byte[] TransferBytes(int size, byte[] bytesIn)
        {
            byte[] result = new byte[size];

            for (int i = 0; i < size; i++)
            {
                result[i] = bytesIn[i];
            }

            return result;
        }


        public byte[] Encrypt(CipherModes mode, byte[] plain, string key, byte[] iv, int sublength)
        {
            return BouncyCastleCrypto(true, mode, plain, _encoding.GetBytes(key), iv, sublength);
        }
        public byte[] Encrypt(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int sublength)
        {
            return BouncyCastleCrypto(true, mode, plain, key, iv, sublength);
        }

        public byte[] Decrypt(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            return BouncyCastleCrypto(false, mode, plain, key, iv, subLength);
        }


        private byte[] BouncyCastleCrypto(bool forEncrypt, CipherModes mode, byte[] input, byte[] key, byte[] iv, int subLength, bool error)
        {
            byte[] result = null;
            var cipher = CipherUtils.CreateTwofishCipher(forEncrypt, mode, key, iv, subLength);
            int len = 0;
           // forEncrypt = error;

            if (forEncrypt)
            {

                result = GetBouncyBytes(true, input, cipher);
            }
            else
            {
                result = GetBouncyBytes(false, input, cipher);

            }
            return result;



        }

        public byte[] GetBouncyBytes(bool enc, byte[] input, IBufferedCipher cipher)
        {
            if (enc)
            {
                byte[] _in = input;
                byte[] _out = new byte[cipher.GetOutputSize(_in.Length)];
                int len1 = cipher.ProcessBytes(_in, 0, _in.Length, _out, 0);
                len1 += cipher.DoFinal(_out, len1);
                return TransferBytes(len1, _out);
            }
            else
            {
                byte[] _in = input;
                byte[] temp = new byte[cipher.GetOutputSize(_in.Length)];
                int len2 = cipher.ProcessBytes(_in, 0, _in.Length, temp, 0);
                return TransferBytes(len2, temp);


            }


            // cipher.DoFinal(_out, len1);
        }

        public byte[] Decrypt(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength, bool error)
        {
            return BouncyCastleCrypto(false, mode, plain, key, iv, subLength, error);
        }

    }
}
