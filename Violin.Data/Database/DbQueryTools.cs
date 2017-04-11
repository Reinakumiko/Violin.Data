using System;
using System.Data.Common;

namespace Violin.Data.Database
{
	using System.Data;

	public class DbQueryTools<TConnection, TDataAdapter>
			where TConnection : DbConnection
			where TDataAdapter : DbDataAdapter
	{
		public DataTable GetQueryResult(TConnection sqlConn, string queryString)
		{
			var table = new DataTable();

			var adapter = Activator.CreateInstance(typeof(TDataAdapter), queryString, sqlConn) as TDataAdapter;
			adapter.Fill(table);

			return table;
		}

		public DataTable ExcuteQuery(TConnection sqlConn, string queryString)
		{
			return GetQueryResult(sqlConn, queryString);
		}

		public int ExcuteNonQuery(TConnection sqlConn, string queryString)
		{
			using (var command = sqlConn.CreateCommand())
			{
				command.CommandText = queryString;
				return command.ExecuteNonQuery();
			}
		}

		public DataTable GetTableWithoutCondition(TConnection sqlConn, string tableName)
		{
			return GetTable(sqlConn, tableName, "1=1");
		}

		public DataTable GetTableWithoutCondition(TConnection sqlConn, string tableName, params string[] colNames)
		{
			return GetTable(sqlConn, tableName, "1=1", colNames);
		}

		public DataTable GetTable(TConnection sqlConn, string tableName, string condition)
		{
			return GetQueryResult(sqlConn, string.Format("select * from {0} where {1}", tableName, condition));
		}

		public DataTable GetTable(TConnection sqlConn, string tableName, string condition, params string[] colNames)
		{
			var columns = string.Join("],[", colNames);

			return GetQueryResult(sqlConn, string.Format("select [{0}] from {1} where {2}", columns, tableName, condition));
		}

		public void TruncateTable(TConnection sqlConn, string tableName)
		{
			if (sqlConn.State == ConnectionState.Closed)
				sqlConn.Open();

			var command = sqlConn.CreateCommand();
			command.CommandText = string.Format("truncate table {0}", tableName);
			command.ExecuteNonQuery();
		}
	}
}
