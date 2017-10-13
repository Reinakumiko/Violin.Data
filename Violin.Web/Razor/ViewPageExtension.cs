using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Violin.Web.Razor
{
	public static class ViewPageExtension
	{

		/// <summary>
		/// 对一个集合进行一个操作，返回一行中顺序列的数据
		/// 返回一个 <see cref="Func{IEnumerable{DataRow}, Func{DataRow, dynamic}, Object}"/> 的闭包方法。
		/// </summary>
		/// <typeparam name="T">从 <see cref="IEnumerable{T}"/> 中计算出来的类型</typeparam>
		/// <typeparam name="Arg">数据源的类型</typeparam>
		/// <param name="page">基于 Razor 视图的扩展方法</param>
		/// <param name="cellList">要循环的列</param>
		/// <param name="predicate">对列数据筛选的条件</param>
		/// <param name="resultAction">对列集合数据的处理方式</param>
		/// <returns>返回一个 <see cref="Func{IEnumerable{DataRow}, Func{DataRow, dynamic}, Object}"/> 的闭包方法。</returns>
		public static Func<IEnumerable<Arg>, Func<Arg, T>, string, object> LoopAction<T, Arg>(this WebViewPage page, IEnumerable<string> cellList, Func<Arg, string, bool> predicate, Func<IEnumerable<T>, dynamic> resultAction)
		{
			return page.LoopAction<T, Arg>(cellList, predicate, (collection, format) => resultAction(collection));
		}

		/// <summary>
		/// 对一个集合进行一个操作，返回一行中顺序列的数据
		/// 返回一个 <see cref="Func{IEnumerable{DataRow}, Func{DataRow, dynamic}, Object}"/> 的闭包方法。
		/// </summary>
		/// <typeparam name="T">从 <see cref="IEnumerable{T}"/> 中计算出来的类型</typeparam>
		/// <typeparam name="Arg">数据源的类型</typeparam>
		/// <param name="page">基于 Razor 视图的扩展方法</param>
		/// <param name="cellList">要循环的列</param>
		/// <param name="predicate">对列数据筛选的条件</param>
		/// <param name="resultAction">对列集合数据的处理方式</param>
		/// <returns>返回一个 <see cref="Func{IEnumerable{DataRow}, Func{DataRow, dynamic}, Object}"/> 的闭包方法。</returns>
		public static Func<IEnumerable<Arg>, Func<Arg, T>, string, object> LoopAction<T, Arg>(this WebViewPage page, IEnumerable<string> cellList, Func<Arg, string, bool> predicate, Func<IEnumerable<T>, string, dynamic> resultAction)
		{
			//collection: 传入的集合
			//func: 需要计算的列字段名
			//fmt: 数据的格式化字符串
			return (collection, func, fmt) =>
			{
				if (string.IsNullOrWhiteSpace(fmt))
					fmt = "g";

				var formatResult = "{0:" + fmt + "}";

				var listResult = new List<object>();
				foreach (var cell in cellList)
				{
					var cellResult = collection.Where(r => predicate(r, cell));
					var currentResult = resultAction(cellResult.Select(r => func(r)), formatResult);
					listResult.Add(currentResult);
				}

				return page.Html.Raw(string.Join("</td><td>", listResult));
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="page"></param>
		/// <param name="data"></param>
		public static void WriteCell<T>(this WebViewPage page, T data)
		{
			page.Write(string.Format("<td>{0}</td>", data));
		}
	}
}
