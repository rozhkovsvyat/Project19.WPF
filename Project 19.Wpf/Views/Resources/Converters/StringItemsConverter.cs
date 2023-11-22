using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: IEnumerable{str} -> str
/// </summary>
[ValueConversion(typeof(IEnumerable<string>), typeof(string))]
public class StringItemsConverter : IValueConverter
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
	    if (value is not List<string> strings) 
		    throw new ArgumentNullException(nameof(value));

	    var result = string.Empty;

	    for (var i = 0; i < strings.Count; i++)
	    {
			if (i > 0) result += Post;
			result += $"{Pre}{strings[i]}";
	    }

	    return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotSupportedException();

	#endregion
}
