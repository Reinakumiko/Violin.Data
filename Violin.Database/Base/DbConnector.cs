using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Data.Database.Base
{
	public static class DbConnector
	{
		/// <summary>
		/// 使用一个 <see cref="DbConnectString"/> 创建数据库连接。 
		/// </summary>
		/// <typeparam name="T">数据库连接</typeparam>
		/// <param name="conn">继承自 <see cref="DbConnection"/> 的数据库连接</param>
		/// <param name="connString"><see cref="DbConnectString"/> 连接配置实例</param>
		/// <returns>目标数据库连接</returns>
		public static T GenerateConnect<T>(DbConnectString connString) where T : DbConnection
		{
			return GenerateConnect<T>(connString.ToString());
		}

		/// <summary>
		/// 使用一个 <see cref="DbConnectString"/> 创建数据库连接。 
		/// </summary>
		/// <typeparam name="T">数据库连接</typeparam>
		/// <param name="conn">继承自 <see cref="DbConnection"/> 的数据库连接</param>
		/// <param name="connString">连接字符串</param>
		/// <returns>目标数据库连接</returns>
		public static T GenerateConnect<T>(string connString) where T : DbConnection
		{
			var dbConnection = Activator.CreateInstance<T>();
			dbConnection.ConnectionString = connString;

			return dbConnection;
		}
	}
}
