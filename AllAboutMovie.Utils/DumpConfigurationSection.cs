using System.Configuration;

namespace AllAboutMovie.Utils
{
	public class DumpConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("enabled", IsRequired = false)]
		public bool Enabled
		{
			get { return (bool)this["enabled"]; }
			set { this["enabled"] = value; }
		}

		[ConfigurationProperty("path", IsRequired = false)]
		public string Path
		{
			get { return (string)this["path"]; }
			set { this["path"] = value; }
		}

		[ConfigurationProperty("cleanup", IsRequired = false, DefaultValue = true)]
		public bool Cleanup
		{
			get { return (bool)this["cleanup"]; }
			set { this["cleanup"] = value; }
		}
	}
}