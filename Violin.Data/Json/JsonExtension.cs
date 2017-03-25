using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Violin.Data.Json
{
	public static class JsonExtension
	{
		/// <summary>
		/// 将对象序列化为等效的 Json 字符串
		/// </summary>
		/// <typeparam name="T">需要序列化的类型</typeparam>
		/// <param name="obj">需要序列化的实例</param>
		/// <returns>已转化成为其等效的字符串</returns>
		public static string ToJson<T>(this T obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		/// <summary>
		/// 将对象序列化为 <see cref="Newtonsoft.Json.Linq.JToken"/> 对象
		/// </summary>
		/// <typeparam name="T">需要序列化的类型</typeparam>
		/// <param name="obj">需要转换的对象</param>
		/// <returns>已被序列化成为 <see cref="Newtonsoft.Json.Linq.JToken"/> 的对象</returns>
		public static JToken ToJToken<T>(this T obj)
		{
			return obj.ConvertTo<JToken>();
		}

		/// <summary>
		/// 将对象转换为指定类型的对象
		/// </summary>
		/// <typeparam name="T">要转换成为的对象</typeparam>
		/// <param name="obj">需要转换对象的实例</param>
		/// <returns>被转换过后的对象</returns>
		public static T ConvertTo<T>(this object obj)
		{
			return JsonConvert.DeserializeObject<T>(obj.ToJson());
		}

		/// <summary>
		/// 为当前实例创建一个副本
		/// </summary>
		/// <typeparam name="T">要创建副本的类型</typeparam>
		/// <param name="obj">需要复制的类型实例</param>
		/// <returns>当前实例的副本</returns>
		public static T Clone<T>(this T obj)
		{
			return obj.ConvertTo<T>();
		}
	}
}
