using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ErrorHandlerEngine;

namespace TestErrorHandlerAppNet2
{
    static class Program
    {
        private static IErrorHandlerAdapter iErrorHandler;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Type myClassAdapterType = Type.GetTypeFromProgID("ErrorHandlerEngineAdapter.ErrorHandlerAdapter");
            var myClassAdapterType = Type.GetTypeFromCLSID(new Guid("11BE9CF0-218D-45C6-A9AD-55C891F936F0"));
            var myClassAdapterInstance = Activator.CreateInstance(myClassAdapterType);
            iErrorHandler = (IErrorHandlerAdapter)myClassAdapterInstance;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new Form1());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            iErrorHandler.Raise(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }
    }
}
