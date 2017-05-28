using System;
using System.Windows;
using System.Windows.Controls;

namespace BSK_Project
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
      

        private string Email { get; set; }
        private string Password { get; set; }

        private ListBox UserListBox { get; set; }

        public CreateUserWindow(ListBox userListBox)
        {
            UserListBox = userListBox;
            InitializeComponent();
            Closed += new EventHandler(Refresh);
            
           
        }

        private void createUserButton_Click(object sender, EventArgs e)
        {
            Email = emailBox.Text;
            UserCreateService userCreateService = new UserCreateService(Email, HashUtil.GenerateSha256Hash(passwordBox.Password));
            passwordBox.Clear();
            userCreateService.AddUser();
           
            Close();
               
        }

      

        public void Refresh(object sender, EventArgs e)
        {
            AdditionalUtils.FillUserListBox(UserListBox);
         
        }
    }
}
