using System.ComponentModel.DataAnnotations;

namespace Tool.Compet.Core;

/// <summary>
/// Src: https://gist.github.com/kinetiq/faed1e3b2da4cca922896d1f7cdcc79b
/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validator.tryvalidateobject?view=net-6.0#System_ComponentModel_DataAnnotations_Validator_TryValidateObject_System_Object_System_ComponentModel_DataAnnotations_ValidationContext_System_Collections_Generic_ICollection_System_ComponentModel_DataAnnotations_ValidationResult__System_Boolean_
/// </summary>
public class DkValidators {
	/// <summary>
	/// Manually validate the model.
	///
	/// Unfortunately, the standard behavior of `Validator.TryValidateObject()` which
	/// does not recursively validate the property values of the object.
	///
	/// If any validators are invalid, Validator.ValidateObject will abort validation and return the failure(s).
	/// </summary>
	public static (bool valid, List<ValidationResult> errors) Validate(object model) {
		var context = new ValidationContext(model);
		var errors = new List<ValidationResult>();

		var valid = Validator.TryValidateObject(model, context, errors, true);

		return (valid, errors);
	}

	/// <summary>
	/// Check given value is defined in the enum (value's type).
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool IsValidEnumValue(object? value) {
		// We can check enum with: Enum.IsDefined(type, value)
		var type = value?.GetType();
		return type != null && type.IsEnum && type.IsEnumDefined(value!);
	}
}
