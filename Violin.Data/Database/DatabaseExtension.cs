namespace Violin.Data.Database
{
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Text;
	using Json;
	using Reflection;

	public static class DatabaseExtension
	{
		static public bool UpdateToDatabase<TEntity>(this DataTable table, SqlConnection sqlConn) where TEntity : class
		{
			var tableName = ReflectionExtension.GetClassName<TEntity>();
			return table.UpdateToDatabase<TEntity>(tableName, sqlConn);
		}

		static public bool UpdateToDatabase<TEntity>(this DataTable table, string tableName, SqlConnection sqlConn) where TEntity : class
		{
			return UpdateToDatabase(table.ToJToken().ToObject<List<TEntity>>(), tableName, sqlConn);
		}

		static public bool UpdateToDatabase<TEntity>(this IEnumerable<TEntity> rows, SqlConnection sqlConn) where TEntity : class
		{
			var tableName = ReflectionExtension.GetClassName<TEntity>();
			return rows.UpdateToDatabase(tableName, sqlConn);
		}

		static public bool UpdateToDatabase<TEntity>(this IEnumerable<TEntity> rows, string tableName, SqlConnection sqlConn) where TEntity : class
		{
			//SQL 查询
			var queryBuilder = new StringBuilder();

			//遍历集合中的泛型对象
			foreach (var item in rows)
			{
				var keyValues = item.GetPropertyName();

				keyValues.Where(value => value.Value.Contains(",")).Select(val =>
				{
					val.Value.Replace("'", "''").Replace("\n", "");
					return val;
				});

				queryBuilder.AppendLine(string.Format("insert into [{0}] ([{1}]) values ('{2}');", tableName, string.Join("],[", keyValues.Keys), string.Join("','", keyValues.Values)));
			}

			//打开数据库连接
			if (sqlConn.State == ConnectionState.Closed)
				sqlConn.Open();

			//
			var command = sqlConn.CreateCommand();
			command.CommandText = queryBuilder.ToString();
			command.CommandTimeout = 600;

			if (string.IsNullOrWhiteSpace(command.CommandText))
				return false;

			//
			return command.ExecuteNonQuery() > 0;
		}
	}
}
