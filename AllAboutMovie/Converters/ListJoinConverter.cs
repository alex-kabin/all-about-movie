using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AllAboutMovie.Converters
{
	/// <summary>The list to string converter.</summary>
	[ValueConversion(typeof(IEnumerable), typeof(string))]
	public class ListJoinConverter : MarkupExtension, IValueConverter
	{
		/// <summary>Constructor.</summary>
		public ListJoinConverter()
		{
			Separator = ",";
		}

		/// <summary>Separator to use when joining strings.</summary>
		public string Separator { get; set; }

		#region IValueConverter Members
		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value == DependencyProperty.UnsetValue)
			{
				return string.Empty;
			}

			var collection = value as IEnumerable;
			if (collection == null)
			{
				throw new ArgumentException("ListJoinConverter expects IEnumerable value.", "value");
			}

			string format = "{0}";
			if (parameter is string)
				format = parameter as string;

			string[] stringsArray =
				collection.Cast<object>().Select(obj => obj.ToString()).Where(str => !String.IsNullOrEmpty(str)).Select(
					str => String.Format(format, str)).ToArray();

			return string.Join(Separator, stringsArray);
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
		#endregion

		/// <summary>Returns the converter.</summary>
		/// <param name="serviceProvider"></param>
		/// <returns>The provide value.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new ListJoinConverter { Separator = Separator };
		}
	}
}