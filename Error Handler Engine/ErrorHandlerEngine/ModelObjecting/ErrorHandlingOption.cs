﻿using System;
using System.Runtime.InteropServices;

namespace ErrorHandlerEngine.ModelObjecting
{

    /// <summary>
    /// Specifies the application elements on which it is valid to apply an attribute.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    [ComVisible(true)]
    [Flags]
    [Serializable]
    public enum ErrorHandlingOption
    {
        None = 0,

        IsHandled = 1,
        AlertUnHandledError = 2,
        Snapshot = 4,
        FetchServerDateTime = 8,
        ReSizeSnapshots = 16,

        Default = Snapshot | FetchServerDateTime | AlertUnHandledError,
        All = 0xFFF // Combined value of all
    }

}
