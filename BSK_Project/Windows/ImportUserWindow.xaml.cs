using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace BSK_Project.Windows
{
    /// <summary>
    /// Interaction logic for ImportUserWindow.xaml
    /// </summary>
    public partial class ImportUserWindow : Window
    {
        private string _publicKeyPath;
        private ListBox _userListBox;
        public ImportUserWindow(ListBox userListBox)
        {
            _userListBox = userListBox;
            InitializeComponent();
            Closed += new EventHandler(Refresh);
        }
        private void chooseImportKeyButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != true) return;
            publicKeyImport.Text = dialog.FileName;
            _publicKeyPath = publicKeyImport.Text;
        }

        private void Imp_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_publicKeyPath))
            {
                var name = Path.GetFileName(_publicKeyPath);
                var newKey = Constants.PublicKeysFolderPath + name;
                if (!File.Exists(newKey))
                {
                    File.Copy(_publicKeyPath, Constants.PublicKeysFolderPath + name);

                }
                else
                {
                    MessageBox.Show("Nie można zaimportować klucza, użytkownik o takiej nazwie już istnieje", "Import", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                

            }
            else
            {
                MessageBox.Show("Nie wybrano klucza publicznego!", "Wybieranie pliku", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }
        public void Refresh(object sender, EventArgs e)
        {
            AdditionalUtils.FillUserListBox(_userListBox);

        }
    }
}
