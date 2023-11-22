using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных деталей элемента типа <see cref="Models.Contact"/>
/// </summary>
public class ContactDetailsVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (User.Identity is not { IsAuthenticated: true }) return await NotFoundAsync();

		if (arg is not int id) return await NotFoundAsync();

		try
		{
			Contact = await _contacts.AddToken(Token).GetAsync(id) ?? throw new KeyNotFoundException();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<ContactDetailsVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Поставщик модели контактов
	/// </summary>
	private readonly IContacts _contacts;

	/// <summary>
	/// Элемент типа <see cref="Models.Contact"/>
	/// </summary>
	public Contact Contact
	{
		get => _contact ??= new Contact();
		private set
		{
			_contact = value;
			OnPropertyChanged();
		}
	}
	private Contact? _contact;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="contacts">Поставщик модели контактов</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public ContactDetailsVm(IVm owner, IContacts contacts, IViewFactory viewFactory)
		: base(viewFactory.Get(nameof(ContactDetailsVm)), owner) => _contacts = contacts;
}
