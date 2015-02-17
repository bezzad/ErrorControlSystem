using System.Diagnostics;
using ConnectionsManager;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ExceptionManager
{

    /// <summary>
    /// SingleTon class Kernel for control all roles ...
    /// Just initial once time
    /// </summary>
    public static class Kernel
    {
        #region SingleTon Kernel Instance

        //private static object syncObj = new object();
        //private static Kernel _instance;

        //public static Kernel Instance
        //{
        //    get
        //    {
        //        Debug.Assert(Conn.SqlConn != null, "Connection can not be NULL !!!");

        //        lock (syncObj)
        //        {
        //            return _instance ?? (_instance = new Kernel());
        //        }
        //    }
        //}

        #endregion

        #region Properties

        public static volatile bool IsSelfException = false;

        public static UploadController Uploader { get; private set; }

        public static RoutingDataStoragePath Router { get; private set; }

        public static FetchErrorsToDisk CacheManager { get; private set; }
        
        #endregion

        #region Protected Constructor

        static Kernel()
        {
            Uploader = new UploadController(ConnectionManager.Items["UM"]);
            Router = new RoutingDataStoragePath();
            CacheManager = new FetchErrorsToDisk(Router);

            // First time create history of errors to buffer any occurrence error
            CacheReader.ErrorHistory = CacheReader.ErrorHistory ?? new ErrorUniqueCollection();
            //
            // Load error log data to history of errors without snapshot images
            if (CacheReader.ErrorHistory.Count <= 0)
            {
                Router.ReadCacheToHistory();
            }
        }


        #endregion

        #region Methods

        //public static void SetConnection(Connection conn)
        //{
        //    Conn = ConnectionManager.Add(conn);
        //}
        

        #endregion
    }
}
