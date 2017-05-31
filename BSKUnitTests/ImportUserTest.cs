using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BSK_Project;

namespace BSKUnitTests
{
    [TestClass]
    public class ImportUserTest
    {
        [TestMethod]
        public void ImportingUserTest()
        {
            var name = "testImport";
            File.Create(name).Dispose();
            var path = Path.GetFullPath(name);
            var newKey = Constants.PublicKeysFolderPath + name;
            if (!File.Exists(newKey))
            {
                File.Copy(path, Constants.PublicKeysFolderPath + name);
            }
            Assert.IsTrue(File.Exists(Constants.PublicKeysFolderPath + name));
            File.Delete(Constants.PublicKeysFolderPath + name);


        }
    }
}
