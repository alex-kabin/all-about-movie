using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using AllAboutMovie.Core;
using AllAboutMovie.Utils;

namespace AllAboutMovie.Exporters.Xslt
{
	[Export(typeof(IMovieExporter))]
	[MovieExporterMetadata(Name = "Xslt")]
	public class XslMovieExporter : IFileMovieExporter
	{
		private XslObjectFormatter _formatter;

		public void Export(Movie movie)
		{
			if(_formatter == null)
				throw new InvalidOperationException("Formatter is not initialized");

			if(String.IsNullOrEmpty(FileName))
				throw new InvalidOperationException("FileName is not specified");

			using(var writer = new StreamWriter(FileName))
				_formatter.Format(movie, writer.BaseStream);
		}

		private string _stylesheet;
		public string Stylesheet
		{
			get { return _stylesheet; }
			set
			{
				var stylesheetConfig = ExporterConfiguration.Current.Stylesheets[value];
				if(stylesheetConfig == null)
					throw new ArgumentException(String.Format("Stylesheet with name '{0}' is not registered", value));

				_stylesheet = value;

				var stylesheetUri = GetStylesheetUri(stylesheetConfig.File);
				_formatter = new XslObjectFormatter(stylesheetUri);
			}
		}

		private static string GetStylesheetUri(string fileName)
		{
			var path = ExporterConfiguration.Current.Folder;
			if(String.IsNullOrWhiteSpace(path) || path == ".")
			{
				path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}
			return Path.Combine(path, fileName);
		}

		public string FileName
		{
			get; set;
		}

		public FileType GetOutputFileType()
		{
			var stylesheetConfig = ExporterConfiguration.Current.Stylesheets[Stylesheet];
			return new FileType(stylesheetConfig.OutputType, stylesheetConfig.OutputDescription);
		}

		public string[] GetStylesheets()
		{
			return ExporterConfiguration.Current.Stylesheets.Cast<StylesheetConfigurationElement>().Select(s => s.Name).ToArray();
		}
	}
}