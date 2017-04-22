using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Violin.Data.Enumeration
{
	public static class EnumerationExtension
	{
		/// <summary>
		/// 获取枚举成员的属性 (Attribute) 的属性 (Property) 值
		/// </summary>
		/// <param name="enumName">枚举成员</param>
		/// <returns></returns>
		public static string GetAttributeValue<T>(this Enum enumName, Func<T, string> func) where T : Attribute
		{
			return enumName.GetAttributeValue<T, string>(func);
		}


		public static object GetAttributeValue<T>(this Enum enumName, Func<T, object> func) where T : Attribute
		{
			return enumName.GetAttributeValue<T, object>(func);
		}
		
		public static V GetAttributeValue<T, V>(this Enum enumName, Func<T, V> func) where T : Attribute
		{
			// 获取枚举类型
			var enumType = enumName.GetType();

			// 获取枚举字段
			var field = enumType.GetField(Enum.GetName(enumType, enumName));

			// 获取字段的属性
			var attr = Attribute.GetCustomAttribute(field, typeof(T)) as T;

			if (attr == null)
				return default(V);

			return func(attr);
		}
	}
}
