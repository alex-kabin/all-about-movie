using System;
using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public class FileHtmlDocumentProvider : IHtmlDocumentProvider
	{
		private readonly string _searchFilePath;
		private readonly string _detailsFilePath;
		
		public FileHtmlDocumentProvider(string searchFilePath, string detailsFilePath)
		{
			_searchFilePath = searchFilePath;
			_detailsFilePath = detailsFilePath;
		}
		
		public WebDocument GetSearchDocument(string documentUrl)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.Load(_searchFilePath);
			return new WebDocument() {Document = htmlDocument, Url = documentUrl};
		}
		
		public WebDocument GetDetailsDocument(string documentUrl)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.Load(_detailsFilePath);
			return new WebDocument() { Document = htmlDocument, Url = documentUrl };
		}
	}
}