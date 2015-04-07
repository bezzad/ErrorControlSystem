
using System;
using System.Runtime.InteropServices;
using ErrorControlSystem.Shared;
using System.Runtime.ConstrainedExecution;

namespace ErrorControlSystem
{

    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class UnhandledErrorEventArgs : EventArgs
    {
        private Error _Exception;

        public UnhandledErrorEventArgs(Error exception)
        {
            _Exception = exception;
        }

        public Error ErrorObject
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get { return _Exception; }
        }
    }

    /// <summary>
    /// Represents the method that will handle the event raised by an exception that is not handled by the application domain.
    /// </summary>
    /// <param name="sender">The source of the unhandled exception event. </param><param name="e">An <paramref name="ErrorControlSystem.UnhandledErrorEventArgs"/> that contains the event data. </param><filterpriority>2</filterpriority>
    [ComVisible(true)]
    [Serializable]
    public delegate void UnhandledErrorEventHandler(object sender, ErrorControlSystem.UnhandledErrorEventArgs e);
}
