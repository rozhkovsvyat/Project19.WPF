namespace Project_19.Views;

/// <summary>
/// Представление смены пароля аккаунта
/// </summary>
public partial class ChangePasswordView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ChangePasswordView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
