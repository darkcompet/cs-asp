namespace Tool.Compet.Core;

using System.ComponentModel.DataAnnotations;

/// Src: https://gist.github.com/kinetiq/faed1e3b2da4cca922896d1f7cdcc79b
/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validator.tryvalidateobject?view=net-6.0#System_ComponentModel_DataAnnotations_Validator_TryValidateObject_System_Object_System_ComponentModel_DataAnnotations_ValidationContext_System_Collections_Generic_ICollection_System_ComponentModel_DataAnnotations_ValidationResult__System_Boolean_
public class DkValidators {
	/// <summary>
	/// Validate the model and return a response, which includes any validation messages and an IsValid bit.
	/// </summary>
	public static ValidationResponse Validate(object model) {
		var context = new ValidationContext(model);
		var errors = new List<ValidationResult>();

		var valid = Validator.TryValidateObject(model, context, errors, true);

		return new ValidationResponse(valid: valid, errors: errors);
	}

	/// <summary>
	/// Validate the model and return a bit indicating whether the model is valid or not.
	/// </summary>
	public static bool IsValid(object model) {
		return Validate(model).valid;
	}

	/// <summary>
	/// Check given value is defined in the enum (value's type).
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool IsValidEnumValue(object? value) {
		if (value is null) {
			return false;
		}
		var type = value.GetType();
		return type.IsEnum && Enum.IsDefined(type, value);
	}
}

public class ValidationResponse(bool valid, List<ValidationResult> errors) {
	public bool valid = valid;
	public List<ValidationResult> errors = errors;
}
