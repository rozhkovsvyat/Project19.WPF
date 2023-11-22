using System;

namespace Project_19;

/// <summary>
/// Предоставляет свойства и методы представления
/// </summary>
public interface IView
{
	/// <summary>
	/// Контекст данных
	/// </summary>
	object DataContext { get; set; }

	/// <summary>
	/// Отображает <see cref="IView"/>
	/// </summary>
	/// <param name="onComplete">Действие после выполнения операции</param>
	void Display(Action? onComplete = null);

	/// <summary>
	/// Скрывает <see cref="IView"/>
	/// </summary>
	/// <param name="onComplete">Действие после выполнения операции</param>
	void Hide(Action? onComplete = null);

	/// <summary>
	/// Устанавливает блокировку <see cref="IView"/>
	/// </summary>
	/// <param name="value">Значение блокировки</param>
	void Lock(bool value);
}

/// <summary>
/// Базовая реализация <see cref="IView"/>
/// </summary>
public class DefaultView : IView 
{
	/// <inheritdoc/>
	public object DataContext { get; set; } = new();

	/// <inheritdoc/>
	public void Display(Action? onComplete = null) { }
	/// <inheritdoc/>
	public void Hide(Action? onComplete = null) { }
	/// <inheritdoc/>
	public void Lock(bool value) { }
}
