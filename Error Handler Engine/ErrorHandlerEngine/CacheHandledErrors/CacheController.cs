using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.Properties;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    internal static class CacheController
    {
        public static bool AreErrorsInSendState = false;

        #region Methods

        /// <summary>
        /// Check Cache State to Send Data to Server or Not ?
        /// </summary>
        public static void CheckState()
        {
            if (AreErrorsInSendState) return;

            HandleExceptions.IsSelfException = true;
            try
            {
                AreErrorsInSendState = true;

                // C:\Users\[User Name.Domain]\AppData\Local\MyApp\
                // Example ==> C:\Users\khosravifar.b.DBI\AppData\Local\TestErrorHandlerBySelf v1
                var rootDir = RoutingDataStoragePath.ErrorLogFilePath.Substring(0, RoutingDataStoragePath.ErrorLogFilePath.LastIndexOf('\\'));

                long errorDataSize = new DirectoryInfo(rootDir).GetDirectorySize();

                var maxSize = Int64.Parse(Resources.SizeLimitBytes);


                // if errors caching data was larger than limited size then send it to server 
                // and if successful sent then clear them...
                if (errorDataSize >= maxSize && ConnectionManager.GetDefaultConnection().IsReady)
                {
                    CacheReader.ReadCacheToServerUploader();
                }
            }
            finally
            {
                HandleExceptions.IsSelfException = false;
                AreErrorsInSendState = false;
            }
        }

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
    }
}