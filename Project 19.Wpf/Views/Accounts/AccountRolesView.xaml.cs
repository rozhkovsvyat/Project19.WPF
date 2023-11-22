namespace Project_19.Views;

/// <summary>
/// Представление коллекции ролей аккаунта
/// </summary>
public partial class AccountRolesView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountRolesView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
