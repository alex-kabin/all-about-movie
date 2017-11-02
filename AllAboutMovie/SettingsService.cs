using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Xml;
using AllAboutMovie.Core;

namespace AllAboutMovie
{
	[Export(typeof(ISettingsService))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class SettingsService : ISettingsService
	{
		private readonly ClientSettingsSection _section;
		private readonly Configuration _configuration;

		private static bool IsPortable
		{
			get { return ConfigurationManager.AppSettings["Portable"] != null; }
		}

		public SettingsService()
		{
			if(IsPortable)
				_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			else
				_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
			
			_section = _configuration.GetSection("userSettings/settings") as ClientSettingsSection;
		}

		public string this[string settingKey]
		{
			get
			{
				var setting = _section.Settings.Get(settingKey);
				if(setting != null)
				{
					return setting.Value.ValueXml.InnerText;
				}
				return null;
			}
			set 
			{
				var setting = _section.Settings.Get(settingKey);
				if(setting == null)
				{
					setting = new SettingElement(settingKey, SettingsSerializeAs.String)
					          	{
					          		Value = { ValueXml = new XmlDocument().CreateElement("value") }
					          	};
					_section.Settings.Add(setting);
				}
				setting.Value.ValueXml.InnerText = value;
			}
		}

		public void Save()
		{
			if (IsPortable)
				_configuration.Save();
			else
				_configuration.Save(ConfigurationSaveMode.Modified, true);
		}
	}
}