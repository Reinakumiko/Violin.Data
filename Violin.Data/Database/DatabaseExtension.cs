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
		/// <summary>
		/// 将一个 <see cref="DataTable"/> 转换为等效的T-SQL语句并执行。将会读取类与属性的 <see cref="Attributes.NameAttribute"/> 特性作为键，属性值作为值的形式。
		/// </summary>
		/// <typeparam name="TEntity">需要将 <see cref="DataTable"/> 中每一行映射到的类型</typeparam>
		/// <param name="table">需要处理的 <see cref="DataTable"/> 的数据表</param>
		/// <param name="sqlConn">结果执行到的 <see cref="SqlConnection"/> 目标</param>
		/// <returns>查询执行成功与否</returns>
		static public bool UpdateToDatabase<TEntity>(this DataTable table, SqlConnection sqlConn) where TEntity : class
		{
			var tableName = ReflectionExtension.GetClassName<TEntity>();
			return table.UpdateToDatabase<TEntity>(tableName, sqlConn);
		}

		/// <summary>
		/// 将一个 <see cref="DataTable"/> 转换为等效的T-SQL语句并执行。将会读取类与属性的 <see cref="Attributes.NameAttribute"/> 特性作为键，属性值作为值的形式。
		/// </summary>
		/// <typeparam name="TEntity">需要将 <see cref="DataTable"/> 中每一行映射到的类型</typeparam>
		/// <param name="table">需要处理的 <see cref="DataTable"/> 的数据表</param>
		/// <param name="tableName">结果执行的目标数据库表名</param>
		/// <param name="sqlConn">结果执行到的 <see cref="SqlConnection"/> 目标</param>
		/// <returns>查询执行成功与否</returns>
		static public bool UpdateToDatabase<TEntity>(this DataTable table, string tableName, SqlConnection sqlConn) where TEntity : class
		{
			return UpdateToDatabase(table.ToJToken().ToObject<List<TEntity>>(), tableName, sqlConn);
		}

		/// <summary>
		/// 将一个 <see cref="DataTable"/> 转换为等效的T-SQL语句并执行。将会读取类与属性的 <see cref="Attributes.NameAttribute"/> 特性作为键，属性值作为值的形式。
		/// </summary>
		/// <typeparam name="TEntity">需要将 <see cref="DataTable"/> 中每一行映射到的类型</typeparam>
		/// <param name="rows">需要处理的 <see cref="IEnumerable{T}"/> 的数据集合</param>
		/// <param name="sqlConn">结果执行到的 <see cref="SqlConnection"/> 目标</param>
		/// <returns>查询执行成功与否</returns>
		static public bool UpdateToDatabase<TEntity>(this IEnumerable<TEntity> rows, SqlConnection sqlConn) where TEntity : class
		{
			var tableName = ReflectionExtension.GetClassName<TEntity>();
			return rows.UpdateToDatabase(tableName, sqlConn);
		}

		/// <summary>
		/// 将一个 <see cref="DataTable"/> 转换为等效的T-SQL语句并执行。将会读取类与属性的 <see cref="Attributes.NameAttribute"/> 特性作为键，属性值作为值的形式。
		/// </summary>
		/// <typeparam name="TEntity">需要将 <see cref="DataTable"/> 中每一行映射到的类型</typeparam>
		/// <param name="rows">需要处理的 <see cref="IEnumerable{T}"/> 的数据集合</param>
		/// <param name="tableName">结果执行的目标数据库表名</param>
		/// <param name="sqlConn">结果执行到的 <see cref="SqlConnection"/> 目标</param>
		/// <returns>查询执行成功与否</returns>
		static public bool UpdateToDatabase<TEntity>(this IEnumerable<TEntity> rows, string tableName, SqlConnection sqlConn, SqlTransaction sqlTrans = null) where TEntity : class
		{
			//SQL 查询
			var queryBuilder = new StringBuilder();

			//遍历集合中的泛型对象
			foreach (var item in rows)
			{
				var keyValues = item.GetPropertyName();

				var newKeyValue = keyValues.Select(val =>
				{
					var value = val.Value?.Replace("'", "''").Replace("\n", "") ?? val.Value;

					return new KeyValuePair<string, string>(val.Key, value);
				}).ToDictionary(r => r.Key, r => r.Value);

				queryBuilder.AppendLine(string.Format("insert into [{0}] ([{1}]) values ('{2}');", tableName, string.Join("],[", newKeyValue.Keys), string.Join("','", newKeyValue.Values)));
			}

			return sqlConn.Excute(queryBuilder.ToString(), sqlTrans);
		}

		/// <summary>
		/// 将对象插入到对应的数据库表中
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <param name="sqlConn"></param>
		/// <returns></returns>
		static public bool InsertToDatabase<TEntity>(this TEntity entity, SqlConnection sqlConn) where TEntity : class
		{
			var tableName = entity.GetClassName();
			var keyValues = entity.GetPropertyName();

			keyValues.Where(value => value.Value.Contains(",")).Select(val =>
			{
				val.Value.Replace("'", "''").Replace("\n", "");
				return val;
			});

			var query = string.Format("insert into [{0}] ([{1}]) values ('{2}');", tableName, string.Join("],[", keyValues.Keys), string.Join("','", keyValues.Values));
			return sqlConn.Excute(query);
		}

		/// <summary>
		/// 执行数据库查询
		/// </summary>
		/// <param name="sqlConn"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		static private bool Excute(this SqlConnection sqlConn, string query, SqlTransaction transaction = null)
		{
			//打开数据库连接
			if (sqlConn.State == ConnectionState.Closed)
				sqlConn.Open();

			//
			var command = sqlConn.CreateCommand();
			command.Transaction = transaction;
			command.CommandText = query;
			command.CommandTimeout = 600;

			if (string.IsNullOrWhiteSpace(command.CommandText))
				return false;

			//
			return command.ExecuteNonQuery() > 0;
		}
	}
}
