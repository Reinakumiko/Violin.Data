namespace Violin.Data.Reflection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Violin.Data.Database.Attributes;

	public static class ReflectionExtension
	{
		/// <summary>
		/// 获取类的列表名与属性值的键值对
		/// </summary>
		/// <typeparam name="TEntity">需要查询的类型</typeparam>
		/// <param name="entity">需要查询的实例</param>
		/// <returns>类中标记的键与属性值</returns>
		static public Dictionary<string, string> GetPropertyName<TEntity>(this TEntity entity)
		{
			//获得泛型的类
			var type = typeof(TEntity);

			//获得泛型的字段
			var properties = type.GetProperties();

			//对应名与值对应字典
			var keyValues = new Dictionary<string, string>();

			//遍历
			foreach (var property in properties)
			{
				//将属性转换为对象
				var attrColumn = Attribute.GetCustomAttribute(property, typeof(NameAttribute)) as NameAttribute;

				//获取字段值
				dynamic value = property.GetValue(entity, null);

				//将时间转换为标准格式
				if (value is DateTime)
					value = value.ToString("yyyy-MM-dd HH:mm:ss");

				//将特性的列名与字段值添加到键值对
				keyValues.Add(attrColumn.Name, value);
			}

			return keyValues;
		}

		/// <summary>
		/// 获取类对应的表名
		/// </summary>
		/// <typeparam name="TEntity">需要查询的类</typeparam>
		/// <param name="entity">需要查询的实例</param>
		/// <returns>该类对应的表名</returns>
		static public string GetClassName<TEntity>(this TEntity entity)
		{
			return entity.GetClassName();
		}

		/// <summary>
		/// 获取类对应的表名
		/// </summary>
		/// <typeparam name="TEntity">需要查询的类</typeparam>
		/// <returns>该类对应的表名</returns>
		static public string GetClassName<TEntity>()
		{
			var attr = Attribute.GetCustomAttribute(typeof(TEntity), typeof(NameAttribute)) as NameAttribute;

			return attr.Name;
		}
	}
}
