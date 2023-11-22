using System.Collections.Generic;
using System.Windows.Data;
using System.Linq;
using System;

namespace Project_19;

/// <summary>
/// Конфигуратор-строитель фабрики валидаторов
/// </summary>
public class ValidatorFactoryConfigurator
{
	/// <summary>
	/// Фабрика валидаторов
	/// </summary>
	private IValidatorFactory _factory;

	/// <summary>
	/// Опции валидаторов
	/// </summary>
	private Dictionary<ValidationType, DependencyBag> _options = new();

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="factory">Фабрика валидаторов</param>
	public ValidatorFactoryConfigurator(IValidatorFactory factory) => _factory = factory;

	/// <summary>
	/// Возвращает сконфигурированную фабрику валидаторов
	/// </summary>
	/// <returns>Фабрика валидаторов</returns>
	public IValidatorFactory BuildConfigured()
	{
		var validators = new Dictionary<string, IValidator>();
		var types = Enum.GetValues(typeof(ValidationType))
			.Cast<ValidationType>().ToArray();

		foreach (var type in types)
		{
			var validator = _factory.Get(type.ToString());
			if (validator is DefaultValidator) continue;

			if (_options.TryGetValue(type, out var options)) 
				validator.Options = options;

			validators.Add(type.ToString(), validator);
		}
		return new SimpleValidatorFactory(validators);
	}

	/// <summary>
	/// Задает фабрику валидаторов
	/// </summary>
	/// <param name="factory">Фабрика валидаторов</param>
	public ValidatorFactoryConfigurator SetFactory(IValidatorFactory factory)
	{
		_factory = factory;
		return this;
	}

	/// <summary>
	/// Конфигурирует опции валидаторов
	/// </summary>
	/// <param name="type">Тип валидатора</param>
	/// <param name="option">Опции валидатора</param>
	public ValidatorFactoryConfigurator AddOption(ValidationType type, DependencyBag option)
	{
		_options.TryAdd(type, option);
		return this;
	}

	/// <summary>
	/// Конфигурирует опции валидаторов
	/// </summary>
	/// <param name="type">Тип валидатора</param>
	public ValidatorFactoryConfigurator RemoveOption(ValidationType type)
	{
		_options.Remove(type);
		return this;
	}

	/// <summary>
	/// Конфигурирует опции валидаторов
	/// </summary>
	/// <param name="options">Опции валидаторов</param>
	public ValidatorFactoryConfigurator AddOptions(IEnumerable<KeyValuePair<ValidationType, DependencyBag>> options)
	{
		foreach (var o in options) _options.TryAdd(o.Key, o.Value);
		return this;
	}

	/// <summary>
	/// Конфигурирует опции валидаторов
	/// </summary>
	/// <param name="options">Опции валидаторов</param>
	public ValidatorFactoryConfigurator AddOptions(Dictionary<ValidationType, DependencyBag> options)
	{
		_options = options;
		return this;
	}

	/// <summary>
	/// Конфигурирует опции валидаторов
	/// </summary>
	public ValidatorFactoryConfigurator RemoveOptions()
	{
		_options.Clear();
		return this;
	}
}
