using System;
using System.Threading;
using System.Windows.Forms;
using ErrorControlSystem;

namespace ErrorLogAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Thread.Sleep(2000); // First Open Example Apps then this app

            ExceptionHandler.Engine.Start(".", "UsersManagements", "sa", "123");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogReader());
        }
    }
}
