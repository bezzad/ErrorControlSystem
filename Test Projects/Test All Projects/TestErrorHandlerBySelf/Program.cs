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

            ExpHandlerEngine.Start(new Connection("localhost", "UsersManagements"));

            Application.Run(new FormTest());
        }

        public static void UIExp()
        {
            throw new Exception("My UI Unhandled Exception");
        }
    }
}
