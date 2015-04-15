namespace ErrorControlSystem
{
    using System;
    using System.Runtime.InteropServices;


    /// <summary>
    /// Specifies the application elements on which it is valid to apply an attribute.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    [ComVisible(true)]
    [Flags]
    [Serializable]
    public enum ErrorHandlingOptions
    {
        None = 0,
        All = 0xFFFF, // Combined value of all

        DisplayUnhandledExceptions = 1,
        ReportHandledExceptions = 2,
        Snapshot = 4,
        FetchServerDateTime = 8,
        ResizeSnapshots = 16,
        EnableNetworkSending = 32,
        FilterExceptions = 64,
        ExitApplicationImmediately = 128,
        HandleProcessCorruptedStateExceptions = 256,
        DisplayDeveloperUI = 512,


        Default = All & ~ExitApplicationImmediately & ~HandleProcessCorruptedStateExceptions
    }
}
