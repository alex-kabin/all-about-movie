using System.IO;

namespace AllAboutMovie.Utils
{
	public interface IObjectFormatter
	{
		void Format(object obj, Stream output);
	}
}