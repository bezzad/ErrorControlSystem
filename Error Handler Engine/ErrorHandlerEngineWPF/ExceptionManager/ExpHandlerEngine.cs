using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security.Permissions;
using System.Threading.Tasks;
using CacheErrors;
using DbConnectionsManager;
using ServerController;
using System.Windows;

namespace ExceptionManager
{

    /// <summary>
    /// Exceptions Handler Engine Class
    /// for handling any exception from your attachment applications. 
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.AllFlags)]
    public static class ExpHandlerEngine
    {
        #region Properties

        private static ErrorHandlerOption _option;

        #endregion

        #region Static Constructors

        static ExpHandlerEngine()
        {
            ExceptionHandler.IsSelfException = true;

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            // Catch all handled exceptions in managed code, before the runtime searches the Call Stack 
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            // Catch all unhandled exceptions in all threads.
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            // Catch all unobserved task exceptions.
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Catch all WPF unhandled exceptions.
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            ExceptionHandler.IsSelfException = false;
        }

        #endregion

        #region Methods

        public static async void Start(ErrorHandlerOption option = ErrorHandlerOption.Default)
        {
            _option = option & ~ErrorHandlerOption.SendCacheToServer;

            await ServerTransmitter.InitialTransmitterAsync();
        }


        public static async void Start(Connection conn, ErrorHandlerOption option = ErrorHandlerOption.Default)
        {
            _option = option;

            ConnectionManager.Add(conn, "ErrorHandlerServer");
            ConnectionManager.SetToDefaultConnection("ErrorHandlerServer");

            await ServerTransmitter.InitialTransmitterAsync();

            var publicSetting = await ServerTransmitter.GetErrorHandlerOptionAsync();
            if (publicSetting != 0)
                _option = publicSetting;

            await CacheController.CheckStateAsync();
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
            e.Exception.RaiseLog(_option | ErrorHandlerOption.IsHandled);
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
            e.Exception.RaiseLog(_option & ~ErrorHandlerOption.IsHandled, "Unobserved Task Exception");
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
            (e.ExceptionObject as Exception).RaiseLog(_option & ~ErrorHandlerOption.IsHandled, "Unhandled UI Exception");

            Application.Current.Shutdown();
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Prevent default unhandled exception processing
            e.Handled = true;

            e.Exception.RaiseLog(_option & ~ErrorHandlerOption.IsHandled, "Unhandled Thread Exception");
        }

        #endregion
    }
}