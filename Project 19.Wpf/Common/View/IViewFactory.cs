namespace Project_19;

/// <summary>
/// Предоставляет методы и свойства фабрики элементов <see cref="IView"/>
/// </summary>
public interface IViewFactory : IFactory<IView> { }

/// <summary>
/// Базовая реализация <see cref="IViewFactory"/>
/// </summary>
public class DefaultViewFactory : IViewFactory
{
	public IView Get(string title) => new DefaultView();
}
