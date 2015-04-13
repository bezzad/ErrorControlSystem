
using System;
using ErrorControlSystem.Shared;
using System.Runtime.ConstrainedExecution;

namespace ErrorControlSystem
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class UnhandledErrorEventArgs : EventArgs
    {
        private readonly Error _exception;

        public UnhandledErrorEventArgs(Error exception)
        {
            _exception = exception;
        }

        public Error ErrorObject
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get { return _exception; }
        }
    }
}
