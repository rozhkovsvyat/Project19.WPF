namespace Project_19.Views;

/// <summary>
/// Представление смены пароля аккаунта
/// </summary>
public partial class AccountChangePasswordView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountChangePasswordView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
