using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
namespace EnterprisePortalWebAPI.Utility
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Readability", "RCS1043")]
	public static partial class StringTools
	{
		/// <inheritdoc cref="String.IsNullOrWhiteSpace(string)"/>
#if NETCOREAPP3_0_OR_GREATER
		public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string value)
#else
	public static bool IsNullOrWhiteSpace(this string value)
#endif
			=> String.IsNullOrWhiteSpace(value);

		public static string? NullIfEmpty(this string value)
			=> value.IsNullOrWhiteSpace() ? null : value.Trim();

		/// <inheritdoc cref="String.Format(string, object)"/>
		public static string Format(this object arg0, string format)
			=> String.Format(format, arg0);

		/// <inheritdoc cref="Format(object, string)"/>
		public static string ToString(this object arg0, string format)
			=> String.Format(format, arg0);

		private static readonly Type[] ParseParamTypes = [typeof(string)];

		private static readonly Dictionary<Type, MethodInfo> ParseMethodInfos = [];

		public static object Convert(this string value, Type resultType)
		{
			if (resultType == typeof(string))
				return value;

			if (resultType == typeof(bool))
				return value switch
				{
					"0" or "false" or "False" or "FALSE" => false,
					"1" or "true" or "True" or "TRUE" => true,
					_ => Boolean.Parse(value)
				};

			if (resultType == typeof(int))
				return Int32.Parse(value);

			if (resultType == typeof(double))
				return Double.Parse(value);

			if (resultType.IsEnum)
				return Enum.Parse(resultType, value, true);

			if (!ParseMethodInfos.TryGetValue(resultType, out var parseMethodInfo))
				parseMethodInfo
					= ParseMethodInfos[resultType]
					= resultType.GetMethod("Parse", ParseParamTypes)!;

			if (parseMethodInfo == null)
				return System.Convert.ChangeType(value, resultType);

			return parseMethodInfo.Invoke(null, [value])!;
		}

		public static bool TryConvert(this string value, Type type, out object? result)
		{
			result = default;
			if (value.IsNullOrWhiteSpace())
				return false;

			try
			{
				result = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
					? Convert(value, type.GetGenericArguments()[0])
					: Convert(value, type);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool TryConvert<T>(this string value, out T result)
		{
			if (value.TryConvert(typeof(T), out var output))
			{
				result = (T)output!;
				return true;
			}

			result = default!;
			return false;
		}

		public static object? Convert(this string value, object replacement, Type type)
			=> value.TryConvert(type, out var result)
				? result
				: replacement;

		public static T Convert<T>(this string value, T replacement = default!)
			=> (T)Convert(value, replacement!, typeof(T))!;

		/// <inheritdoc cref="String.IsNullOrEmpty(String)"/>
#if NETCOREAPP3_0_OR_GREATER
		public static bool IsNullOrEmpty([NotNullWhen(false)] this string value)
#else
	public static bool IsNullOrEmpty(this string value)
#endif
			=> String.IsNullOrEmpty(value);

		public static string IsNullOrEmpty(this string value, string replacement)
			=> value.IsNullOrEmpty() ? replacement : value;

		[Obsolete("Use Array.Empty<string()")]
		public static readonly string[] EmptyArray = [];

		/// <inheritdoc cref="String.Split(char[], StringSplitOptions)"/>
		public static string[] Split(string value, char[] separator, StringSplitOptions options)
			=> String.IsNullOrEmpty(value) ? [] : value.Split(separator, options);

		/// <inheritdoc cref="String.Split(char[])"/>
		public static string[] Split(string value, params char[] separator)
			=> Split(value, separator, StringSplitOptions.None);

#if NET7_0_OR_GREATER
		[GeneratedRegex("""(?m)(?<a>_)|(?<b>^[\da-z])|(?<c>(?<=[\da-z])[A-Z]+)""")]
		private static partial Regex GetSpaceCapitalsRegex();
		/// <inheritdoc cref="GetSpaceCapitalsRegex"/>
		private static readonly Regex SpaceCapitalsRegex = GetSpaceCapitalsRegex();
#else
	private static readonly Regex SpaceCapitalsRegex = new("""(?m)(?<a>_)|(?<b>^[\da-z])|(?<c>(?<=[\da-z])[A-Z]+)""");
#endif

		public static string SpaceCapitals(this string value)
			=> value.IsNullOrEmpty() ? value
			: SpaceCapitalsRegex.Replace(value, m
			=> m.Groups["a"].Success ? " "
			: m.Groups["b"].Success ? m.Value.ToUpper()
			: m.Groups["c"].Success ? " " + m.Value
			: m.Value);

		[Obsolete("Use Mid")]
		public static string Substring(string value, int startIndex, int length)
			=> value.Mid(startIndex, length)!;

		[Obsolete("Use Mid")]
		public static string Substring(string value, int startIndex)
			=> value.Mid(startIndex)!;

#nullable enable
		public static string? Mid(this string? value, int startIndex, int? length = default)
			=> value?.Substring(
				startIndex: Math.Min(startIndex, value.Length),
				length: Math.Min(length ?? value.Length, value.Length - startIndex));

		public static string? Left(this string? value, int length)
			=> value?[..Math.Min(value.Length, length)];

		public static string? Right(this string? value, int length)
			=> value?[^Math.Min(value.Length, length)..];

		public static bool RemoveIfStartsWith(this string? value, char prefix, out string? output)
			=> (value?.StartsWith(prefix) is true
			? (result: true, output = value[1..])
			: (result: false, output = value))
			.result;

		public static string? RemoveIfStartsWith(this string? value, char prefix)
			=> value.RemoveIfStartsWith(prefix, out var result) ? result : result;

		public static bool RemoveIfStartsWith(this string? value, string prefix, out string? output)
			=> (value?.StartsWith(prefix) is true
			? (result: true, output = value[prefix.Length..])
			: (result: false, output = value))
			.result;

		public static string? RemoveIfStartsWith(this string? value, string prefix)
			=> value.RemoveIfStartsWith(prefix, out var result) ? result : result;

		public static bool RemoveIfEndsWith(this string? value, string suffix, out string? output)
			=> (value?.EndsWith(suffix) is true
			? (result: true, output = value[..^suffix.Length])
			: (result: false, output = value))
			.result;

		public static string? RemoveIfEndsWith(this string? value, string suffix)
			=> value.RemoveIfEndsWith(suffix, out var result) ? result : result;
#nullable disable

		/// <inheritdoc cref="String.Equals(string, string, StringComparison)"/>
		/// <remarks>Using <see cref="StringComparison.OrdinalIgnoreCase"/></remarks>
		public static bool IEquals(this string a, string b)
			=> String.Equals(a, b, StringComparison.OrdinalIgnoreCase);

		/// <inheritdoc cref="String.Equals(object?)"/>
		/// <remarks>Using <see cref="StringComparison.OrdinalIgnoreCase"/></remarks>
		public static bool IEquals(this string a, object b)
			=> b is string s && a.IEquals(s);

		/// <inheritdoc cref="String.Equals(object?)"/>
		/// <remarks>Using <see cref="StringComparison.OrdinalIgnoreCase"/></remarks>
		public static bool IEquals(this object a, string b)
			=> a is string s && s.IEquals(b);

#if !NET5_0_OR_GREATER
	public static bool Contains(this string value, char ch)
		=> value.IndexOf(ch) >= 0;
#endif

#if NET5_0_OR_GREATER
		public static bool IContains(this string a, string b)
			=> a is not null && b is not null && a.Contains(b, StringComparison.OrdinalIgnoreCase);
#else
	public static bool IContains(this string a, string b)
		=> a is not null && b is not null && a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0;
#endif

		[Obsolete("Use a.IEquals(b)")]
		public static bool IgnoreCaseEquals(this string a, string b)
			=> a.IEquals(b);

		/// <inheritdoc cref="String.StartsWith(string)"/>
		/// <param name="a"><inheritdoc cref="String.StartsWith(string)"/></param>
		/// <param name="b"><inheritdoc cref="String.StartsWith(string)"/></param>
		/// <remarks>Any of <paramref name="a"/> or <paramref name="b"/></remarks>
		public static bool StartsWithAnyOf(this string s, string a, string b)
			=> s?.Length > 0 && (s.StartsWith(a) || s.StartsWith(b));

#if !NETCOREAPP2_0_OR_GREATER
	public static bool StartsWith(this string value, char ch)
		=> value?.Length > 0 && value[0] == ch;

	public static bool EndsWith(this string value, char ch)
		=> value?.Length > 0 && value[^1] == ch;
#endif

		/// <inheritdoc cref="String.StartsWith(string, StringComparison)"/>
		/// <remarks>using <see cref="StringComparison.OrdinalIgnoreCase"/></remarks>
		public static bool IStartsWith(this string a, string b)
			=> a.StartsWith(b, StringComparison.OrdinalIgnoreCase);

		[Obsolete("Use a.IStartsWith(b)")]
		public static bool IgnoreCaseStartsWith(this string a, string b)
			=> a.IStartsWith(b);

		public static bool IEndsWith(this string a, string b)
			=> a.EndsWith(b, StringComparison.OrdinalIgnoreCase);

		[Obsolete("Use a.IEndsWith(b)")]
		public static bool IgnoreCaseEndsWith(this string a, string b)
			=> a.IEndsWith(b);

		/// <inheritdoc cref="System.Linq.Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource, IEqualityComparer{TSource})"/>
		/// <remarks>Using <see cref="StringComparer.OrdinalIgnoreCase"/></remarks>
		public static bool IIn(this string value, IEnumerable<string> source)
			=> source.Contains(value, StringComparer.OrdinalIgnoreCase);

		/// <inheritdoc cref="IIn(string, IEnumerable{string})"/>
		[Obsolete("Use value.IIn(source)")]
		public static bool IgnoreCaseIn(this string value, IEnumerable<string> source)
			=> value.IIn(source);

		/// <inheritdoc cref="IIn(string, IEnumerable{string})"/>
		[Obsolete("Use IEquals")]
		public static bool IIn(this string value, string item1)
			=> value.IEquals(item1);

		/// <inheritdoc cref="IIn(string, IEnumerable{string})"/>
		public static bool IIn(this string value, string item1, string item2)
			=> value.IIn([item1, item2]);

		/// <inheritdoc cref="IIn(string, IEnumerable{string})"/>
		public static bool IIn(this string value, string item1, string item2, string item3)
			=> value.IIn([item1, item2, item3]);

		public static bool IIn(this string value, ReadOnlySpan<string> source)
		{
			foreach (var item in source)
				if (value.IEquals(item))
					return true;

			return false;
		}

		/// <inheritdoc cref="IIn(string, IEnumerable{string})"/>
		public static bool IIn(this string value, params string[] source)
			=> value.IIn(source.AsSpan());

		[Obsolete("Use value.IIn(list)")]
		public static bool IgnoreCaseIn(this string value, params string[] list)
			=> value.IIn(list.AsSpan());

#if NET2x || NET3x
	public static string IsNullOrWhiteSpace(this string value, string replacement)
#else
		public static string IsNullOrWhiteSpace(string value, string replacement)
#endif
			=> value.IsNullOrWhiteSpace() ? replacement : value;

		public static string Replace(string value, string oldValue, string newValue)
			=> value.IsNullOrEmpty() ? value : value.Replace(oldValue, newValue);

		public static StringBuilder Replace(this StringBuilder sb, ReadOnlySpan<(string oldValue, string newValue)> values)
		{
			foreach (var (oldValue, newValue) in values)
				sb.Replace(oldValue, newValue);

			return sb;
		}

		public static string Replace(this string value, ReadOnlySpan<(string oldValue, string newValue)> values)
			=> value is not { Length: > 0 } || values is not { Length: > 0 } ? value
			: new StringBuilder(value).Replace(values).ToString();

		[Obsolete("Use Replace(this string value, ReadOnlySpan<(string oldValue, string newValue)> values)")]
		public static string Replace(this string value, params (string oldValue, string newValue)[] values)
			=> Replace(value, values.AsSpan());

		public static string Replace(this string value, ReadOnlySpan<string> oldValues, string newValue)
		{
			if (value == null)
				return value;

			var sb = new StringBuilder(value);

			foreach (var oldValue in oldValues)
				sb.Replace(oldValue, newValue);

			return sb.ToString();
		}

		public static void Replace(ref Span<char> value, char oldChar, char newChar)
		{
			for (var i = 0; i < value.Length; i++)
				if (value[i] == oldChar)
					value[i] = newChar;
		}

		public static void Replace(ref Span<char> value, ReadOnlySpan<char> oldChars, char newChar)
		{
			foreach (var oldChar in oldChars)
				Replace(ref value, oldChar, newChar);
		}

		public static string Replace(this string value, ReadOnlySpan<char> oldChars, char newChar)
		{
			Span<char> span = stackalloc char[value.Length];
			value.AsSpan().CopyTo(span);
			Replace(ref span, oldChars, newChar);
			return span.ToString();
		}

		public static string Replace(this string value, char newChar, params char[] oldChars)
			=> value.IsNullOrEmpty() ? value : new string(value.Select(c => oldChars.Contains(c) ? newChar : c).Where(c => c != '\0').ToArray());

		public static string FromBase64String(string value, Encoding encoding)
			=> encoding.GetString(System.Convert.FromBase64String(value));

		public static string FromBase64String(string value)
			=> FromBase64String(value, Encoding.UTF8);

		public static string ToBase64String(string value, Encoding encoding)
			=> System.Convert.ToBase64String(encoding.GetBytes(value));

		public static string ToBase64String(string value)
			=> ToBase64String(value, Encoding.UTF8);

		public static TResult Convert<TResult>(this string value, ReadOnlySpan<(string name, TResult value)> args)
		{
			foreach (var arg in args)
				if (arg.name == value)
					return arg.value;

			return (TResult)value.Convert(typeof(TResult));
		}

		public static TResult Convert<TResult>(this string value, string value1, TResult result1)
			=> value.Convert([(value1, result1)]);

		public static TResult Convert<TResult>(this string value, string value1, TResult result1, string value2, TResult result2)
			=> value.Convert([(value1, result1), (value2, result2)]);

		public static TResult Convert<TResult>(this string value, string value1, TResult result1, string value2, TResult result2, string value3, TResult result3)
			=> value.Convert([(value1, result1), (value2, result2), (value3, result3)]);

		public static T? ConvertNullable<T>(this string value, T replacement = default) where T : struct
			=> value.IsNullOrEmpty() ? null : value.Convert(replacement);

		public static string GetDescription(Type type, string fieldName)
		{
			var fi = type.GetField(fieldName);
			var attributes = fi == null ? null : (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
			return (attributes?.Length > 0) ? attributes[0].Description : fieldName;
		}

		public static string GetDescription(this Enum value)
			=> GetDescription(value.GetType(), value.ToString());

		/// <inheritdoc cref="String.Join(String, IEnumerable{String})" path="/*[not(self::param[@name='separator'])]"/>
		/// <param name="separator">
		/// <para><inheritdoc cref="String.Join(String, IEnumerable{String})." path="/param[@name='separator']"/></para>
		/// <para>Default <paramref name="separator"/> is <see cref="Environment.NewLine"/></para>
		/// </param>
		public static string Join(this IEnumerable<string> values, string separator = null)
			=> String.Join(separator ?? Environment.NewLine, values);

		/// <inheritdoc cref="Join(IEnumerable{string}, string)"/>
		public static string Join<T>(this IEnumerable<T> values, string separator = null)
			=> values.Select(item
			=> item?.ToString()).Join(separator);

		[Obsolete("Use value.Join(separator)")]
		public static string Join(string separator, IEnumerable<string> values)
			=> values.Join(separator);

		public static string Quote(this string item, string quote = "\"")
			=> quote + (item ?? String.Empty).Replace(quote, quote + quote) + quote;

		public static string Join(this IEnumerable<string> values, string separator, string quote)
			=> values.Select(item
			=> item.Quote(quote)).Join(separator);

		public static string Join<T>(this IEnumerable<T> value, string separator, string quote)
			=> value.Select(item
			=> item?.ToString()).Join(separator, quote);

		[Obsolete("Use value.Join(separator, quote)")]
		public static string Join(string separator, string quote, IEnumerable<string> value)
			=> value.Join(separator, quote);

		/// <summary>
		/// Filters out empty <see cref="string"/> elements from the input sequence of values.
		/// </summary>
		/// <param name="source"><inheritdoc cref="System.Linq.Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})" path="/param[@name='source']"/></param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains non-empty <see cref="string"/> elements from the input sequence.</returns>
		public static IEnumerable<string> NonEmpty(this IEnumerable<string> source)
			=> source.Where(item
			=> !item.IsNullOrEmpty());

		/// <inheritdoc cref="StringTools.Join(IEnumerable{String}, String)"/>
		/// <remarks>Only non-empty <see cref="string"/> elements are included</remarks>
		public static string JoinNonEmpty(this IEnumerable<string> values, string separator = null)
			=> values.NonEmpty().Join(separator);

		/// <inheritdoc cref="JoinNonEmpty(IEnumerable{String}, String)" />
		[Obsolete("Use values.JoinNonEmpty(separator)")]
		public static string JoinNonEmpty(string separator, IEnumerable<string> values)
			=> values.JoinNonEmpty(separator);

		/// <inheritdoc cref="JoinNonEmpty(IEnumerable{String}, String)" />
		public static string JoinNonEmpty(string separator, params string[] values)
			=> JoinNonEmpty(values, separator);

		/// <inheritdoc cref="JoinNonEmpty(IEnumerable{String}, String)" path="/*[not(self::summary) and not(self::param[@name='values'])]" />
		/// <inheritdoc cref="String.Join{T}(string, IEnumerable{T})" path="/*[.=self::summary or .=self::param[@name='values']]"/>
		public static string JoinNonEmpty<TValue>(this IEnumerable<TValue> values, string separator = null)
			=> values.Select(value
			=> value?.ToString()).JoinNonEmpty(separator);

		public const string Ellipsis = "...";

		public static string TrimWithEllipsis(this string value, int length)
			=> value == null || value.Length <= length ? value
			: value.Left(Math.Max(0, length - Ellipsis.Length)) + Ellipsis;

		public static string TrimLeftWithEllipsis(this string value, int length)
			=> value == null || value.Length <= length ? value
			: Ellipsis + value.Mid(value.Length - Math.Max(0, length - Ellipsis.Length));

		public static string FormatWithPrecision(this double value, int precision)
			=> String.Format("{0:F" + precision + "}", value);

		public static IEnumerable<string> ReadLines(this string value)
		{
			using var reader = new StringReader(value);
			for (var s = reader.ReadLine(); s != null; s = reader.ReadLine())
				yield return s;
		}

		public static string AsString(this TimeSpan value)
			=> $"{(value.Days == 0 ? null : $"{value.Days}.")}{value.Hours:00}:{value.Minutes:00}:{value.Seconds:00}";

		public static char At(this string value, int index, char replacement = default)
			=> index >= 0 && value?.Length > index ? value[index] : replacement;

		/// <inheritdoc cref="Convert.ToByte(string, int)"/>
		/// <remarks>Using base <c>16</c></remarks>
		public static IEnumerable<byte> HexToBytes(this string hex)
			=> from ci in hex.Select((c, i) => (c, i))
				 group ci.c by ci.i / 2 into g
				 select System.Convert.ToByte(new string([.. g]), 16);

		/// <inheritdoc cref="Convert.ToBase64String(byte[])"/>
		/// <remarks>Using <see cref="HexToBytes(string)"/></remarks>
		public static string HexToBase64(this string hex)
			=> System.Convert.ToBase64String(hex.HexToBytes().ToArray());

		private static readonly Regex NumericRegex = new(String.Format(/*lang=regex*/"""(\+|\-)?(([\d\{1}]+(\{0}\d+)?)|\{0}\d+)([eE](\+|\-)?\d+)?""",
			Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator));

		public static Match GetNumericMatch(this string value)
			=> NumericRegex.Match(value);

		public static bool IsHtml(this string value)
			=> value is not null && (value.IStartsWith("<html") || value.IStartsWith("<!DOCTYPE html"));

		/// <summary>
		/// Checks testString for RichTextFormat
		/// </summary>
		/// <param name="value">The string to check</param>
		/// <returns>True if testString is in RichTextFormat</returns>
		public static bool IsRichText(this string value)
			=> value?.TrimStart().StartsWith("{\\rtf") is true;

		public static string StripDomainName(this string userName)
			=> userName.Contains('\\') ? userName[(userName.IndexOf('\\') + 1)..] : userName;
	}
}