using System;
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
    public enum ExceptionHandlerOption
    {
        None = 0,

        IsHandled = 1,
        AlertUnHandledError = 2,
        Snapshot = 4,
        FetchServerDateTime = 8,
        ReSizeSnapshots = 16,
        SendCacheToServer = 32,

        Default = Snapshot | FetchServerDateTime | AlertUnHandledError | SendCacheToServer,
        All = 0xFFFF // Combined value of all
    }

}
