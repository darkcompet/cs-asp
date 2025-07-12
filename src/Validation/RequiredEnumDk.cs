namespace Tool.Compet.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Require the value is one of the defined enum values.
/// </summary>
public class RequiredEnumDk() : RequiredAttribute {
	public override bool IsValid(object? value) {
		return DkValidators.IsValidEnumValue(value);
	}
}
