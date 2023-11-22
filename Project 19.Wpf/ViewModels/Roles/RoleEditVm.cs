using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных изменения элемента типа <see cref="Models.Role"/>
/// </summary>
public class RoleEditVm : Vm
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

		catch (AuthenticationException) { return await AuthorizeAsync<RoleEditVm>(arg); }

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
	/// Элемент типа <see cref="Models.Role"/>
	/// </summary>
	public Role Role
	{
		get => _role ??= new Role();
		private set
		{
			_role = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Name));
		}
	}
	private Role? _role;

	/// <inheritdoc cref="Role.Name"/>
	public string Name
	{
		get => Role.Name;
		set
		{
			Role.Name = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region Validation

	/// <summary>
	/// Флаг валидации <see cref="Name"/>
	/// </summary>
	public bool ValidateName
	{
		get => _validateName;
		set
		{
			if (_validateName == value) return;
			_validateName = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Name));
		}
	}
	private bool _validateName;

	/// <summary>
	/// Возвращает результат проверки <see cref="Name"/>
	/// </summary>
	private bool IsNameError
		=> !Name.Validate(_validatorFactory,
			ValidationType.Required);

	/// <summary>
	/// Статус валидации
	/// </summary>
	private bool InValidation
	{
		get
		{
			if (!ValidateName) ValidateName = IsNameError;
			return IsNameError;
		}
		set => ValidateName = value;
	}

	#endregion

	/// <summary>
	/// Поставщик идентификации
	/// </summary>
	private readonly IIdentity _identity;

	/// <inheritdoc cref="IValidatorFactory"/>
	private readonly IValidatorFactory _validatorFactory;

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
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	public RoleEditVm(IViewFactory viewFactory, IVm owner, IIdentity identity, 
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(RoleEditVm)), owner)
	{
		_identity = identity;
		_validatorFactory = validatorFactory;
	}

	/// <summary>
	/// Подтверждает отправку формы
	/// </summary>
	/// <remarks>Опционально: при неуспешном результате выполняет удаленную команду</remarks>
	public Cmd SubmitCmd 
		=> _submitCmd ??= new Cmd(o => 
		{ 
			if (InValidation) return;

			App.Dispatcher.InvokeAsync(async () => 
			{
				View.Lock(true);

				try
				{
					await _identity.AddToken(Token).UpdateRoleAsync(Role);
					ReturnCmd?.Execute(this);
				}

				catch (InvalidOperationException e)
				{
					InValidation = false;
					Errors.Clear();
					Errors.AddRange(e.Message.Deserialize());
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (AuthenticationException)
				{
					Authorize<RoleEditVm>(new[] 
						{ ReturnCmd, Role.Id as object });
				}

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		}); 
	private Cmd? _submitCmd; 
}
