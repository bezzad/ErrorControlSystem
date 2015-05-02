using System;
using System.Windows.Forms;
using ErrorControlSystem.Examples.WinForms.SomeNamespace;
using ErrorControlSystem.Shared;

namespace ErrorControlSystem.Examples.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //
            // ------------------ Initial Error Control System --------------------------------
            //
            ExceptionHandler.Engine.Start("localhost", "UsersManagements",
                ErrorHandlingOptions.Default &
                   ~ErrorHandlingOptions.ResizeSnapshots);
            
            // Or Set Option this way:
            ErrorHandlingOption.ResizeSnapshots = false;
            ErrorHandlingOption.ReportHandledExceptions = false;

            //
            // Some of the optional configuration items.
            //
            // Except 'NotImplementedException' from raise log
            ExceptionHandler.Filter.ExemptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(typeof(FormatException));

            // Add extra data for labeling exceptions
            ExceptionHandler.Filter.AttachExtraData.Add("WinForms v3", "beta version");

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
            // Show unhandled exception message customized mode. 
            ExceptionHandler.OnShowUnhandledError += AlertUnhandledErrors;
            //
            // ---------------------------------------------------------------------------------
            //

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormTest());
        }

        /// <summary>
        /// Show unhandled exception message customized mode.
        /// </summary>
        /// <param name="sender">Raw exception object</param>
        /// <param name="e">Compiled error object</param>
        public static void AlertUnhandledErrors(object sender, UnhandledErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorObject.Message);
        }

        public static void Exp()
        {
            throw new Exception("Test UnHandled MainThread Exception");
        }
    }
}
