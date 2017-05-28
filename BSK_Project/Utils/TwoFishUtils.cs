
using System;
using System.IO;
using System.Security;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using static BSK_Project.CipherMode;

namespace BSK_Project
{
    internal class TwoFishUtils
    {



        //public static string TwoFishEncryption(CipherModes mode,string plain, string key, byte[] iv, int subLength)
        //{
        //    BCEngine bcEngine = new BCEngine(Encoding.UTF8);

        //    return bcEngine.Encrypt(mode, plain, key,iv,subLength);
        //}

        public static byte[] TwoFishEncryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Encrypt(mode, plain, key, iv, subLength);
        }

        public static byte[] TwoFishPrivateKeyEncryption(CipherModes mode, string plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Encrypt(mode, plain, key, iv, subLength);
        }

        public static byte[] TwoFishPrivateKeyDecryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Decrypt(mode, plain, key, iv, subLength);
        }
        public static byte[] TwoFishFileEncryption(CipherModes mode, byte[] plain, string key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Encrypt(mode, plain, key, iv, subLength);
        }

        internal static bool SaveData(string fileName, byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            try
            {
                // Create a new stream to write to the file
                var writer = new BinaryWriter(File.Open(fileName, FileMode.Append));
                // Add new line
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                writer.Write(newline, 0, newline.Length);
                // Writer raw data                
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (ArgumentNullException e)
            {
                return false;
            }

            return true;
        }

        public static string ConvertKeyToString(AsymmetricKeyParameter key)
        {
            TextWriter textWriter = new StringWriter();
            var pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(key);
            pemWriter.Writer.Flush();

            var convertedKey = textWriter.ToString();
            return convertedKey;
        }
        
        //public static string TwoFishDecryption(string cipher, string key, bool fips)
        //{
        //    BCEngine bcEngine = new BCEngine(new TwofishEngine(), Encoding.ASCII);
        //    bcEngine.SetPadding(new Pkcs7Padding());
        //    return bcEngine.Decrypt(cipher, key);
        //}

        //public static byte[] TwoFishFileDecryption(byte[] cipher, string key, bool fips)
        //{
        //    BCEngine bcEngine = new BCEngine(new TwofishEngine(), Encoding.ASCII);
        //    bcEngine.SetPadding(new Pkcs7Padding());
        //    return bcEngine.Decrypt(cipher, key);
        //}




    }
}
