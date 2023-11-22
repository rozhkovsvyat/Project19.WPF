using System.Collections.Generic;
using Tools;

namespace Project_19.ViewModels;

/// <summary>
/// Фабрика элементов <see cref="IVm"/>
/// </summary>
public class VmFactory : RecipeFactory<IVm>, IVmFactory
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public VmFactory() : base(() => new DefaultVm()) { }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="recipes">Коллекция именованных рецептов</param>
	public VmFactory(IEnumerable<KeyValuePair<string, Recipe<IVm>>> recipes) : this()
	{ foreach (var recipe in recipes) AddRecipe(recipe.Value, recipe.Key); }
}
