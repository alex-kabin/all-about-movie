using System;
using System.Configuration;
using System.Reflection;
using AllAboutMovie.WebCatalogBase;

namespace AllAboutMovie.Catalogs.Kinopoisk
{
	internal class CatalogConfiguration
	{
		private static CatalogConfigurationSection _section;

		public static CatalogConfigurationSection Current
		{
			get
			{
				if (_section == null)
				{
					var configFileName = Assembly.GetExecutingAssembly().Location;
					var configuration = ConfigurationManager.OpenExeConfiguration(configFileName);
					_section = configuration.GetSection("kinopoisk") as CatalogConfigurationSection;
				}
				return _section;
			}
		}
	}


	internal class CatalogConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("urlTemplates", IsRequired = true)]
		public UrlTemplateConfigurationElement UrlTemplate
		{
			get { return (UrlTemplateConfigurationElement)this["urlTemplates"]; }
			set { this["urlTemplates"] = value; }
		}

		[ConfigurationProperty("debug", IsRequired = false)]
		public DebugConfigurationElement Debug
		{
			get { return (DebugConfigurationElement)this["debug"]; }
			set { this["debug"] = value; }
		}

		[ConfigurationProperty("headers", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(KeyValueConfigurationCollection), AddItemName = "add",
			ClearItemsName = "clear",
			RemoveItemName = "remove")]
		public KeyValueConfigurationCollection Headers
		{
			get
			{
				return (KeyValueConfigurationCollection)base["headers"];
			}
		}
	}

	internal class UrlTemplateConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("searchByTitleAndYear", IsRequired = true)]
		public string SearchByTitleAndYear
		{
			get { return (string)this["searchByTitleAndYear"]; }
			set { this["searchByTitleAndYear"] = value; }
		}

		[ConfigurationProperty("searchByTitle", IsRequired = true)]
		public string SearchByTitle
		{
			get { return (string)this["searchByTitle"]; }
			set { this["searchByTitle"] = value; }
		}
	}
}