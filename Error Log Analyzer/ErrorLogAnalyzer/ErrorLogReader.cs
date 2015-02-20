using System;
using System.Threading.Tasks.Dataflow;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;
using Newtonsoft.Json;

namespace ErrorLogAnalyzer
{
    public static class ErrorLogReader
    {
        public static EventHandler<LazyErrorEventArgs> OnReadLazyError = delegate { };

        #region Action Block Fetch Errors By Event-Driven System

        private static ActionBlock<String> abFetchByEventDriven = new ActionBlock<String>(async (json) =>
        {
            // Help Link for JSON parser: http://jsonformatter.curiousconcept.com/

            var errors = await JsonConvert.DeserializeObjectAsync<Error[]>(json);

            foreach (var error in errors)
            {
                OnReadLazyError(error, new LazyErrorEventArgs(error));
            }
        },
            new ExecutionDataflowBlockOptions
            {
                MaxMessagesPerTask = 1
            });

        #endregion

        public static async void ReadAsync()
        {
            var allJsonString = await RoutingDataStoragePath.ReadLogAsync();

            if (!string.IsNullOrEmpty(allJsonString))
            {
                allJsonString = string.Format("{0} {1} {2}", @"[", allJsonString.Substring(0, allJsonString.Length - 3), "]");

                await abFetchByEventDriven.SendAsync(allJsonString);
            }
        }


    }
}
