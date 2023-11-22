namespace Project_19.Views;

/// <summary>
/// Представление деталей аккаунта
/// </summary>
public partial class AccountDetailsView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountDetailsView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
