using System;
using System.Threading.Tasks.Dataflow;
using ConnectionsManager;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public class UploadController
    {
        // maybe the network have exception then dead loop occurred,
        // so this variable closed that
        public static volatile bool CanToSent = true;

        public TransformBlock<LazyError, Tuple<LazyError, bool>> ErrorListenerTransformBlock;

        public UploadController(ConnectionManager Conn)
        {
            Conn.CheckDbConnection();


            ErrorListenerTransformBlock = new TransformBlock<LazyError, Tuple<LazyError, bool>>(
                async (e) =>
                {
                    if (Conn.IsReady && CanToSent) // Server Connector to online or offline ?
                    {
                        try
                        {
                            CanToSent = await Uploader.SentOneErrorToDbAsync(Conn, e);
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
                    return new Tuple<LazyError, bool>(e, CanToSent);
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxMessagesPerTask = 1,
                    MaxDegreeOfParallelism = 1
                });

            ErrorListenerTransformBlock.LinkTo(AcknowledgeController.AcknowledgeActionBlock);
        }
    }
}
