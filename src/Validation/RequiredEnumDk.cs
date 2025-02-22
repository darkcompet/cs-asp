namespace Tool.Compet.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Require the value is one of the enum values.
/// </summary>
public class RequiredEnumDk(bool allowNull = false) : RequiredAttribute {
	public override bool IsValid(object? value) {
		if (allowNull && value is null) {
			return true;
		}
		return DkValidators.IsValidEnumValue(value);
	}
}
