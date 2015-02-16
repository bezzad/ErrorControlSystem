using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.Properties;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    internal static class CacheController
    {
        public static bool AreErrorsInSendState = false;

        #region Methods

        #region Check Cache State to Send Data to Server or Not ?

        /// <summary>
        /// Check cache state to send data or not
        /// </summary>
        public static void CheckState(RoutingDataStoragePath router)
        {
            if (AreErrorsInSendState) return;


            Kernel.IsSelfException = true;
            try
            {
                AreErrorsInSendState = true;

                // Sent to server is ON or OFF
                if (Kernel.Conn.SqlConn != null)
                {
                    // C:\Users\[User Name]\AppData\Local\MyApp v1.*\
                    var rootDir = router.ErrorLogFilePath.Substring(0, router.ErrorLogFilePath.LastIndexOf('\\'));

                    long errorDataSize = new DirectoryInfo(rootDir).GetDirectorySize();

                    var maxSize = Int64.Parse(Resources.SizeLimitBytes);


                    // if errors caching data was larger than limited size then send it to server 
                    // and if successful sent then clear them...
                    if (errorDataSize >= maxSize)
                    {
                        CacheReader.ReadCacheToServerUploader();
                    }
                }
            }
            finally
            {
                Kernel.IsSelfException = false;
                AreErrorsInSendState = false;
            }
        }

        #endregion


        #region Get Directory Size

        /// <summary>
        /// Get Size of directory by all sub directory and files.
        /// </summary>
        /// <param name="dir">The Directory</param>
        /// <returns>Size (bytes) of Directory</returns>
        public static long GetDirectorySize(this DirectoryInfo dir)
        {
            long sum = 0;

            // get IEnumerable from all files in the current directory and all sub directories
            try
            {
                var subDirectories = dir.GetDirectories()
                    .Where(d => (d.Attributes & FileAttributes.ReparsePoint) == 0).AsParallel();

                Parallel.ForEach(subDirectories, d =>
                {
                    var value = d.GetDirectorySize();
                    Interlocked.Add(ref sum, value);
                });

                // get the size of all files
                sum += (from file in dir.GetFiles() select file.Length).Sum();
            }
            catch { }


            return sum;
        }

        #endregion

        #endregion
    }
}