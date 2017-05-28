using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BSK_Project.Utils;
using Microsoft.Win32;
using static BSK_Project.CipherMode;

namespace BSK_Project
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AlgorithmDetails _fileToDecryptAlgorithmDetails;

        string _fileToEncryptPath;
        string _fileToSaveEncryptedPath;

        string _fileToDecryptPath;
        string _fileToSaveDecryptedPath;

        
        byte[] _fileToBeEncrypted;
        byte[] _encryptedFile;
        byte[] fileToBeDecrypted;
        byte[] decryptedFile;

        private byte[] _userPrivateKey;
        public MainWindow()
        {

            InitializeComponent();
            InitializeValues();
        }

        public void InitializeValues()
        {
            var files = AdditionalUtils.GetUsersFromFolder();
            AdditionalUtils.FillUserListBox(userListBox);
            AdditionalUtils.FillKeyLengthComboBox(keyLengthComboBox);
            AdditionalUtils.FillCipherModeComboBox(CipherModeComboBox);
            AdditionalUtils.FillSubBlockSizeComboBox(SubBlockSizeComboBox);
        }



        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void filePickerButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                _fileToEncryptPath = dlg.FileName;

                fileToEncrypt.Text = _fileToEncryptPath;
                _fileToBeEncrypted = System.IO.File.ReadAllBytes(_fileToEncryptPath);
            }

        }

        private void filePickerToSaveButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();
            _fileToSaveEncryptedPath = dlg.FileName;
            fileToSaveEncrypt.Text = _fileToSaveEncryptedPath;


        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            var fileEncryptionService = new FileEncryptionService();

            var size = keyLengthComboBox.SelectedItem != null ? int.Parse(keyLengthComboBox.SelectedItem.ToString()) : 128;

            var mode = (CipherModes)CipherModeComboBox.SelectedItem;
            var iv = fileEncryptionService.GenerateKey(128);
            var subLength = (int)SubBlockSizeComboBox.SelectedItem;
           



            var sessionKey = fileEncryptionService.GenerateKey(size);

            _encryptedFile = TwoFishUtils.TwoFishEncryption(mode, _fileToBeEncrypted, sessionKey, iv, 0);

            var usersEncryptedSessionKeys = new SortedDictionary<string, byte[]>();
            foreach (var user in choosenUserListBox.Items)
            {
                var userName = user.ToString();
                var publicKey = fileEncryptionService.GetPublicKey(userName);
                var encryptedSessionKeyPerUser = fileEncryptionService.GetEncryptedByRsaSessionKey(publicKey, sessionKey);
                usersEncryptedSessionKeys.Add(userName,encryptedSessionKeyPerUser);

            }

            var algorithmDetails = new AlgorithmDetails(Constants.TwoFish, mode, size, iv, subLength,
                usersEncryptedSessionKeys);
            
            XmlUtils.CreateXml(_fileToSaveEncryptedPath, algorithmDetails,_encryptedFile);
            sessionKey = null;
           //TwoFishUtils.SaveData(_fileToSaveEncryptedPath, _encryptedFile);



        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            //txtDecryptedText = TwoFishUtils.TwoFishDecryption(txtEncryptedText, key, true);
            //label4.Content = txtDecryptedText;


        }
        //DECRYPT
        private void fileToDecryptChooser_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result != true) return;
            _fileToDecryptPath = dlg.FileName;
            FileToDecrypt.Text = _fileToDecryptPath;

           _fileToDecryptAlgorithmDetails =  XmlUtils.GetAlgorithmDetails(_fileToDecryptPath);
            foreach (var user in _fileToDecryptAlgorithmDetails.UserSessionKeysDictionary)
            {
                allowedUsersToDecryptComboBox.Items.Add(user.Key);
            }

           
            fileToBeDecrypted = System.IO.File.ReadAllBytes(_fileToDecryptPath);
        }

        private void fileToSaveDecryptChooser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != true) return;
            _fileToSaveDecryptedPath = dialog.FileName;

            FileToSaveDecrypt.Text = _fileToSaveDecryptedPath;
            decryptedFile = System.IO.File.ReadAllBytes(_fileToSaveDecryptedPath);
        }

        private void DecryptButton_Click_2(object sender, RoutedEventArgs e)
        {
            //  decryptedFile = TwoFishUtils.TwoFishFileDecryption(fileToBeDecrypted, key, true);
            //  TwoFishUtils.SaveData(FileToSaveDecryptedPath, decryptedFile);

        }



        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow createUserWindow = new CreateUserWindow(userListBox);
            createUserWindow.Show();


        }

        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var userToDelete = userListBox.SelectedItem;
            if (userToDelete != null)
            {
                label4.Content = userToDelete;
                UserDeleteService userDeleteService = new UserDeleteService(userListBox, userToDelete);
                userDeleteService.DeleteFullUserInfo();
            }
        }

        private void deleteUserFromChoosenButton_Click(object sender, RoutedEventArgs e)
        {
            var userToDelete = choosenUserListBox.SelectedItem;
            UserDeleteService userDeleteService = new UserDeleteService(choosenUserListBox, userToDelete);
            userDeleteService.DeleteFromList();
        }

        private void chooseUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (userListBox.SelectedItem != null)
            {
                choosenUserListBox.Items.Add(userListBox.SelectedItem);
                userListBox.SelectedIndex = -1;
            }

        }

        private void DecryptButton_Click_1(object sender, RoutedEventArgs e)
        {
            var passwordHashed = HashUtil.GenerateSha256Hash(passwordToDecryptBox.Password);
            var _decryptedBytes = TwoFishUtils.TwoFishPrivateKeyDecryption(CipherModes.Ecb, _fileToBeEncrypted, _fileToDecryptAlgorithmDetails.UserSessionKeysDictionary.GetEnumerator().Current.Value, null, 0);
            TwoFishUtils.SaveData(_fileToSaveDecryptedPath, _decryptedBytes);
            

        }

        private void allowedUsersToDecryptComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(allowedUsersToDecryptComboBox.SelectedItem.ToString());
            var selectedUser = allowedUsersToDecryptComboBox.SelectedItem.ToString();
            _userPrivateKey  = System.IO.File.ReadAllBytes(Constants.PrivateKeysFolderPath + selectedUser);

            Console.WriteLine(_userPrivateKey);
        
        }
    }
}

