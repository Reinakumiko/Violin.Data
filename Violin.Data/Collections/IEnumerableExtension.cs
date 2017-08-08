using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Violin.Data.Collections
{
	public static class EnumerableExtension
	{
		/// <summary>
		/// 对每个 <see cref="IEnumerable{T}"/> 执行指定操作
		/// </summary>
		/// <typeparam name="T"><see cref="IEnumerable{T}"/> 中每个元素的类型</typeparam>
		/// <param name="collection">要遍历的 <see cref="IEnumerable{T}"/> 实例</param>
		/// <param name="callback">要对每个 <see cref="IEnumerable{T}"/> 元素执行的 <see cref="Action{T}"/> 委托</param>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> callback)
		{
			foreach (var item in collection)
			{
				callback(item);
			}
		}

		/// <summary>
		/// 对每个 <see cref="IEnumerable"/> 执行指定操作
		/// </summary>
		/// <param name="collection">要遍历的 <see cref="IEnumerable"/> 实例</param>
		/// <param name="callback">要对每个 <see cref="IEnumerable"/> 元素执行的 <see cref="Action{T}"/> 委托</param>
		public static void ForEach(this IEnumerable collection, Action<object> callback)
		{
			foreach (var item in collection)
			{
				callback(item);
			}
		}

		/// <summary>
		/// 将 <see cref="IEnumerable{T}"/> 中的每个元素拼接成一个字符串
		/// </summary>
		/// <typeparam name="T"><see cref="IEnumerable{T}"/> 中存储的类型</typeparam>
		/// <param name="collection">需要拼接的集合</param>
		/// <param name="separator">拼接时要使用的连接字符串</param>
		/// <returns>拼接完成的字符串</returns>
		public static string Join<T>(this IEnumerable<T> collection, string separator)
		{
			return collection.Join(separator, i => i);
		}

		/// <summary>
		/// 将 <see cref="IEnumerable{T}"/> 中每个元素的某一个字段拼接成一个字符串
		/// </summary>
		/// <typeparam name="T"><see cref="IEnumerable{T}"/> 中存储的类型</typeparam>
		/// <typeparam name="V">要拼接的元素的类型</typeparam>
		/// <param name="collectioin">需要拼接的集合</param>
		/// <param name="separator">拼接时要使用的连接字符串</param>
		/// <param name="action">处理每个元素要拼接内容的 <see cref="Func{T, TResult}"/></param>
		/// <returns>拼接完成的字符串</returns>
		public static string Join<T, V>(this IEnumerable<T> collectioin, string separator, Func<T, V> func)
		{
			return string.Join(separator, collectioin.Select(i => func(i)));
		}

		/// <summary>
		/// 返回 <see cref="IEnumerable{T}"/> 中每个元素指定列的非重复集合
		/// </summary>
		/// <typeparam name="T"><see cref="IEnumerable{T}"/> 中每个元素的类型</typeparam>
		/// <typeparam name="V">指定的非重复列的类型</typeparam>
		/// <param name="collection">需要抽取非重复列的 <see cref="IEnumerable{T}"/> 集合</param>
		/// <param name="func">对每个元素抽取要运算非重复列操作的 <see cref="Func{T, TResult}"/></param>
		/// <returns>指定非重复列的 <see cref="IEnumerable{T}"/> 集合</returns>
		public static IEnumerable<V> Distinct<T, V>(this IEnumerable<T> collection, Func<T, V> func)
		{
			return collection.Select(r => func(r)).Distinct();
		}
	}
}
