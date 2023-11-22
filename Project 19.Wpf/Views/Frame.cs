using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace Project_19.Views;

/// <summary>
/// Абстрактный класс вложенного представления
/// </summary>
public abstract class Frame : Page, IView
{
	#region IView

	/// <inheritdoc/>
	public void Display(Action? onComplete = null)
	{
		var fading = new DoubleAnimation(1, FadeSpan);
		fading.Completed += (_, _) => onComplete?.Invoke();
		BeginAnimation(OpacityProperty, fading);
		Focus();
	}
	/// <inheritdoc/>
	public void Hide(Action? onComplete = null)
	{
		Opacity = uint.MinValue;
		onComplete?.Invoke();
	}
	/// <inheritdoc/>
	public void Lock(bool value)
	{
		if (value)
		{
			CursorBag.Put(Mouse.OverrideCursor, 
				this, nameof(Frame));
			Mouse.OverrideCursor = Cursors.AppStarting;
			OnUnlock += Unlock;

			void Unlock() 
			{
				OnUnlock -= Unlock;
				if (CursorBag.TryRetrieve(out var cursor, 
					    this, nameof(Frame))) 
					Mouse.OverrideCursor = cursor;
			}
		}
		else OnUnlock?.Invoke();

	}

	#endregion

	/// <summary>
	/// Длительность анимации проявления
	/// </summary>
	protected virtual TimeSpan FadeSpan => TimeSpan.FromMilliseconds(150);
	/// <summary>
	/// Действие при разблокировке <see cref="IView"/>
	/// </summary>
	protected Action? OnUnlock;

	/// <summary>
	/// Событие обновления <see cref="Frame"/>
	/// </summary>
	protected Action? OnUpdate { get; set; }

	/// <summary>
	/// Пустой конструктор
	/// </summary>
	protected Frame()
	{
		Opacity = uint.MinValue;
		FocusVisualStyle = null;
		Focusable = true;
	}

	#region Cmd

	/// <summary>
	/// Вызывает событие <see cref="OnUpdate"/>
	/// </summary>
	public virtual Cmd UpdateCmd
		=> _updateCmd ??= new Cmd(_ => OnUpdate?.Invoke());
	private Cmd? _updateCmd;

	#endregion
}
