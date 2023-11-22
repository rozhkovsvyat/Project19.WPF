using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: bool x !bool
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public class BooleanInverter : IValueConverter
{
    #region IValueConverter

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    { 
        if (value == null) throw new ArgumentNullException(nameof(value));

		if (targetType != typeof(bool) && targetType != typeof(bool?)) 
            throw new InvalidOperationException("The target must be a boolean"); 

        return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        => Convert(value, targetType, parameter, culture);

    #endregion
}
