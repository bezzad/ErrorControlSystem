//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Control System                                                           //
//   This Class Library creates a way of handling structured exception handling,    //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2014-2015                                                        //
//   Behzad Khosravifar                                                             //
//   mailto:Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program published by the Free Software Foundation,                        //
//   either version 1.0.0 of the License, or (at your option) any later version.        //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                           //
//                                                                                  //
//**********************************************************************************//


namespace ErrorControlSystem
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Security;
    using System.Windows.Forms;

    using ErrorControlSystem.CacheErrors;
    using ErrorControlSystem.Resources;
    using ErrorControlSystem.Shared;

    /// <summary>
    /// Additional Data attached to exception object.
    /// </summary>
    public static partial class ExceptionHandler
    {
        #region Properties

        internal static bool AssembelyLoaded { get; set; }

        /// <summary>
        /// Represents the method that will handle the event raised by an exception that is not handled by the application domain.
        /// </summary>
        public static UnhandledErrorEventHandler OnShowUnhandledError = delegate { };

        #endregion

        #region Constructors
        static ExceptionHandler()
        {
            Filter.ExemptedCodeScopes.Add(new CodeScope(Assembly.GetExecutingAssembly().GetName().Name, "CacheErrors", null, null));

            LoadAssemblies();

            StorageRouter.ReadyCache();
        }

        #endregion

        #region Methods

        internal static void LoadAssemblies()
        {
            EmbeddedAssembly.Load("System.Data.SqlServerCe.dll");
            EmbeddedAssembly.Load("System.Threading.Tasks.Dataflow.dll");
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => EmbeddedAssembly.Get(e.Name);

            AssembelyLoaded = true;
        }

        /// <summary>
        /// Raise log of handled error's.
        /// </summary>
        /// <param name="exp">The Error object.</param>
        /// <param name="option">The option to select what jobs must be doing and stored in Error object's.</param>
        /// <param name="errorTitle">Determine the mode of occurrence of an error in the program.</param>
        /// <returns></returns>
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public static Error RaiseLog(this Exception exp, ErrorHandlingOptions option = ErrorHandlingOptions.Default,
            String errorTitle = "UnHandled Exception")
        {
            //
            // Create call stack till this method
            // 1# Handled Exception ---> Create from this stack trace (by skip(2): RaiseLog and FirstChance Method)
            // 2# Unhandled Exception & Null Exception.StackTrace ---> Create from this stack trace (by skip(2): RaiseLog and UnhandledException Method)
            // 3# Unhandled Exception & Not Null Exception ---> Create from exp stack trace
            var callStackFrames = !option.HasFlag(ErrorHandlingOptions.IsHandled) && exp.StackTrace != null // 3#
                ? new StackTrace(exp, true).GetFrames() // 3#: Raise from UnhandledException
                : new StackTrace(2, true).GetFrames(); // 1# or 2#: Raise from FirstChance

            #region ---------------------------- Filter exception ---------------------------------------

            if (option.HasFlag(ErrorHandlingOptions.FilterExceptions))
            {
                //
                // Find exception type:
                var expType = exp.GetType();
                //
                // Is exception within non-snapshot list? (yes => remove snapshot option)
                if (Filter.NonSnapshotExceptionTypes.Any(x => x == expType))
                    option &= ~ErrorHandlingOptions.Snapshot;
                //
                // Is exception in exempted list?
                if (Filter.ExemptedExceptionTypes.Any(x => x == expType) ||
                    Filter.ExemptedCodeScopes.Any(x => x.IsCallFromThisPlace(callStackFrames)))
                    return null;
                //
                // Must be exception occurred from these code scopes to that raised by handler.
                if (Filter.JustRaiseErrorCodeScopes.Count > 0 &&
                    !Filter.JustRaiseErrorCodeScopes.Any(x => x.IsCallFromThisPlace(callStackFrames)))
                    return null;
            }

            #endregion ------------------------------------------------------------------------------------

            //
            // initial the error object by additional data 
            var error = new Error(exp, callStackFrames, option);

            if (!option.HasFlag(ErrorHandlingOptions.IsHandled)) // Is Unhandled Exception ?
            {
                //
                // Handle 'OnShowUnhandledError' events
                OnShowUnhandledError(exp, new UnhandledErrorEventArgs(error));
                //
                // Alert Unhandled Error 
                if (option.HasFlag(ErrorHandlingOptions.AlertUnHandledError))
                {
                    MessageBox.Show(exp.Message,
                        errorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
            }

            CacheController.CacheTheError(error, option);

            return error;
        }

        #endregion
    }
}
