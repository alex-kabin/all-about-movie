using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllAboutMovie.Utils
{
	public static class TranslitHelper
	{
		private static readonly IList<KeyValuePair<string, string>> map =
			new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("А", "A"),
					new KeyValuePair<string, string>("а", "a"),
					new KeyValuePair<string, string>("Б", "B"),
					new KeyValuePair<string, string>("б", "b"),
					new KeyValuePair<string, string>("В", "V"),
					new KeyValuePair<string, string>("в", "v"),
					new KeyValuePair<string, string>("Г", "G"),
					new KeyValuePair<string, string>("г", "g"),
					new KeyValuePair<string, string>("Д", "D"),
					new KeyValuePair<string, string>("д", "d"),
					new KeyValuePair<string, string>("Е", "E"),
					new KeyValuePair<string, string>("е", "e"),
					new KeyValuePair<string, string>("Ё", "Yo"),
					new KeyValuePair<string, string>("ё", "yo"),
					new KeyValuePair<string, string>("E", "Jo"),
					new KeyValuePair<string, string>("ё", "jo"),
					new KeyValuePair<string, string>("Ж", "Zh"),
					new KeyValuePair<string, string>("ж", "zh"),
					new KeyValuePair<string, string>("З", "Z"),
					new KeyValuePair<string, string>("з", "z"),
					new KeyValuePair<string, string>("И", "I"),
					new KeyValuePair<string, string>("и", "i"),
					new KeyValuePair<string, string>("Й", "J"),
					new KeyValuePair<string, string>("й", "j"),
					new KeyValuePair<string, string>("К", "K"),
					new KeyValuePair<string, string>("к", "k"),
					new KeyValuePair<string, string>("Л", "L"),
					new KeyValuePair<string, string>("л", "l"),
					new KeyValuePair<string, string>("М", "M"),
					new KeyValuePair<string, string>("м", "m"),
					new KeyValuePair<string, string>("Н", "N"),
					new KeyValuePair<string, string>("н", "n"),
					new KeyValuePair<string, string>("О", "O"),
					new KeyValuePair<string, string>("о", "o"),
					new KeyValuePair<string, string>("П", "P"),
					new KeyValuePair<string, string>("п", "p"),
					new KeyValuePair<string, string>("Р", "R"),
					new KeyValuePair<string, string>("р", "r"),
					new KeyValuePair<string, string>("С", "S"),
					new KeyValuePair<string, string>("с", "s"),
					new KeyValuePair<string, string>("Т", "T"),
					new KeyValuePair<string, string>("т", "t"),
					new KeyValuePair<string, string>("У", "U"),
					new KeyValuePair<string, string>("у", "u"),
					new KeyValuePair<string, string>("Ф", "F"),
					new KeyValuePair<string, string>("ф", "f"),
					new KeyValuePair<string, string>("Х", "H"),
					new KeyValuePair<string, string>("х", "h"),
					new KeyValuePair<string, string>("Ц", "C"),
					new KeyValuePair<string, string>("ц", "c"),
					new KeyValuePair<string, string>("Ч", "Ch"),
					new KeyValuePair<string, string>("ч", "ch"),
					new KeyValuePair<string, string>("Ш", "Sh"),
					new KeyValuePair<string, string>("ш", "sh"),
					new KeyValuePair<string, string>("Щ", "Sch"),
					new KeyValuePair<string, string>("щ", "sch"),
					new KeyValuePair<string, string>("Ы", "Y"),
					new KeyValuePair<string, string>("ы", "y"),
					new KeyValuePair<string, string>("ь", "'"),
					new KeyValuePair<string, string>("Э", "E"),
					new KeyValuePair<string, string>("э", "e"),
					new KeyValuePair<string, string>("Ю", "Yu"),
					new KeyValuePair<string, string>("ю", "yu"),
					new KeyValuePair<string, string>("Ю", "Ju"),
					new KeyValuePair<string, string>("ю", "ju"),
					new KeyValuePair<string, string>("Я", "Ya"),
					new KeyValuePair<string, string>("я", "ya"),
					new KeyValuePair<string, string>("Я", "Ja"),
					new KeyValuePair<string, string>("я", "ja"),
					new KeyValuePair<string, string>("ье", "je"),
					new KeyValuePair<string, string>("ий", "iy"),
					new KeyValuePair<string, string>("ий", "ii"),
					new KeyValuePair<string, string>("ый", "yj"),
					new KeyValuePair<string, string>("ые", "ye"),
					new KeyValuePair<string, string>("ей", "ey"),
					new KeyValuePair<string, string>("ью", "ew"),
				};

		//public static string RusToEng(string text)
		//{
		//    StringBuilder output = new StringBuilder(text);
		//    foreach (KeyValuePair<string, string> key in map)
		//    {
		//        output.Replace(key.Key, key.Value);
		//    }
		//    return output.ToString();
		//}

		private static IList<KeyValuePair<string, string>> _sortedMap;

		private static IList<KeyValuePair<string, string>> SortedMap
		{
			get { return _sortedMap ?? (_sortedMap = map.OrderByDescending(kv => kv.Value.Length).ToList()); }
		}

		public static string EngToRus(string text)
		{
			var output = new StringBuilder(text);
			output = SortedMap.Aggregate(output, (current, key) => current.Replace(key.Value, key.Key));
			return output.ToString();
		}
	}
}