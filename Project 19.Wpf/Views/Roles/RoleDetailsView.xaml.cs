namespace Project_19.Views;

/// <summary>
/// Представление деталей роли
/// </summary>
public partial class RoleDetailsView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public RoleDetailsView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
