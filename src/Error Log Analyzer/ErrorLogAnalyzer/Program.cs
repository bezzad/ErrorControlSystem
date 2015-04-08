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
            ExceptionHandler.Engine.Start(".", "UsersManagements");
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogReader());
        }
    }
}
