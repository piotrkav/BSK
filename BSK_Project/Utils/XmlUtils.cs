using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using static BSK_Project.CipherMode;

namespace BSK_Project.Utils
{
    public class XmlUtils
    {

        public static void CreateXmlKey(string fileName, KeyDetails details)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",

            };

            using (var writer = XmlWriter.Create(fileName, settings))
            {

                writer.WriteStartDocument();
                writer.WriteStartElement(Constants.Key);
                writer.WriteElementString(Constants.Algorithm, details.Algorithm);
                writer.WriteElementString(Constants.KeyType, details.Type);

                writer.WriteStartElement(Constants.KeyValue);
                writer.WriteAttributeString("system", "hex");
                writer.WriteValue(BitConverter.ToString(details.Content).Replace("-", string.Empty));
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


            }
        }
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
                writer.WriteElementString(Constants.BlockSize, Constants.BlockSizeValue.ToString());
                Console.WriteLine(details.CipherMode.ToString());
                writer.WriteElementString(Constants.CipherMode, details.CipherMode.ToString());
                if (details.CipherMode != CipherModes.Ecb)
                {
                    writer.WriteElementString(Constants.IV, Convert.ToBase64String(details.Iv));
                }

                if (details.CipherMode == CipherModes.Cfb || details.CipherMode == CipherModes.Ofb)
                {
                    writer.WriteElementString(Constants.SubBlockSize, details.SubBlockSize.ToString());
                }

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
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();

            }
            const byte stx = 0x02;
            var contentWriter = new BinaryWriter(File.Open(fileName, FileMode.Append));
            contentWriter.Write(stx);
            contentWriter.Write(encryptedBytes);
            contentWriter.Close();
        }
        public static AlgorithmDetails GetAlgorithmDetails(string fileName)
        {
            var details = new AlgorithmDetails();
            var allowedUserDictionary = new SortedDictionary<string, byte[]>();

            try
            {
                using (var reader = XmlReader.Create(fileName))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == Constants.EncryptedFileHeader &&
                            reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }

                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case Constants.Algorithm:
                                reader.Read();
                                details.Algorithm = reader.Value;
                                break;
                            case Constants.BlockSize:
                                reader.Read();
                                details.BlockSize = Convert.ToInt32(reader.Value);
                                break;
                            case Constants.SubBlockSize:
                                reader.Read();
                                details.SubBlockSize = Convert.ToInt32(reader.Value);
                                break;
                            case Constants.KeySize:
                                reader.Read();
                                details.KeySize = Convert.ToInt32(reader.Value);
                                break;
                            case Constants.CipherMode:
                                reader.Read();
                                details.CipherMode = GetEnum(reader.Value);
                                break;
                            case Constants.IV:
                                reader.Read();
                                details.Iv = Convert.FromBase64String(reader.Value);
                                break;
                            case Constants.Email:
                                reader.Read();
                                var email = reader.Value;

                                reader.ReadToFollowing(Constants.SessionKey);
                                reader.Read();
                                var sessionKey = reader.Value;
                                allowedUserDictionary.Add(email, Convert.FromBase64String(sessionKey));
                                break;
                        }
                    }
                }
                details.UserSessionKeysDictionary = allowedUserDictionary;
                return details;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace + e.Message);
                MessageBox.Show("Zły typ pliku wejściowego!", "Wybieranie pliku", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }


        }
        public static byte[] GetKey(string fileName)
        {
            using (var reader = XmlReader.Create(fileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) continue;
                    if (reader.Name.Equals(Constants.KeyValue))
                    {
                        reader.Read();
                        var size = reader.Value.Length;
                        var result = new byte[size / 2];
                        reader.ReadContentAsBinHex(result, 0, size / 2);
                        return result;


                    }
                }
                return null;
            }
        }

        public static bool CheckIfProperKey(string fileName)
        {
            try
            {
                using (var reader = XmlReader.Create(fileName))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case Constants.Key:
                                break;
                            case Constants.Algorithm:
                                break;
                            case Constants.KeyType:
                                break;
                            case Constants.KeyValue:
                                break;
                            default:
                                return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static byte[] GetContent(string fileName)
        {
            var startOffset = GetNumberOfBytesToSkip(fileName);

            var result = File.ReadAllBytes(fileName).Skip(startOffset).ToArray();
            return result;
        }
        public static int GetNumberOfBytesToSkip(string fileName)
        {
            var inputFile = File.ReadAllBytes(fileName);
            const byte stx = 0x02;
            var counter = 0;
            foreach (var t in inputFile)
            {
                counter++;
                if (t == stx)
                {
                    break;
                }
            }
            return counter;
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
