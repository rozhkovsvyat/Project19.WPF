using System.Windows;
using Ninject;

namespace Project_19;

/// <summary>
/// Содержит точку входа приложения
/// </summary>
public partial class App
{
	/// <summary>
	/// Точка входа приложения
	/// </summary>
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		new StandardKernel()
			.AddPhonebookApi()
			.AddValidators()
			.AddViews()
			.AddVms()
			.Run();
	}
}
