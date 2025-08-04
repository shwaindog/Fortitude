using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Types.Mutable.Strings;

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

    public static int IndexOf(this Span<char> subject, char toFind = Terminator, int startingAtIndex = 0)
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

    public static int IndexOf(this Span<char> subject, ICharSequence toFind, int startingAtIndex = 0)
    {
        var length = subject.Length;
        int i      = startingAtIndex;

        var lastFindIndex = toFind.Length - 1;
        for (; i < length; i++)
        {
            var checkChar = subject[i];
            for (var j = 0; j < toFind.Length; j++)
            {
                var compareChar = toFind[j];
                if (checkChar != compareChar) break;
                if (j == lastFindIndex) return i;
            }
        }
        return -1;
    }

    public static int IndexOf(this Span<char> subject, StringBuilder toFind, int startingAtIndex = 0)
    {
        var length = subject.Length;
        int i      = startingAtIndex;

        var lastFindIndex = toFind.Length - 1;
        for (; i < length; i++)
        {
            var checkChar = subject[i];
            for (var j = 0; j < toFind.Length; j++)
            {
                var compareChar = toFind[j];
                if (checkChar != compareChar) break;
                if (j == lastFindIndex) return i;
            }
        }
        return -1;
    }

    public static int CountWhiteSpaceBackwardsFrom(this Span<char> searchSpace, int fromIndex)
    {
        var startIndex      = Math.Min(searchSpace.Length, fromIndex);
        var countWhiteSpace = 0;
        for (int i = startIndex; i >= 0; i--)
        {
            if (!searchSpace[i].IsWhiteSpace()) break;
        }
        return countWhiteSpace;
    }

    public static int CountWhiteSpaceFrom(this Span<char> searchSpace, int searchLength, int fromIndex)
    {
        var startIndex      = fromIndex;
        var stopIndex       = Math.Min(searchSpace.Length, searchLength);
        var countWhiteSpace = 0;
        for (int i = startIndex; i < stopIndex; i++)
        {
            if (!searchSpace[i].IsWhiteSpace()) break;
        }
        return countWhiteSpace;
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

    public static int ReplaceCapped(this Span<char> searchSpace, int searchPopLength, string find, string replace, int occurences = int.MaxValue)
    {
        var findSpan    = find.AsSpan();
        var replaceSpan = replace.AsSpan();
        return ReplaceCapped(searchSpace, searchPopLength, findSpan, replaceSpan, occurences);
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, ICharSequence find, ICharSequence replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, StringBuilder find, StringBuilder replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int PopulatedLength(this Span<char> subject)
    {
        var terminatingIndex = IndexOf(subject);
        return terminatingIndex >= 0 ? terminatingIndex : subject.Length;
    }

    public static Span<char> OverWriteAt(this Span<char> toUpdate, int spanIndex, char item)
    {
        toUpdate[spanIndex] = item;
        return toUpdate;
    }

    public static int OverWriteAt(this Span<char> toUpdate, int spanIndex, ReadOnlySpan<char> toAppend)
    {
        var charsWritten = 0;
        for (int i = 0; i < toAppend.Length && spanIndex < toUpdate.Length; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
        }
        return charsWritten;
    }

    public static int OverWriteAt
        (this Span<char> toUpdate, int spanIndex, ICharSequence toAppend, int buildIndex = 0, int builderMaxLength = int.MaxValue)
    {
        var spanLength = toUpdate.Length;
        builderMaxLength = Math.Min(builderMaxLength, toAppend.Length - buildIndex);
        var charsWritten = 0;
        for (int i = buildIndex; i < builderMaxLength && spanIndex < spanLength; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
        }
        return charsWritten;
    }

    public static int OverWriteAt
        (this Span<char> toUpdate, int spanIndex, StringBuilder toAppend, int buildIndex = 0, int builderMaxLength = int.MaxValue)
    {
        var spanLength = toUpdate.Length;
        builderMaxLength = Math.Min(builderMaxLength, toAppend.Length - buildIndex);
        var charsWritten = 0;
        for (int i = buildIndex; i < builderMaxLength && spanIndex < spanLength; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
        }
        return charsWritten;
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

    public static string? SequenceIndexOfAnyOf(this ReadOnlySpan<char> toCheck, (string Open, string Close) matchCandidates, int fromIndex = 0)
    {
        if (toCheck.SequenceMatches(matchCandidates.Open, fromIndex)) return matchCandidates.Open;
        if (toCheck.SequenceMatches(matchCandidates.Close, fromIndex)) return matchCandidates.Close;
        return null;
    }

    public static readonly (string Open, string Close) DefaultTokenStartEndDelimiter = ("{", "}");

    public static bool ContainsTokens(this ReadOnlySpan<char> maybeTokenFormatting, (string Open, string Close)? tokenDelimiter = null)
    {
        tokenDelimiter ??= DefaultTokenStartEndDelimiter;
        var openClose    = tokenDelimiter ?? DefaultTokenStartEndDelimiter;
        var foundOpening = false;
        var foundClosing = false;
        for (int i = 0; i < maybeTokenFormatting.Length; i++)
        {
            var delimiterMatch = maybeTokenFormatting.SequenceIndexOfAnyOf(openClose, i);
            if (delimiterMatch == openClose.Open)
            {
                foundOpening = true;
            }
            if (delimiterMatch == openClose.Close && foundOpening)
            {
                foundClosing = true;
            }
        }
        return foundOpening && foundClosing;
    }


    public static Range TokenNextTokenOpenMatchingEnd(this ReadOnlySpan<char> tokenisedFormatting, (string Open, string Close) openClose)
    {
        var atIndex       = tokenisedFormatting.Length;
        var nextTokenOpen = tokenisedFormatting.IndexOf(openClose.Open);
        if (nextTokenOpen >= 0)
        {
            atIndex = nextTokenOpen + openClose.Open.Length;
            var remainingString        = tokenisedFormatting[atIndex..];
            var tokenOpenRange         = new Range(Index.FromStart(nextTokenOpen), Index.FromStart(atIndex));
            var subTokenPlusCloseRange = TokenNextTokenOpenMatchingEnd(remainingString, 1, openClose);
            var totalRange             = tokenOpenRange.ConcatLength(subTokenPlusCloseRange);
            return totalRange;
        }
        return new Range(tokenisedFormatting.Length, 0);
    }

    public static Range TokenNextTokenOpenMatchingEnd
        (this ReadOnlySpan<char> tokenisedFormatting, int currentDepth, (string Open, string Close) openClose)
    {
        var nextTokenOpen  = tokenisedFormatting.IndexOf(openClose.Open);
        var nextTokenClose = tokenisedFormatting.IndexOf(openClose.Close);
        if (nextTokenOpen >= 0 && nextTokenClose > nextTokenOpen)
        {
            var subTokenPostOpenStart = nextTokenClose + openClose.Close.Length;

            currentDepth++;
            var addOpenRange = new Range(Index.FromStart(0),Index.FromStart(subTokenPostOpenStart));
            return addOpenRange.ConcatLength(TokenNextTokenOpenMatchingEnd(tokenisedFormatting[subTokenPostOpenStart..], currentDepth, openClose));
        }
        if (nextTokenClose < 0)
        {
            return new Range(Index.FromStart(0), Index.FromStart(tokenisedFormatting.Length));
        }
        var subTokenPostCloseStart = nextTokenClose + openClose.Close.Length;

        currentDepth--;
        var addCloseRange = new Range(Index.FromStart(0), Index.FromStart(subTokenPostCloseStart));
        if (currentDepth == 0)
        {
            return addCloseRange;
        }
        return addCloseRange.ConcatLength(TokenNextTokenOpenMatchingEnd(tokenisedFormatting[subTokenPostCloseStart..], currentDepth, openClose));
    }

    private static Range ConcatLength(this Range keepStart, Range addLength)
    {
        var sumStartIndex = new Index(keepStart.Start.Value);
        var sumEndIndex   = new Index(keepStart.End.Value + (addLength.End.Value - addLength.Start.Value));
        return new Range(sumStartIndex, sumEndIndex);
    }

    public static List<string> TokenSplit
    (this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;
        var result    = new List<string>();

        var openTokenLength  = openClose.Open.Length;
        var closeTokenLength = openClose.Close.Length;

        bool hasReplace = replaceDelimiter != null;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value > 0)
            {
                var stringPart    = new String(remainingTokenString[..nextTokenStartRange.Start]);
                result.Add(stringPart);
            }
            if (nextTokenStartRange.Start.Value < remainingTokenString.Length && nextTokenStartRange.Length() > 0)
            {
                if (hasReplace)
                {
                    var subTokenString = remainingTokenString[nextTokenStartRange][openTokenLength..^closeTokenLength];
                    var replaceOc = replaceDelimiter!.Value;
                    scratchSpace.Append(replaceOc.ReplaceOpen);
                    scratchSpace.Append(subTokenString);
                    scratchSpace.Append(replaceOc.ReplaceClose);
                    var stringPart = new String(scratchSpace[..scratchSpace.PopulatedLength()]);
                    result.Add(stringPart);
                }
                else
                {
                    var stringPart = new String(remainingTokenString[nextTokenStartRange]);
                    result.Add(stringPart);
                }
            }
            var nextSliceStart = nextTokenStartRange.Start.Value + nextTokenStartRange.Length();
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return result;
    }

    public static int NonTokenCharCount
    (this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;

        int nonTokenCharLength = 0;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value > 0)
            {
                nonTokenCharLength += nextTokenStartRange.Start.Value;
            }
            var nextSliceStart = nextTokenStartRange.Start.Value + nextTokenStartRange.Length();
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return nonTokenCharLength;
    }

    public static int TokenCount(this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;

        int tokenCount = 0;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value < remainingTokenString.Length && nextTokenStartRange.Length() > 0)
            {
                tokenCount += nextTokenStartRange.Start.Value;
            }
            var nextSliceStart = nextTokenStartRange.Start.Value + nextTokenStartRange.Length();
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return tokenCount;
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
    (this ReadOnlySpan<char> toCheck, out ReadOnlySpan<char> identifier, out ReadOnlySpan<char> padding, out ReadOnlySpan<char> formatting
      , int fromIndex = 0)
    {
        identifier = toCheck.Slice(0, 0);
        padding    = toCheck.Slice(0, 0);
        formatting = toCheck.Slice(0, 0);

        var paramCount = 0;

        var remainingSpan = toCheck[fromIndex..];
        if (toCheck[0] != '{') throw new ArgumentException($"toCheck {toCheck} at {fromIndex} was expected to be '{{");
        remainingSpan = toCheck[1..^1];
        int stage         = 0;
        while (remainingSpan.Length > 0)
        {
            switch (stage)
            {
                case 0:
                    var stageEnd = remainingSpan.Length;
                    stage = 3;
                    var colonIndex = remainingSpan.IndexOf(":");
                    if (colonIndex > 0)
                    {
                        stage    = 2;
                        stageEnd = colonIndex;
                    }
                    var commaIndex = remainingSpan.IndexOf(",");
                    if (commaIndex > 0 && commaIndex < colonIndex)
                    {
                        stage    = 1;
                        stageEnd = commaIndex;
                    }
                    identifier = remainingSpan[..stageEnd];
                    paramCount++;
                    var startNext = stageEnd + 1;
                    if (startNext < remainingSpan.Length)
                    {
                        remainingSpan = remainingSpan[startNext..];
                    }
                    break;
                case 1:
                    stageEnd   = remainingSpan.Length;
                    stage      = 3;
                    colonIndex = remainingSpan.IndexOf(":");
                    if (colonIndex > 0)
                    {
                        stage    = 2;
                        stageEnd = colonIndex;
                    }
                    padding = remainingSpan[..stageEnd];
                    paramCount++;
                    startNext = stageEnd + 1;
                    if (startNext < remainingSpan.Length)
                    {
                        remainingSpan = remainingSpan[startNext..];
                    }
                    break;
                case 2:
                    stage      = 3;
                    formatting = remainingSpan;
                    paramCount++;
                    break;
                default: return paramCount;
            }
        }
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
            buildChars[index++] = ':';
            for (int i = 0; i < formatting.Length; i++)
            {
                buildChars[index++] = formatting[i];
            }
        }
        buildChars[index++] = '}';
        return buildChars[..buildChars.PopulatedLength()].ToString();
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

    public static int? ExtractInt(this ReadOnlySpan<char> maybeDigitsSpan, int startingFrom = 0)
    {
        var  sign  = 1;
        int? value = null;

        var foundNonWhiteSpace = false;
        for (var i = startingFrom; i < maybeDigitsSpan.Length; i++)
        {
            var checkChar = maybeDigitsSpan[i];
            if (!foundNonWhiteSpace && checkChar.IsWhiteSpace()) continue;
            if (checkChar.IsMinus()) sign = -1;
            if (checkChar is < '0' or > '9')
            {
                value ??= 0;
                value *=  10;
                value +=  checkChar - '0';
            }
            else
            {
                break;
            }
        }
        return value * sign;
    }
}
