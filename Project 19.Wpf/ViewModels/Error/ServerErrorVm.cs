namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных внутренней ошибки
/// </summary>
public class ServerErrorVm : Vm
{
	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public ServerErrorVm(IVm owner, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(ServerErrorVm)), owner) { }
}
