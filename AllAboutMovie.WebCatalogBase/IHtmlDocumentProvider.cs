using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public interface IHtmlDocumentProvider
	{
		WebDocument GetSearchDocument(string documentUrl);
		WebDocument GetDetailsDocument(string documentUrl);
	}
}