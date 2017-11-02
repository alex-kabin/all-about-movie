using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public class WebDocument
	{
		public string Url { get; set; }
		public HtmlDocument Document { get; set; }
	}
}