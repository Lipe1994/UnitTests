using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClass;

namespace MyClessesTest
{
    [TestClass]
    public class FileProcessTest
    {
        public const string BAD_FILE_NAME = @"c:\BAD.TXT";
        private string _GoodFileName;

        public TestContext TestContext { get; set; }


        private void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];

            if (_GoodFileName.Contains("[AppName]"))
                _GoodFileName = _GoodFileName.Replace("[AppName]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        [TestInitialize]
        public void TesteInitialize()
        {
            if (TestContext.TestName == "FileNameDoesExists")
            {
                if (string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Busca path dp arquivo");
                    SetGoodFileName();
                }
            }
        }

        [TestCleanup]
        public void TesteCleanup()
        {
            if (TestContext.TestName == "FileNameDoesExists")
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Deleta arquivo");
                    File.Delete(_GoodFileName);
                }
        }

        [TestMethod]
        public void FileNameDoesExists()
        {
            var fileProcess = new FileProcess();
            bool value;

            TestContext.WriteLine("Cria arquivo");
            File.AppendAllText(_GoodFileName, "teste");

            value = fileProcess.FileExists(_GoodFileName);

            Assert.IsTrue(value);
        }


        [TestMethod]
        public void FileNameDoesNotExists()
        {
            var fileProcess = new FileProcess();
            bool value;

            
            value = fileProcess.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsNewArgumentNullException()
        {
            var fileProcess = new FileProcess();

            fileProcess.FileExists(null);

        }

        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsNewArgumentNullException_UsingTryCatch()
        {
            try
            {
                var fileProcess = new FileProcess();
                fileProcess.FileExists(null);
            }
            catch
            {
                return;
            }
            Assert.Fail();
        }
    }
}
