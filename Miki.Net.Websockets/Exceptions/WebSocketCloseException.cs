using System;
using System.Collections.Generic;
using System.Text;

namespace Miki.Net.WebSockets.Exceptions
{
    public class WebSocketCloseException : Exception
    {
        public int ErrorCode { get; }
        public object ErrorData { get; }

        public WebSocketCloseException(int errorCode, object errorData) : base()
        {
            ErrorCode = errorCode;
            ErrorData = errorData;
        }
        public WebSocketCloseException(int errorCode, object errorData, string message) : base(message)
        {
            ErrorCode = errorCode;
            ErrorData = errorData;
        }
        public WebSocketCloseException(int errorCode, object errorData, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            ErrorData = errorData;
        }
        public WebSocketCloseException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        public WebSocketCloseException(int errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }       
    }
}
