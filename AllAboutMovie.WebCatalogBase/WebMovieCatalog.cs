using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using AllAboutMovie.Core;
using AllAboutMovie.Utils;
using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public abstract class WebMovieCatalog : IMovieCatalog
	{
		private readonly string _catalogName;
		
		protected WebMovieCatalog(string catalogName)
		{
			_catalogName = catalogName;
		}

		protected DebugConfigurationElement DebugConfiguration { get; set; }
		protected KeyValueConfigurationCollection RequestHeadersConfiguration { get; set; }

		private IHtmlDocumentProvider _documentProvider;
		protected virtual IHtmlDocumentProvider DocumentProvider
		{
			get
			{
				if(_documentProvider != null)
					return _documentProvider;

				if(DebugConfiguration != null)
				{
					var searchFile = DebugConfiguration.SearchFile;
					var detailsFile = DebugConfiguration.DetailsFile;
					if (!String.IsNullOrEmpty(searchFile) && !String.IsNullOrEmpty(detailsFile))
						_documentProvider = new FileHtmlDocumentProvider(searchFile, detailsFile);
				}

				if(_documentProvider == null)
				{
					IEnumerable<KeyValuePair<string, string>> headers = null;
					if(RequestHeadersConfiguration != null)
					{
						headers =
							RequestHeadersConfiguration.Cast<KeyValueConfigurationElement>().Select(
								kv => new KeyValuePair<string, string>(kv.Key, kv.Value));
					}
					_documentProvider = new WebHtmlDocumentProvider(headers);
				}
				return _documentProvider;
			}
		}
		
		protected abstract string GetSearchByTitleUrl(string title);
		protected abstract string GetSearchByTitleAndYearUrl(string title, int year);
		protected virtual string GetSearchByQueryUrl(Expression<Predicate<Movie>> query)
		{
			throw new NotImplementedException();
		}
		
		protected abstract ICollection<Movie> ParseSearchDocument(string url, HtmlDocument document);
		protected abstract Movie ParseDetailsDocument(string url, HtmlDocument document);
		
		protected virtual string GetDumpFileName(string name)
		{
			return DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + name;
		}

		private static void DumpUrlDocument(string dumpName, string fileName, string url, HtmlDocument document)
		{
			Dump.Write(dumpName, folder =>
			                         	{
			                         		File.AppendAllText(Path.Combine(folder, fileName + ".url"), url);
													document.Save(Path.Combine(folder, fileName + ".html"));
			                         	});
		}

		private ICollection<Movie> Search(string searchUrl)
		{
			var searchDocument = DocumentProvider.GetSearchDocument(searchUrl);
			DumpUrlDocument(_catalogName, GetDumpFileName("search"), searchDocument.Url, searchDocument.Document);

			var movies = ParseSearchDocument(searchDocument.Url, searchDocument.Document);
			Dump.Write(_catalogName, GetDumpFileName("search"), movies);

			return movies;
		}

		public ICollection<Movie> SearchByTitle(string title)
		{
			var searchUrl = GetSearchByTitleUrl(title);
			return Search(searchUrl);
		}

		public ICollection<Movie> SearchByTitleAndYear(string title, int year)
		{
			var searchUrl = GetSearchByTitleAndYearUrl(title, year);
			return Search(searchUrl);
		}

		public ICollection<Movie> SearchByQuery(Expression<Predicate<Movie>> query)
		{
			var searchUrl = GetSearchByQueryUrl(query);
			return Search(searchUrl);
		}

		public bool SupportsSearchByQuery { get; protected set; }

		public Movie GetById(string id)
		{
			var detailsUrl = id;
			var detailsDocument = DocumentProvider.GetDetailsDocument(detailsUrl);
			
			DumpUrlDocument(_catalogName, GetDumpFileName("details"), detailsDocument.Url, detailsDocument.Document);

			var movie = ParseDetailsDocument(detailsDocument.Url, detailsDocument.Document);

			Dump.Write(_catalogName, GetDumpFileName("details"), movie);

			return movie;
		}
	}
}
