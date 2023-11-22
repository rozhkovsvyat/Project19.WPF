﻿using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных изменения элемента типа <see cref="Models.Contact"/>
/// </summary>
public class ContactEditVm : Vm
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

		catch (AuthenticationException) { return await AuthorizeAsync<ContactEditVm>(arg); }

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
	/// Элемент типа <see cref="Models.Contact"/>
	/// </summary>
	public Contact Contact
	{
		get => _contact ??= new Contact();
		private set
		{
			_contact = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(LastName));
			OnPropertyChanged(nameof(FirstName));
			OnPropertyChanged(nameof(Patronymic));
			OnPropertyChanged(nameof(MobileNumber));
			OnPropertyChanged(nameof(Address));
			OnPropertyChanged(nameof(Description));
		}
	}
	private Contact? _contact;

	/// <inheritdoc cref="Contact.LastName"/>
	public string LastName
	{
		get => Contact.LastName;
		set
		{
			Contact.LastName = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.FirstName"/>
	public string FirstName
	{
		get => Contact.FirstName;
		set
		{
			Contact.FirstName = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.Patronymic"/>
	public string Patronymic
	{
		get => Contact.Patronymic;
		set
		{
			Contact.Patronymic = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.MobileNumber"/>
	public string MobileNumber
	{
		get => Contact.MobileNumber;
		set
		{
			Contact.MobileNumber = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.Address"/>
	public string Address
	{
		get => Contact.Address;
		set
		{
			Contact.Address = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.Description"/>
	public string Description
	{
		get => Contact.Description;
		set
		{
			Contact.Description = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="Contact.LastModifiedBy"/>
	public string LastModifiedBy => Contact.LastModifiedBy;
	/// <summary>
	/// Имя текущего пользователя
	/// </summary>
	private string CurrentUserName => string.Concat
		(User.Identity?.Name ?? string.Empty, " [⊞]").Trim();

	#endregion

	#region Validation

	/// <summary>
	/// Флаг валидации <see cref="LastName"/>
	/// </summary>
	public bool ValidateLastName
	{
		get => _validateLastName;
		set
		{
			if (_validateLastName == value) return;
			_validateLastName = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(LastName));
		}
	}
	private bool _validateLastName;
	/// <summary>
	/// Флаг валидации <see cref="FirstName"/>
	/// </summary>
	public bool ValidateFirstName
	{
		get => _validateFirstName;
		set
		{
			if (_validateFirstName == value) return;
			_validateFirstName = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(FirstName));
		}
	}
	private bool _validateFirstName;
	/// <summary>
	/// Флаг валидации <see cref="Patronymic"/>
	/// </summary>
	public bool ValidatePatronymic
	{
		get => _validatePatronymic;
		set
		{
			if (_validatePatronymic == value) return;
			_validatePatronymic = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Patronymic));
		}
	}
	private bool _validatePatronymic;
	/// <summary>
	/// Флаг валидации <see cref="MobileNumber"/>
	/// </summary>
	public bool ValidateMobileNumber
	{
		get => _validateMobileNumber;
		set
		{
			if (_validateMobileNumber == value) return;
			_validateMobileNumber = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(MobileNumber));
		}
	}
	private bool _validateMobileNumber;
	/// <summary>
	/// Флаг валидации <see cref="Address"/>
	/// </summary>
	public bool ValidateAddress
	{
		get => _validateAddress;
		set
		{
			if (_validateAddress == value) return;
			_validateAddress = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Address));
		}
	}
	private bool _validateAddress;
	/// <summary>
	/// Флаг валидации <see cref="Description"/>
	/// </summary>
	public bool ValidateDescription
	{
		get => _validateDescription;
		set
		{
			if (_validateDescription == value) return;
			_validateDescription = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Description));
		}
	}
	private bool _validateDescription;

	/// <summary>
	/// Возвращает результат проверки <see cref="LastName"/>
	/// </summary>
	private bool IsLastNameError
		=> !LastName.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="FirstName"/>
	/// </summary>
	private bool IsFirstNameError
		=> !FirstName.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="Patronymic"/>
	/// </summary>
	private bool IsPatronymicError
		=> !Patronymic.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="MobileNumber"/>
	/// </summary>
	private bool IsMobileNumberError
		=> !MobileNumber.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="Address"/>
	/// </summary>
	private bool IsAddressError
		=> !Address.Validate(_validatorFactory,
			ValidationType.Required);
	/// <summary>
	/// Возвращает результат проверки <see cref="Description"/>
	/// </summary>
	private bool IsDescriptionError
		=> !Description.Validate(_validatorFactory,
			ValidationType.Required);

	/// <summary>
	/// Статус валидации
	/// </summary>
	private bool InValidation
	{
		get
		{
			if (!ValidateLastName) ValidateLastName = IsLastNameError;
			if (!ValidateFirstName) ValidateFirstName = IsFirstNameError;
			if (!ValidatePatronymic) ValidatePatronymic = IsPatronymicError;
			if (!ValidateMobileNumber) ValidateMobileNumber = IsMobileNumberError;
			if (!ValidateAddress) ValidateAddress = IsAddressError;
			if (!ValidateDescription) ValidateDescription = IsDescriptionError;
			return IsLastNameError || IsFirstNameError || IsPatronymicError 
			       || IsMobileNumberError || IsAddressError || IsDescriptionError;
		}
		set => ValidateLastName = ValidateFirstName = ValidatePatronymic 
				= ValidateMobileNumber = ValidateAddress = ValidateDescription = value;
	}

	#endregion

	/// <summary>
	/// Поставщик модели контактов
	/// </summary>
	private readonly IContacts _contacts;

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
	/// <param name="contacts">Поставщик модели контактов</param>
	/// <param name="validatorFactory">Фабрика валидаторов</param>
	public ContactEditVm(IViewFactory viewFactory, IVm owner, IContacts contacts, 
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(ContactEditVm)), owner)
	{
		_contacts = contacts;
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
					Contact.LastModifiedBy = CurrentUserName;
					await _contacts.AddToken(Token).UpdateAsync(Contact);
					ReturnCmd?.Execute(this);
				}

				catch (KeyNotFoundException e)
				{
					Contact.LastModifiedBy = LastModifiedBy;
					InValidation = false;
					Errors.Clear();
					Errors.AddRange(e.Message.Deserialize());
					OnPropertyChanged(nameof(Errors));
					o.TryExecute();
				}

				catch (AuthenticationException)
				{
					Authorize<ContactEditVm>(new[] 
						{ ReturnCmd, Contact.Id as object });
				}

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		}); 
	private Cmd? _submitCmd; 
}
