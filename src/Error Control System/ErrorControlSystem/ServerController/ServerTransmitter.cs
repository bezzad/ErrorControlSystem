namespace ErrorControlSystem.ServerController
{
    using System;
    using System.Data;
    using System.Runtime.Remoting;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using ErrorControlSystem.CacheErrors;
    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.Shared;

    public static partial class ServerTransmitter
    {
        #region Properties

        public static TransformBlock<ProxyError, Tuple<ProxyError, bool>> ErrorListenerTransformBlock;

        #endregion

        #region Constructor

        /// <summary>
        /// Initials the transmitter asynchronous.
        /// Check the server and then database existence and ...
        /// </summary>
        public static async Task InitialTransmitterAsync()
        {
            await ServerValidatorAsync();

            ErrorListenerTransformBlock = new TransformBlock<ProxyError, Tuple<ProxyError, bool>>(
                async (e) => await TransmitOneError(e),
                new ExecutionDataflowBlockOptions()
                {
                    MaxMessagesPerTask = 1,
                    MaxDegreeOfParallelism = 1
                });

            ErrorListenerTransformBlock.LinkTo(CacheController.AcknowledgeActionBlock);
        }

        #endregion

        #region Methods


        private static async Task ServerValidatorAsync()
        {
            var cm = ConnectionManager.GetDefaultConnection(); // fetch default connection

            if (await cm.IsServerOnlineAsync()) // Is server on ?
            {
                await SqlServerManager.CreateDatabaseAsync(); // Check or Create Raw Database

                if (await cm.CheckDbConnectionAsync()) // Is database exist on server ?
                {
                    await SqlServerManager.CreateTablesAndStoredProceduresAsync(); // Check or create Tables and StoredProcedures
                }
                else // database is not exist !!!
                {
                    ErrorHandlingOption.EnableNetworkSending = false;
                    new DataException("Database is not exist or corrupted").RaiseLog();
                }
            }
            else // server is off !!
            {
                ErrorHandlingOption.EnableNetworkSending = false;
                new ServerException("Server is not online!").RaiseLog();
            }
        }

        [DebuggerStepThrough]
        private static async Task<Tuple<ProxyError, bool>> TransmitOneError(ProxyError error)
        {
            if (ErrorHandlingOption.EnableNetworkSending) // Server Connector to online or offline ?
            {
                try
                {
                    await SqlServerManager.InsertErrorAsync(error);
                }
                catch (AggregateException exp)
                {
                    // If an unhandled exception occurs during dataflow processing, all 
                    // exceptions are propagated through an AggregateException object.
                    ErrorHandlingOption.EnableNetworkSending = false;

                    exp.Handle(e =>
                    {
                        ErrorHandlingOption.AtSentState = false;
                        return true;
                    });
                }
            }
            // Mark the head of the pipeline as complete. The continuation tasks  
            // propagate completion through the pipeline as each part of the  
            // pipeline finishes.
            else
            {
                ErrorListenerTransformBlock.Complete();
                ErrorHandlingOption.AtSentState = false;
            }

            if (ErrorListenerTransformBlock.InputCount == 0)
                ErrorHandlingOption.AtSentState = false;
            //
            // Post to Acknowledge Action Block:
            return new Tuple<ProxyError, bool>(error, ErrorHandlingOption.EnableNetworkSending);
        }

        #endregion
    }
}
