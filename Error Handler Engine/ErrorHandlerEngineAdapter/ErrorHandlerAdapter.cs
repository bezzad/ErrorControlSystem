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
            var option = ExceptionHandlerOption.None;
            if (isHandled) option |= ExceptionHandlerOption.IsHandled;
            if(alertError) option|= ExceptionHandlerOption.AlertUnHandledError;
            if (screenCapture) option |= ExceptionHandlerOption.Snapshot;

            exp.RaiseLog(option);
        }
    }
}
