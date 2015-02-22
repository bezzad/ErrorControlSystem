using System;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class ProxyErrorEventArgs : EventArgs
    {
        public ProxyError Error { get; private set; }

        public ProxyErrorEventArgs(Error error)
        {
            Error = new ProxyError(error);
        }
    }
}
