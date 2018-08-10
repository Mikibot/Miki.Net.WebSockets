using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Miki.Net.Websockets
{
    public interface IWebsocketClient
    {
		Action<string> OnMessageReceived { get; set; }

		Task ConnectAsync();

		Task CloseAsync();

		Task SendAsync();
	}
}
