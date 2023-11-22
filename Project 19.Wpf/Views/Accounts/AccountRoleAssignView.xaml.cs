namespace Project_19.Views;

/// <summary>
/// Представление назначения роли аккаунту
/// </summary>
public partial class AccountRoleAssignView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountRoleAssignView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
