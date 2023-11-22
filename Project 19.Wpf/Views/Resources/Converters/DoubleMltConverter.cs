using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: double / double -> double
/// </summary>
public class DoubleMltConverter : IValueConverter
{
	#region IValueConverter

	/// <inheritdoc/>
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not double d) throw new ArgumentNullException(nameof(value));
		if (!double.TryParse(parameter?.ToString()?.Replace(".",","), 
			    out var p)) p = 1;
		return d * p;
	}

	/// <inheritdoc/>
	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion

}	