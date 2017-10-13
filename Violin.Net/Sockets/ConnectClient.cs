using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Net.Sockets
{
	public class ConnectClient
	{
		public Socket Client { get; set; }
		public bool IsWebSocket { get; set; }

		public ConnectClient(Socket client)
		{
			Client = client;
		}
	}
}
