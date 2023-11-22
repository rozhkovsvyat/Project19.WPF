using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных деталей элемента типа <see cref="Models.Role"/>
/// </summary>
public class RoleDetailsVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		if (arg is not string id) return await NotFoundAsync();

		try
		{
			Role = await _identity.AddToken(Token).GetRoleByIdAsync(id) ?? throw new KeyNotFoundException();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<RoleDetailsVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

	/// <summary>
	/// Элемент типа <see cref="Models.Role"/>
	/// </summary>
	public Role Role
	{
		get => _role ??= new Role();
		private set
		{
			_role = value;
			OnPropertyChanged();
		}
	}
	private Role? _role;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public RoleDetailsVm(IVm owner, IIdentity identity, IViewFactory viewFactory)
		: base(viewFactory.Get(nameof(RoleDetailsVm)), owner) => _identity = identity;
}
