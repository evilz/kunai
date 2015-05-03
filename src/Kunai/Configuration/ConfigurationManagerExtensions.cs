using System.Configuration;

namespace Kunai.Configuration
{
	public static class ConfigurationManagerExt
	{
		public static T AppSettings<T>(string key) where T : struct
		{
			if (string.IsNullOrWhiteSpace(key))
				return default(T);

			AppSettingsReader r = new AppSettingsReader();
			var val = r.GetValue(key, typeof(T));
			return (T)val;
		}
	}
}
