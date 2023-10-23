namespace FortitudeCommon.Extensions;

public static class StringExtensions
{
    public const char MatchAnyChar = '*';
    public const string MatchAny = "*";

    public static bool MatchesPatternQuery(this string subject, string? query)
    {
        if (subject == null) return query == null;
        if (query == null) return false;
        if (query == MatchAny) return true;
        if (query.Equals(subject, StringComparison.InvariantCultureIgnoreCase)) return true;
        if (query.StartsWith(MatchAny) && query.Count(ch => ch == MatchAnyChar) == 1)
        {
            var remainingMatch = query.Substring(1);
            return subject.Contains(remainingMatch);
        }

        if (query.EndsWith(MatchAny) && query.Count(ch => ch == MatchAnyChar) == 1)
        {
            var remainingMatch = query.Substring(0, query.Length - 1);
            return subject.Contains(remainingMatch);
        }

        if (query.StartsWith(MatchAny) && query.EndsWith(MatchAny) && query.Count(ch => ch == MatchAnyChar) == 2)
        {
            var remainingMatch = query.Substring(1, query.Length - 2);
            return subject.Contains(remainingMatch);
        }

        return false;
    }

    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);
    public static bool IsEmptyString(this object? value) => Equals(value, string.Empty);
}
