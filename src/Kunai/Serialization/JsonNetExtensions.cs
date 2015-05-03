using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kunai.Serialization
{
	public static class JsonNetExtensions
	{

		/// <summary>
		/// Makes a copy from the object.
		/// Doesn't copy the reference memory, only data.
		/// </summary>
		/// <typeparam name="T">Type of the return object.</typeparam>
		/// <param name="item">Object to be copied.</param>
		/// <returns>Returns the copied object.</returns>
		public static T Clone<T>(this object item)
		{
			if (item != null)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				MemoryStream stream = new MemoryStream();

				formatter.Serialize(stream, item);
				stream.Seek(0, SeekOrigin.Begin);

				T result = (T)formatter.Deserialize(stream);

				stream.Close();

				return result;
			}
			else
				return default(T);
		}

		#region XML serilizer



		// TODO : CHANGE ALL THIS !!!
		public static string ToXML<T>(this T o)
		where T : new()
		{
			string retVal;
			using (var ms = new MemoryStream())
			{
				var xs = new XmlSerializer(typeof(T));
				xs.Serialize(ms, o);
				ms.Flush();
				ms.Position = 0;
				var sr = new StreamReader(ms);
				retVal = sr.ReadToEnd();
			}
			return retVal;
		}


		public static T Deserialize<T>(this XDocument xmlDocument)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			using (XmlReader reader = xmlDocument.CreateReader())
				return (T)xmlSerializer.Deserialize(reader);
		}


		// TODO : move to XmlSerializer 
		public static string SerializeToXml(this object obj)
		{
			XDocument doc = new XDocument();
			using (XmlWriter xmlWriter = doc.CreateWriter())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
				xmlSerializer.Serialize(xmlWriter, obj);
				xmlWriter.Close();
			}
			return doc.ToString();
		}
		#endregion
	}
}
