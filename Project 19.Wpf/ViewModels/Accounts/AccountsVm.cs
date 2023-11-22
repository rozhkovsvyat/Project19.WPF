using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Windows.Data;
using System.Linq;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных коллекции <see cref="Account"/>
/// </summary>
public class AccountsVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		try
		{
			Source = await _identity.AddToken(Token).GetAsync();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountsVm>(arg); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Имя текущего пользователя
	/// </summary>
	public string CurrentUserId => User.FindFirst
		(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

	/// <summary>
	/// Упорядоченная коллекция <see cref="Account"/>
	/// </summary>
	public CollectionView? Accounts { get; protected set; }

	/// <summary>
	/// Коллекция <see cref="Account"/>
	/// </summary>
	public IEnumerable<Account> Source
	{
		get => _source;
		private set
		{ 
			_source = value;
			OnPropertyChanged();

			Accounts = (CollectionView)CollectionViewSource.GetDefaultView(_source);
			//Accounts?.SortDescriptions.Add(new SortDescription(nameof(Account.Login), 
			//	ListSortDirection.Ascending));

			OnPropertyChanged(nameof(Accounts));
			OnPropertyChanged(nameof(CurrentUserId));
		}
	}
	private IEnumerable<Account> _source = Enumerable.Empty<Account>();

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public AccountsVm(IVm owner, IIdentity identity, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(AccountsVm)), owner) => _identity = identity;
}
