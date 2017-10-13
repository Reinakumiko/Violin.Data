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
		public static string GetWebContent(string url)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			using (Stream webStream = webRequest.GetResponse().GetResponseStream())
			using (StreamReader sr = new StreamReader(webStream))
			{
				return sr.ReadToEnd();
			}
		}

		public static string GetWebContent(this Uri uri)
		{
			return GetWebContent(uri.ToString());
		}

		public static Stream GetWebStream(string url)
		{
			return WebRequest.Create(url).GetResponse().GetResponseStream();
		}

		public static Stream GetWebStream(this Uri uri)
		{
			return GetWebStream(uri.ToString());
		}
	}
}
