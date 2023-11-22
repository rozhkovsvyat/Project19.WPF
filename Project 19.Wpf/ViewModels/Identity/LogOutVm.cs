using System.Threading.Tasks;
using System.Windows.Input;
using System;

using Project_19.Models;

namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных выхода из <see cref="Account"/>
/// </summary>
public class LogOutVm : Vm
{
	#region Vm

	/// <inheritdoc/>
	public override async Task<IVm> InitializedAsync(object? arg = null)
	{
		SubmitCmd.Execute(arg);
		return await base.InitializedAsync();
	}

	#endregion

	/// <summary>
	/// Минимальная продолжительность выхода из <see cref="Account"/>
	/// </summary>
	private const int MinDurationInMs = 500;

	/// <inheritdoc cref="IIdentity"/>
	private readonly IIdentity _identity;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	/// <param name="identity">Идентификация</param>
	public LogOutVm(IViewFactory viewFactory, IVm owner, IIdentity identity) 
		: base(viewFactory.Get(nameof(LogOutVm)), owner) => _identity = identity;

	/// <summary>
	/// Подтверждает отправку формы
	/// </summary>
	public Cmd SubmitCmd => _submitCmd 
		??= new Cmd(o => 
		{
			App.Dispatcher.InvokeAsync(async () => 
			{
				View.Lock(true);

				try
				{
					await Task.WhenAll(_identity.SignOutAsync(),
						Task.Delay(TimeSpan.FromMilliseconds(MinDurationInMs)));
					o.TryExecute();
				}

				catch (Exception ) { ConnectionError(); }

				View.Lock(false);
			});
		}); 
	private Cmd? _submitCmd; 
}
