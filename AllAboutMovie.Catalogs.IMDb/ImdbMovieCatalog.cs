using System.Collections.Generic;
using System.ComponentModel.Composition;
using AllAboutMovie.Catalogs.Imdb;
using AllAboutMovie.Core;
using AllAboutMovie.WebCatalogBase;
using HtmlAgilityPack;

namespace AllAboutMovie.Catalogs.IMDb
{
	[Export(typeof(IMovieCatalog))]
	[MovieCatalogMetadata(Name = "IMDb.com",
		IconUri = "pack://application:,,,/AllAboutMovie.Catalogs.IMDb;component/imdb.ico")]
	public class ImdbMovieCatalog : WebMovieCatalog
	{
		public ImdbMovieCatalog() : base("imdb")
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