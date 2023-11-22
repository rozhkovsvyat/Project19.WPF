using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных удаления элемента типа <see cref="Models.Role"/>
/// </summary>
public class RoleDeleteVm : Vm
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
			Role = await _identity.AddToken(Token).GetRoleByIdAsync(id) ?? throw new KeyNotFoundException();
			if (Role.Name == "admin") throw new KeyNotFoundException();
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<RoleDeleteVm>(arg); }

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
	/// Команда, выполняемая после подтверждения формы
	/// </summary>
	public Cmd? ReturnCmd { get; private set; }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="identity">Поставщик идентификации</param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public RoleDeleteVm(IVm owner, IIdentity identity, IViewFactory viewFactory)
		: base(viewFactory.Get(nameof(RoleDeleteVm)), owner) => _identity = identity;

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
					await _identity.AddToken(Token).RemoveRoleByIdAsync(Role.Id);
					ReturnCmd?.Execute(this);
				}

				catch (AuthenticationException)
				{
					Authorize<RoleDeleteVm>(new[]
						{ ReturnCmd, Role.Id as object });
				}

				catch (InvalidOperationException) { ServerError(); }

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
