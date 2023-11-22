using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных назначения <see cref="Role"/> элемента типа <see cref="Models.Account"/>
/// </summary>
public class AccountRoleAssignVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		if (arg is object?[] { Length: 2 } args) ReturnCmd = args[0] as Cmd;
		else return await ServerErrorAsync();

		if (args[1] is not string id) return await NotFoundAsync();

		try
		{
			Account = await _identity.AddToken(Token).GetByIdAsync(id) ?? throw new KeyNotFoundException();
			Roles = await _identity.AddToken(Token).GetAvailableRolesAsync(Account.Login);
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountRoleAssignVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	#region Form

	/// <summary>
	/// Список ошибок запроса
	/// </summary>
	public List<string> Errors { get; } = new();

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
	/// Коллекция <see cref="Role"/>
	/// </summary>
	public IEnumerable<Role> Roles
	{
		get => _roles;
		private set
		{
			_roles = value;
			OnPropertyChanged();
			SelectedRole = _roles.FirstOrDefault();
			OnPropertyChanged(nameof(SelectedRole));
		}
	}
	private IEnumerable<Role> _roles = Enumerable.Empty<Role>();

	/// <summary>
	/// Выбранная роль
	/// </summary>
	public Role? SelectedRole { get; set; }

	#endregion

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

	/// <summary>
	/// Команда, выполняемая после подтверждения формы
	/// </summary>
	public Cmd? ReturnCmd { get; private set; }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	public AccountRoleAssignVm(IViewFactory viewFactory, IVm owner, IIdentity identity) 
		: base(viewFactory.Get(nameof(AccountRoleAssignVm)), owner) => _identity = identity;

	/// <summary>
	/// Подтверждает отправку формы
	/// </summary>
	/// <remarks>Опционально: при неуспешном результате выполняет удаленную команду</remarks>
	public Cmd SubmitCmd 
		=> _submitCmd ??= new Cmd(o => 
		{
			if (SelectedRole is null) return;

			App.Dispatcher.InvokeAsync(async () => 
			{
				View.Lock(true);

				try
				{
					await _identity.AddToken(Token).AddToRoleAsync
						(Account.Login, SelectedRole.Name);
					ReturnCmd?.Execute(this);
				}

				catch (InvalidOperationException e)
				{
					Errors.Clear();
					Errors.AddRange(e.Message.Deserialize());
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (AuthenticationException)
				{
					Authorize<AccountRoleAssignVm>(new[] 
						{ ReturnCmd, Account.Id as object });
				}

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		}); 
	private Cmd? _submitCmd; 
}
