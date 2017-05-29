using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSK_Project;

namespace BSKUnitTests
{
    [TestClass]
    public class CreateUserTest
    {
        [TestMethod]
        public void CreatingUserTest()
        {
            byte[] password = HashUtil.GenerateSha256Hash("password");
            string name = "kawa";
            UserCreateService service = new UserCreateService("kawa", password);
            service.AddUser();
            Assert.AreEqual(name, service.GetEmail());
            Assert.AreEqual(password, service.GetPassword());

            Assert.IsTrue(File.Exists(Constants.PrivateKeysFolderPath + name));
            Assert.IsTrue(File.Exists(Constants.PublicKeysFolderPath + name));
            File.Delete(Constants.PrivateKeysFolderPath + name);
            File.Delete(Constants.PublicKeysFolderPath + name);
        }

    }
}
