using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;

namespace Project_19.Views;

/// <summary>
/// Валидатор: электронная почта
/// </summary>
public partial class EmailValidator : ValidationRule, IValidator
{
	#region IValidator

	/// <inheritdoc/>
	public DependencyBag Options { get; set; } = new();
	/// <inheritdoc/>
	public string ErrorContent 
		=> $"The {((IValidator)this).Field} " +
		   $"{nameof(IValidator.Field).ToLower()} is not valid.";

	/// <inheritdoc cref="IValidator.Validate"/>
	public override ValidationResult Validate(object value, CultureInfo? cultureInfo)
		=> !Options.IsEnabled ? ValidationResult.ValidResult : value is string s && EmailRegex().IsMatch(s) 
			? ValidationResult.ValidResult : ((IValidator)this).InvalidResult;

	#endregion

	[GeneratedRegex("^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$")]
	private static partial Regex EmailRegex();
}
