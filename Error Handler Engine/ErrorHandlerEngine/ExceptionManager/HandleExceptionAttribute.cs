using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectionsManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ExceptionManager
{
    [Serializable]
    [ComVisible(true)]
    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain |
        SecurityPermissionFlag.ControlThread | SecurityPermissionFlag.UnmanagedCode)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)] // Non-Multiuse attribute.
    public class HandleExceptionsAttribute : Attribute
    {
        #region Private Properties

        private bool _notInitial = true;
        private ErrorHandlingOption _option;

        #endregion

        #region Constructors

        public HandleExceptionsAttribute(ErrorHandlingOption option = ErrorHandlingOption.Default) // Constructor just for Caching not sent to server
        {
            _option = option;
            if (_notInitial) Initialize();
        }

        public HandleExceptionsAttribute(string dataSource, string initialCatalog, int timeOut, string username,
            string pass, int portNumber, string attachDbFilename, string providerName = "System.Data.SqlClient",
            ErrorHandlingOption option = ErrorHandlingOption.Default) // Send caching data to server
            : this(option)
        {
            Kernel.SetConnection(
                new Connection("UM", dataSource, initialCatalog, timeOut, username, pass, portNumber, attachDbFilename, providerName));
        }


        public HandleExceptionsAttribute(string dataSource, string initialCatalog, int timeOut, string username,
            string pass, int portNumber = 1433,
            ErrorHandlingOption option = ErrorHandlingOption.Default) // Send caching data to server
            : this(option)
        {
            Kernel.SetConnection(new Connection("UM", dataSource, initialCatalog, timeOut, username, pass, portNumber));
        }

        public HandleExceptionsAttribute(string dataSource, string initialCatalog, int timeOut = 30,
            ErrorHandlingOption option = ErrorHandlingOption.Default) // Send caching data to server
            : this(option)
        {
            Kernel.SetConnection(new Connection("UM", dataSource, initialCatalog, timeOut));
        }
        #endregion

        #region Methods

        private void Initialize()
        {
            _notInitial = true;

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Catch all handled exceptions in managed code, before the runtime searches the Call Stack 
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            // Catch all unhandled exceptions in all threads.
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            // Catch all unobserved task exceptions.
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Catch all unhandled exceptions.
            Application.ThreadException += ThreadExceptionHandler;
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
        protected virtual void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            e.Exception.RaiseLog(_option | ErrorHandlingOption.IsHandled);
        }

        /// <summary>
        /// If you are using Tasks, then you may have "unobserved task exceptions". 
        /// This event allows you to trap them. It also has a method called SetObserved,
        /// which allows you to try to recover from the issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.Exception.RaiseLog(_option, "Unobserved Task Exception");
        }

        /// <summary>
        /// If you are hosting any WinForm components in your WPF application, 
        /// this final event is one to watch. There's no way to influence events thereafter, 
        /// but at least you get to see what the problem was.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs args)
        {
            args.Exception.RaiseLog(_option, "Unhandled Thread Exception");
        }

        /// <summary>
        /// Catch all unhandled exceptions in all threads.
        /// Although Application.DispatcherUnhandledException covers most issues in the current AppDomain, 
        /// in extremely rare circumstances, you may be running a thread on a second AppDomain. 
        /// This event conveys the other AppDomain unhandled exception, 
        /// but there are no Handled property, just an IsTerminating flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
#if DEBUG // In debug mode do not custom-handle the exception, let Visual Studio handle it
            MessageBox.Show(((Exception)args.ExceptionObject).Message, "Unhandled UI Exception - In Debug Mode");
#else
            (args.ExceptionObject as Exception).RaiseLog(_Option, "Unhandled UI Exception");
            Application.Exit();
#endif
        }

        #endregion

    }
}
