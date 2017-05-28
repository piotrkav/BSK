using System.Collections.Generic;

namespace BSK_Project
{
    public class AlgorithmDetails
    {
        public string Algorithm { get; set; }
        public int KeySize { get; set; }
        public int BlockSize { get; set; }
        public CipherMode.CipherModes CipherMode { get; set; }
        public byte[] Iv { get; set; }

        public SortedDictionary<string, byte[]> UserSessionKeysDictionary;

        public AlgorithmDetails() { }
        public AlgorithmDetails(string algorithm, CipherMode.CipherModes cipherMode, int keySize, byte[] iv, int blockSize, SortedDictionary<string, byte[]> userSessionKeysDictionary)
        {
            Algorithm = algorithm;
            BlockSize = blockSize;
            CipherMode = cipherMode;
            Iv = iv;
            KeySize = keySize;
            UserSessionKeysDictionary = userSessionKeysDictionary;
        }
    }
}
