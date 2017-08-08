using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Data.Database.Base
{
	/// <summary>
	/// 表示一个数据库连接的基本信息
	/// </summary>
	public class DbConnectionInfo
	{
		/// <summary>
		/// 服务器的链接地址
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// 服务器的连接端口
		/// </summary>
		public uint? Port { get; set; }

		/// <summary>
		/// 用于连接服务器的用户
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// 用于连接数据库的用户密码
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 需要连接的数据库
		/// </summary>
		public string Database { get; set; }

		public override string ToString()
		{
			StringBuilder connectionBuilder = new StringBuilder();
			connectionBuilder.AppendFormat("server={0};", Server);
			connectionBuilder.AppendFormat("user id={0};", User);
			connectionBuilder.AppendFormat("password={0};", Password);
			connectionBuilder.AppendFormat("initial catalog={0};", Database);

			if (Port.HasValue)
				connectionBuilder.AppendFormat("port={0};", Port);

			return connectionBuilder.ToString();
		}
	}
}
