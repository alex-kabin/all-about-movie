using System;
using System.Collections.Generic;

namespace AllAboutMovie.Core
{
	public interface IFileMovieExporter : IMovieExporter
	{
		string FileName { get; set; }
		FileType GetOutputFileType();
	}
}