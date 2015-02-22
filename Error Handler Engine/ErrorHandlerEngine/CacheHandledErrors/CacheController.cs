using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.Properties;
using ErrorHandlerEngine.ServerUploader;
using Newtonsoft.Json;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    internal static class CacheController
    {
        public static bool AreErrorsInSendState = false;

        public static ErrorUniqueCollection ErrorHistory = new ErrorUniqueCollection();

        #region Methods

        /// <summary>
        /// Check Cache State to Send Data to Server or Not ?
        /// </summary>
        public static void CheckState()
        {
            if (AreErrorsInSendState) return;

            ExpHandlerEngine.IsSelfException = true;
            try
            {
                AreErrorsInSendState = true;

                // C:\Users\[User Name.Domain]\AppData\Local\MyApp\
                // Example ==> C:\Users\khosravifar.b.DBI\AppData\Local\TestErrorHandlerBySelf v1
                var rootDir = StorageRouter.ErrorLogFilePath.Substring(0, StorageRouter.ErrorLogFilePath.LastIndexOf('\\'));

                long errorDataSize = new DirectoryInfo(rootDir).GetDirectorySize();

                var maxSize = Int64.Parse(Resources.SizeLimitBytes);


                // if errors caching data was larger than limited size then send it to server 
                // and if successful sent then clear them...
                if (errorDataSize >= maxSize && ConnectionManager.GetDefaultConnection().IsReady)
                {
                    UploadCacheAsync();
                }
            }
            finally
            {
                ExpHandlerEngine.IsSelfException = false;
                AreErrorsInSendState = false;
            }
        }

        #region Read Cache to Error History

        /// <summary>
        /// Read cache and fill ErrorHistory array
        /// </summary>
        public static async void ReadCacheFromDiskAsync()
        {
            var errors = await GetErrosFromLogAsync();
            //
            // Read any error in errors array to sent it to ServerUploader
            ErrorHistory.AddRange(errors);
        }

        #endregion

        #region Error Read from Json Log file

        private static async Task<Error[]> GetErrosFromLogAsync()
        {
            ExpHandlerEngine.IsSelfException = true;
            try
            {
                //
                // Read Error Log Json File
                var allJsonString = await StorageRouter.ReadLogAsync();
                //
                // Check file is not empty ?
                if (String.IsNullOrEmpty(allJsonString)) return null;

                //
                // Convert json string to Error array's.
                return await JsonConvert.DeserializeObjectAsync<Error[]>(allJsonString);
            }
            finally
            {
                ExpHandlerEngine.IsSelfException = false;
            }

        }
        #endregion

        public static async void UploadCacheAsync()
        {
            await Task.Run(async () =>
            {
                foreach (var error in ErrorHistory)
                {
                    await Uploader.ErrorListenerTransformBlock.SendAsync(new ProxyError(error));
                }
            });

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