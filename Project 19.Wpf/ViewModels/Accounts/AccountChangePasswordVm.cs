using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных смены пароля <see cref="Account"/>
/// </summary>
public class AccountChangePasswordVm : ChangePasswordVm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin") || arg is not string id) return await NotFoundAsync();

		try
		{
			Account = await Identity.AddToken(Token).GetByIdAsync(id) ?? throw new KeyNotFoundException();
			return await base.InitializedAsync(Account);
		}

		catch (AuthenticationException) { return await AuthorizeAsync<AccountChangePasswordVm>(arg); }

		catch (KeyNotFoundException) { return await NotFoundAsync(); }

		catch (Exception) { return await ConnectionErrorAsync(); }
	}

	#endregion

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
		}
	}
	private Account? _account;

	/// <inheritdoc/>
	public AccountChangePasswordVm(IViewFactory viewFactory, IVm owner, IIdentity identity,
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(AccountChangePasswordVm)),
		owner, identity, validatorFactory) { }

	/// <inheritdoc/>
	public override Cmd SubmitCmd 
		=> _submitCmd ??= new Cmd(o =>
		{
			if (InValidation) return;

			App.Dispatcher.InvokeAsync(async () =>
			{
				View.Lock(true);

				try
				{
					await Identity.AddToken(Token).UpdatePasswordAsync(Form);
					View.Lock(false);
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

				catch (AuthenticationException) { Authorize<ChangePasswordVm>(Account.Id); }

				catch (KeyNotFoundException) { NotFound(); }

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
