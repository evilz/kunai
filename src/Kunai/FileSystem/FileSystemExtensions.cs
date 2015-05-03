using System.IO;

namespace Kunai.FileSystemExt
{
	public static class FileSystemExtensions
	{
		/// <summary>
		/// Delete files in a folder that are like the searchPattern, don't include subfolders.
		/// </summary>
		/// <param name="di"></param>
		/// <param name="searchPattern">DOS like pattern (example: *.xml, ??a.txt)</param>
		/// <returns>Number of files that have been deleted.</returns>
		public static int DeleteFiles(this DirectoryInfo di, string searchPattern)
		{
			return DeleteFiles(di, searchPattern, false);
		}

		/// <summary>
		/// Delete files in a folder that are like the searchPattern
		/// </summary>
		/// <param name="di"></param>
		/// <param name="searchPattern">DOS like pattern (example: *.xml, ??a.txt)</param>
		/// <param name="includeSubdirs"></param>
		/// <returns>Number of files that have been deleted.</returns>
		/// <remarks>
		/// This function relies on DirectoryInfo.GetFiles() which will first get all the FileInfo objects in memory. This is good for folders with not too many files, otherwise
		/// an implementation using Windows APIs can be more appropriate. I didn't need this functionality here but if you need it just let me know.
		/// </remarks>
		public static int DeleteFiles(this DirectoryInfo di, string searchPattern, bool includeSubdirs)
		{
			int deleted = 0;
			foreach (FileInfo fi in di.GetFiles(searchPattern, includeSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
				fi.Delete();
				deleted++;
			}

			return deleted;
		}

		public static long GetSize(this DirectoryInfo dir)
		{
			long length = 0;

			// Loop through files and keep adding their size
			foreach (FileInfo nextfile in dir.GetFiles())
				length += nextfile.Exists ? nextfile.Length : 0;

			// Loop through subdirectories and keep adding their size
			foreach (DirectoryInfo nextdir in dir.GetDirectories())
				length += nextdir.Exists ? nextdir.GetSize() : 0;

			return length;
		}

		public static string GetParentDirectoryPath(this string folderPath, int levels)
		{
			string result = folderPath;
			for (int i = 0; i < levels; i++)
			{
				if (Directory.GetParent(result) != null)
				{
					result = Directory.GetParent(result).FullName;
				}
				else
				{
					return result;
				}
			}
			return result;
		}


		public static string GetParentDirectoryPath(this string folderPath)
		{
			return GetParentDirectoryPath(folderPath, 1);
		}


		public static string GetDirectoryPath(this string filePath)
		{
			return Path.GetDirectoryName(filePath);
		}

		/// <summary>
		/// Move current instance and rename current instance when needed
		/// </summary>
		/// <param name="fileInfo">Current instance</param>
		/// <param name="destFileName">The Path to move current instance to, which can specify a different file name</param>
		/// <param name="renameWhenExists">Bool to specify if current instance should be renamed when exists</param>
		public static void MoveTo(this FileInfo fileInfo, string destFileName, bool renameWhenExists = false)
		{
			string newFullPath = string.Empty;

			if (renameWhenExists)
			{
				int count = 1;

				string fileNameOnly = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				string extension = Path.GetExtension(fileInfo.FullName);
				newFullPath = Path.Combine(destFileName, fileInfo.Name);

				while (File.Exists(newFullPath))
				{
					string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
					newFullPath = Path.Combine(destFileName, tempFileName + extension);
				}
			}

			fileInfo.MoveTo(renameWhenExists ? newFullPath : destFileName);
		}

		/// <summary>
		/// Recursively create directory
		/// </summary>
		/// <param name="dirInfo">Folder path to create.</param>
		public static void CreateDirectory(this DirectoryInfo dirInfo)
		{
			if (dirInfo.Parent != null) CreateDirectory(dirInfo.Parent);
			if (!dirInfo.Exists) dirInfo.Create();
		}

	}
}
