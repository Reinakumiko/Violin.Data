using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Data.Streams
{
	public static class StreamExtensions
	{
		private static void WriteContent(this Stream stream, byte[] content)
		{
			stream.Write(content, 0, content.Length);
		}

		public static void Save(this Stream stream, string path)
		{
			stream.Save(new FileInfo(path));
		}

		public static void Save(this Stream stream, FileInfo info)
		{
			var _fileInfo = info.CheckPathUnique();

			if (!_fileInfo.Directory.Exists)
				_fileInfo.Directory.Create();

			using (var _file = _fileInfo.Open(FileMode.Create, FileAccess.Write))
			{
				var byteList = new byte[stream.Length];

				stream.Read(byteList, 0, byteList.Length);

				_file.WriteContent(byteList);
			}
		}

		public static void Save(this MemoryStream stream, string path)
		{
			stream.Save(new FileInfo(path));
		}

		public static void Save(this MemoryStream stream, FileInfo info)
		{
			var _fileInfo = info.CheckPathUnique();

			if (!_fileInfo.Directory.Exists)
				_fileInfo.Directory.Create();

			using (var _file = _fileInfo.Open(FileMode.Create, FileAccess.Write))
			{
				_file.WriteContent(stream.ToArray());
			}
		}
	}
}
