using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AllAboutMovie.Core
{
	public interface IMovieCatalog : IPlugin
	{
		ICollection<Movie> SearchByTitle(string title);

		ICollection<Movie> SearchByTitleAndYear(string title, int year);

		ICollection<Movie> SearchByQuery(Expression<Predicate<Movie>> query);

		bool SupportsSearchByQuery { get; }

		Movie GetById(string id);
	}
}