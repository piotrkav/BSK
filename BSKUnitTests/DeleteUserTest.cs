using System;
using System.IO;
using System.Windows.Controls;
using BSK_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BSKUnitTests
{
    [TestClass]
    public class DeleteUserTest
    {
        [TestMethod]
        public void DeletingUserTest()
        {
            var privatePath = Constants.PrivateKeysFolderPath + "test";
            var publicPath = Constants.PublicKeysFolderPath + "test";

            var listBox = new ListBox();
            listBox.Items.Add("test");

            File.Create(privatePath).Dispose();
            File.Create(publicPath).Dispose();

            UserDeleteService service = new UserDeleteService(listBox, listBox.Items.GetItemAt(0));
            service.DeleteFromList();
            service.DeleteUserKeysForTest(publicPath, privatePath);

            Assert.AreEqual(new ListBox().Items.Count, listBox.Items.Count);
            Assert.IsFalse(File.Exists(publicPath));
            Assert.IsFalse(File.Exists(publicPath));
            
        }
    }
}
