using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ErrorControlSystem.Shared;

namespace ErrorControlSystem
{
    public static partial class ExceptionHandler
    {
        public static class Filter
        {
            #region Properties
            
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
            /// The just raise error from these code scope collection.
            /// Do not raise any exception in other code places.
            /// </summary>
            public static List<CodeScope> JustRaiseErrorCodeScopes = new List<CodeScope>();
            
            #endregion

            /// <summary>
            /// Determines whether the specified exp is filtering.
            /// </summary>
            /// <param name="exp">The exp.</param>
            /// <param name="callStackFrames"><see cref="StackTrace"/> array</param>
            /// <param name="snapshot">has snapshot or not?</param>
            /// <returns></returns>
            public static bool IsFiltering(Exception exp, StackFrame[] callStackFrames, out bool snapshot)
            {
                snapshot = ErrorHandlingOption.Snapshot;
                
                if (ErrorHandlingOption.FilterExceptions)
                {
                    //
                    // Find exception type:
                    var expType = exp.GetType();
                    //
                    // Is exception within non-snapshot list? (yes => remove snapshot option)
                    if (NonSnapshotExceptionTypes.Any(x => x == expType))
                        snapshot = false;
                    //
                    // Is exception in exempted list?
                    if (ExemptedExceptionTypes.Any(x => x == expType) ||
                        ExemptedCodeScopes.Any(x => x.IsCallFromThisPlace(callStackFrames)))
                        return true;
                    //
                    // Must be exception occurred from these code scopes to that raised by handler.
                    if (JustRaiseErrorCodeScopes.Count > 0 &&
                        !JustRaiseErrorCodeScopes.Any(x => x.IsCallFromThisPlace(callStackFrames)))
                        return true;
                }

                if (ErrorHandlingOption.CacheCodeScope.IsCallFromThisPlace(callStackFrames))
                    return true;

                return false;
            }
        }
    }
}
