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

namespace ErrorHandlerEngine.CacheHandledErrors
{
    internal static class CacheController
    {
        public static bool AreErrorsInSendState = false;

        public static ActionBlock<Tuple<ProxyError, bool>> AcknowledgeActionBlock;

        public static ActionBlock<Error> ErrorSaverActionBlock;

        #region Methods


        static CacheController()
        {
            #region Save Error object in Error.Log (SQL SDF) file's [Action Block]

            ErrorSaverActionBlock = new ActionBlock<Error>(async error =>
            {
                await SdfFileManager.InsertOrUpdateAsync(error);

                await CheckStateAsync();
            },
                new ExecutionDataflowBlockOptions
                {
                    MaxMessagesPerTask = 1
                });

            #endregion

            #region Acknowledge Action Block

            AcknowledgeActionBlock = new ActionBlock<Tuple<ProxyError, bool>>(
                async ack =>
                {
                    if (ack.Item2) // Error Successful sent to server database
                    {
                        //
                        // Remove Error from Log file:
                        await SdfFileManager.DeleteAsync(ack.Item1.Id);
                        //
                        // De-story error from Memory (RAM):
                        if (ack.Item1 != null) ack.Item1.Dispose();
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxMessagesPerTask = 1
                });

            #endregion
        }


        /// <summary>
        /// Check Cache State to Send Data to Server or Not ?
        /// </summary>
        public static async Task CheckStateAsync()
        {
            if (AreErrorsInSendState) return;

            ExceptionHandler.IsSelfException = true;
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
                    await UploadCacheAsync();
                }
            }
            finally
            {
                ExceptionHandler.IsSelfException = false;
                AreErrorsInSendState = false;
            }
        }

        public static async Task UploadCacheAsync()
        {
            foreach (var error in SdfFileManager.GetErrors())
            {
                await Uploader.ErrorListenerTransformBlock.SendAsync(new ProxyError(error));
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


        public static async void CacheTheError(Error error)
        {
            await ErrorSaverActionBlock.SendAsync(error);
        }

        #endregion
    }
}