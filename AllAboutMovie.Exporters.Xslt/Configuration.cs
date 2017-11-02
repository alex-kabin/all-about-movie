using System.Configuration;
using System.Reflection;

namespace AllAboutMovie.Exporters.Xslt
{
	internal class ExporterConfiguration
	{
		private static ExporterConfigurationSection _section;

		public static ExporterConfigurationSection Current
		{
			get 
			{
				if (_section == null)
				{
					var configFileName = Assembly.GetExecutingAssembly().Location;
					var configuration = ConfigurationManager.OpenExeConfiguration(configFileName);
					_section = configuration.GetSection("xslt") as ExporterConfigurationSection;
				}
				return _section;
			}
		}
	}


	internal class ExporterConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("folder", IsRequired = false)]
		public string Folder
		{
			get { return (string)this["folder"]; }
			set { this["folder"] = value; }
		}

		[ConfigurationProperty("stylesheets", IsDefaultCollection = false)]
		public StylesheetCollection Stylesheets
		{
			get
			{
				return (StylesheetCollection)base["stylesheets"];
			}
		}
	}

	internal class StylesheetCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new StylesheetConfigurationElement();
		}


		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			return new StylesheetConfigurationElement(elementName);
		}


		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((StylesheetConfigurationElement)element).Name;
		}

		public StylesheetConfigurationElement this[int index]
		{
			get
			{
				return (StylesheetConfigurationElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		new public StylesheetConfigurationElement this[string name]
		{
			get
			{
				return (StylesheetConfigurationElement)BaseGet(name);
			}
		}

		public int IndexOf(StylesheetConfigurationElement link)
		{
			return BaseIndexOf(link);
		}

		public void Add(StylesheetConfigurationElement link)
		{
			BaseAdd(link);
			// Add custom code here.
		}

		protected override void BaseAdd(ConfigurationElement element)
		{
			BaseAdd(element, false);
			// Add custom code here.
		}

		public void Remove(StylesheetConfigurationElement link)
		{
			if (BaseIndexOf(link) >= 0)
				BaseRemove(link.Name);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}

		public void Clear()
		{
			BaseClear();
			// Add custom code here.
		}
	}

	internal class StylesheetConfigurationElement : ConfigurationElement
	{
		public StylesheetConfigurationElement()
		{
		}

		public  StylesheetConfigurationElement(string name)
		{
			Name = name;
		}
		
		[ConfigurationProperty("name", IsRequired=true, IsKey=true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("file", IsRequired=true)]
		public string File
		{
			get { return (string)this["file"]; }
			set { this["file"] = value; }
		}

		[ConfigurationProperty("outputType", IsRequired=true)]
		public string OutputType
		{
			get { return (string)this["outputType"]; }
			set { this["outputType"] = value; }
		}

		[ConfigurationProperty("outputDescription", IsRequired=false)]
		public string OutputDescription
		{
			get { return (string)this["outputDescription"]; }
			set { this["outputDescription"] = value; }
		}
		
	}
}