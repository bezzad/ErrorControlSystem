using System;
using System.Windows.Forms;
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

#if DEBUG
            HandleExceptions.Start(".", "UsersManagements");
#else
            HandleExceptions.Start(ErrorHandlingOption.Default & ~ErrorHandlingOption.Snapshot)
#endif

            Application.Run(new Form1());
        }

        public static void TestUiException()
        {
            int a = 10, v = 0, c = a / v;
        }
    }
}
