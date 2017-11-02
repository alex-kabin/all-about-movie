using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public class WebHtmlDocumentProvider : IHtmlDocumentProvider
	{
		private IEnumerable<KeyValuePair<string, string>> _requestHeaders;

		public WebHtmlDocumentProvider()
		{
		}

		public WebHtmlDocumentProvider(IEnumerable<KeyValuePair<string, string>> requestHeaders)
		{
			_requestHeaders = requestHeaders;
		}

		public WebDocument GetSearchDocument(string documentUrl)
		{
			return LoadHtmlDocument(documentUrl);
		}

		public WebDocument GetDetailsDocument(string documentUrl)
		{
			return LoadHtmlDocument(documentUrl);
		}

		private WebDocument LoadHtmlDocument(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AllowAutoRedirect = true;
			SetRequestHeaders(request);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				HtmlDocument doc = new HtmlDocument();
				using (var stream = response.GetResponseStream())
				{
					doc.Load(stream);
				}

				return new WebDocument() {Document = doc, Url = response.ResponseUri.ToString()};
			}
		}

		private void SetRequestHeaders(HttpWebRequest request)
		{
			var headers = _requestHeaders; 
			if (headers != null)
			{
				foreach (var header in headers)
				{
					switch (header.Key.ToLower())
					{
						case "user-agent":
							request.UserAgent = header.Value;
							break;
						case "referer":
							request.Referer = header.Value;
							break;
						case "accept":
							request.Accept = header.Value;
							break;
					}
				}
			}
		}
	}
}