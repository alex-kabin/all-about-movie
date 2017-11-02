using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace AllAboutMovie.Utils
{
	public static class HtmlHelper
	{
		public static string Decode(string s)
		{
			var t = Regex.Replace(s,
			                      @"&#(?<code>\w+);",
			                      m =>
			                      	{
												var symbol = "?";
												var codeString = m.Groups["code"].Value;
			                      		
												var numberStyle = NumberStyles.None;
												if(codeString.StartsWith("x", StringComparison.InvariantCultureIgnoreCase))
												{
													codeString = codeString.Substring(1);
													numberStyle = NumberStyles.HexNumber;
												}

												ushort code;
												if (ushort.TryParse(codeString, numberStyle, CultureInfo.InvariantCulture.NumberFormat, out code))
												{
													symbol = code > byte.MaxValue
													         	? Encoding.Unicode.GetString(new[] { (byte)code, (byte)(code >> 8) })
													         	: Encoding.Default.GetString(new[] { (byte)code });
												}
			                      		return symbol;
			                      	});
			return WebUtility.HtmlDecode(t);
		}
	}
}