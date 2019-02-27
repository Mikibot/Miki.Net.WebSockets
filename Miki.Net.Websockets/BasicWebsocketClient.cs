using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Runtime.InteropServices;
using Miki.Logging;
using Miki.Net.WebSockets.Exceptions;

namespace Miki.Net.WebSockets
{
	public class BasicWebSocketClient : IWebSocketClient
	{
		ClientWebSocket _wsClient;

        public bool IsValid {
            get
            {
                if (_wsClient == null)
                {
                    return false;
                }
                return _wsClient.State == WebSocketState.Open;
            }
        }

		public async Task CloseAsync(CancellationToken token)
		{
            if (IsValid)
            {
                _wsClient.Abort();
                await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
            }
            _wsClient = null;
		}

		public async Task ConnectAsync(Uri connectionUri, CancellationToken token)
		{
            _wsClient = new ClientWebSocket();
			await _wsClient.ConnectAsync(connectionUri, token);        
		}

		public async Task SendAsync(WebSocketContent data, CancellationToken token)
		{
            if(!IsValid)
            {
                return;
            }

			if (!MemoryMarshal.TryGetArray<byte>(data, out var segment))
			{
				throw new ArgumentException("could not get ArraySegment from Memory");
			}

			await _wsClient.SendAsync(segment, (WebSocketMessageType)data.ContentType, true, token);
		}

		public async Task<WebSocketResponse> ReceiveAsync(ArraySegment<byte> data, CancellationToken token)
		{
            if(!IsValid)
            {
                return new WebSocketResponse
                {
                    CloseReason = null,
                    Count = 0,
                    EndOfMessage = true,
                    HasClosed = true,
                    MessageType = WebSocketContentType.Close
                };
            }

			var response = await _wsClient.ReceiveAsync(data, token);

			if (_wsClient.State == WebSocketState.Closed || _wsClient.CloseStatus.HasValue)
			{
                throw new WebSocketCloseException((int)_wsClient.CloseStatus.Value, _wsClient.CloseStatusDescription);
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
