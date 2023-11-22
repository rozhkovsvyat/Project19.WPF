namespace Project_19.Views;

/// <summary>
/// Представление удаления контакта
/// </summary>
public partial class ContactDeleteView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ContactDeleteView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
