using System;
using System.Threading.Tasks.Dataflow;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    /// <summary>
    /// Read Incoming Handled Errors object to save and cache.
    /// </summary>
    public static class ErrorsReceiver
    {
        #region Data-Flow Blocks

        private static TransformBlock<Error, Error> tbErrorSnapshotSaver;

        private static ActionBlock<Error> abErrorSaver;

        #endregion

        #region Constructor
        static ErrorsReceiver()
        {
            #region Initialize Add Error to History [Action Block]

            abErrorSaver = new ActionBlock<Error>(async error =>
            {
                await CacheReader.ErrorHistory.AddByConcurrencyToFileAsync(error);

                CacheController.CheckState();
            },
                new ExecutionDataflowBlockOptions
                {
                    MaxMessagesPerTask = 1
                });
            #endregion

            #region Initialize Error Snapshot Saver on Disk [Transform Block]

            tbErrorSnapshotSaver = new TransformBlock<Error, Error>(async error =>
            {
                // Save error.Snapshot image file on Disk and set that's address
                error.SnapshotAddress = await StorageRouter.SaveSnapshotImageOnDiskAsync(error);

                // Dispose Image of Error On Memory 
                error.GetSnapshot().Dispose();

                // send to error saver action block
                return error;

            },
                new ExecutionDataflowBlockOptions
                {
                    SingleProducerConstrained = true,
                    MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                });
            #endregion


            #region Error Snapshot Transform Block Linked to Add Error to History

            tbErrorSnapshotSaver.LinkTo(abErrorSaver);

            #endregion
        }

        #endregion

        #region Methods

        public static async void OnErrorHandled(object sender, EventArgs e)
        {
            var error = sender as Error;

            if (CacheReader.ErrorHistory.Contains(error))
                // Don't Save Snapshot because that error is duplicate and not need to image 
                await abErrorSaver.SendAsync(error);

            else
                await tbErrorSnapshotSaver.SendAsync(error);
        }

        #endregion
    }
}
