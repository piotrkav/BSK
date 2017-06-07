using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BSK_Project
{
    public class UserDeleteService
    {
        private object User { get; set; }
        private ListBox UserListBox { get; set; }


        public UserDeleteService(ListBox userListBox, object user)
        {
            UserListBox = userListBox;
            User = user;
        }

        public void DeleteFullUserInfo()
        {
            
            DeleteFromList();
            DeleteUserKeys();
        }
        public void DeleteFromList()
        {
               UserListBox.Items.Remove(User);
        }
        private void DeleteUserKeys()
        {
            File.Delete(Constants.PrivateKeysFolderPath + User);
            File.Delete(Constants.PublicKeysFolderPath + User);
        }
        public void DeleteUserKeysForTest(string p1, string p2)
        {
            File.Delete(p1);
            File.Delete(p2);
        }
    }
}
