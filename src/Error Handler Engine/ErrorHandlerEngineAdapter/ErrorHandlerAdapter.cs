using System;
using System.Runtime.InteropServices;
using ExceptionManager;

namespace ErrorHandlerEngine
{
    [Serializable]
    [ComVisible(true)]
    [Guid("11BE9CF0-218D-45C6-A9AD-55C891F936F0")]
    public class ErrorHandlerAdapter : IErrorHandlerAdapter
    {

        public void Raise(Exception exp, bool isHandled = true, bool alertError = false, bool screenCapture = true)
        {
            var option = ErrorHandlerOption.None;
            if (isHandled) option |= ErrorHandlerOption.IsHandled;
            if(alertError) option|= ErrorHandlerOption.AlertUnHandledError;
            if (screenCapture) option |= ErrorHandlerOption.Snapshot;

            exp.RaiseLog(option);
        }
    }
}
