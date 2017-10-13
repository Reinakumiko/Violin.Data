using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Violin.Web.Data;

namespace Violin.Web.Razor
{
	public static class ControllerExtension
	{
		/// <summary>
		/// 将 <see cref="ViewMessageResult{T}"/> 中的数据以 Json 字符串的形式响应
		/// </summary>
		/// <typeparam name="T">包含在 <see cref="ViewMessageResult{T}"/> 中的数据类型</typeparam>
		/// <param name="controller">对指定的控制器响应操作</param>
		/// <param name="result">需要响应的 <see cref="ViewMessageResult{T}"/> 数据</param>
		/// <returns></returns>
		public static ActionResult ViewResult<T>(this Controller controller, ViewMessageResult<T> result)
		{
			controller.Response.Write(JsonConvert.SerializeObject(result));
			return new HttpStatusCodeResult(HttpStatusCode.OK, result.Message);
		}
	}
}
