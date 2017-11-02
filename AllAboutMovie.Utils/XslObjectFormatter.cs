using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace AllAboutMovie.Utils
{
	public class XslObjectFormatter : IObjectFormatter
	{
		private readonly Dictionary<Type, XmlSerializer> _serializersCache = new Dictionary<Type, XmlSerializer>();
		private readonly XslCompiledTransform _transform;

		protected XslObjectFormatter()
		{
			_transform = new XslCompiledTransform();
		}

		public XslObjectFormatter(XslCompiledTransform transform)
		{
			_transform = transform;
		}
		
		public XslObjectFormatter(string stylesheetUri) : this()
		{
			_transform.Load(stylesheetUri);
		}

		public XslObjectFormatter(XmlReader stylesheet) : this()
		{
			_transform.Load(stylesheet);
		}

		private XmlSerializer GetSerializer(Type type)
		{
			XmlSerializer serializer = null;
			if(_serializersCache != null && _serializersCache.ContainsKey(type))
				serializer = _serializersCache[type];

			if (serializer == null)
			{
				serializer = new XmlSerializer(type);
				if(_serializersCache != null)
					_serializersCache.Add(type, serializer);
			}

			return serializer;
		}
		
		public void Format(object obj, Stream output)
		{
			var objectType = obj.GetType();
			var serializer = GetSerializer(objectType);

			using (var memoryStream = new MemoryStream())
			{
				var xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				serializer.Serialize(xmlWriter, obj);

				memoryStream.Seek(0, SeekOrigin.Begin);

				var xmlReader = new XmlTextReader(memoryStream);
				var streamWriter = new StreamWriter(output, Encoding.Default);
				_transform.Transform(xmlReader, null, streamWriter);
			}
		}
	}
}