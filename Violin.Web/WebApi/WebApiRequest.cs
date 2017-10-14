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
			return new Uri(url).GetWebContent();
		}

		public static string GetWebContent(this Uri uri)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
			using (Stream webStream = webRequest.GetResponse().GetResponseStream())
			using (StreamReader sr = new StreamReader(webStream))
			{
				return sr.ReadToEnd();
			}
		}

		public static Stream GetWebStream(string url)
		{
			return new Uri(url).GetWebStream();
		}

		public static Stream GetWebStream(this Uri uri)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create(uri);

			return !webRequest.HaveResponse ? null : webRequest.GetResponse().GetResponseStream();
		}
	}
}
