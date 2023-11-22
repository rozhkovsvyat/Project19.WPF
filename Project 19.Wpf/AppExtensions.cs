// ReSharper disable SuggestBaseTypeForParameter
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using Ninject;
using System;
using Tools;

using Project_19.ViewModels;
using Project_19.Services;
using Project_19.Models;
using Project_19.Views;

namespace Project_19;

/// <summary>
/// Содержит методы расширения <see cref="App"/>
/// </summary>
public static class AppExtensions
{
	/// <inheritdoc cref="Project_19.App"/>
	private static Application App => Application.Current;

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> сервисы Phonebook Api
	/// </summary>
	/// <param name="kernel"></param>
	public static IKernel AddPhonebookApi(this IKernel kernel) 
		=> kernel.AddApiContacts().AddApiIdentity();

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> поставщика модели контактов
	/// </summary>
	/// <param name="kernel"></param>
	private static IKernel AddApiContacts(this IKernel kernel)
	{
		kernel.Bind<IContacts>().To<ApiContacts>().InTransientScope()
			.WithConstructorArgument(ConfigurationManager.ConnectionStrings[nameof(ApiContacts)].ConnectionString ??
			                         throw new InvalidOperationException($"Connection string {nameof(ApiContacts)} not found."));
		return kernel;
	}

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> поставщика сервиса аутентификации
	/// </summary>
	/// <param name="kernel"></param>
	private static IKernel AddApiIdentity(this IKernel kernel)
	{
		kernel.Bind<IIdentity>().To<ApiIdentity>().InTransientScope()
			.WithConstructorArgument(ConfigurationManager.ConnectionStrings[nameof(ApiIdentity)].ConnectionString ??
			                         throw new InvalidOperationException($"Connection string {nameof(ApiIdentity)} not found."));
		return kernel;
	}

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> фабрику валидаторов
	/// </summary>
	/// <param name="kernel"></param>
	public static IKernel AddValidators(this IKernel kernel)
	{
		var recipes = new List<KeyValuePair<string, Recipe<IValidator>>>
		{
			new (nameof(ValidationType.Required), kernel.Return<RequiredValidator>),
			new (nameof(ValidationType.Compared), kernel.Return<ComparedValidator>),
			new (nameof(ValidationType.Email), kernel.Return<EmailValidator>)
		};

		kernel.Bind<IValidatorFactory>().To<ValidatorFactory>()
			.InTransientScope().WithConstructorArgument(nameof(recipes), recipes);

		return kernel;
	}

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> фабрику представлений
	/// </summary>
	/// <param name="kernel"></param>
	public static IKernel AddViews(this IKernel kernel)
	{
		var recipes = new List<KeyValuePair<string, Recipe<IView>>>
		{
			new (nameof(LayoutVm), kernel.Return<LayoutView>),
			new (nameof(ContactsVm), kernel.Return<ContactsView>),
			new (nameof(RegisterVm), kernel.Return<RegisterView>),
			new (nameof(SignInVm), kernel.Return<SignInView>),
			new (nameof(LogOutVm), kernel.Return<LogOutView>),
			new (nameof(NotFoundVm), kernel.Return<NotFoundView>),
			new (nameof(ServerErrorVm), kernel.Return<ServerErrorView>),
			new (nameof(ConnectionErrorVm), kernel.Return<ConnectionErrorView>),
			new (nameof(ContactDetailsVm), kernel.Return<ContactDetailsView>),
			new (nameof(ContactEditVm), kernel.Return<ContactEditView>),
			new (nameof(ContactCreateVm), kernel.Return<ContactCreateView>),
			new (nameof(ContactDeleteVm), kernel.Return<ContactDeleteView>),
			new (nameof(ChangePasswordVm), kernel.Return<ChangePasswordView>),
			new (nameof(AccountsVm), kernel.Return<AccountsView>),
			new (nameof(AccountEditVm), kernel.Return<AccountEditView>),
			new (nameof(AccountCreateVm), kernel.Return<AccountCreateView>),
			new (nameof(AccountDetailsVm), kernel.Return<AccountDetailsView>),
			new (nameof(AccountDeleteVm), kernel.Return<AccountDeleteView>),
			new (nameof(AccountChangePasswordVm), kernel.Return<AccountChangePasswordView>),
			new (nameof(AccountRolesVm), kernel.Return<AccountRolesView>),
			new (nameof(AccountRoleAssignVm), kernel.Return<AccountRoleAssignView>),
			new (nameof(AccountRoleRemoveVm), kernel.Return<AccountRoleRemoveView>),
			new (nameof(RolesVm), kernel.Return<RolesView>),
			new (nameof(RoleCreateVm), kernel.Return<RoleCreateView>),
			new (nameof(RoleDetailsVm), kernel.Return<RoleDetailsView>),
			new (nameof(RoleEditVm), kernel.Return<RoleEditView>),
			new (nameof(RoleDeleteVm), kernel.Return<RoleDeleteView>)
		};

		kernel.Bind<IViewFactory>().To<ViewFactory>()
			.InTransientScope().WithConstructorArgument(nameof(recipes), recipes);

		return kernel;
	}

	/// <summary>
	/// Внедряет в <see cref="IKernel"/> фабрику моделей представлений
	/// </summary>
	/// <param name="kernel"></param>
	public static IKernel AddVms(this IKernel kernel)
	{
		kernel.Bind<IVm>().To<LayoutVm>().InSingletonScope();

		var recipes = new List<KeyValuePair<string, Recipe<IVm>>>
		{
			new (nameof(ContactsVm), kernel.Return<ContactsVm>),
			new (nameof(RegisterVm), kernel.Return<RegisterVm>),
			new (nameof(SignInVm), kernel.Return<SignInVm>),
			new (nameof(LogOutVm), kernel.Return<LogOutVm>),
			new (nameof(NotFoundVm), kernel.Return<NotFoundVm>),
			new (nameof(ServerErrorVm), kernel.Return<ServerErrorVm>),
			new (nameof(ConnectionErrorVm), kernel.Return<ConnectionErrorVm>),
			new (nameof(ContactDetailsVm), kernel.Return<ContactDetailsVm>),
			new (nameof(ContactEditVm), kernel.Return<ContactEditVm>),
			new (nameof(ContactCreateVm), kernel.Return<ContactCreateVm>),
			new (nameof(ContactDeleteVm), kernel.Return<ContactDeleteVm>),
			new (nameof(ChangePasswordVm), kernel.Return<ChangePasswordVm>),
			new (nameof(AccountsVm), kernel.Return<AccountsVm>),
			new (nameof(AccountEditVm), kernel.Return<AccountEditVm>),
			new (nameof(AccountCreateVm), kernel.Return<AccountCreateVm>),
			new (nameof(AccountDetailsVm), kernel.Return<AccountDetailsVm>),
			new (nameof(AccountDeleteVm), kernel.Return<AccountDeleteVm>),
			new (nameof(AccountChangePasswordVm), kernel.Return<AccountChangePasswordVm>),
			new (nameof(AccountRolesVm), kernel.Return<AccountRolesVm>),
			new (nameof(AccountRoleAssignVm), kernel.Return<AccountRoleAssignVm>),
			new (nameof(AccountRoleRemoveVm), kernel.Return<AccountRoleRemoveVm>),
			new (nameof(RolesVm), kernel.Return<RolesVm>),
			new (nameof(RoleCreateVm), kernel.Return<RoleCreateVm>),
			new (nameof(RoleDetailsVm), kernel.Return<RoleDetailsVm>),
			new (nameof(RoleEditVm), kernel.Return<RoleEditVm>),
			new (nameof(RoleDeleteVm), kernel.Return<RoleDeleteVm>)
		};

		kernel.Bind<IVmFactory>().To<VmFactory>()
			.InTransientScope().WithConstructorArgument(nameof(recipes), recipes);

		return kernel;
	}

	/// <summary>
	/// Отображает главное представление <see cref="IView"/>
	/// </summary>
	/// <param name="kernel"></param>
	public static void Run(this IKernel kernel)
		=> App.Dispatcher.InvokeAsync(async () 
			=> await RunAsync(kernel));

	/// <summary>
	/// Отображает главное представление <see cref="IView"/>
	/// </summary>
	/// <param name="kernel"></param>
	private static async Task RunAsync(this IKernel kernel) =>
		App.MainWindow = (await await App.Dispatcher.InvokeAsync(async () 
			=> await kernel.Get<IVm>().InitializedAsync())).View.ToWindow();

	/// <summary>
	/// Приводит <see cref="IView"/> к типу <see cref="Window"/>
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	private static Window ToWindow(this IView view)
		=> view as Window ?? throw new InvalidOperationException
			($"{nameof(IView)} is not a {nameof(Window)}");

	/// <summary>
	/// Возвращает объект указанного типа используя текущий <see cref="System.Windows.Threading.Dispatcher"/>
	/// </summary>
	/// <typeparam name="T">Тип возвращаемого объекта</typeparam>
	/// <param name="kernel"></param>
	private static T Return<T>(this IKernel kernel)
		=> App.Dispatcher.Invoke(() => kernel.Get<T>());
}
