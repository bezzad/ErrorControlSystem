using System;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class LazyErrorEventArgs : EventArgs
    {
        public LazyError Error { get; private set; }

        public LazyErrorEventArgs(Error error)
        {
            Error = new LazyError(error);
        }
    }
}
