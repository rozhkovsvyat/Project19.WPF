using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных удаления элемента типа <see cref="Models.Account"/>
/// </summary>
public class AccountDeleteVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		if (arg is object?[] { Length: 2 } args) ReturnCmd = args[0] as Cmd;
		else return await ServerErrorAsync();

		if (args[1] is not string id || id == User.FindFirst
			    (ClaimTypes.NameIdentifier)?.Value) return await NotFoundAsync();

		try
		{
			Account = await _identity.AddToken(Token).GetByIdAsync(id) ?? throw new KeyNotFoundException();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountDeleteVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

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
	/// Команда, выполняемая после подтверждения формы
	/// </summary>
	public Cmd? ReturnCmd { get; private set; }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public AccountDeleteVm(IVm owner, IIdentity identity, IViewFactory viewFactory)
		: base(viewFactory.Get(nameof(AccountDeleteVm)), owner) => _identity = identity;

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
					await _identity.AddToken(Token).RemoveByIdAsync(Account.Id);
					ReturnCmd?.Execute(this);
				}

				catch (AuthenticationException)
				{
					Authorize<AccountDeleteVm>(new[]
						{ ReturnCmd, Account.Id as object });
				}

				catch (InvalidOperationException) { ServerError(); }

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
