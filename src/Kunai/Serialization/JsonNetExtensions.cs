using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kunai.Serialization;

public static class JsonNetExtensions
{
	#region XML serializer



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
