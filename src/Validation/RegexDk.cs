namespace Tool.Compet.Core;

using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

/// <summary>
/// It validates value must match with given regex.
/// The regex will be created with compiled option then cache, so it is faster for high-request app.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class RegexDk(string pattern, RegexOptions options = RegexOptions.Compiled | RegexOptions.CultureInvariant) : ValidationAttribute {
	/// <summary>
	/// For GET action, it is lock-free, consider same speed as normal dictionary.
	/// For ADD action, it is expensive than normal dictionary since lock fee.
	/// </summary>
	private static readonly ConcurrentDictionary<(string, RegexOptions), Regex> cache = new();

	public override bool IsValid(object? value) {
		if (value is not string s) {
			return true;
		}
		var regex = cache.GetOrAdd((pattern, options), p => new Regex(p.Item1, p.Item2));
		return regex.IsMatch(s);
	}

	public override string FormatErrorMessage(string name) {
		return $"Field '{name}' must match pattern: {pattern}";
	}
}
