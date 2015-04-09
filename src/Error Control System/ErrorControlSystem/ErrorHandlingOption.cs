using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorControlSystem.CachErrors;
using ErrorControlSystem.Properties;

namespace ErrorControlSystem
{
    public static class ErrorHandlingOption
    {
        private static bool _displayDeveloperUI;

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
        /// the ECS bug reporters is prepared to be send to server.
        /// Default value is 4194304 bytes.
        /// </summary>
        public static long CacheLimitSize { get { return Settings.Default.CacheLimitSize; } }


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
        /// This setting can either be assigned a full path string or a value from <see cref="ErrorControlSystem.CachErrors.StoragePath"/> enumeration.
        /// </summary>
        public static StoragePath StoragePath { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to enable developer user interface facilities which enable easier diagnosis of
        /// code maps and other internal errors.
        /// Condition for display developer UI is that application running in Debug Mode and from IDE.
        /// Default value is true.
        /// </summary>
        internal static bool DisplayDeveloperUI
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



    }
}
