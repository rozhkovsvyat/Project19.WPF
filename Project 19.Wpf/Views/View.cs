using System.Windows.Media.Animation;
using System.Windows.Interop;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System;

namespace Project_19.Views;

/// <summary>
/// Абстрактный класс представления
/// </summary>
public abstract class View : Window, IView
{
	#region IView

	/// <summary>
	/// Длительность анимации проявления
	/// </summary>
	protected virtual TimeSpan FadeSpan => TimeSpan.FromMilliseconds(150);

	/// <inheritdoc/>
	public void Display(Action? onComplete = null)
	{
		Show();
		var fading = new DoubleAnimation(1, FadeSpan);
		fading.Completed += (_, _) => onComplete?.Invoke();
		BeginAnimation(OpacityProperty, fading);
	}
	/// <inheritdoc/>
	public void Hide(Action? onComplete = null)
	{
		var fading = new DoubleAnimation(uint.MinValue, FadeSpan);
		fading.Completed += (_,_) => onComplete?.Invoke();
		BeginAnimation(OpacityProperty, fading);
	}
	/// <inheritdoc/>
	public void Lock(bool value)
	{
		if (value)
		{
			CursorBag.Put(Cursor, this);
			Cursor = Cursors.Wait;
			OnUnlock += Unlock;
			void Unlock()
			{
				OnUnlock -= Unlock;
				if (CursorBag.TryRetrieve(out var cursor, 
					    this)) Cursor = cursor;
			}
		}
		else OnUnlock?.Invoke();
		IsLocked = value;
	}
	protected Action? OnUnlock;

	#endregion

	/// <inheritdoc cref="HwndResizer"/>
	protected HwndResizer Resizer = null!;

	/// <summary>
	/// Флаг полноэкранного режима
	/// </summary>
	public static readonly DependencyProperty 
		FullscreenProperty = DependencyProperty.Register
			(nameof(Fullscreen), typeof(bool), typeof(View));
	/// <inheritdoc cref="FullscreenProperty"/>
	public bool Fullscreen
	{
		get => (bool)GetValue(FullscreenProperty);
		set => SetValue(FullscreenProperty, value);
	}

	/// <summary>
	/// Флаг нахождения в меню
	/// </summary>
	public static readonly DependencyProperty
		InMenuProperty = DependencyProperty.Register
			(nameof(InMenu), typeof(bool), typeof(View));
	/// <inheritdoc cref="InMenuProperty"/>
	public bool InMenu
	{
		get => (bool)GetValue(InMenuProperty);
		set => SetValue(InMenuProperty, value);
	}

	/// <summary>
	/// Флаг нахождения в блокировке
	/// </summary>
	public static readonly DependencyProperty
		IsLockedProperty = DependencyProperty.Register
			(nameof(IsLocked), typeof(bool), typeof(View));
	/// <inheritdoc cref="IsLockedProperty"/>
	public bool IsLocked
	{
		get => (bool)GetValue(IsLockedProperty);
		set => SetValue(IsLockedProperty, value);
	}

	/// <summary>
	/// Пустой конструктор
	/// </summary>
	protected View() => Opacity = uint.MinValue;

	/// <summary>
	/// Вызывается при инициализации представления
	/// </summary>
	protected virtual void OnSourceInitialized(object? sender, EventArgs e) 
		=> Resizer = new HwndResizer(sender.ToHwnd());

	/// <summary>
	/// Вызывается при изменении размера представления
	/// </summary>
	protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e) 
		=> Fullscreen = WindowState == WindowState.Maximized;

	/// <summary>
	/// Вызывается при закрытии представления
	/// </summary>
	protected virtual void OnClosing(object? sender, CancelEventArgs e)
	{
		e.Cancel = true;
		Closing -= OnClosing;
		Hide(Close);
	}

	/// <summary>
	/// Устанавливает новый курсор на время перетаскивания
	/// </summary>
	/// <param name="newCursor">Новый курсор</param>
	protected virtual void SetCursorOnDrag(Cursor newCursor)
	{
		CursorBag.Put(Mouse.OverrideCursor, this);
		Mouse.OverrideCursor = newCursor;
		MouseMove += ResetCursor;

		void ResetCursor(object? s, MouseEventArgs args) 
		{
			if (args.LeftButton == 
			    MouseButtonState.Pressed) return;
			MouseMove -= ResetCursor;
			if (CursorBag.TryRetrieve(out var cursor, this))
				Mouse.OverrideCursor = cursor;
		}
	}

	#region Cmd

	/// <summary>
	/// Закрывает представление
	/// </summary>
	protected void Close(object? sender, RoutedEventArgs? e = null) 
		=> Close();
	/// <inheritdoc cref="Close"/>
	public Cmd CloseCmd => _closeCmd 
		??= new Cmd(o => Close(o));
	private Cmd? _closeCmd;

	/// <summary>
	/// Сворачивает представление
	/// </summary>
	protected void Minimize(object? sender, RoutedEventArgs? e = null) 
		=> WindowState = WindowState.Minimized;
	/// <inheritdoc cref="Minimize"/>
	public Cmd MinimizeCmd => _minimizeCmd 
		??= new Cmd(o => Minimize(o));
	private Cmd? _minimizeCmd;

	/// <summary>
	/// Раскрывает представление на весь экран
	/// </summary>
	protected void Maximize(object? sender, RoutedEventArgs? e = null)
	{
		switch (WindowState)
		{
			case WindowState.Normal:
				WindowState = WindowState.Maximized;
				break;
			case WindowState.Maximized:
				WindowState = WindowState.Normal;
				break;
			case WindowState.Minimized:
			default: break;
		}
	}
	/// <inheritdoc cref="Maximize"/>
	public Cmd MaximizeCmd => _maximizeCmd 
		??= new Cmd(o => Maximize(o));
	private Cmd? _maximizeCmd;

	/// <summary>
	/// Перемещает представление
	/// </summary>
	protected void Drag(object? sender, MouseButtonEventArgs? e = null)
	{
		if (WindowState != WindowState.Normal) return;
		SetCursorOnDrag(Cursors.SizeAll);
		DragMove();
	}
	/// <inheritdoc cref="Drag"/>
	public Cmd DragCmd => _dragCmd
		??= new Cmd(o => Drag(o));
	private Cmd? _dragCmd;

	/// <summary>
	/// Масштабирует представление
	/// </summary>
	protected void Resize(object? sender, MouseEventArgs? e = null)
	{
		if (sender is not Shape clickedShape 
		    || WindowState != WindowState.Normal) return;

		SetCursorOnDrag(clickedShape.Cursor);
		Resizer.Resize(clickedShape.Name switch
		{
			"ResizeN" => HwndResizeCode.Top,
			"ResizeE" => HwndResizeCode.Right,
			"ResizeS" => HwndResizeCode.Bottom,
			"ResizeW" => HwndResizeCode.Left,
			"ResizeNw" => HwndResizeCode.TopLeft,
			"ResizeNe" => HwndResizeCode.TopRight,
			"ResizeSe" => HwndResizeCode.BottomRight,
			"ResizeSw" => HwndResizeCode.BottomLeft,
			_ => null
		});
	}
	/// <inheritdoc cref="Resize"/>
	public Cmd ResizeCmd => _resizeCmd
		??= new Cmd(o => Resize(o));
	private Cmd? _resizeCmd;

	/// <summary>
	/// Устанавливает флаг нахождения в меню
	/// </summary>
	protected void SetMenu(object? sender, MouseEventArgs? e = null) 
		=> InMenu = !InMenu;
	/// <inheritdoc cref="SetMenu"/>
	public Cmd SetMenuCmd => _setMenuCmd 
		??= new Cmd(o => SetMenu(o));
	private Cmd? _setMenuCmd;

	/// <summary>
	/// Сбрасывает флаг нахождения в меню
	/// </summary>
	protected void ResetMenu(object? sender, MouseEventArgs? e = null) 
		=> InMenu = false;
	/// <inheritdoc cref="ResetMenu"/>
	public Cmd ResetMenuCmd => _resetMenuCmd 
		??= new Cmd(o => ResetMenu(o));
	private Cmd? _resetMenuCmd;

	#endregion
}
