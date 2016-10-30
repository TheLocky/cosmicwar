using System;

namespace CWServer.Exceptions {
    public class CWException : Exception {
        public CWException(string message) : base(message) {}
    }
}
