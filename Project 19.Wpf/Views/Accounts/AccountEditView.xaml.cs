namespace Project_19.Views;

/// <summary>
/// Представление изменения аккаунта
/// </summary>
public partial class AccountEditView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public AccountEditView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
