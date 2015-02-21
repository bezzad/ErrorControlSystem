using ConnectionsManager;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ExceptionManager
{

    /// <summary>
    /// Shared class Kernel for control all roles.
    /// Singleton Pattern.
    /// </summary>
    public static class Kernel
    {
        #region Properties

        public static volatile bool IsSelfException = false;

        public static UploadController Uploader { get; private set; }

        public static FetchErrorsToDisk CacheManager { get; private set; }

        #endregion

        #region Constructor

        static Kernel()
        {
            Uploader = new UploadController();
            CacheManager = new FetchErrorsToDisk();

            // First time create history of errors to buffer any occurrence error
            //
            // Load error log data to history of errors without snapshot images
            if (CacheReader.ErrorHistory.Count <= 0)
                CacheReader.ReadCacheToHistory();
        }

        #endregion
    }
}
