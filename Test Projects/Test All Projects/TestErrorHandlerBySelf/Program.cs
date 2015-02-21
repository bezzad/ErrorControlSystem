using System;
using System.Windows.Forms;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;

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

            HandleExceptions.Start(new Connection("localhost", "UsersManagements"),
                ErrorHandlingOption.Default & ~ErrorHandlingOption.ReSizeSnapshots);

            Application.Run(new Form1());
        }

        public static void TestUiException()
        {
            int a = 10, v = 0, c = a / v;
        }
    }
}
