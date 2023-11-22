namespace Project_19.Views;

/// <summary>
/// Представление добавления роли
/// </summary>
public partial class RoleCreateView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public RoleCreateView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
