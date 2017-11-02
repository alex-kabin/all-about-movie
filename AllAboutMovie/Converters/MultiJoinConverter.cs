using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AllAboutMovie.Converters
{
	/// <summary>The multi join converter.</summary>
	public class MultiJoinConverter : IMultiValueConverter
	{
		/// <summary>Initializes a new instance of the <see cref="MultiJoinConverter"/> class.</summary>
		public MultiJoinConverter()
		{
			Separator = ",";
		}

		/// <summary>Gets or sets Separator.</summary>
		public string Separator { get; set; }

		#region IMultiValueConverter Members
		/// <summary>The convert.</summary>
		/// <param name="values">The values.</param>
		/// <param name="targetType">The target type.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>The convert.</returns>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values == DependencyProperty.UnsetValue)
			{
				return string.Empty;
			}

			string format = "{0}";
			if (parameter is string)
			{
				format = parameter as string;
			}

			string[] stringsArray =
				values.Select(obj => obj.ToString()).Where(str => !String.IsNullOrEmpty(str))
					.Select(str => String.Format(format, str)).ToArray();

			return string.Join(Separator, stringsArray);
		}

		/// <summary>The convert back.</summary>
		/// <param name="value">The value.</param>
		/// <param name="targetTypes">The target types.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="culture">The culture.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}