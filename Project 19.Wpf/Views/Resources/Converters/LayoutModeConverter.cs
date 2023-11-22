using System.Windows.Data;
using System;

namespace Project_19.Views;

/// <summary>
/// Конвертер: width (row/columns) -> int (rows/columns count)
/// </summary>
public class LayoutModeConverter : IValueConverter
{
	/// <summary>
	/// Возвращает конвертер в режиме ряда
	/// </summary>
	public static readonly LayoutModeConverter Row = new() { RowMode = true };

	/// <summary>
	/// Возвращает конвертер в режиме столбца
	/// </summary>
	public static readonly LayoutModeConverter Column = new() { RowMode = false };

	/// <summary>
	/// Флаг режима ряда
	/// </summary>
	public bool RowMode { get; set; }

	#region IValueConverter

	public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	{
		var width = System.Convert.ToDouble(value);
		var targetWidth = System.Convert.ToDouble(parameter);
		if (RowMode) return width > targetWidth ? 1 : 2;
		return width > targetWidth ? 2 : 1;
	}

	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		=> throw new NotImplementedException();

	#endregion
}
