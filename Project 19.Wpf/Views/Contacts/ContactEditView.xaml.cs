namespace Project_19.Views;

/// <summary>
/// Представление изменения контакта
/// </summary>
public partial class ContactEditView
{
	/// <summary>
	/// Пустой конструктор
	/// </summary>
	public ContactEditView()
	{
		InitializeComponent();
		OnUpdate += Form.ScrollToTop;
	}
}
