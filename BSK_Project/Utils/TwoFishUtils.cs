
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


        //ENCRYPTION
        public static byte[] TwoFishEncryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Encrypt(mode, plain, key, iv, subLength);
        }

       
       



        public static byte[] TwoFishFileEncryption(CipherModes mode, byte[] plain, string key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Encrypt(mode, plain, key, iv, subLength);
        }
        

        //DECRYPTION
        public static byte[] TwoFishFileDecryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Decrypt(mode, plain, key, iv, subLength);

        }
        public static byte[] TwoFishPrivateKeyDecryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
        {
            BCEngine bcEngine = new BCEngine(Encoding.ASCII);

            return bcEngine.Decrypt(mode, plain, key, iv, subLength);
        }

        public static byte[] TwoFishPrivateKeyEncryption(CipherModes mode, byte[] plain, byte[] key, byte[] iv, int subLength)
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
            string result = "";
            StringBuilder sb=  new StringBuilder(result);
            TextWriter textWriter = new StringWriter(sb);
            var pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(key);
            pemWriter.Writer.Flush();

            return result;
        }

    }
}
