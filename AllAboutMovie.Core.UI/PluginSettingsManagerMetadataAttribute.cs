using System;
using System.ComponentModel.Composition;

namespace AllAboutMovie.Core.UI
{
	public interface IPluginSettingsManagerMetadata
	{
		Type PluginType { get; }
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PluginSettingsManagerMetadataAttribute : ExportAttribute, IPluginSettingsManagerMetadata
	{
		public PluginSettingsManagerMetadataAttribute() : base(typeof(IPluginSettingsManagerMetadata)) { }

		public Type PluginType { get; set; }
	}
}