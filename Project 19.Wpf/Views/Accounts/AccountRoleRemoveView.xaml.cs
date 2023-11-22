namespace Project_19.Views;

/// <summary>
/// Представление удаления роли у аккаунта
/// </summary>
public partial class AccountRoleRemoveView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountRoleRemoveView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
