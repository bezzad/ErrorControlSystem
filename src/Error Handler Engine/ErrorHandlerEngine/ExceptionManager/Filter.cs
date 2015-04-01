using System;
using System.Collections.Generic;
using ErrorHandlerEngine.Shared;

namespace ErrorHandlerEngine.ExceptionManager
{
    public static partial class ExceptionHandler
    {
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
            /// The just raise error from these code scope collection.
            /// Do not raise any exception in other code places.
            /// </summary>
            public static List<CodeScope> JustRaiseErrorCodeScopes = new List<CodeScope>();
        }
    }
}
