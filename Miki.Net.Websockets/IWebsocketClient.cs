using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miki.Net.WebSockets
{
    public interface IWebSocketClient
	{
        bool IsValid { get; }

		Task ConnectAsync(Uri connectionUri, CancellationToken token);

		Task CloseAsync(CancellationToken token);

		Task SendAsync(WebSocketContent data, CancellationToken token);

		Task<WebSocketResponse> ReceiveAsync(ArraySegment<byte> data, CancellationToken token);
	}
}