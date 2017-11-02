using System.Collections.Generic;

namespace AllAboutMovie.Core
{
	public interface IMovieExporter : IPlugin
	{
		void Export(Movie movie);
		// void Export(IEnumerable<Movie> movies);
	}
}