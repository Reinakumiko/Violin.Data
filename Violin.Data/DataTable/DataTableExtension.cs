using System;
using System.Data;

namespace Violin.Data.DataTable
{
	public static class DataTableExtension
	{
		/// <summary>
		/// 将列中的值转换成字符串，如果列为空则赋予默认字符串值
		/// </summary>
		/// <param name="row">需要转换的数据行</param>
		/// <param name="colName">列名</param>
		/// <param name="def">默认值</param>
		/// <returns></returns>
		public static string ToString(this DataRow row, string colName, string def = "")
		{
			var val = row[colName].ToString();
			return val == string.Empty ? def : val;
		}

		/// <summary>
		/// 将列中的值转换成指定的结构类型，如果列为空则赋予默认值
		/// </summary>
		/// <typeparam name="T">要转换的值类型</typeparam>
		/// <param name="row">需要转换的数据行</param>
		/// <param name="colName">列名</param>
		/// <param name="def">默认值</param>
		/// <returns></returns>
		public static T ToValue<T>(this DataRow row, string colName, T def = default(T)) where T : struct
		{
			var originValue = row[colName].ToString();
			return string.IsNullOrWhiteSpace(originValue)
				   ? def
				   : (T)Convert.ChangeType(originValue, typeof(T));
		}
	}
}
