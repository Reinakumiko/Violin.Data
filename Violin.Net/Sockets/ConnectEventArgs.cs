using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Net.Sockets
{
	public class ConnectEventArgs : EventArgs
	{
		public Socket Server { get; set; }
		public Socket Client { get; set; }

		public object Data { get; set; }
	}

	public class ConnectEventArgs<T> : ConnectEventArgs
	{
		public new T Data { get; set; }
	}
}
