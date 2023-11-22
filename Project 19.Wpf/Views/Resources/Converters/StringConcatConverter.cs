using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: str,str,str -> strstrstr
/// </summary>
[ValueConversion(typeof(string), typeof(string))]
public class StringConcatConverter : IValueConverter
{
	/// <summary>
	/// Добавление в начало строки
	/// </summary>
	public string Pre { get; set; } = string.Empty;
	/// <summary>
	/// Добавление в конец строки
	/// </summary>
	public string Post { get; set; } = string.Empty;

	#region IValueConverter

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
	    if (value is not string str) throw new ArgumentNullException(nameof(value));
	    return string.Concat(Pre,str,Post);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion
}
