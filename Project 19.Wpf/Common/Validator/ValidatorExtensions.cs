using System.Windows.Controls;
using System.Linq;

namespace Project_19;

/// <summary>
/// Содержит перечисление типов валидации
/// </summary>
public enum ValidationType
{
	/// <summary>
	/// Валидация обязательного значения
	/// </summary>
	Required,
	/// <summary>
	/// Валидация сопоставления
	/// </summary>
	Compared, 
	/// <summary>
	/// Валидация электронной почты
	/// </summary>
	Email
}

/// <summary>
/// Содержит методы расширения <see cref="IValidator"/>
/// </summary>
public static class ValidatorExtensions
{
	/// <summary>
	/// Выполняет проверку значения
	/// </summary>
	/// <param name="value">Значение</param>
	/// <param name="factory">Фабрика валидаторов</param>
	/// <param name="types">Типы валидации</param>
	/// <returns></returns>
	public static bool Validate(this object value, IValidatorFactory factory, params ValidationType[] types) 
		=> types.Length == 0 || types.All(type => factory.Get(type).Validate(value, null) == ValidationResult.ValidResult);

	/// <summary>
	/// Возвращает валидатор указанного типа или валидатор по-умолчанию
	/// </summary>
	/// <param name="factory">Фабрика валидаторов</param>
	/// <param name="type">Тип валидации</param>
	/// <returns>Валидатор <see cref="IValidator"/></returns>
	public static IValidator Get(this IValidatorFactory factory, ValidationType type) 
		=> factory.Get(type.ToString());
}
