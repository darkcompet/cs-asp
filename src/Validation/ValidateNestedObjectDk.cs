using System.ComponentModel.DataAnnotations;

namespace Tool.Compet.Core;

/// <summary>
/// Use this when we validate manually via `Validator.TryValidateObject()`.
/// This will tell dotnet validate our nested object too.
/// </summary>
public class ValidateNestedObjectDk : ValidationAttribute {
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
		if (value is null) {
			return null;
		}

		var results = new List<ValidationResult>();
		var context = new ValidationContext(value, null, null);

		// Validate each object
		if (value is Array objs) {
			foreach (var item in objs) {
				var result = this.IsValid(item, context);
				if (result != null) {
					results.Add(result);
				}
			}
		}
		// Validate single object
		else {
			Validator.TryValidateObject(value, context, results, true);
		}

		if (results.Count > 0) {
			var compositeResults = new CompositeValidationResult($"Validation for `{validationContext.DisplayName}` failed !");
			results.ForEach(compositeResults.AddResult);

			return compositeResults;
		}

		return ValidationResult.Success;
	}
}

public class CompositeValidationResult : ValidationResult {
	public CompositeValidationResult(string errorMessage) : base(errorMessage) { }

	public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }

	protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

	private readonly List<ValidationResult> results = [];

	public void AddResult(ValidationResult validationResult) {
		this.results.Add(validationResult);
	}
}
