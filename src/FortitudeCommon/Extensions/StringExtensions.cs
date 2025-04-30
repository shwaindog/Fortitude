// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;

#endregion

namespace FortitudeCommon.Extensions;

public static class StringExtensions
{
    public const char   MatchAnyChar = '*';
    public const string MatchAny     = "*";

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

    public static bool IsNullOrEmpty(this string? value)    => string.IsNullOrEmpty(value);
    public static bool IsNotNullOrEmpty(this string? value) => !string.IsNullOrEmpty(value);
    public static bool IsEmpty(this string value)           => string.IsNullOrEmpty(value);
    public static bool IsNotEmpty(this string value)        => !string.IsNullOrEmpty(value);
    public static bool IsEmptyString(this object? value)    => Equals(value, string.Empty);


    public static int? SafeExtractInt(this string value)
    {
        var sb = new StringBuilder();
        foreach (var charInString in value)
            if (charInString is >= '0' and <= '9')
                sb.Append(charInString);
        return int.TryParse(sb.ToString(), out var result) ? result : null;
    }

    public static string CharIndexPosListedSizeString(this int size)
    {
        var sb = new StringBuilder();
        sb.Append("0");
        int i;
        if (size < 1000)
        {
            i = 4;
            for (; i < size + 4; i += 4) sb.AppendFormat("_{0:000}", i);
        }
        else if (size < 10_000)
        {
            i = 5;
            for (; i < size + 5; i += 5) sb.AppendFormat("_{0:0000}", i);
        }
        else if (size < 100_000)
        {
            i = 6;
            for (; i < size + 6; i += 6) sb.AppendFormat("_{0:00000}", i);
        }
        else
        {
            i = 7;
            for (; i < size + 7; i += 7) sb.AppendFormat("_{0:000000}", i);
        }

        return sb.ToString(0, size);
    }
}
