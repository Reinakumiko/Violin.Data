using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace Violin.Compress
{
	public static class CompressUnpack
	{
		/// <summary>
		/// 将一个以 GZip 格式压缩的文件进行解压并保存
		/// </summary>
		/// <param name="path">以 GZip 格式压缩的文件路径</param>
		/// <returns>已使用GZip格式解压的文件</returns>
		public static Stream UnpackGZ(string path)
		{
			return UnpackGZ(new FileInfo(path));
		}

		/// <summary>
		/// 将一个以 GZip 格式压缩的文件进行解压并保存
		/// </summary>
		/// <param name="info">以 GZip 格式压缩的文件路径</param>
		/// <returns>已使用GZip格式解压的文件</returns>
		public static Stream UnpackGZ(FileInfo info)
		{
			if (!info.Exists)
				throw new FileNotFoundException();

			using (var file = info.Open(FileMode.Open, FileAccess.Read))
				return UnpackGZ(file);
		}

		/// <summary>
		/// 将一个以 GZip 格式压缩的文件进行解压并保存
		/// </summary>
		/// <param name="fileStream">以 GZip 格式压缩的文件流</param>
		/// <returns>已使用GZip格式解压的文件</returns>
		public static Stream UnpackGZ(Stream fileStream)
		{
			using (var gzStream = new GZipInputStream(fileStream))
			{
				var stream = new MemoryStream();
				gzStream.CopyTo(stream);

				return stream;
			}
		}

		/// <summary>
		/// 展开一个以 Tar 格式打包的文件，保存到文件目录
		/// </summary>
		/// <param name="path">要展开的 tar 包文件路径</param>
		public static void UnpackTar(string path)
		{
			UnpackTar(new FileInfo(path));
		}

		/// <summary>
		/// 展开一个以 Tar 格式打包的文件，保存到指定目录
		/// </summary>
		/// <param name="path">要展开的 tar 包文件路径</param>
		/// <param name="extractPath">指定的保存目录</param>
		public static void UnpackTar(string path, string extractPath)
		{
			UnpackTar(new FileInfo(path), extractPath);
		}

		/// <summary>
		/// 展开一个以 Tar 格式打包的文件，保存到文件目录
		/// </summary>
		/// <param name="info">要展开的 tar 包文件路径</param>
		public static void UnpackTar(FileInfo info)
		{
			UnpackTar(info, Path.Combine(info.DirectoryName, Path.GetFileNameWithoutExtension(info.Name)));
		}

		/// <summary>
		/// 展开一个以 Tar 格式打包的文件，保存到指定目录
		/// </summary>
		/// <param name="info">要展开的 tar 包文件路径</param>
		/// <param name="extractPath">指定的保存目录</param>
		public static void UnpackTar(FileInfo info, string extractPath)
		{
			using (var file = info.Open(FileMode.Open, FileAccess.Read))
				UnpackTar(file, extractPath);
		}

		/// <summary>
		/// 展开一个以 Tar 格式打包的文件，保存到指定目录
		/// </summary>
		/// <param name="fileStream">要展开的 tar 包文件流</param>
		/// <param name="extractPath">指定的保存目录</param>
		public static void UnpackTar(Stream fileStream, string extractPath)
		{
			if(!Directory.Exists(extractPath))
				Directory.CreateDirectory(extractPath);
			
			using (var archive = TarArchive.CreateInputTarArchive(fileStream))
				archive.ExtractContents(extractPath);
		}

		private static Stream UnPack(FileInfo info)
		{
			using (var file = File.Open(info.FullName, FileMode.Open, FileAccess.Read))
				return new ZipInputStream(file);
		}
	}
}
