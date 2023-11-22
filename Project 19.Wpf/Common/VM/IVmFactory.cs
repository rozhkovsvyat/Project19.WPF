namespace Project_19;

/// <summary>
/// Предоставляет методы и свойства фабрики элементов <see cref="IVm"/>
/// </summary>
public interface IVmFactory : IFactory<IVm> { }

/// <summary>
/// Базовая реализация <see cref="IVmFactory"/>
/// </summary>
public class DefaultVmFactory : IVmFactory
{
	public IVm Get(string title) => new DefaultVm();
}
