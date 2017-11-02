using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AllAboutMovie.Converters
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class EmptyToVisibilityConverter : IValueConverter
	{
		/// <summary>Constructor.</summary>
		public EmptyToVisibilityConverter()
		{
			EmptyVisibility = Visibility.Collapsed;
			NonEmptyVisibility = Visibility.Visible;
		}

		public Visibility EmptyVisibility { get; set; }
		public Visibility NonEmptyVisibility { get; set; }
		
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
				return EmptyVisibility;
			}

			var strValue = value as String;
			if (strValue != null && String.IsNullOrWhiteSpace(strValue))
				return EmptyVisibility;

			var listValue = value as ICollection;
			if(listValue != null && listValue.Count == 0)
				return EmptyVisibility;

			return NonEmptyVisibility;
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
	}
}