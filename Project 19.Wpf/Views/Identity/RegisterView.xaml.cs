namespace Project_19.Views;

/// <summary>
/// Представление регистрации аккаунта
/// </summary>
public partial class RegisterView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public RegisterView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
