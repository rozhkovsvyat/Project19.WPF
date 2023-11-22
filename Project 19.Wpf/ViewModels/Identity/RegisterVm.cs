using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных регистрации <see cref="Account"/>
/// </summary>
public class RegisterVm : Vm
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
	public CreateAccountForm Form { get; }

	/// <inheritdoc cref="CreateAccountForm.Login"/>
	public string Login
	{
		get => Form.Login;
		set
		{
			Form.Login = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="CreateAccountForm.Email"/>
	public string Email
	{
		get => Form.Email;
		set
		{
			Form.Email = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="CreateAccountForm.Password"/>
	public string Password
	{
		get => Form.Password;
		set
		{
			Form.Password = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(ConfirmPassword));
		}
	}

	/// <inheritdoc cref="CreateAccountForm.ConfirmPassword"/>
	public string ConfirmPassword
	{
		get => Form.ConfirmPassword;
		set
		{
			Form.ConfirmPassword = value;
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
			if (value == _validateEmail) return;
			_validateEmail = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Email));
		}
	}
	private bool _validateEmail;
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
	/// Флаг валидации <see cref="ConfirmPassword"/>
	/// </summary>
	public bool ValidateConfirmPassword
	{
		get => _validateConfirmPassword;
		set
		{
			if (value == _validateConfirmPassword) return;
			_validateConfirmPassword = value;
			OnPropertyChanged();
			ConfirmPassword = string.Empty;
		}
	}
	private bool _validateConfirmPassword;

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
	/// Возвращает результат проверки <see cref="Password"/>
	/// </summary>
	private bool IsPasswordError
		=> !Password.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="ConfirmPassword"/>
	/// </summary>
	private bool IsConfirmPasswordError
		=> !ConfirmPassword.Validate(new ValidatorFactoryConfigurator(_validatorFactory)
			.AddOption(ValidationType.Compared, new DependencyBag { CompareWith = Password })
			.BuildConfigured(), ValidationType.Required, ValidationType.Compared);

	/// <summary>
	/// Статус валидации
	/// </summary>
	protected bool InValidation
	{
		get
		{
			if (!ValidateLogin) ValidateLogin = IsLoginError;
			if (!ValidateEmail) ValidateEmail = IsEmailError;
			if (!ValidatePassword) ValidatePassword = IsPasswordError;
			if (!ValidateConfirmPassword) ValidateConfirmPassword = IsConfirmPasswordError;
			return IsLoginError || IsEmailError || IsPasswordError || IsConfirmPasswordError;
		}
		set
		{
			ValidateConfirmPassword = ValidatePassword = ValidateEmail = ValidateLogin = value;
			Password = ConfirmPassword = string.Empty;
		}
	}

	#endregion

	/// <inheritdoc cref="IIdentity"/>
	protected readonly IIdentity Identity;

	/// <inheritdoc cref="IValidatorFactory"/>
	private readonly IValidatorFactory _validatorFactory;

	/// <summary>
	/// Команда, выполняемая после подтверждения формы
	/// </summary>
	public Cmd? ReturnCmd { get; protected set; }

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	/// <param name="identity">Идентификация</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	public RegisterVm(IViewFactory viewFactory, IVm owner, IIdentity identity, 
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(RegisterVm)), owner)
	{
		Identity = identity;
		Form = Identity.CreateAccountForm;
		_validatorFactory = validatorFactory;
	}

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="view">Представление <see cref="IView"/></param>
	/// <param name="identity">Идентификация</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	protected RegisterVm(IView view, IVm owner, IIdentity identity,
		IValidatorFactory validatorFactory) : base(view, owner)
	{
		Identity = identity;
		Form = Identity.CreateAccountForm;
		_validatorFactory = validatorFactory;
	}

	/// <summary>
	/// Подтверждает отправку формы
	/// </summary>
	/// <remarks>Опционально: при неуспешном результате выполняет удаленную команду</remarks>
	public virtual Cmd SubmitCmd 
		=> _submitCmd ??= new Cmd(o => 
		{ 
			if (InValidation) return;

			App.Dispatcher.InvokeAsync(async () => 
			{
				View.Lock(true);

				try 
				{
					await Identity.AddAsync(Form);
					var token = await Identity.SignInAsync
					(Identity.SignInForm.ForLogin(Form.Login)
						.ForPassword(Form.Password));
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
