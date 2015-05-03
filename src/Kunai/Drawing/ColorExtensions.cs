using System.Drawing;

namespace Kunai.DrawingExt
{
	public static class ColorExtensions
	{
		public static Color GetContrastingColor(this Color value)
		{
			var d = 0;
			// Counting the perceptive luminance - human eye favors green color... 
			double a = 1 - (0.299 * value.R + 0.587 * value.G + 0.114 * value.B) / 255;

			if (a < 0.5)
				d = 0; // bright colors - black font
			else
				d = 255; // dark colors - white font

			return Color.FromArgb(d, d, d);
		}

		/// <summary>
		/// Convert a (A)RGB string to a Color object
		/// </summary>
		/// <param name="argb">An RGB or an ARGB string</param>
		/// <returns>a Color object</returns>
		public static Color ToColor(this string argb)
		{
			argb = argb.Replace("#", "");
			byte a = System.Convert.ToByte("ff", 16);
			byte pos = 0;
			if (argb.Length == 8)
			{
				a = System.Convert.ToByte(argb.Substring(pos, 2), 16);
				pos = 2;
			}
			byte r = System.Convert.ToByte(argb.Substring(pos, 2), 16);
			pos += 2;
			byte g = System.Convert.ToByte(argb.Substring(pos, 2), 16);
			pos += 2;
			byte b = System.Convert.ToByte(argb.Substring(pos, 2), 16);
			return Color.FromArgb(a, r, g, b);
		}
	}
}
