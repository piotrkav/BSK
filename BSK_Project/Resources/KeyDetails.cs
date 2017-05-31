namespace BSK_Project.Utils
{
    public class KeyDetails
    {
        public string Algorithm { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
        
        public string User { get; set; }

        public byte[] Modulus { get; set; }

        public byte[] Exponent { get; set; }
        public KeyDetails(string algorithm, string type, string user,  byte[] content, byte[] modulus, byte[] exponent)
        {
            Algorithm = algorithm;
            Type = type;
            Content = content;
            User = user;
            Modulus = modulus;
            Exponent = exponent;
        }



    }
}