using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Runtime.InteropServices;
using Miki.Logging;

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
			if (_wsClient.CloseStatus.HasValue)
			{
				Console.WriteLine(_wsClient.CloseStatusDescription);
			}

			await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
		}

		public async Task ConnectAsync(Uri connectionUri, CancellationToken token)
		{
			if (_wsClient.CloseStatus.HasValue)
			{
				Console.WriteLine(_wsClient.CloseStatusDescription);
			}

			await _wsClient.ConnectAsync(connectionUri, token);
		}

		public async Task SendAsync(WebSocketContent data, CancellationToken token)
		{
			if (_wsClient.CloseStatus.HasValue)
			{
				Console.WriteLine(_wsClient.CloseStatusDescription);
			}

			if (!MemoryMarshal.TryGetArray<byte>(data, out var segment))
			{
				throw new ArgumentException("could not get ArraySegment from Memory");
			}

			await _wsClient.SendAsync(segment, (WebSocketMessageType)data.ContentType, true, token);
		}

		public async Task<WebSocketResponse> ReceiveAsync(ArraySegment<byte> data, CancellationToken token)
		{
			var response = await _wsClient.ReceiveAsync(data, token);

			if (_wsClient.State == WebSocketState.Closed || _wsClient.CloseStatus.HasValue)
			{
				Log.Warning($"Websocket closed with message: {_wsClient.CloseStatus.ToString()}: {_wsClient.CloseStatusDescription}.");

				await CloseAsync(token);
			}

			return new WebSocketResponse
			{
				CloseReason = response.CloseStatusDescription,
				Count = response.Count,
				EndOfMessage = response.EndOfMessage,
				HasClosed = response.CloseStatus != null,
				MessageType = (WebSocketContentType)response.MessageType
			};
		}
	}
}
