using System;
using System.Windows.Forms;
using DbConnectionsManager;
using ExceptionManager;
using TestErrorHandlerDotNet45;

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

            //
            //  ------------------ Initial Error Handler Engine --------------------------------
            //
            ExpHandlerEngine.Start(new Connection(@"localhost", "UsersManagements"),
                ErrorHandlerOption.Default & ~ErrorHandlerOption.ReSizeSnapshots);

            // Except 'NotImplementedException' from raise log
            ExceptionHandler.ExceptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.NonSnapshotExceptionTypes.Add(typeof(Exception));
            
            // Add extra data for labeling exceptions
            ExceptionHandler.AttachExtraData.Add("TestErrorHandlerBySelf v2.1.1.0", "beta version");
            //
            // ---------------------------------------------------------------------------------
            //

            Application.Run(new FormTest());
        }

        public static void Exp()
        {
            throw new Exception("Test UnHandled MainThread Exception");
        }
    }
}
