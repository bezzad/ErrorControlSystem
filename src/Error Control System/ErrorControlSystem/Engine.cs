namespace ErrorControlSystem
{
    using System;
    using System.Diagnostics;
    using System.Runtime.ExceptionServices;
    using System.Security.Permissions;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorControlSystem.CachErrors;
    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.ServerController;

    public static partial class ExceptionHandler
    {
        /// <summary>
        /// Exceptions Handler Engine Class
        /// for handling any exception from your attachment applications. 
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static class Engine
        {
            #region Properties

            private static ErrorHandlingOptions _option;

            #endregion

            #region Static Constructors

            static Engine()
            {
                if (!AssembelyLoaded) LoadAssemblies();

                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

                //System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                // Catch all handled exceptions in managed code, before the runtime searches the Call Stack 
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

                // Catch all unhandled exceptions in all threads.
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

                // Catch all unobserved task exceptions.
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

                // Catch all unhandled exceptions.
                System.Windows.Forms.Application.ThreadException += ThreadExceptionHandler;
            }

            #endregion

            #region Methods

            public static async void Start(ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                _option = option & ~ErrorHandlingOptions.SendCacheToServer;

                await ServerTransmitter.InitialTransmitterAsync();
            }


            public static async void Start(Connection conn, ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                _option = option;

                ConnectionManager.Add(conn, "ErrorControlSystemConnection");
                ConnectionManager.SetToDefaultConnection("ErrorControlSystemConnection");

                await ServerTransmitter.InitialTransmitterAsync();

                var publicSetting = await ServerTransmitter.GetErrorHandlingOptionsAsync();
                if (publicSetting != 0)
                    _option = publicSetting;

                await CacheController.CheckStateAsync();
            }


            public static void Start(string server, string database, string username, string pass,
                ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                var conn = new Connection(server, database, username, pass);

                Start(conn, option);
            }

            public static void Start(string server, string database,
                ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                var conn = new Connection(server, database);

                Start(conn, option);
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
            [HandleProcessCorruptedStateExceptions]
            private static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option | ErrorHandlingOptions.IsHandled);
            }

            /// <summary>
            /// If you are using Tasks, then you may have "unobserved task exceptions". 
            /// This event allows you to trap them. It also has a method called SetObserved,
            /// which allows you to try to recover from the issue.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            [HandleProcessCorruptedStateExceptions]
            private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option & ~ErrorHandlingOptions.IsHandled, "Unobserved Task Exception");
            }

            /// <summary>
            /// If you are hosting any WinForm components in your WPF application, 
            /// this final event is one to watch. There's no way to influence events thereafter, 
            /// but at least you get to see what the problem was.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            [HandleProcessCorruptedStateExceptions]
            private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option & ~ErrorHandlingOptions.IsHandled, "Unhandled Thread Exception");
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
            [HandleProcessCorruptedStateExceptions]
            private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
            {
                (e.ExceptionObject as Exception).RaiseLog(_option & ~ErrorHandlingOptions.IsHandled,
                    "Unhandled UI Exception");

                Environment.Exit(0);
            }

            #endregion
        }
    }
}