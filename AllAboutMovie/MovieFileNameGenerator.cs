using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using AllAboutMovie.Core;
using AllAboutMovie.Utils;
using NLog;
using System;

namespace AllAboutMovie
{
	public class MovieFileNameGenerator : IMovieFileNameGenerator
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private static readonly IMovieFileNameGenerator Instance = new MovieFileNameGenerator();

		public static IMovieFileNameGenerator Default
		{
			get { return Instance; }
		}
		
		private IObjectFormatter _objectFormatter;

		protected virtual IObjectFormatter CreateFormatter()
		{
			const string XSLNS = "http://www.w3.org/1999/XSL/Transform";

			if (ApplicationConfiguration.FileNameTemplate == null)
				return null;

			using (var memoryStream = new MemoryStream())
			{
				var xmlWriter = new XmlTextWriter(memoryStream, Encoding.Default);
				xmlWriter.WriteStartDocument();

				xmlWriter.WriteStartElement("xsl", "stylesheet", XSLNS);
				xmlWriter.WriteAttributeString("version", "1.0");

				xmlWriter.WriteStartElement("output", XSLNS);
				xmlWriter.WriteAttributeString("method", "text");
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("template", XSLNS);
				xmlWriter.WriteAttributeString("match", "Movie");

				xmlWriter.WriteRaw(ApplicationConfiguration.FileNameTemplate.XslTemplate);

				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();

				memoryStream.Seek(0, SeekOrigin.Begin);

				var streamReader = new StreamReader(memoryStream, Encoding.Default);
				Log.Debug(streamReader.ReadToEnd());

				memoryStream.Seek(0, SeekOrigin.Begin);

				var xmlReader = new XmlTextReader(memoryStream);
				return new XslObjectFormatter(xmlReader);
			}
		}

		public string GetMovieFileName(Movie movie)
		{
			if (_objectFormatter == null)
			{
				_objectFormatter = CreateFormatter();
			}

			if (_objectFormatter != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					_objectFormatter.Format(movie, memoryStream);
					memoryStream.Seek(0, SeekOrigin.Begin);
					using (var streamReader = new StreamReader(memoryStream, Encoding.Default))
					{
						var fileName = streamReader.ReadToEnd();
						return fileName.Trim();
					}
				}
			}

			return null;
		}
	}
}