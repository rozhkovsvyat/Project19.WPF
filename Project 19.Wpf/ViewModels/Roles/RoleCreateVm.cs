using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных добавления элемента типа <see cref="Role"/>
/// </summary>
public class RoleCreateVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();

		ReturnCmd = arg as Cmd;
		return await base.InitializedAsync();
	}

	#endregion

	#region Form

	/// <summary>
	/// Список ошибок запроса
	/// </summary>
	public List<string> Errors { get; } = new();

	/// <inheritdoc cref="IIdentity.CreateRoleForm"/>
	public CreateRoleForm Form { get; }

	/// <inheritdoc cref="CreateRoleForm.Name"/>
	public string Name
	{
		get => Form.Name;
		set
		{
			Form.Name = value;
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
	protected bool InValidation
	{
		get
		{
			if (!ValidateName) ValidateName = IsNameError;
			return IsNameError;
		}
		set => ValidateName = value;
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
	public RoleCreateVm(IViewFactory viewFactory, IVm owner, IIdentity identity,
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(RoleCreateVm)), owner)
	{
		Identity = identity;
		Form = Identity.CreateRoleForm;
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
					await Identity.AddToken(Token).AddRoleAsync(Form);
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
					Authorize<RoleCreateVm>
						(ReturnCmd);
				}

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
