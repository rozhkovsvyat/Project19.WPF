using System.Security.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных смены пароля <see cref="Account"/>
/// </summary>
public class ChangePasswordVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin"))
		{
			Form = Identity.ChangePasswordForm.ForLogin(User.Identity?.Name ?? string.Empty);
			return string.IsNullOrEmpty(Form.Login) ? await NotFoundAsync() : await base.InitializedAsync();
		}

		if (arg is not Account account)
		{
			try
			{
				if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value is not { } id) throw new KeyNotFoundException();
				account = await Identity.AddToken(Token).GetByIdAsync(id) ?? throw new KeyNotFoundException();
			}

			catch (AuthenticationException) { return await AuthorizeAsync<ChangePasswordVm>(arg); }

			catch (KeyNotFoundException) { return await NotFoundAsync(); }

			catch (Exception) { return await ConnectionErrorAsync(); }
		}

		Form = Identity.ChangePasswordForm.ForLogin(account.Login);
		return await base.InitializedAsync();
	}

	#endregion

	#region Form

	/// <summary>
	/// Список ошибок запроса
	/// </summary>
	public List<string> Errors { get; } = new();

	/// <summary>
	/// Флаг успешной операции
	/// </summary>
	public bool IsSuccess { get; protected set; }

	/// <inheritdoc cref="IIdentity.ChangePasswordForm"/>
	public ChangePasswordForm Form
	{
		get => _form ??= new ChangePasswordForm();
		set
		{
			_form = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(OldPassword));
			OnPropertyChanged(nameof(Password));
			OnPropertyChanged(nameof(ConfirmPassword));
		}
	}
	private ChangePasswordForm? _form;

	/// <inheritdoc cref="ChangePasswordForm.OldPassword"/>
	public string OldPassword
	{
		get => Form.OldPassword;
		set
		{
			Form.OldPassword = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ChangePasswordForm.Password"/>
	public string Password
	{
		get => Form.Password;
		set
		{
			Form.Password = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ChangePasswordForm.ConfirmPassword"/>
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
	/// Флаг валидации <see cref="OldPassword"/>
	/// </summary>
	public bool ValidateOldPassword
	{
		get => _validateOldPassword;
		set
		{
			if (_validateOldPassword == value) return;
			_validateOldPassword = value;
			OnPropertyChanged();
			OldPassword = string.Empty;
		}
	}
	private bool _validateOldPassword;
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
			if (_validateConfirmPassword == value) return;
			_validateConfirmPassword = value;
			OnPropertyChanged();
			ConfirmPassword = string.Empty;
		}
	}
	private bool _validateConfirmPassword;

	/// <summary>
	/// Возвращает результат проверки <see cref="OldPassword"/>
	/// </summary>
	private bool IsOldPasswordError
		=> !OldPassword.Validate(_validatorFactory,
			ValidationType.Required);
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
			if (!ValidateOldPassword) ValidateOldPassword = IsOldPasswordError;
			if (!ValidatePassword) ValidatePassword = IsPasswordError;
			if (!ValidateConfirmPassword) ValidateConfirmPassword = IsConfirmPasswordError;
			return IsOldPasswordError || IsPasswordError || IsConfirmPasswordError;
		}
		set
		{
			ValidateOldPassword = ValidatePassword = ValidateConfirmPassword = value;
			OldPassword =Password = ConfirmPassword = string.Empty;
		}
	}

	#endregion

	/// <inheritdoc cref="IIdentity"/>
	protected readonly IIdentity Identity;

	/// <inheritdoc cref="IValidatorFactory"/>
	private readonly IValidatorFactory _validatorFactory;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	/// <param name="identity">Идентификация</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	public ChangePasswordVm(IViewFactory viewFactory, IVm owner, 
		IIdentity identity, IValidatorFactory validatorFactory) 
		: base(viewFactory.Get(nameof(ChangePasswordVm)), owner)
	{
		Identity = identity;
		_validatorFactory = validatorFactory;
	}

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="view">Представление <see cref="IView"/></param>
	/// <param name="identity">Идентификация</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	protected ChangePasswordVm(IView view, IVm owner, IIdentity identity,
		IValidatorFactory validatorFactory) : base(view, owner)
	{
		Identity = identity;
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
					await Identity.AddToken(Token).UpdatePasswordAsync(Form);
					InValidation = false;
					IsSuccess = true;
					Errors.Clear();
					OnPropertyChanged(nameof(IsSuccess));
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (InvalidOperationException e)
				{
					InValidation = false;
					IsSuccess = false;
					Errors.Clear();
					OnPropertyChanged(nameof(IsSuccess));
					Errors.AddRange(e.Message.Deserialize());
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (AuthenticationException) { Authorize<ChangePasswordVm>(); }

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
