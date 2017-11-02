using System.Collections.Generic;
using System.ComponentModel.Composition;
using AllAboutMovie.Core;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.Kinopoisk
{
	[Export(typeof(IMovieCatalog))]
	[MovieCatalogMetadata(Name = "kinopoisk.ru",
		IconUri = "pack://application:,,,/AllAboutMovie.Catalogs.Kinopoisk;component/kinopoisk.ico")]
	public class KinopoiskMovieCatalog : WebMovieCatalog
	{
		public KinopoiskMovieCatalog() : base("kinopoisk")
		{
			DebugConfiguration = CatalogConfiguration.Current.Debug;
			RequestHeadersConfiguration = CatalogConfiguration.Current.Headers;
			SupportsSearchByQuery = false;
		}

		protected override string GetSearchByTitleUrl(string title)
		{
			return CatalogConfiguration.Current.UrlTemplate.SearchByTitle.Replace("{TITLE}", title);
		}

		protected override string GetSearchByTitleAndYearUrl(string title, int year)
		{
			return CatalogConfiguration.Current.UrlTemplate.SearchByTitleAndYear
				.Replace("{TITLE}", title)
				.Replace("{YEAR}", year.ToString());
		}

		protected override ICollection<Movie> ParseSearchDocument(string url, HtmlDocument document)
		{
			return MovieSearchHelper.ExtractMoviesFromDocument(url, document);
		}

		protected override Movie ParseDetailsDocument(string url, HtmlDocument document)
		{
			return MovieDetailsHelper.ExtractMovieDetailsFromDocument(url, document);
		}
	}
}