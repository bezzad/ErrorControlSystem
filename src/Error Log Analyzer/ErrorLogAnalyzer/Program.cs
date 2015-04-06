using System;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ExceptionHandler.Engine.Start(".", "UsersManagements");

            Application.Run(new LogReader());
        }
    }
}
