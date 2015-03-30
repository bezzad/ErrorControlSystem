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

        internal static volatile bool IsSelfException = false;

        #endregion


        static ExceptionHandler()
        {
            Filter.ExemptedUnhandledExceptionCodePlaces.Add(new CodePlace(Assembly.GetExecutingAssembly().GetName().Name, null, null));

            EmbeddedAssembly.Load("System.Data.SqlServerCe.dll");
            EmbeddedAssembly.Load("System.Threading.Tasks.Dataflow.dll");
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => EmbeddedAssembly.Get(e.Name);

            StorageRouter.ReadyCache();
        }

        #region Methods

        /// <summary>
        /// Raise log of handled error's.
        /// </summary>
        /// <param name="exp">The Error object.</param>
        /// <param name="option">The option to select what jobs must be doing and stored in Error object's.</param>
        /// <param name="errorTitle">Determine the mode of occurrence of an error in the program.</param>
        /// <returns></returns>
        public static Error RaiseLog(this Exception exp, ErrorHandlingOptions option = ErrorHandlingOptions.Default, String errorTitle = "UnHandled Exception")
        {
            //
            // Self exceptions just run in Handled Mode!
            if (IsSelfException && option.HasFlag(ErrorHandlingOptions.IsHandled))
            {
                IsSelfException = false;
                return null;
            }
            //
            // Filter exception
            if (Filter.IsExemptedException(exp, ref option))
                return null;
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
            /// List of exempted unhandled errors code places to do not logs
            /// </summary>
            public static List<CodePlace> ExemptedUnhandledExceptionCodePlaces = new List<CodePlace>();


            internal static bool IsExemptedException(Exception exp, ref ErrorHandlingOptions opt)
            {
                //
                // Find exception type:
                var exceptionType = exp.GetType();
                //
                // Is exception in non snapshot list? (yes => remove snapshot option)
                if (NonSnapshotExceptionTypes.Any(x => x == exceptionType))
                    opt &= ~ErrorHandlingOptions.Snapshot;
                //
                // Is exception in except list?
                return ExemptedExceptionTypes.Any(x => x == exceptionType) ||
                    ExemptedUnhandledExceptionCodePlaces.Any(x => x.IsFromThisPlace(exp));
            }
        }
    }

    public class CodePlace
    {
        public string AssemblyName = String.Empty;
        public string ClassName = String.Empty;
        public string MethodName = String.Empty;

        public CodePlace(string assemblyName, string className, string methodName)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            MethodName = methodName;
        }

        public bool IsFromThisPlace(Exception exp)
        {
            var st = new System.Diagnostics.StackTrace(exp);

            if (st.GetFrames() == null || !st.GetFrames().Any()) return false;

            IEnumerable<StackFrame> lstFiltering = null;
            //
            // Filter by Assembly Names
            if (!string.IsNullOrEmpty(AssemblyName))
            {
                lstFiltering = st.GetFrames().Where(
                    x => RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase));
            }
            //
            // Filter by Class Names
            if (!string.IsNullOrEmpty(ClassName))
            {
                if (lstFiltering != null) // Before Filtered by Assembly Name
                {
                    lstFiltering = lstFiltering.Where(
                        x =>
                        {
                            var declaringType = x.GetMethod().DeclaringType;
                            return declaringType != null && declaringType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase);
                        });
                }
                else // Not Assembly Name
                {
                    lstFiltering = st.GetFrames().Where(
                        x =>
                        {
                            var declaringType = x.GetMethod().DeclaringType;
                            return declaringType != null && declaringType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase);
                        });
                }
            }
            //
            // Filter by Method Names
            if (!string.IsNullOrEmpty(MethodName))
            {
                if (lstFiltering != null) // Before Filtered by Assembly Name or Class Name
                {
                    lstFiltering = lstFiltering.Where(
                        x => x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
                }
                else // Not any filter before
                {
                    lstFiltering = st.GetFrames().Where(
                        x => x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
                }
            }


            return lstFiltering != null && lstFiltering.Any();
        }

        private string RemoveExtension(string value)
        {
            int dotIndex = value.IndexOf('.');
            return value.Substring(0, dotIndex);
        }
    }
}
