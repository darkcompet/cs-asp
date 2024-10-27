namespace Tool.Compet.Core;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

/// <summary>
/// Require min value.
/// </summary>
public class MinDk : ValidationAttribute {
	private object minValue;

	/// Target type for comparision. Since client can specify the type of comparision,
	/// so this does NOT need match with the type of minValue.
	private readonly Type operandType;

	/// To convert any object to object of operandType
	private Func<object, object>? operandConverter;

	/// NOTE: when add new ctor, must add operand-converter at `this.SetupOperandConverterThrow()`
	public MinDk(int minimum) {
		this.minValue = minimum;
		this.operandType = typeof(int);
	}

	/// NOTE: when add new ctor, must add operand-converter at `this.SetupOperandConverterThrow()`
	public MinDk(double minimum) {
		this.minValue = minimum;
		this.operandType = typeof(double);
	}

	/// <summary>
	/// Allows for specifying range for arbitrary types.
	/// The minimum and maximum strings will be converted to the target type.
	/// </summary>
	/// <param name="minimum">The minimum allowable value.</param>
	/// <param name="type">The type of the range parameters. Must implement IComparable.</param>
	public MinDk(Type type, string minimum) {
		this.minValue = minimum;
		this.operandType = type;
	}

	/// Calculate converter that converts any type to the minValue.
	/// For eg,. when validation value is string type, and the minValue is int type,
	/// this will return converter that converts string to int.
	private Func<object, object> SetupOperandConverterOrThrow() {
		var oprConverter = this.operandConverter;
		if (oprConverter != null) {
			return oprConverter;
		}

		if (this.minValue == null) {
			throw new InvalidOperationException("Minimum value cannot be null");
		}

		// It does:
		// 1. Make operand-converter for any value
		// 2. Convert our values with operand-converter
		Type minValueType = this.minValue.GetType();

		// When value is int: make converter that converts validation-value to int
		if (minValueType == typeof(int)) {
			oprConverter = v => Convert.ToInt32(v, CultureInfo.InvariantCulture);
		}
		// When value is double: make converter that converts validation-value to double
		else if (minValueType == typeof(double)) {
			oprConverter = v => Convert.ToDouble(v, CultureInfo.InvariantCulture);
		}
		// When value is string: make converter that converts validation-value to string
		else {
			// Now, minValue must be in string.
			var minValueInString = (string)this.minValue;

			// Validate operand
			if (!typeof(IComparable).IsAssignableFrom(this.operandType)) {
				throw new InvalidOperationException($"Operand type `{this.operandType}` must be convertable to IComparable");
			}

			var oprTypeConverter = TypeDescriptor.GetConverter(this.operandType);
			oprConverter = v => (v != null && v.GetType() == this.operandType) ? v : oprTypeConverter.ConvertFrom(v)!;

			// Convert minValue with operand-converter
			this.minValue = (IComparable)oprTypeConverter.ConvertFromString(minValueInString)!;
		}

		this.operandConverter = oprConverter;

		return oprConverter;
	}

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
		try {
			// Validate our properties and create the conversion function
			var ConvertWithOperandType = this.SetupOperandConverterOrThrow();

			// Pass if value is null or empty since [RequiredAttribute] should be used to assert a value is not empty.
			if (value == null) {
				return new ValidationResult($"Must at least {this.minValue}");
			}
			if (value is string s && s.Length == 0) {
				return new ValidationResult($"Must at least {this.minValue}");
			}

			// Convert the value (string,...) to our target value (float,...)
			var convertedValue = ConvertWithOperandType(value);

			if (((IComparable)this.minValue).CompareTo(convertedValue) <= 0) {
				return ValidationResult.Success;
			}
		}
		catch {
		}

		return new ValidationResult($"Value must be at least {this.minValue}");
	}
}
