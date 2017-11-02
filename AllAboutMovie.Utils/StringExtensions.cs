using System.Text.RegularExpressions;

namespace AllAboutMovie.Utils
{
	public static class StringExtensions
	{
		public static string NormalizeSpace(this string s)
		{
			if(s != null)
				return Regex.Replace(s.Trim(), @"\s+", " ");
			return null;
		}
	}
}