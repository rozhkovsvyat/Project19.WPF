namespace Project_19.Views;

/// <summary>
/// Представление добавления аккаунта
/// </summary>
public partial class AccountCreateView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountCreateView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
