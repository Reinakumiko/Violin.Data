using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Violin.Web.Data
{
	/// <summary>
	/// 指示一个操作执行的结果
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ViewMessageResult<T>
	{
		/// <summary>
		/// 结果数据的状态码
		/// </summary>
		[JsonProperty("code")]
		public int Code { get; set; }

		/// <summary>
		/// 结果数据的成功与否
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }

		/// <summary>
		/// 结果数据的提示消息
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }

		/// <summary>
		/// 结果数据的包含对象
		/// </summary>
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}
