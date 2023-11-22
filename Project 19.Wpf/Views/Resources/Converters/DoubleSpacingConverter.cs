using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер отступа: (condition) -> double + double -> double
/// </summary>
public class DoubleSpacingConverter : IValueConverter
{
	/// <summary>
	/// Значение отступа по-умолчанию
	/// </summary>
	public double Step { get; set; } = 1;

	#region IValueConverter

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not double d) throw new ArgumentNullException(nameof(value));
		if (parameter is not double p) p = 0;
		return d < p ? p : p + (d - p) * Step;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion

}