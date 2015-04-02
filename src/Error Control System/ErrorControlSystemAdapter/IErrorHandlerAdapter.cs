using System;
using System.Runtime.InteropServices;

namespace ErrorControlSystem
{
    [ComVisible(true)]
    [Guid("2A9A506F-FB51-406C-9D2F-026AD54E2B60")]
    public interface IErrorHandlerAdapter
    {
        void Raise(Exception exp, bool isHandled = true, bool alarmError = false, bool screenCapture = true);
    }
}