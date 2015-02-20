using System;
using System.Threading.Tasks.Dataflow;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    internal static class AcknowledgeController
    {

        #region Acknowledge Action Block

        public static ActionBlock<Tuple<LazyError, bool>> AcknowledgeActionBlock = new ActionBlock
            <Tuple<LazyError, bool>>(
            async acknowledge =>
            {
                if (acknowledge.Item2) // Error Successful sent to server database
                {
                    //
                    // Remove Error Snapshot from Snapshots Folder's:
                    await RoutingDataStoragePath.DeleteSnapshotImageOnDiskAsync(acknowledge.Item1.SnapshotAddress);
                    //
                    // Remove Error from Log file:
                    await CacheReader.ErrorHistory.RemoveByConcurrencyAsync(acknowledge.Item1.GetErrorObject);
                    //
                    // De-story error from Memory (RAM):
                    if (acknowledge.Item1 != null) acknowledge.Item1.Dispose();
                }
            },
            new ExecutionDataflowBlockOptions
            {
                MaxMessagesPerTask = 1
            });

        #endregion
    }
}
