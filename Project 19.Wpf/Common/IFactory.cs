namespace Project_19;

/// <summary>
/// Предоставляет методы и свойства фабрики элементов <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">Тип элементов фабрики</typeparam>
public interface IFactory<out T>
{
	/// <summary>
	/// Возвращает новый элемент <typeparamref name="T"/>
	/// </summary>
	/// <param name="title">Метка элемента</param>
	/// <returns>Новый элемент <typeparamref name="T"/></returns>
	T Get(string title);
}
