using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectionsManager;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ExceptionManager
{

    /// <summary>
    /// Exceptions Handler Engine Class
    /// for handling any exception from your attachment applications. 
    /// </summary>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.AllFlags)]
    public static class ExpHandlerEngine
    {
        #region Properties

        private static ExceptionHandlerOption _option;

        public static volatile bool IsSelfException = false;

        #endregion

        #region Static Constructors

        static ExpHandlerEngine()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Catch all handled exceptions in managed code, before the runtime searches the Call Stack 
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            // Catch all unhandled exceptions in all threads.
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            // Catch all unobserved task exceptions.
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Catch all unhandled exceptions.
            Application.ThreadException += ThreadExceptionHandler;

            // First time create history of errors to buffer any occurrence error
            //
            // Load error log data to history of errors without snapshot images
            if (CacheController.ErrorHistory.Count <= 0)
                CacheController.ReadCacheAsync();
        }

        #endregion

        #region Methods

        public static void Start(ExceptionHandlerOption option = ExceptionHandlerOption.Default)
        {
            _option = option & ~ExceptionHandlerOption.SendCacheToServer;

            // Set Up loader to run static constructor
            Uploader.CanToSent = true;
        }


        public static void Start(Connection conn, ExceptionHandlerOption option = ExceptionHandlerOption.Default)
        {
            ConnectionManager.Add(conn, "ErrorHandlerServer");
            ConnectionManager.SetToDefaultConnection("ErrorHandlerServer");

            Start(option);
        }


        /// <summary>
        /// This is new to .Net 4 and is extremely useful for ensuring that you ALWAYS log SOMETHING.
        /// Whenever any kind of exception is fired in your application, a FirstChangeExcetpion is raised,
        /// even if the exception was within a Try/Catch block and safely handled.
        /// This is GREAT for logging every wart and boil, but can often result in too much spam, 
        /// if your application has a lot of expected/handled exceptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            e.Exception.RaiseLog(_option | ExceptionHandlerOption.IsHandled);
        }

        /// <summary>
        /// If you are using Tasks, then you may have "unobserved task exceptions". 
        /// This event allows you to trap them. It also has a method called SetObserved,
        /// which allows you to try to recover from the issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.Exception.RaiseLog(_option & ~ExceptionHandlerOption.IsHandled, "Unobserved Task Exception");
        }

        /// <summary>
        /// If you are hosting any WinForm components in your WPF application, 
        /// this final event is one to watch. There's no way to influence events thereafter, 
        /// but at least you get to see what the problem was.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            e.Exception.RaiseLog(_option & ~ExceptionHandlerOption.IsHandled, "Unhandled Thread Exception");
        }

        /// <summary>
        /// Catch all unhandled exceptions in all threads.
        /// Although Application.DispatcherUnhandledException covers most issues in the current AppDomain, 
        /// in extremely rare circumstances, you may be running a thread on a second AppDomain. 
        /// This event conveys the other AppDomain unhandled exception, 
        /// but there are no Handled property, just an IsTerminating flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            (e.ExceptionObject as Exception).RaiseLog(_option & ~ExceptionHandlerOption.IsHandled, "Unhandled UI Exception");

            Application.Exit();
        }

        #endregion
    }
}