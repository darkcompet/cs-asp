namespace Tool.Compet.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Require the GUID value is not empty.
/// </summary>
public class RequiredGuidDk() : RequiredAttribute {
	public override bool IsValid(object? value) {
		if (value is Guid guidValue) {
			return guidValue != Guid.Empty;
		}
		return false;
	}
}
