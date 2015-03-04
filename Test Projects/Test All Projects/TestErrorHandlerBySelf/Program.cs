using System;
using System.Windows.Forms;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;

namespace TestErrorHandlerBySelf
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ExpHandlerEngine.Start(new Connection(@"localhost", "UsersManagements"),
                ExceptionHandlerOption.Default & ~ExceptionHandlerOption.ReSizeSnapshots);

            ExceptionHandler.ExceptedExceptionTypes.Add(typeof(NotImplementedException));

            Application.Run(new FormTest());
        }

        public static void Exp()
        {
            throw new Exception("Test UnHandled MainThread Exception");
        }
    }
}
