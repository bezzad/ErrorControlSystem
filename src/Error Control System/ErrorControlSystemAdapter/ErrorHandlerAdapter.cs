using System;
using System.Runtime.InteropServices;

namespace ErrorControlSystem
{
    [Serializable]
    [ComVisible(true)]
    [Guid("11BE9CF0-218D-45C6-A9AD-55C891F936F0")]
    public class ErrorHandlerAdapter : IErrorHandlerAdapter
    {

        public void Raise(Exception exp, bool isHandled = true, bool alertError = false, bool screenCapture = true)
        {
            exp.RaiseLog(isHandled);
        }
    }
}
