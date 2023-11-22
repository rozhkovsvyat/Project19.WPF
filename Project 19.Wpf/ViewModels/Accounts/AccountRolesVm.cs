using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Linq;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных коллекции <see cref="Role"/> элемента типа <see cref="Account"/>
/// </summary>
public class AccountRolesVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		if (arg is not string id) return await NotFoundAsync();

		try
		{
			Account = await _identity.AddToken(Token).GetByIdAsync(id) ?? throw new KeyNotFoundException();
			Source = await _identity.AddToken(Token).GetRolesAsync(Account.Login);
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountRolesVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Элемент типа <see cref="Account"/>
	/// </summary>
	public Account Account
	{
		get => _account ??= new Account();
		private set
		{
			_account = value;
			OnPropertyChanged();
		}
	}
	private Account? _account;

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

	/// <summary>
	/// Упорядоченная коллекция <see cref="Role"/>
	/// </summary>
	public CollectionView? Roles { get; protected set; }

	/// <summary>
	/// Коллекция <see cref="Role"/>
	/// </summary>
	public IEnumerable<Role> Source
	{
		get => _source;
		private set
		{ 
			_source = value;
			OnPropertyChanged();

			Roles = (CollectionView)CollectionViewSource.GetDefaultView(_source);
			//Accounts?.SortDescriptions.Add(new SortDescription(nameof(Account.Login), 
			//	ListSortDirection.Ascending));

			OnPropertyChanged(nameof(Roles));
		}
	}
	private IEnumerable<Role> _source = Enumerable.Empty<Role>();

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public AccountRolesVm(IVm owner, IIdentity identity, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(AccountRolesVm)), owner) => _identity = identity;
}
