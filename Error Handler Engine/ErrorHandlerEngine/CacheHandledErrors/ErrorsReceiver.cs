using System;
using System.Threading.Tasks.Dataflow;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    /// <summary>
    /// Read Incoming Handled Errors object to save and cache.
    /// </summary>
    public static class ErrorsReceiver
    {
        #region Data-Flow Blocks

        private static readonly TransformBlock<Error, Error> ErrorSnapshotSaverTransformBlock;

        private static readonly ActionBlock<Error> ErrorSaverActionBlock;

        #endregion

        #region Constructor
        static ErrorsReceiver()
        {
            #region Initialize Add Error to History [Action Block]

            ErrorSaverActionBlock = new ActionBlock<Error>(async error =>
            {
                await CacheController.ErrorHistory.AddByConcurrencyToFileAsync(error);

                CacheController.CheckState();
            },
                new ExecutionDataflowBlockOptions
                {
                    MaxMessagesPerTask = 1
                });
            #endregion

            #region Initialize Error Snapshot Saver on Disk [Transform Block]

            ErrorSnapshotSaverTransformBlock = new TransformBlock<Error, Error>(async error =>
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

            ErrorSnapshotSaverTransformBlock.LinkTo(ErrorSaverActionBlock);

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Exception Handler occurrance event listener.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void OnErrorHandled(object sender, EventArgs e)
        {
            await ErrorSnapshotSaverTransformBlock.SendAsync(sender as Error);
        }

        #endregion
    }
}
