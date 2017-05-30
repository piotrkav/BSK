using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static BSK_Project.CipherMode;

namespace BSK_Project
{
    internal class AdditionalUtils
    {

        public static bool CheckIfCanEncrypt(string inputFilePath, string outputFilePath, int userCount)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                MessageBox.Show("Nie wybrano pliku do zaszyfrowania!", "Wybieranie pliku", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                MessageBox.Show("Nie wybrano pliku wyjściowego", "Wybieranie pliku", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (userCount == 0)
            {
                MessageBox.Show("Nie wybrano użytkownika/ów dla których zaszyfrować plik!", "Wybieranie użytkownika", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        public static bool checkIfCanDecrypt(string inputFilePath, string outputFilePath, string password)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                MessageBox.Show("Nie wybrano pliku do odszyfrowania!", "Wybieranie pliku", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                MessageBox.Show("Nie wybrano pliku wyjściowego", "Wybieranie pliku", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Nie wpisano hasła", "Wpisywanie hasła", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;

            }
            return true;
        }

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
