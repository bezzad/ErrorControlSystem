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
    public class Kernel
    {
        #region SingleTon Kernel Instance
        
        private static object syncObj = new object();
        private static Kernel _instance;

        public static Kernel Instance
        {
            get
            {
                Debug.Assert(Conn.SqlConn != null, "Connection can not be NULL !!!");

                lock (syncObj)
                {
                    if (_instance == null)
                    {
                        _instance = new Kernel();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Properties
        
        #region Static Fields
        
        public static volatile bool IsSelfException = false;

        public static ConnectionManager Conn { get; protected set; }

        #endregion

        #region Dynamic Fields

        public UploadController Uploader { get; protected set; }

        public RoutingDataStoragePath Router { get; protected set; }

        public FetchErrorsToDisk CacheManager { get; protected set; }

        #endregion

        #endregion

        #region Protected Constructor
        
        protected Kernel()
        {
            Uploader = new UploadController(Conn);
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
        
        public static void SetConnection(Connection conn)
        {
            Conn = new ConnectionManager(conn);
        }

        public static void SetConnection(string connString)
        {
            Conn = new ConnectionManager(connString);
        }

        #endregion
    }
}
