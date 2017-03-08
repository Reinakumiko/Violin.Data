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
	}
}
