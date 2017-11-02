using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AllAboutMovie.Utils
{
	public static class MovieFileHelper
	{
		public static void ExtractTitleYearFromPath(string movieFilePath, out string title, out string year)
		{
			year = String.Empty;

			var movieFileName = Path.GetFileNameWithoutExtension(movieFilePath);
			movieFileName = movieFileName.Replace('_', ' ').Replace('.', ' ');
			movieFileName = Regex.Replace(movieFileName, @"\b\w+rip\b", String.Empty, RegexOptions.IgnoreCase);
			movieFileName += " ";

			var matches = Regex.Matches(movieFileName, @"\b(19\d{2}|20\d{2})\b");
			if (matches.Count > 0)
			{
				var lastMatch = matches[matches.Count - 1];
				year = lastMatch.Groups[1].Value;
				movieFileName = movieFileName.Remove(lastMatch.Index);
			}

			title = movieFileName.TrimEnd('(', '[', '{').Trim();
		}
	}
}