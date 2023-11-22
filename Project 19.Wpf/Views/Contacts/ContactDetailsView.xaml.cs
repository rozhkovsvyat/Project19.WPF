namespace Project_19.Views;

/// <summary>
/// Представление деталей контакта
/// </summary>
public partial class ContactDetailsView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ContactDetailsView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
