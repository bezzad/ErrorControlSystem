//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Handler Engine 1.0.0.2                                                   //
//   This Class Library creates a way of handling structured exception handling,     //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2015                                                             //
//   Behzad Khosravifar                                                             //
//   Email: Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program published by the Free Software Foundation,                        //
//   either version 2 of the License, or (at your option) any later version.        //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                           //
//                                                                                  //
//**********************************************************************************//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ErrorHandlerEngine.CacheErrors;
using ErrorHandlerEngine.Resources;
using ErrorHandlerEngine.Shared;

namespace ErrorHandlerEngine.ExceptionManager
{
    /// <summary>
    /// Additional Data attached to exception object.
    /// </summary>
    public static class ExceptionHandler
    {
        #region Properties
        internal static bool AssembelyLoaded { get; set; }

        #endregion

        #region Constructors
        static ExceptionHandler()
        {
            Filter.ExemptedCodeScopes.Add(new CodeScope(Assembly.GetExecutingAssembly().GetName().Name, null, null));

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
        public static Error RaiseLog(this Exception exp, ErrorHandlingOptions option = ErrorHandlingOptions.Default,
            String errorTitle = "UnHandled Exception")
        {
            #region ---------------------------- Filter exception ---------------------------------------
            //
            // Find exception type:
            var expType = exp.GetType();
            //
            // Is exception within non-snapshot list? (yes => remove snapshot option)
            if (Filter.NonSnapshotExceptionTypes.Any(x => x == expType))
                option &= ~ErrorHandlingOptions.Snapshot;
            //
            // Create call stack till this method
            // Handled exception OR Unhandled exception ?
            var callStackFrames = option.HasFlag(ErrorHandlingOptions.IsHandled)
                ? new StackTrace(2).GetFrames()    // Raise from FirstChance:
                : new StackTrace(exp).GetFrames(); // Raise from UnhandledException:
            //
            // Is exception in exempted list?
            if (Filter.ExemptedExceptionTypes.Any(x => x == expType) ||
                Filter.ExemptedCodeScopes.Any(x => x.IsCallFromThisPlace(callStackFrames)))
                return null;
            #endregion ------------------------------------------------------------------------------------

            //
            // initial the error object by additional data 
            var error = new Error(exp, option);

            if (option.HasFlag(ErrorHandlingOptions.AlertUnHandledError) && !option.HasFlag(ErrorHandlingOptions.IsHandled)) // Alert Unhandled Error 
            {
                MessageBox.Show(exp.Message,
                    errorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

            CacheController.CacheTheError(error, option);

            return error;
        }

        #endregion

        #region Internal Classes

        public static class Filter
        {
            /// <summary>
            /// List of exceptions that happen but not logs.
            /// </summary>
            public static List<Type> ExemptedExceptionTypes = new List<Type>();

            /// <summary>
            /// List of the exception types that do not have a screen capture.
            /// </summary>
            public static List<Type> NonSnapshotExceptionTypes = new List<Type>();

            /// <summary>
            /// Dictionary of key/value data that will be stored in exceptions as additional data.
            /// </summary>
            public static Dictionary<string, string> AttachExtraData = Error.DicExtraData;

            /// <summary>
            /// List of exempted code places to do not raise error logs
            /// </summary>
            public static List<CodeScope> ExemptedCodeScopes = new List<CodeScope>();

            /// <summary>
            /// The just raise error code scope collection.
            /// Do not raise any exception in other code places.
            /// </summary>
            public static List<CodeScope> JustRaiseErrorCodeScopes = new List<CodeScope>();
        }

        #endregion
    }
}
