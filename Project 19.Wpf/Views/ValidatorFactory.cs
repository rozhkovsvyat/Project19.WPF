using System.Collections.Generic;
using Tools;

namespace Project_19.Views;

/// <summary>
/// Фабрика элементов <see cref="IValidator"/>
/// </summary>
public class ValidatorFactory : RecipeFactory<IValidator>, IValidatorFactory
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ValidatorFactory() : base(() => new DefaultValidator()) { }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="recipes">Коллекция именованных рецептов</param>
	public ValidatorFactory(IEnumerable<KeyValuePair<string, Recipe<IValidator>>> recipes) : this()
	{
		foreach (var recipe in recipes) 
			AddRecipe(recipe.Value, recipe.Key);
	}
}
