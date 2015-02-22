using System;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class LazyErrorEventArgs : EventArgs
    {
        public ProxyError Error { get; private set; }

        public LazyErrorEventArgs(Error error)
        {
            Error = new ProxyError(error);
        }
    }
}