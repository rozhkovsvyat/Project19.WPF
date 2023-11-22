using System.Collections.Generic;
using Tools;

namespace Project_19.Views;

/// <summary>
/// Фабрика элементов <see cref="IView"/>
/// </summary>
public class ViewFactory : RecipeFactory<IView>, IViewFactory
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ViewFactory() : base(() => new DefaultView()) { }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="recipes">Коллекция именованных рецептов</param>
	public ViewFactory(IEnumerable<KeyValuePair<string, Recipe<IView>>> recipes) : this()
	{
		foreach (var recipe in recipes) 
			AddRecipe(recipe.Value, recipe.Key);
	}
}
