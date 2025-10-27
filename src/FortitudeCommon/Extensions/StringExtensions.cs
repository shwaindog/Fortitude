// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
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

    public static string RemoveAll(this string toCleanup, params string[] toRemove)
    {
        var sb = new StringBuilder(toCleanup);
        foreach (var remove in toRemove)
        {
            sb.Replace(remove, "");
        }
        return sb.ToString();
    }


    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)   => string.IsNullOrEmpty(value);
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrEmpty(value);
    public static bool IsEmpty(this string value)                               => string.IsNullOrEmpty(value);
    public static bool IsNotEmpty(this string value)                            => !string.IsNullOrEmpty(value);
    public static bool IsEmptyString([NotNullWhen(true)] this object? value)    => Equals(value, string.Empty);

    public static (int?, string) SplitFormattedIntFromString(this string numberPrefixString)
    {
        int? numberFound = null;
        int  sign        = 1;

        int  i               = 0;
        bool finishedPreTrim = false;
        for (; i < numberPrefixString.Length; i++)
        {
            var checkChar = numberPrefixString[i];
            if (!finishedPreTrim && checkChar.IsWhiteSpace()) continue;
            if (!finishedPreTrim && checkChar.IsMinus())
            {
                sign            = -1;
                finishedPreTrim = true;
                continue;
            }
            finishedPreTrim = true;
            if (checkChar.IsDigit())
            {
                numberFound ??= 0;
                numberFound *=  10;
                numberFound +=  checkChar - '0';
            }
            else
            {
                if (!checkChar.IsThousandsSeparator()
                 || i + 1 >= numberPrefixString.Length
                 || !numberPrefixString[i + 1].IsDigit())
                {
                    var remainingString = numberPrefixString.Substring(i);
                    return (numberFound * sign, remainingString);
                }
            }
        }
        return numberFound != null ? (numberFound * sign, "") : (numberFound * sign, numberPrefixString);
    }

    public static (ulong?, string) SplitFormattedULongFromString(this string numberPrefixString)
    {
        ulong? numberFound = null;

        bool finishedPreTrim = false;

        int i = 0;
        for (; i < numberPrefixString.Length; i++)
        {
            var checkChar = numberPrefixString[i];
            if (!finishedPreTrim && checkChar.IsWhiteSpace()) continue;
            finishedPreTrim = true;
            if (checkChar.IsDigit())
            {
                numberFound ??= 0;
                numberFound *=  10;
                numberFound +=  (ulong)(checkChar - '0');
            }
            else
            {
                if (!checkChar.IsThousandsSeparator()
                 || i + 1 >= numberPrefixString.Length
                 || !numberPrefixString[i + 1].IsDigit())
                {
                    var remainingString = numberPrefixString.Substring(i);
                    return (numberFound, remainingString);
                }
            }
        }
        return numberFound != null ? (numberFound, "") : (numberFound, numberPrefixString);
    }


    public static int? SafeExtractInt(this string value)
    {
        var sb = new StringBuilder();
        foreach (var charInString in value)
            if (charInString is >= '0' and <= '9')
                sb.Append(charInString);
        return int.TryParse(sb.ToString(), out var result) ? result : null;
    }


    public static bool ContainsTokens(this string maybeTokenFormatting, (string Open, string Close)? tokenDelimiter = null)
    {
        var formattingSpan = maybeTokenFormatting.AsSpan();
        var containsTokens = formattingSpan.ContainsTokens(tokenDelimiter);
        return containsTokens;
    }

    public static List<string> TokenSplit
        (this string tokenisedFormatting, (string Open, string Close)? tokenDelimiter = null, (string Open, string Close)? replaceDelimiter = null)
    {
        var formattingSpan = tokenisedFormatting.AsSpan();
        var stringParts    = formattingSpan.TokenSplit(tokenDelimiter, replaceDelimiter);
        return stringParts;
    }

    public static string Format(this string formatString, params object?[] args) => string.Format(formatString, args);

    public static string Format(this string formatString, object? arg0)                        => string.Format(formatString, arg0);

    public static string Format(this string formatString, object? arg0, object? arg1)          => string.Format(formatString, arg0, arg1);

    public static string Format(this string formatString, object? arg0, object? arg1, object? arg2) => string.Format(formatString, arg0, arg1, arg2);
    
    public static bool SequenceMatches(this string search, string checkIsSame, int fromIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(count, search.Length - fromIndex);
        if(checkIsSame.Length != cappedLength) return false;
        for (int i = 0; i < cappedLength; i++)
        {
            var checkChar   = search[fromIndex + i];
            var compareChar = checkIsSame[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }

    public static  string TruncateAt(this string toTruncate, char terminatorChar)
    {
        var indexOfTerminator =  toTruncate.IndexOf(terminatorChar);
        if(indexOfTerminator == -1) return toTruncate;
        return toTruncate.Substring(0, indexOfTerminator);
    }

    public static bool SequenceMatches(this string toCheck, string matchWith)
    {
        var cappedLength = Math.Min(matchWith.Length, toCheck.Length);
        if (matchWith.Length != cappedLength || toCheck.Length != cappedLength) return false;
        for (var i = 0; i < cappedLength; i++)
        {
            if (toCheck[i] != matchWith[i]) return false;
        }
        return true;
    }
    
    public static string RemoveLineEndings(this string input)
    {
        return input.Replace("\n", "").Replace("\r", "");
    }
    
    public static string Dos2Unix(this string input)
    {
        return input.Replace("\r\n", "\n");
    }
    
    public static string IndentAll(this string input, string indentChars = "  ")
    {
        return indentChars + input.IndentSubsequentLines();
    }
    
    public static string IndentAll(this string input, int indentMultiple, char indentChar = ' ')
    {
        var sb = new StringBuilder().Append(indentChar, indentMultiple);
        sb.Append(input.IndentSubsequentLines(sb.ToString()));
        return sb.ToString();
    }
    
    public static string IndentSubsequentLines(this string input, string indentChars = "  ")
    {
        return input.Replace("\n", "\n" + indentChars);
    }
    
    public static string IndentSubsequentLines(this string input, int indentMultiple, char indentChar = ' ')
    {
        return input.IndentSubsequentLines(new StringBuilder().Append(indentChar, indentMultiple).ToString());
    }
    
    public static bool IsSglQtBounded(this string input)  => input.Length > 1 &&  input[0] == '\'' && input[^1] == '\'';
    public static bool IsDblQtBounded(this string input)  => input.Length > 1 &&  input[0] == '\"' && input[^1] == '\"';
    public static bool IsSqBrktBounded(this string input) => input.Length > 1 &&  input[0] == '[' && input[^1] == ']';
    public static bool IsBrcBounded(this string input)    => input.Length > 1 &&  input[0] == '{' && input[^1] == '}';

}
