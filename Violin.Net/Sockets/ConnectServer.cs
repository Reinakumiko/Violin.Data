using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Violin.Net.Sockets
{

	public class ConnectServer : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventArg"></param>
		public delegate void OnSocketConnect(ConnectEventArgs eventArg);

		public delegate void OnSocketDisconnect();

		public delegate void OnSocketReceive(ConnectEventArgs<string> eventArg);

		public delegate void OnSocketError(ConnectEventArgs eventArg);

		/// <summary>
		/// 当客户端连接时触发的事件句柄
		/// </summary>
		public event OnSocketConnect OnConnectEventHandle;

		/// <summary>
		/// 当客户端断开连接时触发的事件句柄
		/// </summary>
		public event OnSocketDisconnect OnDisconnectEventHandle;

		/// <summary>
		/// 当客户端接收消息时触发的事件句柄
		/// </summary>
		public event OnSocketReceive OnReceiveEventHandle;

		/// <summary>
		/// 链接地址
		/// </summary>
		private IPEndPoint Address { get; set; }

		/// <summary>
		/// 连接器
		/// </summary>
		private Socket Server { get; set; }

		/// <summary>
		/// 接入连接异步等待结果
		/// </summary>
		public IAsyncResult AccpectResult { get; set; }

		/// <summary>
		/// 初始化一个 <see cref="ConnectServer"/> 实例
		/// </summary>
		/// <param name="port">监听的端口号</param>
		public ConnectServer(int port) : this(IPAddress.Any, port) { }

		/// <summary>
		/// 初始化一个 <see cref="ConnectServer"/> 实例
		/// </summary>
		/// <param name="server">服务器地址</param>
		/// <param name="port">监听端口</param>
		public ConnectServer(string server, int port) : this(IPAddress.Parse(server), port) { }

		/// <summary>
		/// 初始化一个 <see cref="ConnectServer"/> 实例
		/// </summary>
		/// <param name="address">服务器 <see cref="IPAddress"/> 地址的实例</param>
		/// <param name="port">监听端口</param>
		public ConnectServer(IPAddress address, int port) : this(new IPEndPoint(address, port)) { }

		/// <summary>
		/// 初始化一个 <see cref="ConnectServer"/> 实例
		/// </summary>
		/// <param name="endPoint">监听的 <see cref="IPEndPoint"/> 终端地址</param>
		public ConnectServer(IPEndPoint endPoint)
		{
			Address = endPoint;

			Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
		}

		/// <summary>
		/// 绑定监听接入请求
		/// </summary>
		private void BindAcceptSocket()
		{
			AccpectResult = Server.BeginAccept(OnSocketConnectAction, Server);
		}

		private List<ArraySegment<byte>> receivedMessage = new List<ArraySegment<byte>>();
		private void BindReceiveSocket()
		{
			receivedMessage.Clear();
			Server.BeginReceive(receivedMessage, SocketFlags.None, OnSocketReceiveAction, Server);
		}

		/// <summary>
		/// 当服务器接收到连接请求时执行的异步操作
		/// </summary>
		/// <param name="ar"></param>
		private void OnSocketConnectAction(IAsyncResult ar)
		{
			var clientSocket = Server.EndAccept(ar);

			OnConnectEventHandle?.Invoke(new ConnectEventArgs()
			{
				Server = ar.AsyncState as Socket,
				Client = clientSocket
			});
		}

		/// <summary>
		/// 当前服务器接收到的消息内容
		/// </summary>
		/// <param name="ar"></param>
		private void OnSocketReceiveAction(IAsyncResult ar)
		{
			var length = Server.EndReceive(ar);
			var message = Encoding.UTF8.GetString(receivedMessage.SelectMany(i => i).ToArray());

			OnReceiveEventHandle?.Invoke(new ConnectEventArgs<string>()
			{
				Server = Server,
				Client = ar.AsyncState as Socket,
				Data = message
			});
		}

		/// <summary>
		/// 开启服务器监听
		/// </summary>
		public void Start()
		{
			Server.Bind(Address);
			Server.Listen(10);

			BindAcceptSocket();
		}

		/// <summary>
		/// 关闭服务器监听
		/// </summary>
		public void Stop()
		{
			Server.EndAccept(AccpectResult);
			Server.Shutdown(SocketShutdown.Both);
			Server.Close();
		}

		/// <summary>
		/// 释放实例所占用的资源
		/// </summary>
		public void Dispose()
		{

		}
	}
}
