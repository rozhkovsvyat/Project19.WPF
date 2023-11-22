using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных добавления элемента типа <see cref="Account"/>
/// </summary>
public class AccountCreateVm : RegisterVm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		if (!User.IsInRole("admin")) return await NotFoundAsync();
		return await base.InitializedAsync(arg);
	}

	#endregion

	/// <inheritdoc/>
	public AccountCreateVm(IViewFactory viewFactory, IVm owner, IIdentity identity, 
		IValidatorFactory validatorFactory) : base(viewFactory.Get(nameof(AccountCreateVm)), 
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
					await Identity.AddAsync(Form);
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
					Authorize<AccountCreateVm>
						(ReturnCmd);
				}

				catch (Exception) { ConnectionError(); }

				View.Lock(false);
			});
		});
	private Cmd? _submitCmd;
}
