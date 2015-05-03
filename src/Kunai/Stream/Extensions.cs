using System;
using System.IO;
using System.Text;

namespace Kunai.StreamExt
{
	public static class Extensions
	{

		public static MemoryStream ToMemoryStream(this Byte[] buffer)
		{
			MemoryStream ms = new MemoryStream(buffer);
			ms.Position = 0;
			return ms;
		}

		public static void SaveTo(this StringBuilder sBuilder, string path)
		{
			sBuilder.ToString().SaveTo(path);
		}

		public static void SaveTo(this string text, string path)
		{
			File.WriteAllText(path, text);
		}

		public static string GetString(this System.IO.Stream stream)
		{
			// convert stream to string
			StreamReader reader = new StreamReader(stream);
			string text = reader.ReadToEnd();
			return text;
		}

		/// <summary>
		/// Copies an stream to another stream using the buffer passed.
		/// </summary>
		/// <param name="source">source stream</param>
		/// <param name="destination">destination stream</param>
		/// <param name="buffer">buffer to use during copy</param>
		/// <returns>number of bytes copied</returns>
		public static long CopyTo(this System.IO.Stream source, System.IO.Stream destination, byte[] buffer)
		{
			long total = 0;
			int bytesRead;
			while (true)
			{
				bytesRead = source.Read(buffer, 0, buffer.Length);
				if (bytesRead == 0)
					return total;
				total += bytesRead;
				destination.Write(buffer, 0, bytesRead);
			}
		}
		/// <summary>
		/// Copies an stream to another stream using the buffer with specified size.
		/// </summary>
		/// <param name="source">source stream</param>
		/// <param name="destination">destination stream</param>
		/// <param name="bufferLen">length of buffer to create</param>
		/// <returns>number of bytes copied</returns>
		public static long CopyTo(this System.IO.Stream source, System.IO.Stream destination, int bufferLen)
		{
			return source.CopyTo(destination, new byte[bufferLen]);
		}

		/// <summary>
		/// This is a snippet by Chuhukon see:
		/// http://www.koodr.com/item/e207aa7f-0ff1-4e8a-a703-e96f2f175bc9
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this System.IO.Stream stream)
		{

			// ONLY for small stream!
			var streamLength = Convert.ToInt32(stream.Length);
			byte[] data = new byte[streamLength + 1];

			//convert to to a byte array
			stream.Read(data, 0, streamLength);
			stream.Close();

			return data;
		}
	}
}
