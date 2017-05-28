using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Org.BouncyCastle.Crypto.Paddings;
using static BSK_Project.CipherMode;

namespace BSK_Project.Utils
{
    internal class XmlUtils
    {
        public static void CreateXml(string fileName, AlgorithmDetails details, byte[] encryptedBytes)
        {

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",

            };
            using (var writer = XmlWriter.Create(fileName, settings))
            {

                writer.WriteStartDocument();
                //
                //HEADEER
                writer.WriteStartElement(Constants.EncryptedFileHeader);
                //ALGORITHM DETAILS
                writer.WriteElementString(Constants.Algorithm, details.Algorithm);
                writer.WriteElementString(Constants.KeySize, details.KeySize.ToString());
                writer.WriteElementString(Constants.BlockSize, details.BlockSize.ToString());
                writer.WriteElementString(Constants.CipherMode, details.CipherMode.ToString());
                writer.WriteElementString(Constants.IV, Convert.ToBase64String(details.Iv));
                //APPROVED USERS
                writer.WriteStartElement(Constants.ApprovedUsers);
                foreach (var entry in details.UserSessionKeysDictionary)
                {
                    writer.WriteStartElement(Constants.User);
                    writer.WriteElementString(Constants.Email, entry.Key);
                    writer.WriteElementString(Constants.SessionKey, Convert.ToBase64String(entry.Value));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteElementString(Constants.Content,
                    BitConverter.ToString(encryptedBytes).Replace("-", string.Empty));
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static AlgorithmDetails GetAlgorithmDetails(string fileName)
        {
            var details = new AlgorithmDetails();
            var allowedUserDictionary = new SortedDictionary<string, byte[]>();
            using (var reader = XmlReader.Create(fileName))
            {
                while (reader.Read())
                {

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case Constants.Algorithm:
                                reader.Read();
                                details.Algorithm = reader.Value;
                                Console.WriteLine("Algorithm " + reader.Value);
                                break;
                            case Constants.BlockSize:
                                reader.Read(); //this moves reader to next node which is text 
                                details.BlockSize = Convert.ToInt32(reader.Value);
                                Console.WriteLine("BlockSize " + reader.Value);
                                break;
                            case Constants.KeySize:
                                reader.Read();
                                details.KeySize = Convert.ToInt32(reader.Value);
                                Console.WriteLine("KeySize " + reader.Value);
                                break;
                            case Constants.CipherMode:
                                reader.Read();
                                details.CipherMode = GetEnum(reader.Value);
                                Console.WriteLine("CipherMode " + reader.Value);
                                break;
                            case Constants.IV:
                                reader.Read();
                                details.Iv = Convert.FromBase64String(reader.Value);
                                Console.WriteLine("Iv " + reader.Value);
                                break;
                            case Constants.Email:
                                reader.Read();
                                 Console.WriteLine(reader.Value);
                                var email = reader.Value;
                                
                                reader.ReadToFollowing(Constants.SessionKey);
                                reader.Read();
                                Console.WriteLine("Session Key " + reader.Value);
                                var sessionKey = reader.Value;
                                allowedUserDictionary.Add(email, Convert.FromBase64String(sessionKey));
                                //if (reader.Name.Equals(Constants.SessionKey))
                                //{
                                //    reader.Read();
                                //    
                                //}
                                break;


                        }

                    }
                }
            }
            details.UserSessionKeysDictionary = allowedUserDictionary; 
            return details;
        }

        public static byte[] GetEncryptedContent(string fileName)
        {
            
            using (var reader = XmlReader.Create(fileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals(Constants.Content))
                        {
                            reader.Read();
                            var content = reader.Value;
                            return StringToByteArray(content);

                        }
                    }
                }
                return null;
            }
        }

     

        private static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
        private static CipherModes GetEnum(string value)
        {
            foreach (CipherModes mode in Enum.GetValues(typeof(CipherModes)))
            {
                if (mode.ToString().Equals(value))
                {
                    return mode;
                }
            }
            return CipherModes.Ecb;
        }
    }
}
