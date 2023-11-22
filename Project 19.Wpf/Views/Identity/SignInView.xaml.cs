namespace Project_19.Views;

/// <summary>
/// Представление входа в аккаунт
/// </summary>
public partial class SignInView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public SignInView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
