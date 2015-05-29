namespace ErrorControlSystem.CacheErrors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.ServerController;
    using ErrorControlSystem.Shared;


    internal static class CacheController
    {
        #region Properties

        public static SqlCompactEditionManager SdfManager { get; private set; }

        public static ActionBlock<Tuple<ProxyError, bool>> AcknowledgeActionBlock;

        private static ActionBlock<Error> _errorSaverActionBlock;

        #endregion

        #region Methods


        static CacheController()
        {
            // Create or Load log file path
            StorageRouter.CheckLogPathAndName();
            //
            // Create instance of SDF file manager object
            SdfManager = new SqlCompactEditionManager(ErrorHandlingOption.ErrorLogPath);

            #region Acknowledge Action Block

            AcknowledgeActionBlock = new ActionBlock<Tuple<ProxyError, bool>>(
                async ack =>
                {
                    if (ack.Item2) // Error Successful sent to server database
                    {
                        //
                        // Remove Error from Log file:
                        await SdfManager.DeleteAsync(ack.Item1.Id);
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
            if (ErrorHandlingOption.EnableNetworkSending && ConnectionManager.GetDefaultConnection().IsReady)
            {
                // if errors caching data was larger than limited size then send it to server 
                // and if successful sent then clear them...
                if (ErrorHandlingOption.CacheFilled
                    || await SdfManager.GetTheFirstErrorHoursAsync() >= ErrorHandlingOption.ExpireHours
                    || ErrorHandlingOption.SentOnStartup
                    || ErrorHandlingOption.MaxQueuedError <= await SdfManager.CountAsync()
                    || ErrorHandlingOption.AtSentState)
                {
                    await UploadCacheAsync();
                }
            }
        }

        public static async Task UploadCacheAsync()
        {
            if (SdfManager.ErrorIds.Count == 0)
            {
                ErrorHandlingOption.AtSentState = false;
                return;
            }

            IEnumerable<ProxyError> errors = SdfManager.GetErrors();

            if (errors == null || !errors.Any())
            {
                ErrorHandlingOption.AtSentState = false;
                return;
            }

            ErrorHandlingOption.AtSentState = true;

            foreach (var error in errors)
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
                    if (await SdfManager.InsertOrUpdateAsync(e)) // insert or update database and return cache check state
                        if (_errorSaverActionBlock.InputCount == 0 && !ErrorHandlingOption.AtSentState)
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