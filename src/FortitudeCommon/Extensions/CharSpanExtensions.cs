using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Extensions;

public static class CharSpanExtensions
{
    public const char Terminator = '\0';

    public static Span<char> ResetMemory(this Span<char> toClear, int from = 0, int cappedResetLength = int.MaxValue)
    {
        var length = Math.Min(toClear.Length, cappedResetLength);
        for (int i = from; i < length; i++)
        {
            toClear[i] = Terminator;
        }
        return toClear;
    }

    public static int IndexOf(this Span<char> subject, int startingAtIndex = 0, char toFind = Terminator)
    {
        var length = subject.Length;
        int i      = startingAtIndex;
        for (; i < length; i++)
        {
            var checkChar = subject[i];
            if (checkChar == toFind) return i;
        }
        return -1;
    }

    public static ReadOnlySpan<char> LastSplitFrom(this ReadOnlySpan<char> stringAsSpan, char splitChar)
    {
        var length = stringAsSpan.Length;
        int i      = length - 1;
        for (; i >= 0; i--)
        {
            var checkChar = stringAsSpan[i];
            if (checkChar == splitChar)
            {
                var newLength = length - i + 1;
                return stringAsSpan.Slice(i + 1, newLength);
            }
        }
        return stringAsSpan;
    }

    public static int PopulatedLength(this Span<char> subject)
    {
        var terminatingIndex = IndexOf(subject);
        return terminatingIndex >= 0 ? terminatingIndex : subject.Length;
    }

    public static Span<char> InsertAt(this Span<char> toUpdate, int index, char item)
    {
        toUpdate[index] = item;
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, char toAppend, ref int populatedLength)
    {
        toUpdate[populatedLength++] = toAppend;
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, char toAppend)
    {
        var updateLength = toUpdate.PopulatedLength();
        toUpdate[updateLength] = toAppend;
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, string toAppend)
    {
        var updateLength = toUpdate.PopulatedLength();
        for (int i = 0; i < toAppend.Length; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, ReadOnlySpan<char> toAppend)
    {
        var updateLength = toUpdate.PopulatedLength();
        for (int i = 0; i < toAppend.Length; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static bool IsEndOf(this Span<char> subject, string checkSameChars, int subjectPopulatedLength = -1)
    {
        if (subjectPopulatedLength < 0)
        {
            subjectPopulatedLength = subject.PopulatedLength();
        }
        var compareIndex = checkSameChars.Length - 1;
        for (int i = subjectPopulatedLength - 1; i >= 0 && compareIndex >= 0; i++)
        {
            var bufferChar = subject[i];
            var checkChar  = checkSameChars[compareIndex];
            if (bufferChar != checkChar)
            {
                return false;
            }
        }
        return true;
    }

    public static bool SequenceMatches(this ReadOnlySpan<char> toCheck, string matchWith, int fromIndex = 0)
    {
        var matchLength = matchWith.Length;
        var matchIndex  = 0;
        for (var i = fromIndex; i < toCheck.Length && matchIndex < matchLength; i++)
        {
            if (toCheck[i] != matchWith[matchIndex++]) return false;
        }
        return matchIndex == matchLength;
    }

    public static int SequenceIndexOfAnyOf(this ReadOnlySpan<char> toCheck, string[] matchCandidates, int fromIndex = 0)
    {
        for (var i = 0; i < matchCandidates.Length; i++)
        {
            if (toCheck.SequenceMatches(matchCandidates[i], fromIndex)) return i;
        }
        return -1;
    }

    public static readonly string[] DefaultTokenStartEndDelimiter = ["{", "}"];

    public static List<string> TokenSplit
        (this ReadOnlySpan<char> tokenisedFormatting, string[]? tokenDelimiter = null, string[]? replaceDelimiter = null, bool ignoreDouble = true)
    {
        tokenDelimiter ??= DefaultTokenStartEndDelimiter;
        if (replaceDelimiter != null && replaceDelimiter.Length < tokenDelimiter.Length)
        {
            throw new ArgumentException("replaceDelimiter should have a corresponding position with tokenDelimiter");
        }
        var result = new List<string>();

        int  currentRangeStart = 0;
        bool isInToken         = false;

        int i = 0;
        for (; i < tokenisedFormatting.Length; i++)
        {
            var delimiterMatchIndex = tokenisedFormatting.SequenceIndexOfAnyOf(tokenDelimiter, i);
            if (delimiterMatchIndex >= 0)
            {
                var matchString = tokenDelimiter[delimiterMatchIndex];
                if (isInToken)
                {
                    var startMatchIndex      = tokenisedFormatting.SequenceIndexOfAnyOf(tokenDelimiter, currentRangeStart);
                    var startDelimiter       = tokenDelimiter[startMatchIndex];
                    var startDelimiterLength = startDelimiter.Length;
                    if (i > currentRangeStart + startDelimiterLength)
                    {
                        if (replaceDelimiter != null)
                        {
                            var replaceWithStart = replaceDelimiter[startMatchIndex];
                            var replaceWithEnd   = replaceDelimiter[delimiterMatchIndex];

                            var bodyRange = tokenisedFormatting.Slice(currentRangeStart + startDelimiterLength, i + 1);

                            var stringPart = WrapToString(replaceWithStart, bodyRange, replaceWithEnd);
                            result.Add(stringPart);
                        }
                        else
                        {
                            var stringPart = new String(tokenisedFormatting.Slice(currentRangeStart, i + 1));
                            result.Add(stringPart);
                        }
                    }
                }
                else
                {
                    if (i > currentRangeStart)
                    {
                        var stringPart = new String(tokenisedFormatting.Slice(currentRangeStart, i));
                        result.Add(stringPart);
                    }
                }
                currentRangeStart = i + matchString.Length;
            }
        }
        if (i > currentRangeStart + 1)
        {
            var stringPart = new String(tokenisedFormatting.Slice(currentRangeStart, i));
            result.Add(stringPart);
        }

        return result;
    }

    private static string WrapToString(string wrapPrefix, ReadOnlySpan<char> bodyRange, string wrapPostfix)
    {
        var stringSize = wrapPrefix.Length + bodyRange.Length + wrapPostfix.Length;
        var buildChars = stackalloc char[stringSize].ResetMemory();
        buildChars.Append(wrapPrefix);
        buildChars.Append(bodyRange);
        buildChars.Append(wrapPostfix);

        var wrappedString = new String(buildChars);
        return wrappedString;
    }

    public static string ToString(this Span<char> subject, int startingAt = 0, int subjectPopulatedLength = -1)
    {
        if (subjectPopulatedLength < 0)
        {
            subjectPopulatedLength = subject.PopulatedLength();
        }
        var restrictedSpan = (ReadOnlySpan<char>)subject.Slice(startingAt, subjectPopulatedLength);
        return new String(restrictedSpan);
    }

    public static int ExtractStringFormatStages
    (this ReadOnlySpan<char> toCheck, out ReadOnlySpan<char> param, out ReadOnlySpan<char> padding, out ReadOnlySpan<char> formatting
      , int fromIndex = 0)
    {
        param      = toCheck.Slice(0, 0);
        padding    = toCheck.Slice(0, 0);
        formatting = toCheck.Slice(0, 0);
        var i = fromIndex;
        if (toCheck[i] != '{') throw new ArgumentException($"toCheck {toCheck} at {i} was expected to be '{{");
        i++;
        int stage           = 0;
        int stageStartIndex = 1;
        for (; i < toCheck.Length; i++)
        {
            var checkChar = toCheck[i];
            switch (checkChar)
            {
                case ',':
                    switch (stage)
                    {
                        case 0:
                            stage = 1;
                            param = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            continue;
                        default: throw new ArgumentException($"toCheck {toCheck} at {i} at stage {stage} did not expect to have a ','");
                    }
                case ':':
                    switch (stage)
                    {
                        case 0:
                            stage = 2;
                            param = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            continue;
                        case 1:
                            stage   = 2;
                            padding = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            continue;
                        default: throw new ArgumentException($"toCheck {toCheck} at {i} at stage {stage} did not expect to have a comma");
                    }
                case '}':
                    switch (stage)
                    {
                        case 0:
                            stage = 3;
                            param = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            break;
                        case 1:
                            stage   = 3;
                            padding = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            break;
                        case 2:
                            stage      = 3;
                            formatting = toCheck.Slice(stageStartIndex, i);

                            stageStartIndex = i + 1;
                            break;
                        default: throw new ArgumentException($"toCheck {toCheck} at {i} was at an invalid stage");
                    }
                    break;
            }
            if (stage >= 3) break;
        }
        var paramCount = param.Length > 0 ? 1 : 0;
        paramCount += padding.Length > 0 ? 1 : 0;
        paramCount += padding.Length > 0 ? 1 : 0;

        return paramCount;
    }

    public static string BuildStringBuilderFormatting(this ReadOnlySpan<char> param, ReadOnlySpan<char> padding, ReadOnlySpan<char> formatting)
    {
        var stringLength = param.Length + 2;
        stringLength += padding.Length > 0 ? padding.Length + 1 : 0;
        stringLength += formatting.Length > 0 ? formatting.Length + 1 : 0;
        var buildChars = stackalloc char[stringLength].ResetMemory();
        var index      = 0;
        buildChars[index++] = '{';
        for (int i = 0; i < param.Length; i++)
        {
            buildChars[index++] = param[i];
        }
        if (padding.Length > 0)
        {
            buildChars[index++] = ',';
            for (int i = 0; i < padding.Length; i++)
            {
                buildChars[index++] = padding[i];
            }
        }
        if (formatting.Length > 0)
        {
            buildChars[index++] = ',';
            for (int i = 0; i < formatting.Length; i++)
            {
                buildChars[index++] = formatting[i];
            }
        }
        buildChars[index++] = '}';
        return buildChars.ToString();
    }

    public static ReadOnlySpan<char> ExtractDigitsSlice(this ReadOnlySpan<char> maybeDigitsSpan, int startingFrom)
    {
        for (var i = startingFrom; i < maybeDigitsSpan.Length; i++)
        {
            var checkChar = maybeDigitsSpan[i];
            if (checkChar is < '0' or > '9') return maybeDigitsSpan.Slice(startingFrom, i);
        }
        return maybeDigitsSpan.Slice(startingFrom);
    }
}
