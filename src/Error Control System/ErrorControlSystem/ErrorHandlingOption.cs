using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorControlSystem
{
    public static class ErrorHandlingOption
    {
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
        /// Each time an unhandled exception occurs, the bug report is prepared to be send at the next application startup. 
        /// If submission fails (i.e. there is no network connection), the queue grows with each additional
        /// exception and resulting error reports. This limits the max no of queued reports to limit the disk space usage.
        /// Default value is 50.
        /// </summary>
        public static int MaxQueuedReports { get; set; }

        /// <summary>
        /// Gets or sets the size of error caches that each time an exception occurs, 
        /// the bug report is prepared to be send to server.
        /// Default value is 4194304 bytes.
        /// </summary>
        public static long CacheLimitSize { get; set; }


    }
}
