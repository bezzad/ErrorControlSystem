using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ExceptionManager;

namespace TestErrorHandlerBySelf
{
    //[HandleExceptions(ErrorHandlingOption.Default & ~ErrorHandlingOption.Snapshot)]

#if DEBUG
    [HandleExceptions(".", "UsersManagements")]
#endif
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

            Attribute.GetCustomAttributes(typeof(Program));

            Application.Run(new Form1());
        }

        public static void TestUiException()
        {
            int a = 10, v = 0, c = a / v;
        }
    }
}
