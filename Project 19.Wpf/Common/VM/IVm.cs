using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_19;

/// <summary>
/// Предоставляет свойства и методы контекста данных <see cref="IView"/>
/// </summary>
public interface IVm
{
	/// <summary>
	/// Связанное представление <see cref="IView"/>
	/// </summary>
	IView View { get; }
	/// <summary>
	/// Текущий пользователь
	/// </summary>
	ClaimsPrincipal User { get; }
	/// <summary>
	/// Родительский элемент <see cref="IVm"/>
	/// </summary>
	IVm? Owner { get; }
	/// <summary>
	/// Токен
	/// </summary>
	string? Token { get; }

	/// <summary>
	/// Возвращает контекст авторизации
	/// </summary>
	/// <typeparam name="T">Тип <see cref="IVm"/> для возврата</typeparam>
	/// <param name="arg">Аргумент возврата</param>
	/// <returns>Элемент <see cref="IVm"/></returns>
	Task<IVm> AuthorizeAsync<T>(object? arg = null) where T : IVm;
	/// <summary>
	/// Устанавливает контекст авторизации
	/// </summary>
	/// <typeparam name="T">Тип <see cref="IVm"/> для возврата</typeparam>
	/// <param name="arg">Аргумент возврата</param>
	void Authorize<T>(object? arg = null) where T : IVm;
	/// <summary>
	/// Возвращает контекст отсутствия данных
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	/// <returns>Элемент NotFound типа <see cref="IVm"/></returns>
	Task<IVm> NotFoundAsync(object? arg = null);
	/// <summary>
	/// Устанавливает контекст отсутствия данных
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	void NotFound(object? arg = null);
	/// <summary>
	/// Возвращает контекст ошибки сервера
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	/// <returns>Элемент <see cref="IVm"/></returns>
	Task<IVm> ServerErrorAsync(object? arg = null);
	/// <summary>
	/// Устанавливает контекст ошибки сервера
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	void ServerError(object? arg = null);
	/// <summary>
	/// Возвращает контекст ошибки соединения
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	/// <returns>Элемент <see cref="IVm"/></returns>
	Task<IVm> ConnectionErrorAsync(object? arg = null);
	/// <summary>
	/// Устанавливает контекст ошибки соединения
	/// </summary>
	/// <param name="arg">Аргумент возврата</param>
	void ConnectionError(object? arg = null);
	/// <summary>
	/// Возвращает инициализированный контекст
	/// </summary>
	/// <param name="arg">Аргумент инициализации</param>
	/// <returns>Элемент <see cref="IVm"/></returns>
	Task<IVm> InitializedAsync(object? arg = null);
	/// <summary>
	/// Возвращает контекст
	/// </summary>
	/// <returns>Элемент <see cref="IVm"/></returns>
	public async Task<IVm> Async()
	{
		await Task.CompletedTask;
		return this;
	}
	/// <summary>
	/// <inheritdoc cref="IView.Display"/>
	/// </summary>
	/// <returns>Элемент <see cref="IVm"/></returns>
	public IVm Display()
	{
		View.Display();
		return this;
	}
}

/// <summary>
/// Базовая реализация <see cref="IVm"/>
/// </summary>
public class DefaultVm : IVm 
{
	#region IVm

	/// <inheritdoc/>
	public IView View => new DefaultView();

	/// <inheritdoc/>
	public ClaimsPrincipal User => new();

	/// <inheritdoc/>
	public IVm? Owner => null;
	/// <inheritdoc/>
	public string? Token { get; set; } = null;

	/// <inheritdoc/>
	public async Task<IVm> AuthorizeAsync<T>(object? arg) where T : IVm 
		=> await (this as IVm).Async();

	/// <inheritdoc/>
	public void Authorize<T>(object? arg = null) where T : IVm { }

	/// <inheritdoc/>
	public async Task<IVm> NotFoundAsync(object? arg = null) 
		=> await (this as IVm).Async();

	/// <inheritdoc/>
	public void NotFound(object? arg = null) { }

	/// <inheritdoc/>
	public async Task<IVm> ServerErrorAsync(object? arg = null) 
		=> await (this as IVm).Async();

	/// <inheritdoc/>
	public void ServerError(object? arg = null) { }

	/// <inheritdoc/>
	public async Task<IVm> ConnectionErrorAsync(object? arg = null)
		=> await(this as IVm).Async();

	/// <inheritdoc/>
	public void ConnectionError(object? arg = null) { }

	/// <inheritdoc/>
	public async Task<IVm> InitializedAsync(object? arg) 
		=> await (this as IVm).Async();

	#endregion
}
