using System;
using AllAboutMovie.Utils;
using HtmlAgilityPack;

namespace AllAboutMovie.WebCatalogBase
{
	public static class HtmlNodeExtensions
	{
		public static string GetSingleNodeAttributeUrlValueOrNull(this HtmlNode rootNode, string xpath, string attributeName, string baseUrl)
		{
			var url = GetSingleNodeAttributeValueOrNull(rootNode, xpath, attributeName);
			if(url != null)
			{
				var uri = new Uri(url, UriKind.RelativeOrAbsolute);
				if (!uri.IsAbsoluteUri)
					url = new Uri(new Uri(baseUrl), uri).ToString();
			}

			return url;
		}

		public static string GetSingleNodeAttributeValueOrNull(this HtmlNode rootNode, string xpath, string attributeName)
		{
			string attributeValue = null;
			if (rootNode != null)
			{
				var selectedNode = rootNode.SelectSingleNode(xpath);
				if (selectedNode != null)
					attributeValue = selectedNode.GetAttributeValue(attributeName, null);

				if (attributeValue != null)
					attributeValue = HtmlHelper.Decode(attributeValue).NormalizeSpace();
			}
			return attributeValue;
		}

		public static string GetSingleNodeTextOrNull(this HtmlNode rootNode, string xpath)
		{
			string nodeText = null;

			if (rootNode != null)
			{
				var selectedNode = rootNode.SelectSingleNode(xpath);
				if (selectedNode != null)
					nodeText = selectedNode.InnerText;

				if (nodeText != null)
					nodeText = HtmlHelper.Decode(nodeText).NormalizeSpace();
			}

			return nodeText;
		}
	}
}