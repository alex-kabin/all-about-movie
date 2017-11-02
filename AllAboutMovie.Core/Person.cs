using System.Xml.Serialization;

namespace AllAboutMovie.Core
{
	public class Person
	{
		public string ID { get; set; }
		[XmlAttribute]
		public string Name { get; set; }
		public string Url { get; set; }
	}
}