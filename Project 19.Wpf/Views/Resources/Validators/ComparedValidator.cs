using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;

namespace Project_19.Views;

/// <summary>
/// Валидатор: сравнение полей
/// </summary>
public class ComparedValidator : ValidationRule, IValidator
{
	#region IValidator

	/// <inheritdoc/>
	public DependencyBag Options { get; set; } = new();
	/// <inheritdoc/>
	public string ErrorContent => $"{((IValidator)this).Field} mismatch.";

	/// <inheritdoc cref="IValidator.Validate"/>
	public override ValidationResult Validate(object value, CultureInfo? cultureInfo)
		=> !Options.IsEnabled || Options.CompareWith is null ? ValidationResult.ValidResult 
			: value is string sA && Options.CompareWith is string sB && sA == sB ? ValidationResult.ValidResult 
			: ((IValidator)this).InvalidResult;

	#endregion
}
