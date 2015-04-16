using System.IO;

namespace ErrorControlSystem
{
    using System;
    using System.Linq;
    using System.Reflection;


    using ErrorControlSystem.CacheErrors;
    using ErrorControlSystem.Properties;
    using ErrorControlSystem.Shared;

    public static class ErrorHandlingOption
    {
        #region Fields

        private static bool _displayDeveloperUI;

        private static bool _fetchServerDateTime;

        private static bool _resizeSnapshots;


        #endregion

        #region Properties

        /// <summary>
        /// Get or Set startup sent state, that for sent cache data to server in application startup or not.
        /// Default value is false.
        /// Used for know application closing and disconnection between sent state.
        /// Or new exception raising will be caused to sent data to server
        /// </summary>
        public static bool AtSentState
        {
            get { return ReadSetting<bool>("AtSentState"); }
            set { WriteSetting("AtSentState", value); }
        }


        /// <summary>
        /// Get or Set startup sent state, that for sent cache data to server in application startup or not.
        /// Default value is false.
        /// </summary>
        public static bool SentOnStartup
        {
            get { return ReadSetting<bool>("SentOnStartup"); }
            set { WriteSetting("SentOnStartup", value); }
        }


        /// <summary>
        /// Gets or sets the expire hours for sent errors from cache to server.
        /// ExpireHours is how many hours may be past after first logged error.
        /// </summary>
        public static int ExpireHours
        {
            get { return ReadSetting<int>("ExpireHours"); }
            set { WriteSetting("ExpireHours", value); }
        }


        /// <summary>
        /// Gets a value indicating whether the current process is running by IDE (Visual Studio) or Execute file ?
        /// </summary>
        public static bool IsRunningFromIDE { get { return System.Diagnostics.Debugger.IsAttached; } }


        /// <summary>
        /// Gets or sets a value indicating whether the application will exit after handling and logging an unhandled exception.
        /// Default value is true. 
        /// </summary>
        public static bool ExitApplicationImmediately { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to handle exceptions even in a corrupted process thought the 'HandleProcessCorruptedStateExceptions'
        /// flag. The default value for this is false since generating exception handlers for a corrupted process may not be successful so use with caution.
        /// </summary>
        public static bool HandleProcessCorruptedStateExceptions { get; set; }


        /// <summary>
        /// Gets true when cache size filled, otherwise false.
        /// </summary>
        public static bool CacheFilled
        {
            get
            {
                var rootDir = StoragePathHelper.GetPath(StoragePath);
                return new DirectoryInfo(rootDir).GetDirectorySize() >= CacheLimitSize;
            }
        }


        /// <summary>
        /// Gets or sets the size of error caches that each time an exception occurs, 
        /// the ECS bug reporters is prepared to be send to the server.
        /// Default value is 4194304 bytes.
        /// </summary>
        public static long CacheLimitSize
        {
            get { return ReadSetting<long>("CacheLimitSize"); }
            set { WriteSetting("CacheLimitSize", value); }
        }


        /// <summary>
        /// Gets or sets the number of error reports that can be queued for submission. 
        /// Each time an exception occurs, the exception handler is prepared to
        /// be send data. If submission fails (i.e. there is no Internet connection), the queue grows with each additional
        /// exception and resulting bug reports. This limits the max no of queued reports to limit the disk space usage.
        /// Default value is 500.
        /// </summary>
        public static int MaxQueuedError
        {
            get { return ReadSetting<int>("MaxQueuedError"); }
            set { WriteSetting("MaxQueuedError", value); }
        }


        /// <summary>
        /// Gets a value indicating whether in the release mode for the Error Control System library. 
        /// In release mode the internal developer UI is not displayed.
        /// </summary>
        internal static bool ReleaseMode
        {
            get
            {
#if DEBUG
                return false;
#else
                return true;
#endif
            }
        }


        /// <summary>
        /// Gets or sets the ErrorControlSystem errors storage path. 
        /// After an exception occurs, the exception handlers are created and queued for submission.
        /// Until then, the reports will be stored in this location. 
        /// Default value is the LocalApplicationData directory.
        /// This setting can either be assigned a full path string or a value from <see cref="ErrorControlSystem.CacheErrors.StoragePath"/> enumeration.
        /// </summary>
        public static StoragePath StoragePath { get; set; }


        /// <summary>
        /// Gets or sets the ErrorControlSystem errors custom storage path.
        /// Default value is <see cref="String.Empty"/>
        /// </summary>
        public static String CustomStoragePath { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether report handled exceptions or not?
        /// If you want also to report handled exceptions, so set it to <C>true</C>; otherwise, <c>false</c>.
        /// Default value is true.
        /// </summary>
        public static bool ReportHandledExceptions { get; set; }


        /// <summary>
        /// Gets or sets the error log path.
        /// </summary>
        /// <value>
        /// The error log path.
        /// </value>
        public static String ErrorLogPath { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to enable developer user interface facilities which enable easier diagnosis of
        /// code maps and other internal errors.
        /// Condition for display developer UI is that application running from IDE and DisplayUnhandledExceptions value was true.
        /// Default value is true.
        /// </summary>
        public static bool DisplayDeveloperUI
        {
            get { return DisplayUnhandledExceptions && IsRunningFromIDE && _displayDeveloperUI; }
            set
            {
                if (value) DisplayUnhandledExceptions = true;
                _displayDeveloperUI = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to enable send error data to network server.
        /// For example when network is crashed or database is dropped then should be not send data.
        /// Network sending is enabled by default.
        /// </summary>
        public static bool EnableNetworkSending { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether network sending is enabled and can to fetch date and time form server.
        /// Default value is true.
        /// </summary>
        public static bool FetchServerDateTime
        {
            get { return _fetchServerDateTime && EnableNetworkSending; }
            set { _fetchServerDateTime = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether can to capture screen.
        /// Default value is true.
        /// </summary>
        public static bool Snapshot { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether resize snapshots.
        /// Default value is true but snapshot property also must be true.
        /// </summary>
        public static bool ResizeSnapshots
        {
            get { return _resizeSnapshots && Snapshot; }
            set
            {
                if (value) Snapshot = true;
                _resizeSnapshots = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether display unhandled exceptions.
        /// </summary>
        public static bool DisplayUnhandledExceptions { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether filter exceptions enabled or not ?
        /// Default value is true.
        /// </summary>
        public static bool FilterExceptions { get; set; }

        /// <summary>
        /// Gets the cache code scope.
        /// </summary>
        internal static CodeScope CacheCodeScope { get; private set; }


        #endregion

        #region Methods

        public static void SetSetting(ErrorHandlingOptions opt)
        {
            CacheLimitSize = Settings.Default.CacheLimitSize;
            ErrorLogPath = Settings.Default.ErrorLogPath;
            CustomStoragePath = Settings.Default.CustomStoragePath;
            StoragePath = Settings.Default.StoragePath;

            DisplayDeveloperUI = opt.HasFlag(ErrorHandlingOptions.DisplayDeveloperUI);
            EnableNetworkSending = opt.HasFlag(ErrorHandlingOptions.EnableNetworkSending);
            ReportHandledExceptions = opt.HasFlag(ErrorHandlingOptions.ReportHandledExceptions);
            ExitApplicationImmediately = opt.HasFlag(ErrorHandlingOptions.ExitApplicationImmediately);
            HandleProcessCorruptedStateExceptions = opt.HasFlag(ErrorHandlingOptions.HandleProcessCorruptedStateExceptions);
            FetchServerDateTime = opt.HasFlag(ErrorHandlingOptions.FetchServerDateTime);
            Snapshot = opt.HasFlag(ErrorHandlingOptions.Snapshot);
            ResizeSnapshots = opt.HasFlag(ErrorHandlingOptions.ResizeSnapshots);
            DisplayUnhandledExceptions = opt.HasFlag(ErrorHandlingOptions.DisplayUnhandledExceptions);
            FilterExceptions = opt.HasFlag(ErrorHandlingOptions.FilterExceptions);
        }

        /// <summary>
        /// Loads the app config settings to set default all properties.
        /// </summary>
        public static void LoadDefaultSettings()
        {
            SetSetting(ErrorHandlingOptions.Default);
        }

        /// <summary>
        /// Check Alls the Options to true value.
        /// </summary>
        public static void FullOptions()
        {
            SetSetting(ErrorHandlingOptions.All);
        }

        /// <summary>
        /// UnCheck Alls the Options.
        /// </summary>
        private static void NoneOptions()
        {
            SetSetting(ErrorHandlingOptions.None);
        }


        public static bool WriteSetting(string key, object value)
        {
            try
            {
                Settings.Default[key] = value;

                Settings.Default.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T ReadSetting<T>(string key)
        {
            try { return (T)Settings.Default[key]; }
            catch { return default(T); }
        }

        #endregion

        #region Constructors

        static ErrorHandlingOption()
        {
            LoadDefaultSettings();

            CacheCodeScope = new CodeScope(Assembly.GetExecutingAssembly().GetName().Name, "CacheErrors", null, null);
        }

        #endregion
    }
}
