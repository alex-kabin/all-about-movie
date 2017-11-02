using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using AllAboutMovie.Core;

namespace AllAboutMovie.Catalogs.Test
{
	[Export(typeof(IMovieCatalog))]
	[MovieCatalogMetadata(Name = "Test")]
	public class TestMovieCatalog : IMovieCatalog
	{
		private readonly List<Movie> _movies = new List<Movie>()
		                                       	{
		                                       		new Movie()
		                                       			{
															ID = "M1",
		                                       				OriginalTitle = "Movie 1",
		                                       				TranslatedTitle = "Фильм 1",
		                                       				Year = 1999,
		                                       				Runtime = TimeSpan.FromMinutes(95)
		                                       			},
													new Movie()
		                                       			{
															ID = "M2",
		                                       				OriginalTitle = "Movie 2",
		                                       				TranslatedTitle = "Фильм 2",
		                                       				Year = 2009,
		                                       				Runtime = TimeSpan.FromMinutes(135)
		                                       			}
		                                       	};

		public ICollection<Movie> SearchByTitle(string title)
		{
			Thread.Sleep(2000);
			return _movies;
		}

		public ICollection<Movie> SearchByTitleAndYear(string title, int year)
		{
			Thread.Sleep(2000);
			return _movies;
		}

		public ICollection<Movie> SearchByQuery(Expression<Predicate<Movie>> query)
		{
			throw new NotImplementedException();
		}

		public bool SupportsSearchByQuery
		{
			get { return false; }
		}

		public Movie GetById(string id)
		{
			return _movies.FirstOrDefault(m => m.ID == id);
		}
	}
}
