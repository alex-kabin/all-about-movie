using System.ComponentModel;

namespace AllAboutMovie.Core
{
	public interface IPluginMetadata
	{
		string Name { get; }

		[DefaultValue(null)]
		string IconUri { get; }
	}
}