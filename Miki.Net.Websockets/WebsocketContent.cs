using System;
using System.Collections.Generic;
using System.Text;

namespace Miki.Net.WebSockets
{
    public struct WebSocketContent
    {
		public Memory<byte> Data;
		public WebSocketContentType ContentType;

		public WebSocketContent(Memory<char> data)
			: this(Encoding.UTF8.GetBytes(data.ToArray()), WebSocketContentType.Text)
		{
		}
		public WebSocketContent(Memory<byte> data, WebSocketContentType type = WebSocketContentType.Binary)
		{
			Data = data;
			ContentType = type;
		}

		public static implicit operator Memory<byte>(WebSocketContent data)
		{
			return data.Data;
		}
		public static implicit operator ReadOnlyMemory<byte>(WebSocketContent data)
		{
			return data.Data;
		}

		public static implicit operator WebSocketContent(Memory<char> data)
		{
			return new WebSocketContent(data);
		}
		public static implicit operator WebSocketContent(Memory<byte> data)
		{
			return new WebSocketContent(data, WebSocketContentType.Binary);
		}
	}
}
