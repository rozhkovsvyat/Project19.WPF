namespace Project_19.Views;

/// <summary>
/// Представление изменения роли
/// </summary>
public partial class RoleEditView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public RoleEditView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
