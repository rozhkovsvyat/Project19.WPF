namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных ошибки соединения
/// </summary>
public class ConnectionErrorVm : Vm
{
	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public ConnectionErrorVm(IVm owner, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(ConnectionErrorVm)), owner) { }
}
