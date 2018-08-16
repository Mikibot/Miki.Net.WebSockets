using System;
using System.Collections.Generic;
using System.Text;

namespace Miki.Net.WebSockets
{
    public struct WebSocketContent
    {
		public ReadOnlyMemory<byte> Data;
		public WebSocketContentType ContentType;

		public WebSocketContent(ReadOnlyMemory<char> data)
			: this(Encoding.UTF8.GetBytes(data.ToArray()), WebSocketContentType.Text)
		{
		}
		public WebSocketContent(ReadOnlyMemory<byte> data, WebSocketContentType type = WebSocketContentType.Binary)
		{
			Data = data;
			ContentType = type;
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
		public static implicit operator WebSocketContent(string data)
		{
			return new WebSocketContent(data.AsMemory());
		}
	}
}
