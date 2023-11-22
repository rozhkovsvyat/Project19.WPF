using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных изменения элемента типа <see cref="Models.Account"/>
/// </summary>
public class AccountEditVm : Vm
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
			return await base.InitializedAsync();
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountEditVm>(arg); }

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
			OnPropertyChanged(nameof(Login));
			OnPropertyChanged(nameof(Email));
		}
	}
	private Account? _account;

	/// <inheritdoc cref="Account.Login"/>
	public string Login
	{
		get => Account.Login;
		set
		{
			Account.Login = value;
			OnPropertyChanged();
		}
	}
	/// <inheritdoc cref="Account.Email"/>
	public string Email
	{
		get => Account.Email;
		set
		{
			Account.Email = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region Validation

	/// <summary>
	/// Флаг валидации <see cref="Login"/>
	/// </summary>
	public bool ValidateLogin
	{
		get => _validateLogin;
		set
		{
			if (_validateLogin == value) return;
			_validateLogin = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Login));
		}
	}
	private bool _validateLogin;
	/// <summary>
	/// Флаг валидации <see cref="Email"/>
	/// </summary>
	public bool ValidateEmail
	{
		get => _validateEmail;
		set
		{
			if (_validateEmail == value) return;
			_validateEmail = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Email));
		}
	}
	private bool _validateEmail;

	/// <summary>
	/// Возвращает результат проверки <see cref="Login"/>
	/// </summary>
	private bool IsLoginError
		=> !Login.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="Email"/>
	/// </summary>
	private bool IsEmailError
		=> !Email.Validate(_validatorFactory,
			ValidationType.Required, ValidationType.Email);

	/// <summary>
	/// Статус валидации
	/// </summary>
	private bool InValidation
	{
		get
		{
			if (!ValidateLogin) ValidateLogin = IsLoginError;
			if (!ValidateEmail) ValidateEmail = IsEmailError;
			return IsLoginError || IsEmailError;
		}
		set => ValidateLogin = ValidateEmail = value;
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
	public AccountEditVm(IViewFactory viewFactory, IVm owner, IIdentity identity, 
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(AccountEditVm)), owner)
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
					await _identity.AddToken(Token).UpdateAsync(Account);
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
					Authorize<AccountEditVm>(new[] 
						{ ReturnCmd, Account.Id as object });
				}

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		}); 
	private Cmd? _submitCmd; 
}
