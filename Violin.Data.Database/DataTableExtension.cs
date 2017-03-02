namespace Violin.Data.Database
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text;
	using Json;

	public static class DataTableExtension
	{
		static public bool UpdateToDatabase<TEntity>(this DataTable table, string tableName, SqlConnection sqlConn) where TEntity : class
		{
			return UpdateToDatabase(table.ToJToken().ToObject<List<TEntity>>(), tableName, sqlConn);
		}

		static public bool UpdateToDatabase<TEntity>(this IEnumerable<TEntity> rows, string tableName, SqlConnection sqlConn) where TEntity : class
		{
			//获得泛型的类
			var type = typeof(TEntity);

			//获得泛型的字段
			var properties = type.GetProperties();

			//SQL 查询
			var queryBuilder = new StringBuilder();

			//遍历集合中的泛型对象
			foreach (var item in rows)
			{
				var keyValues = new Dictionary<string, dynamic>();

				//遍历
				foreach (var property in properties)
				{
					//获得字段的所有属性
					var attrList = property.GetCustomAttributes(false);

					foreach (var attr in attrList)
					{
						//该特性是否为 ColumnAttribute
						if (!(attr is ColumnAttribute))
							continue;

						//将属性转换为对象
						var attrColumn = attr as ColumnAttribute;

						//获取字段值
						dynamic value = property.GetValue(item, null);

						//将时间转换为标准格式
						if (value is DateTime)
							value = value.ToString("yyyy-MM-dd HH:mm:ss");

						//字符串特殊符号转义
						else if (value is string)
							value = value.Replace("'", "''").Replace("\n", "");

						//将特性的列名与字段值添加到键值对
						keyValues.Add(attrColumn.Name, value);
					}
				}

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
