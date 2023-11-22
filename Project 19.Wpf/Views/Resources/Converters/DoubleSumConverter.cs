using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: double + double -> double
/// </summary>
public class DoubleSumConverter : IValueConverter
{
	/// <summary>
	/// Флаг отрицания параметра
	/// </summary>
	public bool Negate { get; set; } = false;

	#region IValueConverter

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not double d) throw new ArgumentNullException(nameof(value));
		if (parameter is not double p) p = 0;
		return d + (Negate ? -1 * p : p);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion

}