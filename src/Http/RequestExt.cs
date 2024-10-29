namespace Tool.Compet.Core;

/// <summary>
/// Extension for HTTP request.
/// </summary>
public static class RequestExt {
	/// Try to get request ip from various methods.
	/// @returns Client ip address or null if not found.
	public static string? GetClientIpDk(this HttpRequest request, bool tryUseXForwardHeader = true) {
		string? ip = null;

		// X-Forwarded-For (csv list):  Using the First entry in the list seems to work
		// for 99% of cases however it has been suggested that a better (although tedious)
		// approach might be to read each IP from right to left and use the first public IP.
		// http://stackoverflow.com/a/43554000/538763
		if (tryUseXForwardHeader) {
			ip = _GetHeaderValue(request, "X-Forwarded-For");
		}

		// Try with header entry: REMOTE_ADDR
		if (string.IsNullOrWhiteSpace(ip)) {
			ip = _GetHeaderValue(request, "REMOTE_ADDR");
		}

		// This will return localhost address when working locally.
		// This should be final try !
		if (string.IsNullOrWhiteSpace(ip)) {
			ip = request.HttpContext.Connection.RemoteIpAddress?.ToString();
		}

		return ip;
	}

	public static string? GetUserAgentDk(this HttpRequest request) {
		return _GetHeaderValue(request, "User-Agent");
	}

	private static string? _GetHeaderValue(HttpRequest request, string headerName) {
		if (request.HttpContext.Request.Headers.TryGetValue(headerName, out var values)) {
			return values.First();

			// // Writes out as Csv when there are multiple.
			// string rawValues = values.ToString();

			// if (!string.IsNullOrWhiteSpace(rawValues)) {
			// 	// return Convert.ChangeType(values.ToString(), typeof(string));
			// 	return values.ToString();
			// }
		}

		return null;
	}

	// private static List<string> _SplitCsv(string csvList) {
	// 	if (string.IsNullOrWhiteSpace(csvList)) {
	// 		return new List<string>();
	// 	}

	// 	return csvList
	// 		.TrimEnd(',')
	// 		.Split(',')
	// 		.AsEnumerable<string>()
	// 		.Select(s => s.Trim())
	// 		.ToList();
	// }
}
