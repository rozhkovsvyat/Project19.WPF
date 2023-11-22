using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных входа в <see cref="Account"/>
/// </summary>
public class SignInVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		ReturnCmd = arg as Cmd;
		return await base.InitializedAsync();
	}

	#endregion

	#region Form

	/// <summary>
	/// Список ошибок запроса
	/// </summary>
	public List<string> Errors { get; } = new();

	/// <inheritdoc cref="IIdentity.SignInForm"/>
	public SignInForm Form { get; }

	/// <inheritdoc cref="SignInForm.Login"/>
	public string Login
	{
		get => Form.Login;
		set
		{
			Form.Login = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="SignInForm.Password"/>
	public string Password
	{
		get => Form.Password;
		set
		{
			Form.Password = value;
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
	/// Флаг валидации <see cref="Password"/>
	/// </summary>
	public bool ValidatePassword
	{
		get => _validatePassword;
		set
		{
			if (_validatePassword == value) return;
			_validatePassword = value;
			OnPropertyChanged();
			Password = string.Empty;
		}
	}
	private bool _validatePassword;

	/// <summary>
	/// Возвращает результат проверки <see cref="Login"/>
	/// </summary>
	private bool IsLoginError
		=> !Login.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="Password"/>
	/// </summary>
	private bool IsPasswordError
		=> !Password.Validate(_validatorFactory,
			ValidationType.Required);

	/// <summary>
	/// Статус валидации
	/// </summary>
	private bool InValidation
	{
		get
		{
			if (!ValidateLogin) ValidateLogin = IsLoginError;
			if (!ValidatePassword) ValidatePassword = IsPasswordError;
			return IsLoginError || IsPasswordError;
		}
		set
		{
			ValidatePassword = ValidateLogin = value;
			Password = string.Empty;
		}
	}

	#endregion

	/// <inheritdoc cref="IIdentity"/>
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
	/// <param name="identity">Идентификация</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	public SignInVm(IViewFactory viewFactory, IVm owner, 
		IIdentity identity, IValidatorFactory validatorFactory) 
		: base(viewFactory.Get(nameof(SignInVm)), owner)
	{
		_identity = identity;
		Form = _identity.SignInForm;
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
					var token = await _identity.SignInAsync(Form);
					ReturnCmd?.Execute(token);
				}

				catch (InvalidOperationException e)
				{
					InValidation = false;
					Errors.Clear();
					Errors.AddRange(e.Message.Deserialize());
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
