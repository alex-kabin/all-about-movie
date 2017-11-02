using System;
using System.ComponentModel.Composition;

namespace AllAboutMovie.Core.UI
{
	public interface IExporterSettingsControllerMetadata
	{
		Type ExporterType { get; }
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ExporterSettingsControllerMetadataAttribute : ExportAttribute, IExporterSettingsControllerMetadata
	{
		public ExporterSettingsControllerMetadataAttribute() : base(typeof(IExporterSettingsControllerMetadata)) { }

		public Type ExporterType { get; set; }
	}
}