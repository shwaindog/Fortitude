using System.Collections.Concurrent;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class JsonEscapingEncodingTransfer : RecyclableObject, IEncodingTransfer
{
    private static ConcurrentDictionary<string, ((int StartIncl, int EndExcl), string[])[]> cachedMappings = new();

    private IJsonFormattingOptions                     parentJsonOptions      = null!;
    private ((int StartIncl, int EndExcl), string[])[] cachedJsEscapeMappings = null!;
    private (int StartIncl, int EndExcl)[] exemptJsEscapeRanges =
    [
        (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0)
    ];
    private (int StartIncl, int EndExcl)[] unicodeJsEscapeRanges =
    [
        (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0), (StartIncl: 0, EndExcl: 0)
    ];

    // private Func<int, bool>? useDynamicMapping;

    public JsonEscapingEncodingTransfer Initialize(IJsonFormattingOptions owningJsonOptions, (Range, JsonEscapeType, Func<Rune, string>)[] cacheRanges)
    {
        parentJsonOptions = owningJsonOptions;

        var charMappingKey = IJsonFormattingOptions.CreateMappingTableKey(cacheRanges);
        if (cachedMappings.TryGetValue(charMappingKey, out var mapping))
        {
            cachedJsEscapeMappings = mapping;
        }
        else
        {
            cachedJsEscapeMappings = new ((int StartIncl, int EndExcl), string[])[cacheRanges.Length];

            for (int i = 0; i < cacheRanges.Length; i++)
            {
                var (range, _, mappingFunc) = cacheRanges[i];
                cachedJsEscapeMappings[i]   = (range.BoundRangeToUnicodeIndexValues(), BuildCachedEscapedCharsMapping(range, mappingFunc));
            }
            cachedMappings.TryAdd(charMappingKey, cachedJsEscapeMappings);
        }
        var optionsExempt = owningJsonOptions.ExemptEscapingRanges;
        for (var i = 0; i < exemptJsEscapeRanges.Length; i++)
        {
            if(i < optionsExempt.Length)
            {
                exemptJsEscapeRanges[i] = optionsExempt[i].BoundRangeToUnicodeIndexValues();
            }
            else
            {
                exemptJsEscapeRanges[i] = (StartIncl: 0, EndExcl: 0);
            }
        }
        
        var optionsUnicodeRanges = owningJsonOptions.UnicodeEscapingRanges;
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            if(i < optionsUnicodeRanges.Length)
            {
                unicodeJsEscapeRanges[i] = optionsUnicodeRanges[i].BoundRangeToUnicodeIndexValues();
            }
            else
            {
                unicodeJsEscapeRanges[i] = (StartIncl: 0, EndExcl: 0);
            }
        }

        return this;
    }

    protected string[] BuildCachedEscapedCharsMapping(Range cacheRange, Func<Rune, string> mapFunction)
    {
        var cacheSize = cacheRange.Length();

        var toBuild = new string[cacheRange.Length()];
        var offset  = cacheRange.Start.Value;
        for (int i = 0; i < cacheSize; i++)
        {
            var codePoint       = offset + i;
            var runeAtCodePoint = new Rune(codePoint);
            toBuild[i] = mapFunction(runeAtCodePoint);
        }
        return toBuild;
    }

    protected int ProcessRune(Rune toMap, IStringBuilder destSb, int destStartIndex)
    {
        var codePoint = toMap.Value;

        Span<char> twoChars = stackalloc char[2];
        Span<char> encoded  = stackalloc char[12];

        for (var i = 0; i < exemptJsEscapeRanges.Length; i++)
        {
            var exemptRange = exemptJsEscapeRanges[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var numOfChars = toMap.EncodeToUtf16(encoded);
                for (var j = 0; j < numOfChars; j++)
                {
                    destSb[destStartIndex + j] = encoded[j];
                }
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings[i];
            var offset = mappedRange.StartIncl;
            if (mappedRange.WithinRange(codePoint))
            {
                var mappedValue = mappedValues[codePoint - offset];
                var numOfChars  = mappedValue.Length;
                destSb.Length += numOfChars - 1;
                for (var j = 0; j < numOfChars; j++)
                {
                    destSb[destStartIndex + j] = mappedValue[j];
                }
                return numOfChars;
            }
        }
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            var unicodeJsEscapeRange = unicodeJsEscapeRanges[i];
            if (unicodeJsEscapeRange.WithinRange(codePoint))
            {
                var numOfChars   = toMap.EncodeToUtf16(twoChars);
                var escapeLength = 6 * numOfChars;
                destSb.Length += escapeLength - 1;
                encoded[0]    =  '\\';
                encoded[1]    =  'u';
                encoded.AppendLowestShortAsLowerHex(twoChars[0], 2);
                if (numOfChars > 1)
                {
                    encoded[6] = '\\';
                    encoded[7] = 'u';
                    encoded.AppendLowestShortAsLowerHex(twoChars[1], 8);
                }
                for (var j = 0; j < escapeLength; j++)
                {
                    destSb[destStartIndex + j] = encoded[j];
                }
                return escapeLength;
            }
        }
        var unmodifiedChars = toMap.EncodeToUtf16(encoded);
        for (var j = 0; j < unmodifiedChars; j++)
        {
            destSb[destStartIndex + j] = encoded[j];
        }
        return unmodifiedChars;
    }

    protected int ProcessAppendRune(Rune toMap, IStringBuilder destSb)
    {
        var codePoint = toMap.Value;

        Span<char> twoChars = stackalloc char[2];
        Span<char> encoded = stackalloc char[12];

        for (var i = 0; i < exemptJsEscapeRanges.Length; i++)
        {
            var exemptRange = exemptJsEscapeRanges[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var numOfChars = toMap.EncodeToUtf16(encoded);
                destSb.Append(encoded[..numOfChars]);
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings[i];
            var offset = mappedRange.StartIncl;
            if (mappedRange.WithinRange(codePoint))
            {
                var mappedValue = mappedValues[codePoint - offset];
                destSb.Append(mappedValue);
                return mappedValue.Length;
            }
        }
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            var unicodeJsEscapeRange = unicodeJsEscapeRanges[i];
            if (unicodeJsEscapeRange.WithinRange(codePoint))
            {
                var numOfChars   = toMap.EncodeToUtf16(twoChars);
                var escapeLength = 6 * numOfChars;
                encoded[0]    =  '\\';
                encoded[1]    =  'u';
                encoded.AppendLowestShortAsLowerHex(twoChars[0], 2);
                if (numOfChars > 1)
                {
                    encoded[6] = '\\';
                    encoded[7] = 'u';
                    encoded.AppendLowestShortAsLowerHex(twoChars[1], 8);
                }
                destSb.Append(encoded[..escapeLength]);
                return escapeLength;
            }
        }
        var unmodifiedChars = toMap.EncodeToUtf16(encoded);
        destSb.Append(encoded[..unmodifiedChars]);
        return unmodifiedChars;
    }

    protected int ProcessRune(Rune toMap, Span<char> destSpan, int destStartIndex)
    {
        var        codePoint = toMap.Value;
        Span<char> twoChars  = stackalloc char[2];

        for (var i = 0; i < exemptJsEscapeRanges.Length; i++)
        {
            var exemptRange = exemptJsEscapeRanges[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var subSpan    = destSpan[destStartIndex..];
                var numOfChars = toMap.EncodeToUtf16(subSpan);
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings[i];
            var offset = mappedRange.StartIncl;
            if (mappedRange.WithinRange(codePoint))
            {
                var mappedValue = mappedValues[codePoint - offset];
                var numOfChars  = mappedValue.Length;
                for (var j = 0; j < numOfChars; j++)
                {
                    destSpan[destStartIndex + j] = mappedValue[j];
                }
                return numOfChars;
            }
        }
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            var unicodeJsEscapeRange = unicodeJsEscapeRanges[i];
            if (unicodeJsEscapeRange.WithinRange(codePoint))
            {
                var numOfChars   = toMap.EncodeToUtf16(twoChars);
                var escapeLength = 6 * numOfChars;
                destSpan[destStartIndex]     = '\\';
                destSpan[destStartIndex + 1] = 'u';
                destSpan.AppendLowestShortAsLowerHex(twoChars[0], destStartIndex + 2);
                if (numOfChars > 1)
                {
                    destSpan[destStartIndex + 6] = '\\';
                    destSpan[destStartIndex + 7] = 'u';
                    destSpan.AppendLowestShortAsLowerHex(twoChars[1], destStartIndex + 8);
                }
                return escapeLength;
            }
        }
        var unmodifiedSpan = destSpan[destStartIndex..];
        return toMap.EncodeToUtf16(unmodifiedSpan);
    }

    public string EncodingTransferConfigKey { get; private set; } = "";

    public virtual int Transfer(Rune? source, IStringBuilder destSb)
    {
        if (source == null) return 0;
         return ProcessAppendRune(source.Value, destSb);
    }

    public virtual int Transfer(Rune? source, Span<char> dest, int destIndex)
    {
        if (source == null) return 0;
        return ProcessRune(source.Value, dest, destIndex);
    }


    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destSb, destStartIndex);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var  i              = Math.Clamp(sourceFrom, 0, source.Length);
        var  end            = Math.Min(source.Length, maxTransferCount + sourceFrom);
        var  originalLength = sb.Length;
        bool isAppend       = false;
        if (destStartIndex == int.MaxValue || destStartIndex > sb.Length)
        {
            destStartIndex =  originalLength;
            isAppend       =  true;
        }
        else if (destStartIndex < sb.Length)
        {
            if (destStartIndex + end > sb.Length)
            {
                sb.Length = destStartIndex + end;
            }
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 6 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 6;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 12 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 12;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
        }
        if (!isAppend)
        {
            sb.Length = Math.Max(originalLength, destStartIndex + countAdded);
        }
        return sb.Length - originalLength;
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end && desti < destination.Length; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                desti += ProcessRune(iRune, destination, desti);
            }
            else if (i + 1 < end && desti + 1 < destination.Length)
            {
                var iRune = new Rune(iChar, source[++i]);
                desti += ProcessRune(iRune, destination, desti);
            }
        }
        return desti - destStartIndex;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        return Transfer(stringFormatter, source, 0, destSb, destStartIndex);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return Transfer(stringFormatter, source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var preTransferLen = destSb.Length;
        if (!parentJsonOptions.CharArrayWritesString)
        {
            var  cappedEnd    = Math.Clamp(maxTransferCount, 0, source.Length);
            int  j            = Math.Clamp(sourceFrom, 0, source.Length);
            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; j < cappedEnd; j++)
            {
                var item = source[j];
                if (j > 0) stringFormatter.AddCollectionElementSeparator(typeof(char), destSb, j);
                lastAdded    = lastAdded > 0 || j == sourceFrom
                    ?  stringFormatter.CollectionNextItemFormat(item, j, destSb, "")
                    : stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), j - 1, destSb, "");
                previousChar =  lastAdded == 0 ? item : '\0';
            }
            return destSb.Length - preTransferLen;
        }
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        if (!parentJsonOptions.CharArrayWritesString)
        {
            var  charsAdded   = 0;
            var  cappedEnd    = Math.Clamp(maxTransferCount, 0, source.Length);
            int  j            = Math.Clamp(sourceFrom, 0, source.Length);
            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; j < cappedEnd; j++)
            {
                var item = source[j];

                if (j > 0 && lastAdded > 0) charsAdded += stringFormatter.AddCollectionElementSeparator(typeof(char), destSpan, destStartIndex + charsAdded, j);
                
                lastAdded    = lastAdded > 0 || j == sourceFrom
                    ?  stringFormatter.CollectionNextItemFormat(item, j, destSpan, destStartIndex + charsAdded, "")
                    : stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), j - 1, destSpan, destStartIndex + charsAdded, "");
                previousChar =  lastAdded == 0 ? item : '\0';
                charsAdded   += lastAdded;
            }
            return j;
        }
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source.AsSpan(), sourceFrom, sb, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source.AsSpan(), sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destSb, destStartIndex);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var  i              = Math.Clamp(sourceFrom, 0, source.Length);
        var  end            = Math.Min(source.Length, maxTransferCount + sourceFrom);
        var  originalLength = sb.Length;
        bool isAppend       = false;
        if (destStartIndex == int.MaxValue || destStartIndex > sb.Length)
        {
            destStartIndex =  originalLength;
            isAppend       =  true;
        }
        else if (destStartIndex < sb.Length)
        {
            if (destStartIndex + end > sb.Length)
            {
                sb.Length = destStartIndex + end;
            }
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 6 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 6;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 12 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 12;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
        }
        if (!isAppend)
        {
            sb.Length = Math.Max(originalLength, destStartIndex + countAdded);
        }
        return sb.Length - originalLength;
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end && desti < destination.Length; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                desti += ProcessRune(iRune, destination, desti);
            }
            else if (i + 1 < end && desti + 1 < destination.Length)
            {
                var iRune = new Rune(iChar, source[++i]);
                desti += ProcessRune(iRune, destination, desti);
            }
        }
        return desti - destStartIndex;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        return Transfer(stringFormatter, source, 0, destSb, destStartIndex);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return Transfer(stringFormatter, source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        var preTransferLen = destSb.Length;
        if (!parentJsonOptions.CharArrayWritesString)
        {
            var  cappedEnd    = Math.Clamp(maxTransferCount, 0, source.Length);
            int  j            = Math.Clamp(sourceFrom, 0, source.Length);
            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; j < cappedEnd; j++)
            {
                var item = source[j];
                if (j > 0) stringFormatter.AddCollectionElementSeparator(typeof(char), destSb, j);
                lastAdded    = lastAdded > 0 || j == sourceFrom
                    ?  stringFormatter.CollectionNextItemFormat(item, j, destSb, "")
                    : stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), j - 1, destSb, "");
                previousChar =  lastAdded == 0 ? item : '\0';
            }
            return destSb.Length - preTransferLen;
        }
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        if (!parentJsonOptions.CharArrayWritesString)
        {
            var  charsAdded   = 0;
            var  cappedEnd    = Math.Clamp(maxTransferCount, 0, source.Length);
            int  j            = Math.Clamp(sourceFrom, 0, source.Length);
            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; j < cappedEnd; j++)
            {
                var item = source[j];

                if (j > 0 && lastAdded > 0) charsAdded += stringFormatter.AddCollectionElementSeparator(typeof(char), destSpan, destStartIndex + charsAdded, j);
                
                lastAdded    = lastAdded > 0 || j == sourceFrom
                    ?  stringFormatter.CollectionNextItemFormat(item, j, destSpan, destStartIndex + charsAdded, "")
                    : stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), j - 1, destSpan, destStartIndex + charsAdded, "");
                previousChar =  lastAdded == 0 ? item : '\0';
                charsAdded   += lastAdded;
            }
            return j;
        }
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var  i              = Math.Clamp(sourceFrom, 0, source.Length);
        var  end            = Math.Min(source.Length, maxTransferCount + sourceFrom);
        var  originalLength = sb.Length;
        bool isAppend       = false;
        if (destStartIndex == int.MaxValue || destStartIndex > sb.Length)
        {
            destStartIndex =  originalLength;
            isAppend       =  true;
        }
        else if (destStartIndex < sb.Length)
        {
            if (destStartIndex + end > sb.Length)
            {
                sb.Length = destStartIndex + end;
            }
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 6 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 6;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                int appended;
                if (isAppend)
                {
                    appended = ProcessAppendRune(iRune, sb);
                }
                else
                {
                    if (destStartIndex + countAdded + 12 > sb.Length)
                    {
                        sb.Length = destStartIndex + countAdded + 12;
                    }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
        }
        if (!isAppend)
        {
            sb.Length = Math.Max(originalLength, destStartIndex + countAdded);
        }
        return sb.Length - originalLength;
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end && desti < destination.Length; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                desti += ProcessRune(iRune, destination, desti);
            }
            else if (i + 1 < end && desti + 1 < destination.Length)
            {
                var iRune = new Rune(iChar, source[++i]);
                desti += ProcessRune(iRune, destination, desti);
            }
        }
        return desti - destStartIndex;
    }
}
