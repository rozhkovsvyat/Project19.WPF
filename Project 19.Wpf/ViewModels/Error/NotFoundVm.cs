namespace Project_19.ViewModels;

/// <summary>
/// Контекст данных отсутствия элемента
/// </summary>
public class NotFoundVm : Vm
{
	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="owner">Родительский элемент <see cref="IVm"/></param>
	/// <param name="viewFactory">Фабрика <see cref="IView"/></param>
	public NotFoundVm(IVm owner, IViewFactory viewFactory) 
		: base(viewFactory.Get(nameof(NotFoundVm)), owner) { }
}
