using System.Security.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using System;

namespace Project_19.ViewModels;

/// <summary>
/// Абстрактный класс контекста данных
/// </summary>
public abstract class Vm : PropertyChangedNotifier, IVm
{
	#region IVm

	/// <inheritdoc/>
	public IView View
	{
		get => _view;
		protected set
		{
			_view = value;
			_view.DataContext = this;
			OnPropertyChanged();
		}
	}
	private IView _view = null!;
	/// <inheritdoc/>
	public virtual ClaimsPrincipal User => _user 
		??= Owner?.User ?? new ClaimsPrincipal();
	private ClaimsPrincipal? _user;
	/// <inheritdoc/>
	public IVm? Owner { get; }
	/// <inheritdoc/>
	public virtual string? Token => Owner?.Token;

	/// <inheritdoc/>
	public virtual async Task<IVm> AuthorizeAsync<T>(object? arg = null) where T : IVm
		=> Owner is null ? throw new AuthenticationException()
			: await Owner.AuthorizeAsync<T>(arg);
	/// <inheritdoc/>
	public virtual void Authorize<T>(object? arg = null) where T : IVm
	{
		if (Owner is null) throw new AuthenticationException();
		Owner.Authorize<T>(arg);
	}
	/// <inheritdoc/>
	public virtual async Task<IVm> NotFoundAsync(object? arg = null)
		=> Owner is null ? throw new KeyNotFoundException()
			: await Owner.NotFoundAsync();

	/// <inheritdoc/>
	public virtual void NotFound(object? arg = null)
	{
		if (Owner is null) throw new KeyNotFoundException();
		Owner.NotFound(arg);
	}

	/// <inheritdoc/>
	public virtual async Task<IVm> ServerErrorAsync(object? arg = null) 
		=> Owner is null ? throw new InvalidOperationException() 
			: await Owner.ServerErrorAsync();

	/// <inheritdoc/>
	public virtual void ServerError(object? arg = null)
	{
		if (Owner is null) throw new InvalidOperationException();
		Owner.ServerError(arg);
	}

	/// <inheritdoc/>
	public virtual async Task<IVm> ConnectionErrorAsync(object? arg = null)
		=> Owner is null ? throw new TimeoutException()
			: await Owner.ConnectionErrorAsync();

	/// <inheritdoc/>
	public virtual void ConnectionError(object? arg = null)
	{
		if (Owner is null) throw new TimeoutException();
		Owner.ConnectionError(arg);
	}

	/// <inheritdoc/>
	public virtual async Task<IVm> InitializedAsync(object? arg = null) 
		=> await (this as IVm).Display().Async();

	#endregion

	/// <inheritdoc cref="Project_19.App"/>
	protected static Application App => Application.Current;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="view">Представление <see cref="IView"/> текущего контекста данных</param>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	protected Vm(IView view, IVm? owner = null)
	{
		View = view;
		Owner = owner;
	}

	/// <summary>
	///  Открывает веб-страницу
	/// </summary>
	public Cmd LoadUrlCmd => _loadUrlCmd 
		??= new Cmd(o => 
		{
			if (o is not string url) return;

			var result = Uri.TryCreate
				(url, UriKind.Absolute, out var uriResult);

			if (!result || uriResult == null) return;

			if (uriResult.Scheme != Uri.UriSchemeHttp &&
			    uriResult.Scheme != Uri.UriSchemeHttps) return;

			Process.Start(new ProcessStartInfo
				{ UseShellExecute = true, FileName = url });
		});
	private Cmd? _loadUrlCmd;
}
