using System;
using System.Runtime.InteropServices;
using ErrorHandlerEngine;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine
{
    [Serializable]
    [ComVisible(true)]
    [Guid("11BE9CF0-218D-45C6-A9AD-55C891F936F0")]
    public class ErrorHandlerAdapter : IErrorHandlerAdapter
    {

        public void Raise(Exception exp, bool isHandled = true, bool alertError = false, bool screenCapture = true)
        {
            var option = ErrorHandlingOption.None;
            if (isHandled) option |= ErrorHandlingOption.IsHandled;
            if(alertError) option|= ErrorHandlingOption.AlertUnHandledError;
            if (screenCapture) option |= ErrorHandlingOption.Snapshot;

            exp.RaiseLog(option);
        }
    }
}
