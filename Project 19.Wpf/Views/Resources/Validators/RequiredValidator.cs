using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;

namespace Project_19.Views;

/// <summary>
/// Валидатор: обязательное поле
/// </summary>
public class RequiredValidator : ValidationRule, IValidator
{
	#region IValidator

	/// <inheritdoc/>
	public DependencyBag Options { get; set; } = new();
	/// <inheritdoc/>
	public string ErrorContent 
		=> $"The {((IValidator)this).Field} " +
		   $"{nameof(IValidator.Field).ToLower()} is required.";

	/// <inheritdoc cref="IValidator.Validate"/>
	public override ValidationResult Validate(object value, CultureInfo? cultureInfo)
		=> !Options.IsEnabled ? ValidationResult.ValidResult : value is string s && !string.IsNullOrEmpty(s)
			? ValidationResult.ValidResult : ((IValidator)this).InvalidResult;

	#endregion
}
