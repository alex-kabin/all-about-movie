using AllAboutMovie.Core;

namespace AllAboutMovie
{
	public interface IMovieFileNameGenerator
	{
		string GetMovieFileName(Movie movie);
	}
}