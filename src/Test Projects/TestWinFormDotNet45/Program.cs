using System;
using System.Windows.Forms;
using ErrorControlSystem;
using ErrorControlSystem.Shared;
using TestWinFormDotNet45.TestNameSpace;

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
            // ------------------ Initial Error Control System --------------------------------
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
                new CodeScope("Assembly", "Namespace", "Class", "Method"));

            // Filter a method of a specific class in my assembly from raise unhanded exceptions log
            ExceptionHandler.Filter.ExemptedCodeScopes.Add(
                new CodeScope("", "", "ExpThrower", ""));

            // Do not raise any exception in other code places.
            //ExceptionHandler.Filter.JustRaiseErrorCodeScopes.Add(
            //    new CodeScope("Assembly", "Namespace", "Class", "Method"));
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
