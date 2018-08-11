using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Runtime.InteropServices;

namespace Miki.Net.WebSockets
{
	public class BasicWebSocketClient : IWebSocketClient
	{
		ClientWebSocket _wsClient;

		public BasicWebSocketClient()
		{
			_wsClient = new ClientWebSocket();
		}

		public async Task CloseAsync(CancellationToken token)
		{
			await _wsClient.CloseAsync(WebSocketCloseStatus.Empty, "", token);
		}

		public async Task ConnectAsync(Uri connectionUri, CancellationToken token)
		{
			await _wsClient.ConnectAsync(connectionUri, token);
		}

		public async Task SendAsync(WebSocketContent data, CancellationToken token)
		{
			if (!MemoryMarshal.TryGetArray<byte>(data, out var segment))
			{
				throw new ArgumentException("could not get ArraySegment from Memory");
			}

			await _wsClient.SendAsync(segment, (WebSocketMessageType)data.ContentType, false, token);
		}

		public async Task<WebSocketResponse> ReceiveAsync(Memory<byte> data, CancellationToken token)
		{
			if (!MemoryMarshal.TryGetArray<byte>(data, out var segment))
			{
				throw new ArgumentException("could not get ArraySegment from Memory");
			}

			var response = await _wsClient.ReceiveAsync(segment, token);

			data = segment;

			return new WebSocketResponse
			{
				CloseReason = response.CloseStatusDescription,
				EndOfMessage = response.EndOfMessage,
				HasClosed = response.CloseStatus != null,
				MessageType = (WebSocketContentType)response.MessageType
			};
		}
	}
}
