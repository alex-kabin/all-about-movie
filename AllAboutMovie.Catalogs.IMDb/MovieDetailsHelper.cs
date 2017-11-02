using System;
using System.Linq;
using System.Text.RegularExpressions;
using AllAboutMovie.Core;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.IMDb
{
	internal static class MovieDetailsHelper
	{
		public static Movie ExtractMovieDetailsFromDocument(string url, HtmlDocument document)
		{
			var baseUrl = new Uri(url).GetLeftPart(UriPartial.Authority);

			var movieNode = document.DocumentNode.SelectSingleNode("//div[@id='pagecontent']");
			if (movieNode == null)
			{
				return null;
			}

			var movie = new Movie();

			movie.Url = movie.ID = url;

			var posterNode = movieNode.SelectSingleNode("//td[@id='img_primary']/a");
			if (posterNode != null)
			{
				movie.PosterUrl = posterNode.GetSingleNodeAttributeUrlValueOrNull("img", "src", baseUrl);
			}

			var titleNode = movieNode.SelectSingleNode("//h1");
			if(titleNode != null)
			{
				var title1 = titleNode.FirstChild != null ? titleNode.FirstChild.GetSingleNodeTextOrNull(".") : null;
				string title2 = null;
				var title2Node = titleNode.SelectSingleNode("span[@class='title-extra']");
				if(title2Node != null)
					title2 = title2Node.FirstChild != null ? title2Node.FirstChild.GetSingleNodeTextOrNull(".") : null;

				if (title2 != null)
				{
					movie.OriginalTitle = title2;
					movie.TranslatedTitle = title1;
				}
				else
				{
					movie.OriginalTitle = title1;
				}

				var yearText = titleNode.GetSingleNodeTextOrNull("span");
				if(yearText != null)
				{
					var match = Regex.Match(yearText, @"\d{4}");
					if (match.Success)
					{
						int year;
						if (int.TryParse(match.Value, out year))
							movie.Year = year;
					}
				}
			}

			var infoNode = movieNode.SelectSingleNode("//div[@class='infobar']");
			if(infoNode != null)
			{
				var infoText = infoNode.GetSingleNodeTextOrNull(".");
				if (infoText != null)
				{
					var match = Regex.Match(infoText, @"(\d+)\smin");
					if (match.Success)
					{
						int runtimeInMinutes;
						if (int.TryParse(match.Groups[1].Value, out runtimeInMinutes))
							movie.RuntimeInMinutes = runtimeInMinutes;
					}
				}
			}

			var ratingNode = movieNode.SelectSingleNode("//div[@class='star-box-details']");
			if (ratingNode != null)
			{
				var imdbRating = new Rating() { RatedBy = "IMDb" };
				var imdbRatingValue = ratingNode.GetSingleNodeTextOrNull("//span[@itemprop='ratingValue']");
				var imdbRatingMax = ratingNode.GetSingleNodeTextOrNull("//span[@itemprop='bestRating']");
				if (imdbRatingValue != null)
				{
					imdbRating.Value = imdbRatingValue;
					imdbRating.MaxValue = imdbRatingMax;
				}

				var imdbRatingCount = ratingNode.GetSingleNodeTextOrNull("//span[@itemprop='ratingCount']");
				if (!String.IsNullOrEmpty(imdbRatingCount))
				{
					imdbRatingCount = imdbRatingCount.Replace(",", String.Empty);
					var match = Regex.Match(imdbRatingCount, @"\d+");
					if (match.Success)
						imdbRating.Votes = SafeConvertStringToInt(match.Value);
				}

				movie.Ratings.Add(imdbRating);

				//var otherRatingNode = ratingNode.SelectSingleNode("span[strong]");
				//if (otherRatingNode != null)
				//{
				//   var otherRating = new Rating();
				//   var votesText = otherRatingNode.GetSingleNodeTextOrNull("a[@href='criticreviews']");
				//   if (votesText != null)
				//   {
				//      var match = Regex.Match(votesText, @"\d+");
				//      if (match.Success)
				//      {
				//         int votes;
				//         if (int.TryParse(match.Value, out votes))
				//            otherRating.Votes = votes;
				//      }
				//   }

				//   otherRating.RatedBy = otherRatingNode.GetSingleNodeTextOrNull("a[last()]");
				//   var valueText = otherRatingNode.GetSingleNodeTextOrNull("strong");
				//   var maxText = otherRatingNode.GetSingleNodeTextOrNull("span");
				//   otherRating.Value = valueText;
				//   if (maxText != null)
				//      otherRating.MaxValue = maxText.Trim('/');

				//   movie.Ratings.Add(otherRating);
				//}
			}

			var directorNodes = movieNode.SelectNodes("//div[contains(child::h4, 'Director')]/a");
			if(directorNodes != null)
			{
				var directors = from dn in directorNodes
								let name = dn.GetSingleNodeTextOrNull(".")
								let link = dn.GetSingleNodeAttributeUrlValueOrNull(".", "href", baseUrl)
								select new Person() { Name = name, ID = link, Url = link};
				movie.Directors.AddRange(directors);
			}

			var castNode = movieNode.SelectSingleNode("//table[@class='cast_list']");
			if(castNode != null)
			{
				var actorNodes = castNode.SelectNodes("tr/td[@class='name']");
				if (actorNodes != null)
				{
					var actors = from an in actorNodes
					             let name = an.GetSingleNodeTextOrNull(".")
					             let link = an.GetSingleNodeAttributeUrlValueOrNull("a", "href", baseUrl)
					             select new Person() { Name = name, ID = link, Url = link};
					movie.Actors.AddRange(actors);
				}
			}

			movie.Storyline = movieNode.GetSingleNodeTextOrNull("//p[contains(preceding-sibling::h2, 'Storyline')]");

			var sloganNode = movieNode.SelectSingleNode("//h4[contains(., 'Taglines')]");
			if (sloganNode != null)
				movie.Slogan = sloganNode.NextSibling.GetSingleNodeTextOrNull(".");

			var genreNodes = movieNode.SelectNodes("//div[contains(h4, 'Genres')]/a");
			if(genreNodes != null)
			{
				movie.Genres.AddRange(genreNodes.Select(gn => gn.GetSingleNodeTextOrNull(".")));
			}

			var countryNodes = movieNode.SelectNodes("//div[contains(h4, 'Country')]/a");
			if (countryNodes != null)
			{
				movie.Countries.AddRange(countryNodes.Select(gn => gn.GetSingleNodeTextOrNull(".")));
			}

			var budgetNode = movieNode.SelectSingleNode("//h4[contains(., 'Budget')]");
			if (budgetNode != null)
			{
				var budgetText = budgetNode.NextSibling.GetSingleNodeTextOrNull(".");
				if (!String.IsNullOrEmpty(budgetText))
					movie.Budget = Regex.Replace(budgetText, @"\(.*\)", String.Empty).Trim();
			}

			var mpaaNode = movieNode.SelectSingleNode("//h4[contains(., 'MPAA')]");
			if(mpaaNode != null)
			{
				var mpaaText = mpaaNode.NextSibling.GetSingleNodeTextOrNull(".");
				if (mpaaText != null)
				{
					var match = Regex.Match(mpaaText, @"\s(G|PG|PG-13|R|NC-17)\s");
					if(match.Success)
					{
						switch(match.Groups[1].Value)
						{
							case "G": movie.Mpaa = MPAA.G; break;
							case "PG": movie.Mpaa = MPAA.PG; break;
							case "PG-13": movie.Mpaa = MPAA.PG13; break;
							case "R": movie.Mpaa = MPAA.R; break;
							case "NC-17": movie.Mpaa = MPAA.NC17; break;
							default: movie.Mpaa = MPAA.None; break;
						}
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
	}
}