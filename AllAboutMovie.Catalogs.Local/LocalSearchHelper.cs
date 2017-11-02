using System.Collections.Generic;
using System.IO;
using AllAboutMovie.Core;
using System;
using System.Linq;
using AllAboutMovie.Utils;

namespace AllAboutMovie.Catalogs.Local
{
	public static class LocalSearchHelper
	{
		public static ICollection<Movie> Search(string[] folders, string[] extensions, string title, int year)
		{
			var movies = new List<Tuple<Movie, int>>();

			HashSet<string> foundFiles = new HashSet<string>();

			foreach (var folder in folders)
			{
				foreach (var extension in extensions)
				{
					foundFiles.UnionWith(Directory.GetFiles(folder.Trim(), extension.Trim(), SearchOption.AllDirectories));
				}
			}

			foreach (var foundFile in foundFiles)
			{
				var movie = GetMovieInfo(foundFile);

				int rel = 0;
				if (String.Compare(movie.OriginalTitle, title, true) == 0 || String.Compare(movie.TranslatedTitle, title, true) == 0)
					rel += 3;
				else
				{
					var originalTitle = movie.OriginalTitle != null ? movie.OriginalTitle.ToLower() : "";
					var translatedTitle = movie.TranslatedTitle != null ? movie.TranslatedTitle.ToLower() : "";
					var t = title.ToLower();
					if (originalTitle.Contains(t) || translatedTitle.Contains(t))
						rel += 2;
				}

				if (year > 0 && movie.Year == year)
					rel += 1;

				if(rel > 0)
					movies.Add(Tuple.Create(movie, rel));
			}

			return movies.OrderByDescending(m => m.Item2).Select(m => m.Item1).ToList();
		}

		private static Movie GetMovieInfo(string filePath)
		{
			string title;
			string yearString;
			MovieFileHelper.ExtractTitleYearFromPath(filePath, out title, out yearString);

			int year;
			Int32.TryParse(yearString, out year);

			return new Movie() { ID = filePath, Url = filePath, OriginalTitle = title, Year = year };
		}

		public static Movie GetMovieDetails(string filePath)
		{
			return GetMovieInfo(filePath);
		}
	}
}