using System.Collections.Generic;

namespace Project_19;

/// <summary>
/// Предоставляет методы и свойства фабрики элементов <see cref="IValidator"/>
/// </summary>
public interface IValidatorFactory : IFactory<IValidator> { }

/// <summary>
/// Базовая реализация <see cref="IValidatorFactory"/>
/// </summary>
public class DefaultValidatorFactory : IValidatorFactory
{
	/// <inheritdoc/>
	public IValidator Get(string title) => new DefaultValidator();
}

/// <summary>
/// Простая реализация <see cref="IValidatorFactory"/>
/// </summary>
public class SimpleValidatorFactory : IValidatorFactory
{
	/// <summary>
	/// Валидатор по-умолчанию
	/// </summary>
	private readonly IValidator _defaultValidator = new DefaultValidator();
	/// <summary>
	/// Словарь валидаторов
	/// </summary>
	private readonly Dictionary<string, IValidator> _validators;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="validators">Словарь валидаторов</param>
	public SimpleValidatorFactory(Dictionary<string,
		IValidator> validators) => _validators = validators;

	/// <inheritdoc/>
	public IValidator Get(string title) => _validators.TryGetValue
		(title, out var result) ? result : _defaultValidator;
}