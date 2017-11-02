using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AllAboutMovie.Core;
using AllAboutMovie.Utils;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.Kinopoisk
{
	internal static class MovieDetailsHelper
	{
		public static Movie ExtractMovieDetailsFromDocument(string url, HtmlDocument document)
		{
			var baseUrl = new Uri(url).GetLeftPart(UriPartial.Authority);

			var movieNode = document.DocumentNode.SelectSingleNode("//div[@class='movie']");
			if (movieNode == null)
			{
				return null;
			}

			var movie = new Movie();

			movie.Url = movie.ID = url;

			var titleNode = movieNode.SelectSingleNode("//h1/parent::td/parent::tr/parent::table");
			if (titleNode != null)
			{
				var title1 = titleNode.GetSingleNodeTextOrNull("tr[1]/td[1]/h1");
				var title2 = titleNode.GetSingleNodeTextOrNull("tr[2]/td[1]/table/tr[1]/td[1]/span");

				if (String.IsNullOrEmpty(title2))
				{
					movie.OriginalTitle = title1;
				}
				else
				{
					movie.OriginalTitle = title2;
					movie.TranslatedTitle = title1;
				}

				var oscarNode = titleNode.SelectSingleNode("parent::td/parent::tr/td[1]/a");
				if (oscarNode != null)
				{
					movie.WonOscar = true;
				}


				var posterNode = titleNode.SelectSingleNode("parent::td/parent::tr/parent::table/parent::td/parent::tr/parent::table/tr[2]/td[1]/div//img");
				if (posterNode != null)
				{
					movie.PosterUrl = posterNode.GetSingleNodeAttributeUrlValueOrNull(".", "src", baseUrl);
				}
			}

			var movieInfoNode = movieNode.SelectSingleNode("//table[@class='info']");
			if (movieInfoNode != null)
			{
				var yearString =
					movieInfoNode.GetSingleNodeTextOrNull("tr/td[normalize-space(preceding-sibling::td[@class='type'])='год']/div");
				int year;
				if (int.TryParse(yearString, out year))
				{
					movie.Year = year;
				}

				var countries = ExtractStrings(movieInfoNode,
				                               "tr/td[normalize-space(preceding-sibling::td[@class='type'])='страна']/div");
				movie.Countries.AddRange(countries);

				var directors = ExtractPersons(movieInfoNode,
				                               "tr/td[normalize-space(preceding-sibling::td[@class='type'])='режиссер']/a", baseUrl);
				movie.Directors.AddRange(directors);

				var genres = ExtractStrings(movieInfoNode, "tr/td[normalize-space(preceding-sibling::td[@class='type'])='жанр']");
				movie.Genres.AddRange(genres);

				var runtime =
					movieInfoNode.GetSingleNodeTextOrNull("tr/td[normalize-space(preceding-sibling::td[@class='type'])='время']");
				if (runtime != null)
				{
					var match = Regex.Match(runtime, @"\d+");
					if (match.Success)
					{
						int runtimeInMinutes;
						if (int.TryParse(match.Value, out runtimeInMinutes))
						{
							movie.RuntimeInMinutes = runtimeInMinutes;
						}
					}
				}

				var budgetString =
					movieInfoNode.GetSingleNodeTextOrNull("tr/td[normalize-space(preceding-sibling::td[@class='type'])='бюджет']/div");
				if (budgetString != null)
				{
					movie.Budget = budgetString.Trim();
				}

				var slogan =
					movieInfoNode.GetSingleNodeTextOrNull("tr/td[normalize-space(preceding-sibling::td[@class='type'])='слоган']");
				if (slogan != null && slogan.Length > 1)
				{
					movie.Slogan = slogan.Trim();
				}

				var mpaa =
					movieInfoNode.GetSingleNodeAttributeValueOrNull(
						"tr/td[normalize-space(preceding-sibling::td[@class='type'])='рейтинг MPAA']/a/img", "alt");
				if (mpaa != null)
				{
					var mpaaString = mpaa.Replace("рейтинг ", String.Empty).ToLower();
					switch (mpaaString)
					{
						case "r":
							movie.Mpaa = MPAA.R;
							break;
						case "g":
							movie.Mpaa = MPAA.G;
							break;
						case "pg":
							movie.Mpaa = MPAA.PG;
							break;
						case "pg-13":
							movie.Mpaa = MPAA.PG13;
							break;
						case "nc-17":
							movie.Mpaa = MPAA.NC17;
							break;
					}
				}
			}

			var actors = ExtractPersons(movieNode,
			                            "//td[@class='actor_list']/div/span[@itemprop='actors']/a",
			                            baseUrl);
			movie.Actors.AddRange(actors);

			movie.Storyline = movieNode.GetSingleNodeTextOrNull("//td[@class='news']/span[@class='_reachbanner_']");

			var ratingNode = movieNode.SelectSingleNode("//div[@id='block_rating']");
			if (ratingNode != null)
			{
				var kinopoiskRating = ratingNode.GetSingleNodeTextOrNull("div[@class='block_2']/div[1]/a");
				if (kinopoiskRating != null)
				{
					var match = Regex.Match(kinopoiskRating, @"^(\d+\.?\d+)\s+(\d+\s?\d+)$");
					if (match.Success)
					{
						var rating = new Rating
						             	{
						             		RatedBy = "kinopoisk.ru",
						             		Value = match.Groups[1].Value,
											MaxValue = "10",
						             		Votes = SafeConvertStringToInt(match.Groups[2].Value)
						             	};
						movie.Ratings.Add(rating);
					}
				}

				var topRating = ratingNode.GetSingleNodeTextOrNull("div[@class='block3']/div[1]/span/a");
				if (topRating != null)
				{
					var rating = new Rating
					             	{
					             		RatedBy = "kinopoisk.ru Top-250",
					             		Value = topRating,
										MaxValue = "250",
					             		Votes = 0
					             	};
					movie.Ratings.Add(rating);
				}

				var otherRating = ratingNode.GetSingleNodeTextOrNull("div[@class='block_2']/div[2]");
				if (otherRating != null)
				{
					var match = Regex.Match(otherRating, @"^(\w+):\s*(\d+\.?\d+)\s+\((\d+\s?\d+)\)$");
					if (match.Success)
					{
						var rating = new Rating
						             	{
						             		RatedBy = match.Groups[1].Value,
						             		Value = match.Groups[2].Value,
						             		Votes = SafeConvertStringToInt(match.Groups[3].Value)
						             	};
						movie.Ratings.Add(rating);
					}
				}
			}

			return movie;
		}

		private static int SafeConvertStringToInt(string str)
		{
			int result = 0;
			if (!String.IsNullOrEmpty(str))
			{
				var s = Regex.Replace(str, @"\s", String.Empty);
				int.TryParse(s, out result);
			}
			return result;
		}

		private static IEnumerable<Person> ExtractPersons(HtmlNode node, string xpath, string baseUrl)
		{
			var nodes = node.SelectNodes(xpath);
			return from a in nodes
			       let name = a.InnerText.NormalizeSpace()
			       where name != "..."
			       let url = a.GetSingleNodeAttributeUrlValueOrNull(".", "href", baseUrl)
			       select new Person { Name = name, ID = url, Url = url };
		}

		private static IEnumerable<string> ExtractStrings(HtmlNode rootNode, string xpath, char splitChar = ',')
		{
			var nodeText = rootNode.GetSingleNodeTextOrNull(xpath);
			if (String.IsNullOrEmpty(nodeText))
			{
				return new string[0];
			}

			var strings = nodeText.Split(splitChar);
			return
				from str in strings
				let s = str.Trim()
				where !String.IsNullOrEmpty(s) && s != "..."
				select s;
		}
	}
}