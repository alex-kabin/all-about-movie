using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace AllAboutMovie.Utils
{
	public static class Dump
	{
		private static string _rootDumpFolder = null;

		private static DumpConfigurationSection DumpConfiguration
		{
			get
			{
				return ConfigurationManager.GetSection("dump") as DumpConfigurationSection;
			}
		}

		public static bool Enabled
		{
			get
			{
				var config = DumpConfiguration;
				return config != null && config.Enabled;
			}
		}

		private static string PrepareDumpFolder(string dumpName)
		{
			if (_rootDumpFolder == null)
			{
				string dir = null;

				var config = DumpConfiguration;
				if (config != null && !String.IsNullOrEmpty(config.Path))
				{
					if (Path.IsPathRooted(config.Path))
						dir = config.Path;
					else
					{
						var currentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
						dir = Path.Combine(currentFolder, config.Path);
						dir = dir.Replace(@"\.", String.Empty);
					}
				}
				else
				{
					var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
					dir = Path.GetDirectoryName(configuration.FilePath);
					dir = Path.Combine(dir, "Dump");
				}

				_rootDumpFolder = dir; // Path.Combine(dir, "Dump");

				if(config == null || config.Cleanup)
					if (Directory.Exists(_rootDumpFolder)) 
						Directory.Delete(_rootDumpFolder, true);

				Directory.CreateDirectory(_rootDumpFolder);
			}

			var dumpFolder = _rootDumpFolder;
			if(!String.IsNullOrEmpty(dumpName))
				dumpFolder = Path.Combine(dumpFolder, dumpName);

			Directory.CreateDirectory(dumpFolder);
			return dumpFolder;
		}

		public static void Write(string dumpName, Action<string> dumpAction)
		{
			if (!Enabled)
				return;

			try
			{
				var folder = PrepareDumpFolder(dumpName);
				dumpAction(folder);
			}
			catch (Exception ex)
			{
			}
		}

		public static void Write(string dumpName, string fileName, string text)
		{
			if (!Enabled)
				return;

			try
			{
				var folder = PrepareDumpFolder(dumpName);
				File.AppendAllText(Path.Combine(folder, fileName + ".txt"), text);
			}
			catch (Exception ex)
			{
			}
		}

		public static void Write(string dumpName, string fileName, object obj)
		{
			if (!Enabled)
				return;

			try
			{
				var folder = PrepareDumpFolder(dumpName);
				using (var stream = File.CreateText(Path.Combine(folder, fileName + ".xml")))
				{
					var xs = new XmlSerializer(obj.GetType());
					xs.Serialize(stream, obj);
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}