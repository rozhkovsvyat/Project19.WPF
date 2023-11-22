using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: brush x color
/// </summary>
[ValueConversion(typeof(Brush), typeof(Color))]
public class BrushToColorConverter : IValueConverter
{
	#region IValueConverter

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
	    if (value is not Brush b) throw new ArgumentNullException(nameof(value));
	    return ((SolidColorBrush)b).Color;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion
}
