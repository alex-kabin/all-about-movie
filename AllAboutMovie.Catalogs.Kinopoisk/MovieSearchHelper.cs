using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AllAboutMovie.Core;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.Kinopoisk
{
	internal static class MovieSearchHelper
	{
		public static ICollection<Movie> ExtractMoviesFromDocument(string url, HtmlDocument document)
		{
			var baseUrl = new Uri(url).GetLeftPart(UriPartial.Authority);

			var foundMovies = new List<Movie>();

			var nodes =
				document.DocumentNode.SelectNodes(
					"//div[@class='search_results'] | //div[@class='search_results search_results_last']");
			if (nodes == null)
			{
				var movie = MovieDetailsHelper.ExtractMovieDetailsFromDocument(url, document);
				if(movie != null)
					foundMovies.Add(movie);

				return foundMovies;
			}

			foreach (var node in nodes)
			{
				var elementNodes = node.SelectNodes("div[@class='element most_wanted'] | div[@class='element']");
				if (elementNodes != null)
				{
					foundMovies.AddRange(elementNodes.Select(e => ExtractMovie(baseUrl, e)));
				}
			}

			return foundMovies;
		}

		private static Movie ExtractMovie(string baseUrl, HtmlNode movieNode)
		{
			var movie = new Movie();
			movie.ID = movie.Url = movieNode.GetSingleNodeAttributeUrlValueOrNull("p[@class='pic']/a[@href]", "href", baseUrl);
		   movie.Url = movie.Url.Replace("https:", "http:");

			var thumbnailNode = movieNode.SelectSingleNode("p[@class='pic']/a/img");
			if (thumbnailNode != null)
			{
				string alt = String.Empty;
				var altAttribute = thumbnailNode.Attributes["alt"];
				if(altAttribute != null)
					alt = altAttribute.Value;

				var url = (from a in thumbnailNode.Attributes let value = a.Value where a.Name=="title" && value != alt select value).FirstOrDefault();
				if (url != null)
				{
					var uri = new Uri(url, UriKind.RelativeOrAbsolute);
					if (!uri.IsAbsoluteUri)
						url = new Uri(new Uri(baseUrl), uri).ToString();
				}
				movie.ThumbnailUrl = url;
			}

			var infoNode = movieNode.SelectSingleNode("div[@class='info']");
			if (infoNode != null)
			{
				string yearString = infoNode.GetSingleNodeTextOrNull("p[@class='name']/span[@class='year']");
				int year;
				if (int.TryParse(yearString, out year))
				{
					movie.Year = year;
				}

				string name1 = infoNode.GetSingleNodeTextOrNull("p[@class='name']/a");
				movie.OriginalTitle = name1;

				string nameTimeString = infoNode.GetSingleNodeTextOrNull("span[1]");
				if (!String.IsNullOrEmpty(nameTimeString))
				{
					string name2 = null;
					string time = null;

					var nameTimeMatch = Regex.Match(nameTimeString, @"^(?<name>.*?),?\s*(?:(?<time>\d+)\sмин)?$");
					if (nameTimeMatch.Success)
					{
						name2 = nameTimeMatch.Groups["name"].Value;
						time = nameTimeMatch.Groups["time"].Value;
					}

					if (!String.IsNullOrEmpty(name2))
					{
						movie.OriginalTitle = name2;
						movie.TranslatedTitle = name1;
					}

					int runtime;
					if (int.TryParse(time, out runtime))
					{
						movie.RuntimeInMinutes = runtime;
					}
				}

				string countryDirectorGenresString = infoNode.GetSingleNodeTextOrNull("span[2]");
				if (countryDirectorGenresString != null)
				{
					countryDirectorGenresString = countryDirectorGenresString.Replace("...", String.Empty);
					var countryDirectorGenresMatch =
						Regex.Match(countryDirectorGenresString,
						            @"^(?:(?<country>.+?),?\s?)?реж\.\s?(?<director>.+?)(?:\s+\((?<genres>.+)\))?$");
					if (countryDirectorGenresMatch.Success)
					{
						var country = countryDirectorGenresMatch.Groups["country"].Value.Trim();
						if (!String.IsNullOrEmpty(country))
						{
							country = country.Trim();
							movie.Countries.Add(country);
						}

						var directorName = countryDirectorGenresMatch.Groups["director"].Value.Trim();
						if (!String.IsNullOrEmpty(directorName))
						{
							movie.Directors.Add(new Person { Name = directorName });
						}

						var genresString = countryDirectorGenresMatch.Groups["genres"].Value;
						if (!String.IsNullOrEmpty(genresString))
						{
							var genresArray = genresString.Split(',').Select(s => s.Trim());
							movie.Genres.AddRange(genresArray);
						}
					}
				}
			}
			return movie;
		}
	}
}