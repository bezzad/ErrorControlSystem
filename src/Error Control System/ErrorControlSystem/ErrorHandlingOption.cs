using System;
using System.Configuration;
using System.Linq.Expressions;
using ErrorControlSystem.CacheErrors;
using ErrorControlSystem.Properties;

namespace ErrorControlSystem
{
    public static class ErrorHandlingOption
    {
        #region Fields

        private static bool _displayDeveloperUI;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets a value indicating whether the unhandled exception handlers in <see cref="ErrorControlSystem.ExceptionHandler"/> 
        /// class actually handle exceptions.
        /// </summary>
        public static bool IsHandled { get; set; }


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
        /// Gets or sets the number of bug reports that can be queued for cache submission. 
        /// Each time an exception occurs, the exception handlers is prepared to be send to server. 
        /// If submission fails (i.e. there is no network connection), the queue grows with each additional
        /// exception and resulting error reports. This limits the max no of queued reports to limit the disk space usage.
        /// Default value is 500.
        /// </summary>
        public static int MaxQueuedCacheErrors { get; set; }


        /// <summary>
        /// Gets or sets the size of error caches that each time an exception occurs, 
        /// the ECS bug reporters is prepared to be send to the server.
        /// Default value is 4194304 bytes.
        /// </summary>
        public static long CacheLimitSize { get; set; }


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
        /// Gets or sets a value indicating whether Report handled exceptions or not?
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
        /// Condition for display developer UI is that application running in Debug Mode and from IDE.
        /// Default value is true.
        /// </summary>
        public static bool DisplayDeveloperUI
        {
            get { return !ReleaseMode && IsRunningFromIDE && _displayDeveloperUI; }
            set { _displayDeveloperUI = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to enable send error data to network server.
        /// For example when network is crashed or database is dropped then should be not send data.
        /// Network sending is enabled by default.
        /// </summary>
        internal static bool? EnableNetworkSending { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Replicate the behavior of normal Properties.Settings class via getting default values for null settings.
        /// Use this like GetDefaultValue(() =&gt; CacheLimitSize);
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetDefaultValue<T>(Expression<Func<T>> propertyExpression)
        {
            var defaultSetting =
                typeof(Properties.Settings).GetProperty(((MemberExpression)propertyExpression.Body).Member.Name)
                                           .GetCustomAttributes(typeof(DefaultSettingValueAttribute), false)[0] as DefaultSettingValueAttribute;
            return defaultSetting != null ? defaultSetting.Value : null;
        }


        /// <summary>
        /// Loads the app config settings to set default all properties.
        /// </summary>
        private static void LoadSettings()
        {
            CacheLimitSize = Settings.Default.CacheLimitSize;
            DisplayDeveloperUI = Settings.Default.DisplayDeveloperUI;
            EnableNetworkSending = Settings.Default.EnableNetworkSending;
            ReportHandledExceptions = Settings.Default.ReportHandledExceptions;
            ErrorLogPath = Settings.Default.ErrorLogPath;
            ExitApplicationImmediately = Settings.Default.ExitApplicationImmediately;
            CustomStoragePath = Settings.Default.CustomStoragePath;
            MaxQueuedCacheErrors = Settings.Default.MaxQueuedCacheErrors;
            HandleProcessCorruptedStateExceptions = Settings.Default.HandleProcessCorruptedStateExceptions;
            StoragePath = Settings.Default.StoragePath;
        }

        #endregion

        #region Constructors

        static ErrorHandlingOption()
        {
            LoadSettings();
        }

        #endregion
    }
}
