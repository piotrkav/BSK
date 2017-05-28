using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using static BSK_Project.CipherMode;

namespace BSK_Project
{
    internal class AdditionalUtils
    {
        public static List<string> GetUsersFromFolder()
        {
            var list = new List<string>();
            var pathToFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\public_keys"));
            var files = Directory.GetFiles(pathToFolder).Select(Path.GetFileName);
            foreach (var item in files)
            {
                list.Add(item);
            }
            return list;
        }

        public static void FillUserListBox(ListBox listBox)
        {

            if (listBox.Items.Count > 0)
            {
                listBox.Items.Clear();
            }
            var items = GetUsersFromFolder();
            foreach (var item in items)
            {
                listBox.Items.Add(item);
            }
        }
        public static void FillKeyLengthComboBox(ComboBox keyLengthComboBox)
        {
            foreach (var keyLength in Constants.KeyLengths)
            {
                keyLengthComboBox.Items.Add(keyLength);
            }
        }

        public static void FillCipherModeComboBox(ComboBox cipherModeComboBox)
        {
            foreach (CipherModes mode in Enum.GetValues(typeof(CipherModes)))
            {
                cipherModeComboBox.Items.Add(mode);
            }
        }

        public static void FillSubBlockSizeComboBox(ComboBox subBlockSizeComboBox)
        {
            foreach (var subBlockSize in Constants.SubBlockSizes)
            {
                subBlockSizeComboBox.Items.Add(subBlockSize);
            }
            
        }
    }
}
