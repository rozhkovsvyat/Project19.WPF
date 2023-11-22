using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных удаления элемента типа <see cref="Models.Contact"/>
/// </summary>
public class ContactDeleteVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		if (arg is object?[] { Length: 2 } args) ReturnCmd = args[0] as Cmd;
		else return await ServerErrorAsync();

		if (args[1] is not int id) return await NotFoundAsync();

		try
		{
			Contact = await _contacts.AddToken(Token).GetAsync(id) ?? throw new KeyNotFoundException();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<ContactDeleteVm>(arg); }

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
	/// Команда, выполняемая после подтверждения формы
	/// </summary>
	public Cmd? ReturnCmd { get; private set; }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="contacts">Поставщик модели контактов</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public ContactDeleteVm(IVm owner, IContacts contacts, IViewFactory viewFactory)
		: base(viewFactory.Get(nameof(ContactDeleteVm)), owner) => _contacts = contacts;

	/// <summary>
	/// Подтверждает отправку формы
	/// </summary>
	/// <remarks>Опционально: при неуспешном результате выполняет удаленную команду</remarks>
	public Cmd SubmitCmd
		=> _submitCmd ??= new Cmd(o =>
		{
			App.Dispatcher.InvokeAsync(async () =>
			{
				View.Lock(true);

				try
				{
					await _contacts.AddToken(Token).RemoveAsync(Contact.Id);
					ReturnCmd?.Execute(this);
				}

				catch (AuthenticationException)
				{
					Authorize<ContactDeleteVm>(new[]
						{ ReturnCmd, Contact.Id as object });
				}

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
