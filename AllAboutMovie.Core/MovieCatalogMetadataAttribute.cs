using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace AllAboutMovie.Core
{
	public interface IMovieCatalogMetadata : IPluginMetadata
	{
		
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class MovieCatalogMetadataAttribute : ExportAttribute, IMovieCatalogMetadata
	{
		public MovieCatalogMetadataAttribute() : base(typeof(IMovieCatalogMetadata)) { }

		public string Name { get; set; }
		public string IconUri { get; set; }
	}
}