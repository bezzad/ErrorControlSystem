
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
        public static async Task<bool> SentOneErrorToDbAsync(ProxyError error)
        {
            if (CanToSent)
            {
                try
                {
                    await DynamicStoredProcedures.InsertErrorStoredProcedureAsync(error);
                }
                catch
                {
                    CanToSent = false;
                }
            }
            return CanToSent;
        }

        public static bool SentOneErrorToDb(ProxyError error)
        {
            if (CanToSent)
            {
                try
                {
                    DynamicStoredProcedures.InsertErrorStoredProcedure(error);
                }
                catch
                {
                    CanToSent = false;
                }
            }
            return CanToSent;
        }

        // maybe the network have exception then dead loop occurred,
        // so this variable closed that
        public static volatile bool CanToSent = true;

        public static TransformBlock<ProxyError, Tuple<ProxyError, bool>> ErrorListenerTransformBlock;

        static Uploader()
        {
            Task.Run(async () => await ConnectionManager.GetDefaultConnection().CheckDbConnectionAsync());


            ErrorListenerTransformBlock = new TransformBlock<ProxyError, Tuple<ProxyError, bool>>(
                async (e) =>
                {
                    if (ConnectionManager.GetDefaultConnection().IsReady && CanToSent) // Server Connector to online or offline ?
                    {
                        try
                        {
                            CanToSent = await SentOneErrorToDbAsync(e);
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
