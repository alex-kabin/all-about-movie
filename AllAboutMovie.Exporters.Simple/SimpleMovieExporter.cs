using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Serialization;
using AllAboutMovie.Core;

namespace AllAboutMovie.Exporters.Simple
{
	[Export(typeof(IMovieExporter))]
	[MovieExporterMetadata(Name="Simple")]
	public class SimpleMovieExporter : IFileMovieExporter
	{
		public string FileName { get; set; }

		public FileType GetOutputFileType()
		{
			return new FileType("xml");
		}

		public void Export(Movie movie)
		{
			if (String.IsNullOrEmpty(FileName))
				throw new InvalidOperationException("FileName is not specified");

			var serializer = new XmlSerializer(movie.GetType());
			using(var writer = new StreamWriter(FileName))
				serializer.Serialize(writer, movie);
		}
	}
}