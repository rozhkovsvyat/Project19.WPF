namespace Project_19.Views;

/// <summary>
/// Представление добавления контакта
/// </summary>
public partial class ContactCreateView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ContactCreateView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
