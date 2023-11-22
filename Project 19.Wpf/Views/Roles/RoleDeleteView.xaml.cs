namespace Project_19.Views;

/// <summary>
/// Представление удаления роли
/// </summary>
public partial class RoleDeleteView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public RoleDeleteView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
