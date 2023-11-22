using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System;

namespace Project_19;

/// <summary>
/// Предоставляет свойства и методы валидации
/// </summary>
public interface IValidator
{
	/// <summary>
	/// Название проверяемого поля
	/// </summary>
	string Field => string.IsNullOrEmpty(Options.Title) ? nameof(Field) : Options.Title;

	/// <summary>
	/// Возвращает отрицательный результат проверки
	/// </summary>
	/// <returns>Отрицательный <see cref="ValidationResult"/></returns>
	ValidationResult InvalidResult => new(false, ErrorContent);

	/// <summary>
	/// Набор зависимых свойств
	/// </summary>
	DependencyBag Options { get; set; }

	/// <summary>
	/// Содержание ошибки валидации
	/// </summary>
	string ErrorContent { get; }

	/// <summary>
	/// Выполняет проверку
	/// </summary>
	/// <param name="value">Значение</param>
	/// <param name="cultureInfo"><see cref="CultureInfo"/></param>
	/// <returns>Результат проверки <see cref="ValidationResult"/></returns>
	ValidationResult Validate(object value, CultureInfo? cultureInfo);

	/// <summary>
	/// Конфигурирует валидатор
	/// </summary>
	/// <param name="options">Опции валидатора</param>
	/// <returns>Валидатор <see cref="IValidator"/></returns>
	IValidator Configure(DependencyBag options)
	{
		Options = options;
		return this;
	}
}

/// <summary>
/// Базовая реализация <see cref="IValidator"/>
/// </summary>
public class DefaultValidator : IValidator
{
	#region IValidator

	/// <inheritdoc/>
	public DependencyBag Options { get; set; } = new();
	/// <inheritdoc/>
	public string ErrorContent { get; set; } = string.Empty;

	/// <inheritdoc/>
	public ValidationResult Validate(object value, 
		CultureInfo? cultureInfo) => OnValidate.Invoke(value, cultureInfo);

	#endregion

	/// <summary>
	/// Функция валидации
	/// </summary>
	public Func<object, CultureInfo?, ValidationResult> OnValidate { get; set; } 
		= (_, _) => ValidationResult.ValidResult;
}
