using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Violin.Data.Json
{
	public static class JsonExtension
	{
		/// <summary>
		/// 将对象序列化为 <see cref="Newtonsoft.Json.Linq.JToken"/> 对象
		/// </summary>
		/// <param name="obj">需要转换的对象</param>
		/// <returns>已被序列化成为 <see cref="Newtonsoft.Json.Linq.JToken"/> 的对象</returns>
		public static JToken ToJToken(this object obj)
		{
			return obj.ConvertTo<JToken>();
		}

		/// <summary>
		/// 将对象转换为指定类型的对象
		/// </summary>
		/// <typeparam name="T">要转换成为的对象</typeparam>
		/// <param name="obj">需要转换的对象</param>
		/// <returns>被转换过后的对象</returns>
		public static T ConvertTo<T>(this object obj)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
		}
	}
}
