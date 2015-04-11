using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ErrorControlSystem.DbConnectionManager;
using ErrorControlSystem.ServerController;
using ErrorControlSystem.Shared;

namespace ErrorControlSystem.CacheErrors
{
    internal static class CacheController
    {
        #region Properties

        public static volatile bool AreErrorsInSendState = false;

        public static ActionBlock<Tuple<ProxyError, bool>> AcknowledgeActionBlock;

        private static ActionBlock<Error> _errorSaverActionBlock;

        #endregion

        #region Methods


        static CacheController()
        {
            #region Acknowledge Action Block

            AcknowledgeActionBlock = new ActionBlock<Tuple<ProxyError, bool>>(
                async ack =>
                {
                    if (ack.Item2) // Error Successful sent to server database
                    {
                        //
                        // Remove Error from Log file:
                        await SqlCompactEditionManager.DeleteAsync(ack.Item1.Id);
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

            try
            {
                AreErrorsInSendState = true;

                if (ConnectionManager.GetDefaultConnection().IsReady && ErrorHandlingOption.EnableNetworkSending)
                {
                    // if errors caching data was larger than limited size then send it to server 
                    // and if successful sent then clear them...
                    if (ErrorHandlingOption.CacheFilled
                        || await SqlCompactEditionManager.GetTheFirstErrorHoursAsync() >= ErrorHandlingOption.ExpireHours
                        || ErrorHandlingOption.SentOnStartup
                        || ErrorHandlingOption.MaxQueuedError <= await SqlCompactEditionManager.CountAsync())
                    {
                        await UploadCacheAsync();
                    }
                }
            }
            finally
            {
                AreErrorsInSendState = false;
            }
        }

        public static async Task UploadCacheAsync()
        {
            foreach (var error in SqlCompactEditionManager.GetErrors())
            {
                await ServerTransmitter.ErrorListenerTransformBlock.SendAsync(new ProxyError(error));

                if (!ErrorHandlingOption.EnableNetworkSending) break;
            }
        }

        public static async void CacheTheError(Error error)
        {
            if (_errorSaverActionBlock == null ||
                _errorSaverActionBlock.Completion.IsFaulted)
            {
                #region Initile Action Block Again

                _errorSaverActionBlock = new ActionBlock<Error>(async e =>
                {
                    if (await SqlCompactEditionManager.InsertOrUpdateAsync(e)) // insert or update database and return cache check state
                        if (_errorSaverActionBlock.InputCount == 0 && ErrorHandlingOption.EnableNetworkSending)
                            await CheckStateAsync();
                },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxMessagesPerTask = 1
                    });

                #endregion
            }

            await _errorSaverActionBlock.SendAsync(error);
        }

        #endregion
    }
}