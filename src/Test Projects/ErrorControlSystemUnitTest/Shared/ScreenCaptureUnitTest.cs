using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ErrorControlSystem.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErrorHandlerEngineUnitTest.Shared
{
    /// <summary>
    /// Summary description for ScreenCaptureUnitTest
    /// </summary>
    [TestClass]
    public class ScreenCaptureUnitTest
    {
        //private static string StorageDirPath;

        public ScreenCaptureUnitTest()
        {
            //StorageDirPath = "";
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
            //var img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.ResizeImage(800, 600).Save(StorageDirPath + "\\test800.png");
            //    Assert.IsTrue(File.Exists(StorageDirPath + "\\test800.png"));
            //}
            ////
            //// JPEG
            ////
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.jpg", ImageFormat.Jpeg);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.jpg"));
            //}
            ////
            //// PNG
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.png", ImageFormat.Png);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.png"));
            //}
            ////
            //// GIF
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.gif", ImageFormat.Gif);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.gif"));
            //    File.Delete(StorageDirPath + "test.gif");
            //    Assert.IsFalse(File.Exists(StorageDirPath + "test.gif"));
            //}
            ////
            //// BMP
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.bmp", ImageFormat.Bmp);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.bmp"));
            //}
            ////
            //// EMF
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.emf", ImageFormat.Emf);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.emf"));
            //}
            ////
            //// EXIF
            //// 
            
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.exif", ImageFormat.Exif);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.exif"));
            //}
            ////
            //// ICON
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.ico", ImageFormat.Icon);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.ico"));
            //}
            ////
            //// Memory BMP
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "MemoryBmp.bmp", ImageFormat.MemoryBmp);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "MemoryBmp.bmp"));
            //}
            ////
            //// Tiff
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.Tiff", ImageFormat.Tiff);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.Tiff"));
            //}
            ////
            //// WMF
            //// 
            //img = ScreenCapture.Capture();
            //if (img != null)
            //{
            //    img.Save(StorageDirPath + "test.wmf", ImageFormat.Wmf);
            //    Assert.IsTrue(File.Exists(StorageDirPath + "test.wmf"));
            //}
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            //File.Delete(StorageDirPath + "test.png");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.png"));

            //File.Delete(StorageDirPath + "\\test800.png");
            //Assert.IsFalse(File.Exists(StorageDirPath + "\\test800.png"));

            //File.Delete(StorageDirPath + "test.jpg");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.jpg"));

            //File.Delete(StorageDirPath + "test.wmf");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.wmf"));

            //File.Delete(StorageDirPath + "test.Tiff");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.Tiff"));

            //File.Delete(StorageDirPath + "MemoryBmp.bmp");
            //Assert.IsFalse(File.Exists(StorageDirPath + "MemoryBmp.bmp"));

            //File.Delete(StorageDirPath + "test.ico");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.ico"));

            //File.Delete(StorageDirPath + "test.exif");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.exif"));

            //File.Delete(StorageDirPath + "test.emf");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.emf"));

            //File.Delete(StorageDirPath + "test.bmp");
            //Assert.IsFalse(File.Exists(StorageDirPath + "test.bmp"));

            //Directory.Delete(StorageDirPath);
            //Assert.IsFalse(Directory.Exists(StorageDirPath));
        }
    }
}
