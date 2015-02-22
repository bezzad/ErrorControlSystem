using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErrorHandlerEngine.ModelObjecting;

namespace TestErrorHandlerEngine.ModelObjecting
{
    /// <summary>
    /// Summary description for ScreenCaptureUnitTest
    /// </summary>
    [TestClass]
    public class ScreenCaptureUnitTest
    {
        private static string StorageDirPath;

        public ScreenCaptureUnitTest()
        {
            // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Application Name and Major Version 
            var appNameVer = "TestErrorHandlerEngine v1";

            // Storage Path LocalApplicationData\[AppName] v[AppMajorVersion]\
            StorageDirPath = Path.Combine(appDataDir, appNameVer);

            if (!Directory.Exists(StorageDirPath))
                Directory.CreateDirectory(StorageDirPath);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [TestCategory("ScreenCapture.cs")]
        public void TestTakeScreenCaptureAndDelete()
        {
            ScreenCapture.CaptureScreenToFile(StorageDirPath + "\\test.png", System.Drawing.Imaging.ImageFormat.Png);
            Assert.IsTrue(File.Exists(StorageDirPath + "\\test.png"));

            ScreenCapture.CaptureScreen().ResizeImage(800, 600).Save(StorageDirPath + "\\test800.png");
            Assert.IsTrue(File.Exists(StorageDirPath + "\\test800.png"));
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            File.Delete(StorageDirPath + "\\test.png");
            Assert.IsFalse(File.Exists(StorageDirPath + "\\test.png"));

            File.Delete(StorageDirPath + "\\test800.png");
            Assert.IsFalse(File.Exists(StorageDirPath + "\\test800.png"));
        }
    }
}
