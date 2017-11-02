using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AllAboutMovie.Core;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.Imdb
{
	internal static class MovieSearchHelper
	{
		public static ICollection<Movie> ExtractMoviesFromDocument(string url, HtmlDocument document)
		{
			var baseUrl = new Uri(url).GetLeftPart(UriPartial.Authority);

			var foundMovies = new List<Movie>();

			var nodes = document.DocumentNode.SelectNodes("//table[@class='results']/tr[contains(@class, 'detailed')]");
			//var nodes = document.DocumentNode.SelectNodes("p");
			if (nodes == null)
			{
				return foundMovies;
			}

			foundMovies.AddRange(nodes.Select(e => ExtractMovie(baseUrl, e)));
			
			return foundMovies;
		}

		private static Movie ExtractMovie(string baseUrl, HtmlNode movieNode)
		{
			var movie = new Movie();

			movie.ThumbnailUrl = movieNode.GetSingleNodeAttributeUrlValueOrNull("td[@class='image']//img", "src", baseUrl);

			var infoNode = movieNode.SelectSingleNode("td[@class='title']");
			if (infoNode != null)
			{
				movie.ID = movie.Url = infoNode.GetSingleNodeAttributeUrlValueOrNull("a", "href", baseUrl);
				movie.OriginalTitle = infoNode.GetSingleNodeTextOrNull("a");
				var yearTypeText = infoNode.GetSingleNodeTextOrNull("span[@class='year_type']");
				if (!String.IsNullOrEmpty(yearTypeText))
				{
					var match = Regex.Match(yearTypeText, @"\d{4}");
					if (match.Success)
					{
						int year;
						if (int.TryParse(match.Value, out year))
							movie.Year = year;
					}
				}

				var creditNode = infoNode.SelectSingleNode("span[@class='credit']");
				if(creditNode != null)
				{
					var directorName = creditNode.GetSingleNodeTextOrNull("a[contains(preceding-sibling::text(), 'Dir')]");
					if(!String.IsNullOrEmpty(directorName))
						movie.Directors.Add(new Person() {Name = directorName});
				}

				var genreNodes = infoNode.SelectNodes("span[@class='genre']/a");
				if (genreNodes != null)
					movie.Genres.AddRange(from gn in genreNodes
					                      let genre = gn.GetSingleNodeTextOrNull(".")
					                      where genre != null
					                      select genre);

				var runtimeText = infoNode.GetSingleNodeTextOrNull("span[@class='runtime']");
				if (!String.IsNullOrEmpty(runtimeText))
				{
					var match = Regex.Match(runtimeText, @"\d+");
					if (match.Success)
					{
						int runtime;
						if (int.TryParse(match.Value, out runtime))
							movie.RuntimeInMinutes = runtime;
					}
				}
			}
			

			return movie;
		}
	}
}