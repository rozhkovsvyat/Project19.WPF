using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Linq;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных коллекции <see cref="Role"/>
/// </summary>
public class RolesVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		try
		{
			Source = await _identity.AddToken(Token).GetRolesAsync();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<RolesVm>(arg); }

		catch(Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

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
			//Accounts?.SortDescriptions.Add(new SortDescription(nameof(Role.Name), 
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
	public RolesVm(IVm owner, IIdentity identity, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(RolesVm)), owner) => _identity = identity;
}
