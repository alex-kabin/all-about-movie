using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace AllAboutMovie.Core
{
	public interface IMovieExporterMetadata : IPluginMetadata
	{
		
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class MovieExporterMetadataAttribute : ExportAttribute, IMovieExporterMetadata
	{
		public MovieExporterMetadataAttribute() : base(typeof(IMovieExporterMetadata)) { }

		public string Name { get; set; }
		public string IconUri { get; set; }
	}
}