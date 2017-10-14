namespace Violin.Data.Path
{
	using System.IO;

	public static class PathExtension
	{
		/// <summary>
		/// 检查路径是否唯一，如该路径已存在则返回一个新的唯一路径
		/// </summary>
		/// <param name="path">要检查的路径</param>
		/// <returns>新的路径</returns>
		public static FileInfo CheckPathUnique(this FileInfo path)
		{
			var currentPath = path;
			var retryCount = 0;
			var fileName = Path.GetFileNameWithoutExtension(path.FullName);

			while (currentPath.Exists)
			{
				currentPath = new FileInfo($"{path.DirectoryName}/{fileName} ({++retryCount}){currentPath.Extension}");
			}

			return currentPath;
		}
	}
}
