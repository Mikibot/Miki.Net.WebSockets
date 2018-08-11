using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Miki.Net.WebSockets
{
    public struct WebSocketResponse
    {
		public string CloseReason;

		public int Count;

		public bool EndOfMessage;

		public WebSocketContentType MessageType;

		public bool? HasClosed;
    }
}
