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
		/// 读取属性的 <see cref="NameAttribute"/> 特性的 <see cref="NameAttribute.Name"/> 作为键，属性值作为值后生成一个 <see cref="Dictionary{TKey, TValue}"/> 实例。
		/// </summary>
		/// <typeparam name="TEntity">需要映射的类型</typeparam>
		/// <param name="entity">需要映射的类型实例</param>
		/// <returns>包含了属性键与值的 <see cref="Dictionary{TKey, TValue}"/> 实例 </returns>
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

				//跳过未设置特性的属性
				if (attrColumn == null)
					continue;

				//获取字段值
				dynamic value = property.GetValue(entity, null);

				//将数据转换为指定类型
				if (attrColumn.Type != null)
					value = Convert.ChangeType(value, attrColumn.Type);

				//将数据以指定格式序列化
				if (!string.IsNullOrWhiteSpace(attrColumn.Format))
				{
					var formatString = string.Format("{{0:{0}}}", attrColumn.Format);

					value = string.Format(formatString, value);
				}
				
				//将时间转换为标准格式
				if (value is DateTime && string.IsNullOrWhiteSpace(attrColumn.Format))
					value = value.ToString("yyyy-MM-dd HH:mm:ss");

				//将特性的列名与字段值添加到键值对
				keyValues.Add(attrColumn.Name, Convert.ToString(value));
			}

			return keyValues;
		}

		/// <summary>
		/// 读取类的 <see cref="NameAttribute"/> 特性获取表名并将其返回。
		/// </summary>
		/// <typeparam name="TEntity">需要查询的类</typeparam>
		/// <param name="entity">需要查询的实例</param>
		/// <returns>该类对应的表名</returns>
		static public string GetClassName<TEntity>(this TEntity entity)
		{
			return GetClassName<TEntity>();
		}

		/// <summary>
		/// 读取类的 <see cref="NameAttribute"/> 特性获取表名并将其返回。
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
