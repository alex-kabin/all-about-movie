using System.Configuration;
using System.Reflection;

namespace AllAboutMovie
{
	internal class ApplicationConfiguration
	{
		public static FileNameTemplateConfigurationSection FileNameTemplate
		{
			get { return ConfigurationManager.GetSection("fileNameTemplate") as FileNameTemplateConfigurationSection; }
		}
	}


	internal class FileNameTemplateConfigurationSection : ConfigurationSection
	{
		protected override void DeserializeSection(System.Xml.XmlReader reader)
		{
			if(reader.Read())
				XslTemplate = reader.ReadElementContentAsString();
		}

		public string XslTemplate { get; private set; }
		
	}
}