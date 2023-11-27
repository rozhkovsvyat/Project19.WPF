using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project_19.ViewModels;

/// <summary>
/// Главный контекст данных
/// </summary>
public class LayoutVm : Vm
{
	#region Vm

	/// <inheritdoc cref="Vm.Token"/>
	public override string? Token => LayoutToken;
	/// <inheritdoc cref="Vm.User"/>
	public override ClaimsPrincipal User => LayoutUser;
	/// <inheritdoc/>
	public override async Task<IVm> AuthorizeAsync<T>(object? arg = null)
		=> await ResetToken().GetAsync<SignInVm>(new Cmd(o
			=> TryAddToken(o).SetFromCmd<T>(arg)));
	/// <inheritdoc/>
	public override async void Authorize<T>(object? arg = null) 
		=> Set(await AuthorizeAsync<T>(arg));
	/// <inheritdoc/>
	public override async Task<IVm> NotFoundAsync(object? arg = null) 
		=> await GetAsync<NotFoundVm>(arg);
	/// <inheritdoc/>
	public override async void NotFound(object? arg = null)
		=> Set(await NotFoundAsync(arg));
	/// <inheritdoc/>
	public override async Task<IVm> ServerErrorAsync(object? arg = null) 
		=> await GetAsync<ServerErrorVm>(arg);
	/// <inheritdoc/>
	public override async void ServerError(object? arg = null)
		=> Set(await ServerErrorAsync(arg));
	/// <inheritdoc/>
	public override async Task<IVm> ConnectionErrorAsync(object? arg = null)
		=> await GetAsync<ConnectionErrorVm>(arg);
	/// <inheritdoc/>
	public override async void ConnectionError(object? arg = null)
		=> Set(await ConnectionErrorAsync(arg));
	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		await SetAsync<ContactsVm>(arg.TryExecute());
		return await base.InitializedAsync(arg);
	}

	#endregion

	#region Properties

	/// <summary>
	/// Фабрика элементов <see cref="IVm"/>
	/// </summary>
	private readonly IVmFactory _vmFactory;

	/// <inheritdoc cref="Vm.Token"/>
	/// <remarks>Предоставляет возможность устанавливать значение <see cref="Vm.Token"/> внутри класса <see cref="LayoutVm"/></remarks>
	private string? LayoutToken
	{
		get => _layoutToken;
		set
		{
			if (_layoutToken == value) return;
			_layoutToken = value;
			OnPropertyChanged();

			if (string.IsNullOrEmpty(_layoutToken))
			{
				LayoutUser = new ClaimsPrincipal();
				return;
			}

			var jwt = new JwtSecurityTokenHandler()
				.ReadJwtToken(_layoutToken);

			LayoutUser = new ClaimsPrincipal(new ClaimsIdentity
				(jwt.Claims, nameof(LayoutUser)));
		}
	}
	private string? _layoutToken;
	/// <inheritdoc cref="Vm.User"/>
	/// <remarks>Предоставляет возможность устанавливать значение <see cref="Vm.User"/> внутри класса <see cref="LayoutVm"/></remarks>
	public ClaimsPrincipal LayoutUser
	{
		get => _layoutUser;
		protected set
		{
			if (_layoutUser == value) return;
			_layoutUser = value;
			OnPropertyChanged();

			IsAuthenticated = LayoutUser.Identity?.IsAuthenticated ?? false;
			UserName = LayoutUser.Identity?.Name ?? string.Empty;
			IsAdmin = LayoutUser.IsInRole("admin");
		}
	}
	private ClaimsPrincipal _layoutUser = new();
	/// <summary>
	/// Флаг аутентификации
	/// </summary>
	public bool IsAuthenticated
	{
		get => _isAuthenticated;
		protected set
		{
			if (_isAuthenticated == value) return;
			_isAuthenticated = value;
			OnPropertyChanged();
		}
	}
	private bool _isAuthenticated;
	/// <summary>
	/// Имя пользователя
	/// </summary>
	public string UserName
	{
		get => _userName;
		protected set
		{
			if (_userName == value) return;
			_userName = value;
			OnPropertyChanged();
		}
	}
	private string _userName = string.Empty;
	/// <summary>
	/// Флаг администратора
	/// </summary>
	public bool IsAdmin
	{
		get => _isAdmin;
		protected set
		{
			if (_isAdmin == value) return;
			_isAdmin = value;
			OnPropertyChanged();
		}
	}
	private bool _isAdmin;

	/// <summary>
	/// Вложенный элемент <see cref="IVm"/>
	/// </summary>
	public IVm Content
	{
		get => _content;
		set
		{
			_content = value;
			OnPropertyChanged();
		}
	}
	private IVm _content = null!;

	#endregion

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="viewFactory">Фабрика элементов <see cref="IView"/></param>
	/// <param name="vmFactory">Фабрика элементов <see cref="IVm"/></param>
	public LayoutVm(IViewFactory viewFactory, IVmFactory vmFactory) 
		: base(viewFactory.Get(nameof(LayoutVm))) => _vmFactory = vmFactory;

	#region Locker

	/// <summary>
	/// Устанавливает блокировку вложенного контекста, если до этого она не была установлена
	/// </summary>
	/// <remarks>При попытке установки возвращает значение блокировки до вызова метода</remarks>
	/// <param name="value">Значение блокировки</param>
	/// <returns><see langword="True"/> если блокировка была установлена до вызова метода, <see langword="False"/> если блокировка установлена или снята текущим методом</returns>
	private bool Lock(bool value)
	{
		if (_lockContent && value) return true;
		View.Lock(value);
		_lockContent = value;
		return false;
	}
	private bool _lockContent;

	#endregion

	#region Private

	/// <summary>
	/// Возвращает элемент типа <see cref="IVm"/>
	/// </summary>
	/// <remarks> Предварительно вызывает метод <see cref="IVm.InitializedAsync"/></remarks>
	/// <typeparam name="T">Тип элемента <see cref="IVm"/></typeparam>
	/// <param name="arg">Аргумент инициализации</param>
	/// <returns>Элемент типа <see cref="IVm"/></returns>
	private async Task<IVm> GetAsync<T>(object? arg = null) where T : IVm
		=> await _vmFactory.Get(typeof(T).Name).InitializedAsync(arg);

	/// <summary>
	/// Устанавливает элемент типа <see cref="IVm"/> в качестве вложенного контекста
	/// </summary>
	/// <remarks>При возникновении ошибки устанавливает контекст <see cref="ConnectionErrorVm"/></remarks>
	/// <typeparam name="T">Тип элемента <see cref="IVm"/></typeparam>
	/// <param name="arg">Аргумент инициализации</param>
	private async Task SetAsync<T>(object? arg) where T : IVm
	{
		if (Lock(true)) return;
		Content = await GetAsync<T>(arg); 
		Lock(false);
	}

	/// <summary>
	/// Устанавливает элемент типа <see cref="IVm"/> в качестве вложенного контекста
	/// </summary>
	/// <param name="vm">Элемент типа <see cref="IVm"/></param>
	private void Set(IVm vm)
	{
		if (Lock(true)) return;
		Content = vm;
		Lock(false);
	}

	/// <inheritdoc cref="SetAsync{T}"/>
	private void Set<T>(object? arg) where T : IVm
		=> App.Dispatcher.InvokeAsync(async () => await SetAsync<T>(arg));

	/// <inheritdoc cref="SetFromCmd{T}(object?, bool)"/>
	private void SetFromCmd<T>(object? arg = null)
		where T : IVm => SetFromCmd<T>(arg, true);

	/// <summary>
	/// <inheritdoc cref="Set{T}"/>
	/// </summary>
	/// <remarks>Производит попытку исполнения аргумента, если он является <see cref="Cmd"/></remarks>
	/// <param name="arg">Аргумент или <see cref="Cmd"/></param>
	/// <param name="tryExecute">Флаг попытки исполнения аргумента, если он является <see cref="Cmd"/></param>
	private void SetFromCmd<T>(object? arg, bool tryExecute)
		where T : IVm => Set<T>(tryExecute ? arg.TryExecute() : arg);

	/// <inheritdoc cref="Set{T}"/>
	/// <remarks>Производит попытку исполнения аргумента, если он является <see cref="Cmd"/>, затем производит замещение аргумента</remarks>
	/// <param name="argOrCmd">Аргумент или <see cref="Cmd"/></param>
	/// <param name="arg">Аргумент замещения</param>
	private void SetFromCmd<T>(object? argOrCmd, object? arg)
		where T : IVm => Set<T>(argOrCmd.TryExecuteAndReplace(arg));

	/// <inheritdoc cref="Set{T}"/>
	/// <remarks>Пeредает в качестве аргумента команду <see cref="Cmd"/> и аргумент инициализации</remarks>
	/// <param name="cmd">Команда <see cref="Cmd"/></param>
	/// <param name="arg">Аргумент инициализации</param>
	private void SetFromCmd<T>(Cmd cmd, object? arg = null)
		where T : IVm => Set<T>(new[] {cmd, arg});

	/// <summary>
	/// Производит попытку установки <see cref="Token"/> и возвращает текущий контекст
	/// </summary>
	/// <param name="arg">Аргумент</param>
	/// <returns>Элемент <see cref="LayoutVm"/></returns>
	private LayoutVm TryAddToken(object? arg)
	{
		if (arg is string token) LayoutToken = token;
		return this;
	}

	/// <summary>
	/// Сбрасывает <see cref="Token"/> и возвращает текущий контекст
	/// </summary>
	/// <returns>Элемент <see cref="LayoutVm"/></returns>
	private LayoutVm ResetToken()
	{
		LayoutToken = null;
		return this;
	}

	#endregion

	#region Cmd:Errors

	/// <summary>
	/// Устанавливает контекст отсутствия элемента
	/// </summary>
	public Cmd NotFoundCmd => _notFoundCmd
		??= new Cmd(SetFromCmd<NotFoundVm>);
	private Cmd? _notFoundCmd;
	/// <summary>
	/// Устанавливает контекст внутренней ошибки
	/// </summary>
	public Cmd ServerErrorCmd => _serverErrorCmd
		??= new Cmd(SetFromCmd<ServerErrorVm>);
	private Cmd? _serverErrorCmd;

	#endregion

	#region Cmd:Identity

	/// <summary>
	/// Устанавливает контекст <see cref="SignInVm"/>
	/// </summary>
	public Cmd SignInCmd => _signInCmd ??= new Cmd(o
		=> SetFromCmd<SignInVm>(o, new Cmd
			(t => TryAddToken(t).SetFromCmd<ContactsVm>())));
	private Cmd? _signInCmd;

	/// <inheritdoc cref="SignInCmd"/>
	/// <remarks>Передает команду возврата</remarks>
	public Cmd SignInReturnCmd => _signInReturnCmd
		??= new Cmd(o => SetFromCmd<SignInVm>(o, false));
	private Cmd? _signInReturnCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="RegisterVm"/>
	/// </summary>
	public Cmd RegisterCmd => _registerCmd ??= new Cmd(o
		=> SetFromCmd<RegisterVm>(o, new Cmd
			(t => TryAddToken(t).SetFromCmd<ContactsVm>())));
	private Cmd? _registerCmd;

	/// <inheritdoc cref="RegisterCmd"/>
	/// <remarks>Передает команду возврата</remarks>
	public Cmd RegisterReturnCmd => _registerReturnCmd
		??= new Cmd(o => SetFromCmd<RegisterVm>(o, false));
	private Cmd? _registerReturnCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="LogOutVm"/>
	/// </summary>
	public Cmd LogOutCmd => _logOutCmd ??= new Cmd(o
		=> SetFromCmd<LogOutVm>(o, new Cmd
			(_ => ResetToken().SetFromCmd<ContactsVm>())));
	private Cmd? _logOutCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ChangePasswordVm"/>
	/// </summary>
	public Cmd ChangePasswordCmd => _changePasswordCmd
		??= new Cmd(SetFromCmd<ChangePasswordVm>);
	private Cmd? _changePasswordCmd;

	#endregion

	#region Cmd:Contacts

	/// <summary>
	/// Устанавливает контекст <see cref="ContactsVm"/>
	/// </summary>
	public Cmd ContactsCmd => _contactsCmd 
		??= new Cmd(SetFromCmd<ContactsVm>);
	private Cmd? _contactsCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ContactDetailsVm"/>
	/// </summary>
	public Cmd ContactDetailsCmd => _contactDetailsCmd 
		??= new Cmd(SetFromCmd<ContactDetailsVm>);
	private Cmd? _contactDetailsCmd;

	/// <inheritdoc cref="ContactDetailsCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd ContactDetailsSafeCmd => _contactDetailsSafeCmd
		??= new Cmd(o => { if (o is >= 0) ContactDetailsCmd.Execute(o); });
	private Cmd? _contactDetailsSafeCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ContactEditVm"/>
	/// </summary>
	public Cmd ContactEditCmd => _contactEditCmd
		??= new Cmd(o => SetFromCmd<ContactEditVm>(ContactsCmd, o));
	private Cmd? _contactEditCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ContactCreateVm"/>
	/// </summary>
	public Cmd ContactCreateCmd => _contactCreateCmd
		??= new Cmd(o => SetFromCmd<ContactCreateVm>(o, false));
	private Cmd? _contactCreateCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ContactDeleteVm"/>
	/// </summary>
	public Cmd ContactDeleteCmd => _contactDeleteCmd
		??= new Cmd(o => SetFromCmd<ContactDeleteVm>(ContactsCmd, o));
	private Cmd? _contactDeleteCmd;

	/// <inheritdoc cref="ContactDeleteCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd ContactDeleteSafeCmd => _contactDeleteSafeCmd
		??= new Cmd(o => { if (o is >= 0) ContactDeleteCmd.Execute(o); });
	private Cmd? _contactDeleteSafeCmd;

	#endregion

	#region Cmd:Accounts

	/// <summary>
	/// Устанавливает контекст <see cref="AccountsVm"/>
	/// </summary>
	public Cmd AccountsCmd => _accountsCmd
		??= new Cmd(SetFromCmd<AccountsVm>);
	private Cmd? _accountsCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountCreateVm"/>
	/// </summary>
	public Cmd AccountCreateCmd => _accountCreateCmd
		??= new Cmd(o => SetFromCmd<AccountCreateVm>(o, false));
	private Cmd? _accountCreateCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountEditVm"/>
	/// </summary>
	public Cmd AccountEditCmd => _accountEditCmd
		??= new Cmd(o => SetFromCmd<AccountEditVm>(AccountsCmd, o));
	private Cmd? _accountEditCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountDetailsVm"/>
	/// </summary>
	public Cmd AccountDetailsCmd => _accountDetailsCmd
		??= new Cmd(SetFromCmd<AccountDetailsVm>);
	private Cmd? _accountDetailsCmd;

	/// <inheritdoc cref="AccountDetailsCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd AccountDetailsSafeCmd => _accountDetailsSafeCmd
		??= new Cmd(o => 
			{ if (o is string str && !string.IsNullOrEmpty(str)) AccountDetailsCmd.Execute(o); });
	private Cmd? _accountDetailsSafeCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountDeleteVm"/>
	/// </summary>
	public Cmd AccountDeleteCmd => _accountDeleteCmd
		??= new Cmd(o => SetFromCmd<AccountDeleteVm>(AccountsCmd, o));
	private Cmd? _accountDeleteCmd;

	/// <inheritdoc cref="AccountDeleteCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd AccountDeleteSafeCmd => _accountDeleteSafeCmd
		??= new Cmd(o =>
			{ if (o is string str && !string.IsNullOrEmpty(str)) AccountDeleteCmd.Execute(o); });
	private Cmd? _accountDeleteSafeCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="ChangePasswordVm"/>
	/// </summary>
	public Cmd AccountChangePasswordCmd => _accountChangePasswordCmd
		??= new Cmd(SetFromCmd<AccountChangePasswordVm>);
	private Cmd? _accountChangePasswordCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountRolesVm"/>
	/// </summary>
	public Cmd AccountRolesCmd => _accountRolesCmd
		??= new Cmd(o => SetFromCmd<AccountRolesVm>(o, false));
	private Cmd? _accountRolesCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountRoleAssignVm"/>
	/// </summary>
	public Cmd AccountRoleAssignCmd => _accountRoleAssignCmd
		??= new Cmd(o => SetFromCmd<AccountRoleAssignVm>(AccountsCmd, o));
	private Cmd? _accountRoleAssignCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="AccountRoleRemoveVm"/>
	/// </summary>
	public Cmd AccountRoleRemoveCmd => _accountRoleRemoveCmd
		??= new Cmd(o => SetFromCmd<AccountRoleRemoveVm>(AccountsCmd, o));
	private Cmd? _accountRoleRemoveCmd;

	#endregion

	#region Cmd:Roles

	/// <summary>
	/// Устанавливает контекст <see cref="RolesVm"/>
	/// </summary>
	public Cmd RolesCmd => _rolesCmd
		??= new Cmd(SetFromCmd<RolesVm>);
	private Cmd? _rolesCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="RoleCreateVm"/>
	/// </summary>
	public Cmd RoleCreateCmd => _roleCreateCmd
		??= new Cmd(o => SetFromCmd<RoleCreateVm>(o, false));
	private Cmd? _roleCreateCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="RoleEditVm"/>
	/// </summary>
	public Cmd RoleEditCmd => _roleEditCmd
		??= new Cmd(o => SetFromCmd<RoleEditVm>(RolesCmd, o));
	private Cmd? _roleEditCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="RoleDetailsVm"/>
	/// </summary>
	public Cmd RoleDetailsCmd => _roleDetailsCmd
		??= new Cmd(SetFromCmd<RoleDetailsVm>);
	private Cmd? _roleDetailsCmd;

	/// <inheritdoc cref="RoleDetailsCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd RoleDetailsSafeCmd => _roleDetailsSafeCmd
		??= new Cmd(o => 
			{ if (o is string str && !string.IsNullOrEmpty(str)) RoleDetailsCmd.Execute(o); });
	private Cmd? _roleDetailsSafeCmd;

	/// <summary>
	/// Устанавливает контекст <see cref="RoleDeleteVm"/>
	/// </summary>
	public Cmd RoleDeleteCmd => _roleDeleteCmd
		??= new Cmd(o => SetFromCmd<RoleDeleteVm>(RolesCmd, o));
	private Cmd? _roleDeleteCmd;

	/// <inheritdoc cref="RoleDeleteCmd"/>
	/// <remarks>Выполняется, если аргумент проходит проверку</remarks>
	public Cmd RoleDeleteSafeCmd => _roleDeleteSafeCmd
		??= new Cmd(o =>
			{ if (o is string str && !string.IsNullOrEmpty(str)) RoleDeleteCmd.Execute(o); });
	private Cmd? _roleDeleteSafeCmd;

	#endregion
}
