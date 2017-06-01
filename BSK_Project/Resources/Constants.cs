using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSK_Project
{
    public static class Constants
    {
        public const int RsaKeySize = 2048;

        public static string PublicKeysFolderPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\public_keys\\"));

        public static string PrivateKeysFolderPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\private_keys\\"));

        public const int ByteSize = 8;
        public const string Algorithm = "Algorithm";
        public const string KeySize = "KeySize";
        public const string BlockSize = "BlockSize";
        public const string SubBlockSize = "SubBlockSize";
        public const string Key = "Key";
        public const string CipherMode = "CipherMode";
        public const string IV = "IV";
        public const string ApprovedUsers = "ApprovedUsers";
        public const string User = "User";
        public const string Email = "Email";
        public const string SessionKey = "SessionKey";
        public const string EncryptedFileHeader = "EncryptedFileHeader";
        public static int[] KeyLengths = { 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256 };
        public static int[] SubBlockSizes = { 8, 16, 24, 32, 40, 48, 56, 64 };
        public const string TwoFish = "Twofish";
        public const string Content = "Content";
        public const string Rsa = "RSA";
        public const string PublicType = "Public";
        public const string PrivateType = "Private";
        public const string KeyType = "KeyType";
        public const string KeyValue = "KeyValue";
        public const int BlockSizeValue = 128;
        public const string Modulus = "Modulus";
        public const string Exponent = "Exponent";
    }
}
