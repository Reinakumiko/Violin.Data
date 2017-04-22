using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Web.WebApi
{
	public static class WebApiRequest
	{
		public static string GetResult(string url)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			using (Stream webStream = webRequest.GetResponse().GetResponseStream())
			using (StreamReader sr = new StreamReader(webStream))
			{
				return sr.ReadToEnd();
			}
		}
	}
}
