using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using static BSK_Project.CipherMode;
using BSK_Project.Utils;

namespace BSK_Project
{
    public class BCEngine
    {
        private readonly Encoding _encoding;

        public BCEngine(Encoding encoding)
        {

            _encoding = encoding;
        }

        public byte[] EncryptWithMode(CipherModes mode, byte[] input, byte[] key, byte[] iv, int sublength)
        {

            return BouncyCastleCrypto(true, mode, input, key, iv, sublength);
        }
        private byte[] BouncyCastleCrypto(bool forEncrypt, CipherModes mode, byte[] input, byte[] key, byte[] iv, int subLength)
        {
            byte[] result;
            var cipher = CipherUtils.CreateTwofishCipher(forEncrypt, mode, key, iv, subLength);
            byte[] output = new byte[cipher.GetOutputSize(input.Length)];
            int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);
            cipher.DoFinal(output, len);
            return output;
            //if (forEncrypt)
            //{
            //    byte[] _in = input;
            //    byte[] _out = new byte[cipher.GetOutputSize(_in.Length)];
            //    int len1 = cipher.ProcessBytes(_in, 0, _in.Length, _out, 0);
            //    cipher.DoFinal(_out, len1);
            //    result = _out;
            //}
            //else
            //{
            //    byte[] _in = input;
            //    byte[] temp = new byte[cipher.GetOutputSize(_in.Length)];
            //    int len = cipher.ProcessBytes(_in, 0, _in.Length, temp, 0);
            //    len += cipher.DoFinal(temp, len);

            //    result = TransferBytes(len, temp);
            //    //cipher.ProcessBytes(input);
            //    //return cipher.DoFinal(input);

            //}
            //return result;


        }

        //public byte[] TransferBytes(int size, byte[] bytesIn)
        //{
        //    byte[] result = new byte[size];

        //    for (int i = 0; i < size; i++)
        //    {
        //        result[i] = bytesIn[i];
        //    }

        //    return result;
        //}
        public string Encrypt(CipherModes mode, string plain, string key, byte[] iv, int sublength)
        {
            byte[] result = BouncyCastleCrypto(true, mode, _encoding.GetBytes(plain), _encoding.GetBytes(key), iv, sublength);
            return Convert.ToBase64String(result);
        }
        public byte[] Encrypt(CipherModes mode, string plain, byte[] key, byte[] iv, int sublength)
        {
            return BouncyCastleCrypto(true, mode, _encoding.GetBytes(plain), key, iv, sublength);
        }
        public byte[] Encrypt(CipherModes mode, byte[] plain, string key, byte[] iv, int sublength)
        {
            return BouncyCastleCrypto(true, mode, plain, _encoding.GetBytes(key), iv, sublength);
        }
        public byte[] Encrypt(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int sublength)
        {
            return BouncyCastleCrypto(true, mode, plain, key, iv, sublength);
        }


        //public string Decrypt(string cipher, string key)
        //{
        //    byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher), key);
        //    return _encoding.GetString(result);
        //}

        public byte[] Decrypt(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            return BouncyCastleCrypto(false, mode, plain, key, iv, subLength);
        }
    }
}
