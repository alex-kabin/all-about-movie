using System.Configuration;

namespace AllAboutMovie.WebCatalogBase
{
	public class DebugConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("searchFile", IsRequired = false)]
		public string SearchFile
		{
			get { return (string)this["searchFile"]; }
			set { this["searchFile"] = value; }
		}

		[ConfigurationProperty("detailsFile", IsRequired = false)]
		public string DetailsFile
		{
			get { return (string)this["detailsFile"]; }
			set { this["detailsFile"] = value; }
		}
	}
}