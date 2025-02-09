namespace Tool.Compet.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validate the value is element in given values.
/// </summary>
public class OneOfDk(params object[] values) : ValidationAttribute {
	private readonly object[] values = values;

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
		foreach (var element in this.values) {
			if (element == value) {
				return ValidationResult.Success;
			}
		}

		return new ValidationResult($"Given {value} is not found in required values");
	}
}
