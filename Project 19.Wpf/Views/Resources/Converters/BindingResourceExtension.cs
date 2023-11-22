using System.Windows.Data;
using System.Windows;
using System;

namespace Project_19.Views;

/// <summary>
/// Предоставляет возможность использования <see cref="Binding"/> в качестве ресурса
/// </summary>
public class BindingResourceExtension : StaticResourceExtension
{
	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="resourceKey">Ключ ресурса</param>
	public BindingResourceExtension(object resourceKey) : base(resourceKey) { }

	/// <summary>
	/// Возвращает объект, который следует задать для свойства, в котором применяются эта привязка и это расширение
	/// </summary>
	/// <exception cref="InvalidCastException"></exception>
	public override object ProvideValue(IServiceProvider serviceProvider) 
		=> base.ProvideValue(serviceProvider) is not BindingBase binding 
			? throw new InvalidCastException(nameof(binding)) 
			: binding.ProvideValue(serviceProvider);
}
