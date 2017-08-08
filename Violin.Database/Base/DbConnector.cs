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
		/// 使用一个 <see cref="DbConnectionInfo"/> 创建数据库连接。 
		/// </summary>
		/// <typeparam name="T">数据库连接</typeparam>
		/// <param name="connString"><see cref="DbConnectionInfo"/> 连接配置实例</param>
		/// <returns>目标数据库连接</returns>
		public static T GenerateConnect<T>(DbConnectionInfo connInfo) where T : DbConnection
		{
			return GenerateConnect<T>(connInfo.ToString());
		}

		/// <summary>
		/// 使用连接字符串创建数据库连接。 
		/// </summary>
		/// <typeparam name="T">数据库连接</typeparam>
		/// <param name="connString">连接字符串</param>
		/// <returns>目标数据库连接</returns>
		public static T GenerateConnect<T>(string connString) where T : DbConnection
		{
			var dbConnection = Activator.CreateInstance<T>();
			dbConnection.ConnectionString = connString;
			dbConnection.Open();

			return dbConnection;
		}
	}
}
