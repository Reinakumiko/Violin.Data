using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Data.DateTime
{
	using System;

	public static class DateTimeExtension
	{
		/// <summary>
		/// 将Unix时间戳转换为 <see cref="DateTime"/> 的实例
		/// </summary>
		/// <param name="date"></param>
		/// <param name="stamp">需要转换的时间戳</param>
		/// <returns></returns>
		public static DateTime FromUnixStamp(int stamp)
		{
			return new DateTime(stamp * 10000000L + 621355968000000000);
		}

		public static string GetWeekname(this DateTime date)
		{
			//计算日期与该周周一相差的天数
			var offset = date.DayOfWeek - DayOfWeek.Monday;

			//如果结果为 -1 则这一天是周日, 属于上一周(周一到周日为一周)
			offset = offset == -1 ? 6 : offset;

#if DEBUG
			Console.WriteLine("offset: {0}", offset);
#endif

			//获取该周的首日 (周一)
			var startDate = date.AddDays(-offset);

			//获得该周的周日 (周一 + 6天)
			var endDate = startDate.AddDays(6);

			//组合该周的周字符串 (格式: 2017/7/3-2017/7/10)
#if NET40
			var weekName = string.Format("{0:yyyy^M^d}-{1:yyyy^M^d}", startDate, endDate);
			weekName = weekName.Replace("^", "/");
#else
			var weekName = string.Format("{0:yyyy/M/d}-{1:yyyy/M/d}", startDate, endDate);
#endif

#if DEBUG
			Console.WriteLine(weekName);
#endif

			return weekName;
		}
	}
}
