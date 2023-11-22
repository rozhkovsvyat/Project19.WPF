using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных коллекции <see cref="Contact"/>
/// </summary>
public class ContactsVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		try
		{
			Source = await _contacts.GetAsync();
			return await base.InitializedAsync();
		}

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Поставщик модели контактов
	/// </summary>
	private readonly IContacts _contacts;

	/// <summary>
	/// Упорядоченная коллекция <see cref="Contact"/>
	/// </summary>
	public CollectionView? Contacts { get; protected set; }

	/// <summary>
	/// Коллекция <see cref="Contact"/>
	/// </summary>
	public IEnumerable<Contact> Source
	{
		get => _source;
		private set
		{ 
			_source = value;
			OnPropertyChanged();

			Contacts = (CollectionView)CollectionViewSource.GetDefaultView(_source);
			Contacts?.SortDescriptions.Add(new SortDescription(nameof(Contact.Id), 
				ListSortDirection.Ascending));

			OnPropertyChanged(nameof(Contacts));
		}
	}
	private IEnumerable<Contact> _source = Enumerable.Empty<Contact>();

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="contacts">Поставщик модели контактов</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public ContactsVm(IVm owner, IContacts contacts, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(ContactsVm)), owner) => _contacts = contacts;
}
