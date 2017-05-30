namespace BSK_Project.Utils
{
    public class KeyDetails
    {
        public string Algorithm { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
        
        public string User { get; set; }


        public KeyDetails(string algorithm, string type, string user,  byte[] content)
        {
            Algorithm = algorithm;
            Type = type;
            Content = content;
            User = user;
        }


    }
}