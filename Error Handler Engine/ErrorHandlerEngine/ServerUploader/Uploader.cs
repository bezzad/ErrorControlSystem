
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ConnectionsManager;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class Uploader
    {

        // maybe the network have exception then dead loop occurred,
        // so this variable closed that
        public static volatile bool CanToSent = true;

        public static TransformBlock<ProxyError, Tuple<ProxyError, bool>> ErrorListenerTransformBlock;

        static Uploader()
        {
            Task.Run(async () =>
            {
            CheckDatabase:
                await ConnectionManager.GetDefaultConnection().CheckDbConnectionAsync();

                var cm = ConnectionManager.GetDefaultConnection();

                if (cm.IsReady)
                {
                    await DynamicStoredProcedures.CreateTablesAndStoredProceduresAsync();
                }
                else if (await cm.IsServerOnlineAsync())
                {
                    await DynamicStoredProcedures.CreateDatabaseAsync();

                    goto CheckDatabase;
                }
            });



            ErrorListenerTransformBlock = new TransformBlock<ProxyError, Tuple<ProxyError, bool>>(
                async (e) =>
                {
                    if (CanToSent) // Server Connector to online or offline ?
                    {
                        try
                        {
                            await DynamicStoredProcedures.InsertErrorStoredProcedureAsync(e);
                        }
                        catch (Exception)
                        {
                            CanToSent = false;
                        }
                        finally
                        {
                            CacheController.AreErrorsInSendState = false;
                        }
                    }
                    else ErrorListenerTransformBlock.Complete();
                    //
                    // Post to Acknowledge Action Block:
                    return new Tuple<ProxyError, bool>(e, CanToSent);
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxMessagesPerTask = 1,
                    MaxDegreeOfParallelism = 1
                });

            ErrorListenerTransformBlock.LinkTo(CacheController.AcknowledgeActionBlock);
        }
    }
}
