using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AllAboutMovie.Core;

namespace AllAboutMovie.Catalogs.Local
{
	[Export(typeof(IMovieCatalog))]
	[MovieCatalogMetadata(Name = "Local", IconUri = "pack://application:,,,/AllAboutMovie.Catalogs.Local;component/local.ico")]
	public class LocalMovieCatalog : IMovieCatalog
	{
		public string[] Folders { get; set; }

		public string[] Extensions { get; set; }

		public ICollection<Movie> SearchByTitle(string title)
		{
			return LocalSearchHelper.Search(Folders, Extensions, title, 0);
		}

		public ICollection<Movie> SearchByTitleAndYear(string title, int year)
		{
			return LocalSearchHelper.Search(Folders, Extensions, title, year);
		}

		public bool SupportsSearchByQuery
		{
			get { return false; }
		}

		public Movie GetById(string id)
		{
			return LocalSearchHelper.GetMovieDetails(id);
		}

		public ICollection<Movie> SearchByQuery(Expression<Predicate<Movie>> query)
		{
			throw new NotImplementedException();
		}
	}
}
