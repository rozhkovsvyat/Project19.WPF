namespace Project_19.Views;

/// <summary>
/// Представление удаления аккаунта
/// </summary>
public partial class AccountDeleteView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountDeleteView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
