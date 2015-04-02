using System;
using System.Windows.Forms;
using ErrorHandlerEngine;

namespace TestWinFormDotNet45
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
            ExceptionHandler.Engine.Start("localhost", "UsersManagements",
                   ErrorHandlingOptions.Default & ~ErrorHandlingOptions.ReSizeSnapshots);
            //
            // Some of the optional configuration items.
            //
            // Except 'NotImplementedException' from raise log
            ExceptionHandler.Filter.ExemptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(typeof(FormatException));

            // Add extra data for labeling exceptions
            ExceptionHandler.Filter.AttachExtraData.Add("TestWinFormDotNet45 v3", "beta version");

            // Filter a method of a specific class in my assembly from raise unhanded exceptions log
            ExceptionHandler.Filter.ExemptedCodeScopes.Add(
                new CodeScope("TestWinFormDotNet45", "FormTest", "btnExemptedMethodException_Click"));

            // The just raise error from 'TestWinFormDotNet45'.
            // Do not raise any exception in other code places.
            ExceptionHandler.Filter.JustRaiseErrorCodeScopes.Add(
                new CodeScope("TestWinFormDotNet45", null, null));
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
