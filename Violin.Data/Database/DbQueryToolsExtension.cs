using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Violin.Data.Database
{
	using System.Data;
	using System.Data.Common;

	public static class DbQueryToolsExtension
	{
		static public DataTable GetQueryResult<TDataAdapter>(this DbConnection sqlConn, string queryString) where TDataAdapter : DbDataAdapter
		{
			var table = new DataTable();

			var adapter = Activator.CreateInstance(typeof(TDataAdapter), queryString, sqlConn) as TDataAdapter;
			adapter.Fill(table);

			return table;
		}

		static public DataTable GetTableWithoutCondition<TDataAdapter>(this DbConnection sqlConn, string tableName) where TDataAdapter : DbDataAdapter
		{
			return GetTable<TDataAdapter>(sqlConn, tableName, "1=1");
		}

		static public DataTable GetTableWithoutCondition<TDataAdapter>(this DbConnection sqlConn, string tableName, params string[] colNames) where TDataAdapter : DbDataAdapter
		{
			return GetTable<TDataAdapter>(sqlConn, tableName, "1=1", colNames);
		}

		static public DataTable GetTable<TDataAdapter>(this DbConnection sqlConn, string tableName, string condition) where TDataAdapter : DbDataAdapter
		{
			return GetTable<TDataAdapter>(sqlConn, tableName, condition, "*");
		}

		static public DataTable GetTable<TDataAdapter>(this DbConnection sqlConn, string tableName, string condition, params string[] colNames) where TDataAdapter : DbDataAdapter
		{
			var columns = string.Join("],[", colNames);

			return sqlConn.GetQueryResult<TDataAdapter>(string.Format("select [{0}] from {1} where {2}", columns, tableName, condition));
		}

		/// <summary>
		/// 将表内数据清空
		/// </summary>
		/// <param name="sqlConn">表所在的数据库连接</param>
		/// <param name="tableName">要清空的数据库表名</param>
		static public void TruncateTable(this DbConnection sqlConn, string tableName)
		{
			if (sqlConn.State == ConnectionState.Closed)
				sqlConn.Open();

			var command = sqlConn.CreateCommand();
			command.CommandText = string.Format("truncate table {0}", tableName);
			command.ExecuteNonQuery();
		}
	}
}
