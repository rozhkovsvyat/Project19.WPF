using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: value == value
/// </summary>
public class EqualityConverter : IMultiValueConverter
{
	#region IMultiValueConverter

	/// <inheritdoc/>
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		=> values.Length >= 2 && values[0].Equals(values[1]);

	/// <inheritdoc/>
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();

	#endregion
}
