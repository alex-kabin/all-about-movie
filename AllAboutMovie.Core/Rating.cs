using System.Xml.Serialization;

namespace AllAboutMovie.Core
{
	public class Rating
	{
		[XmlAttribute]
		public string Value { get; set; }
		[XmlAttribute]
		public string MaxValue { get; set; }
		[XmlAttribute]
		public int Votes { get; set; }
		[XmlAttribute]
		public string RatedBy { get; set; }
	}
}