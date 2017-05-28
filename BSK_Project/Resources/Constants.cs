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
        public const string CipherMode = "CipherMode";
        public const string IV = "IV";
        public const string ApprovedUsers = "ApprovedUsers";
        public const string User = "User";
        public const string Email = "Email";
        public const string SessionKey = "SessionKey";
        public const string EncryptedFileHeader = "EncryptedFileHeader";
        public static int[] KeyLengths = {128, 192, 256};
        public static int[] SubBlockSizes = {8, 16, 32, 64};
        public static string TwoFish = "Twofish";
        public static string Content = "Content";
    }
}
