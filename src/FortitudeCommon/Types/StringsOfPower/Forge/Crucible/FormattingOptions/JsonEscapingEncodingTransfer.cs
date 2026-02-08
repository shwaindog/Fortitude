using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class JsonEscapingEncodingTransfer : ReusableObject<JsonEscapingEncodingTransfer>, IEncodingTransfer
{
    protected const char DblQtChar = '"';

    private const int MaxAllowedRanges = 4;

    private static readonly ConcurrentDictionary<string, CappedCachedUnicodeEscapeRangesValuesInlineArray> CachedMappings = new();

    [InlineArray(MaxAllowedRanges)]
    private struct UnicodeEscapeRangesInlineArray
    {
        private (int StartIncl, int EndExcl) element;
    }
    
    private struct CappedUnicodeEscapeRangesInlineArray
    {
        public UnicodeEscapeRangesInlineArray Values;

        public int Length;
    }

    [InlineArray(MaxAllowedRanges)]
    private struct CachedUnicodeEscapeRangesValuesInlineArray
    {
        private ((int StartIncl, int EndExcl), string[]) element;
    }

    private struct CappedCachedUnicodeEscapeRangesValuesInlineArray
    {
        public CachedUnicodeEscapeRangesValuesInlineArray Values;

        public int Length;
    }

    private IJsonFormattingOptions parentJsonOptions = null!;

    private CappedCachedUnicodeEscapeRangesValuesInlineArray cachedJsEscapeMappings = new();

    private CappedUnicodeEscapeRangesInlineArray exemptJsEscapeRanges  = new();
    private CappedUnicodeEscapeRangesInlineArray unicodeJsEscapeRanges = new();
    
    private IEncodingTransfer? layoutEncoder;

    public EncodingType Type => EncodingType.JsonEncoding;

    public IEncodingTransfer LayoutEncoder
    {
        get => layoutEncoder ?? PassThroughEncodingTransfer.FinalEncoder;
        set => layoutEncoder = value;
    }
    
    public IEncodingTransfer WithAttachedLayoutEncoder(IEncodingTransfer toAttach)
    {
        if (toAttach.Type != EncodingType.PassThrough)
        {
            layoutEncoder?.DecrementRefCount();
            layoutEncoder = toAttach;
            toAttach.IncrementRefCount();
        }
        return this;
    }

    public int StringValueDelimiter(IStringBuilder sb) => sb.Append(DblQtChar).ReturnCharCount(1);

    public int StringValueDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public int StringFieldDelimiter(IStringBuilder sb) => sb.Append(DblQtChar).ReturnCharCount(1);

    public int StringFieldDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public JsonEscapingEncodingTransfer Initialize(IJsonFormattingOptions owningJsonOptions,
        (Range, JsonEscapeType, Func<Rune, string>)[] cacheRanges)
    {
        parentJsonOptions = owningJsonOptions;

        var charMappingKey = IJsonFormattingOptions.CreateMappingTableKey(cacheRanges);
        if (CachedMappings.TryGetValue(charMappingKey, out var mapping)) { cachedJsEscapeMappings = mapping; }
        else
        {
            cachedJsEscapeMappings = new CappedCachedUnicodeEscapeRangesValuesInlineArray();

            for (int i = 0; i < MaxAllowedRanges; i++)
            {
                if (i < cacheRanges.Length)
                {
                    var (range, _, mappingFunc) = cacheRanges[i];

                    cachedJsEscapeMappings.Values[i] = (range.BoundRangeToUnicodeIndexValues(), BuildCachedEscapedCharsMapping(range, mappingFunc));
                    cachedJsEscapeMappings.Length    = i + 1;
                }
                else { cachedJsEscapeMappings.Values[i] = ((StartIncl: 0, EndExcl: 0), []); }
            }
            CachedMappings.TryAdd(charMappingKey, cachedJsEscapeMappings);
        }
        var optionsExempt = owningJsonOptions.ExemptEscapingRanges;
        for (var i = 0; i < MaxAllowedRanges; i++)
        {
            if (i < optionsExempt.Length)
            {
                exemptJsEscapeRanges.Values[i] = optionsExempt[i].BoundRangeToUnicodeIndexValues();
                exemptJsEscapeRanges.Length    = i + 1;
            }
            else { exemptJsEscapeRanges.Values[i]                          = (StartIncl: 0, EndExcl: 0); }
        }

        var optionsUnicodeRanges = owningJsonOptions.UnicodeEscapingRanges;
        for (var i = 0; i < MaxAllowedRanges; i++)
        {
            if (i < optionsUnicodeRanges.Length)
            {
                unicodeJsEscapeRanges.Values[i] = optionsUnicodeRanges[i].BoundRangeToUnicodeIndexValues();
                unicodeJsEscapeRanges.Length    = i + 1;
            }
            else { unicodeJsEscapeRanges.Values[i]                                 = (StartIncl: 0, EndExcl: 0); }
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
            var exemptRange = exemptJsEscapeRanges.Values[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var numOfChars = toMap.EncodeToUtf16(encoded);
                for (var j = 0; j < numOfChars; j++) { destSb[destStartIndex + j] = encoded[j]; }
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings.Values[i];
            var offset = mappedRange.StartIncl;
            if (mappedRange.WithinRange(codePoint))
            {
                var mappedValue = mappedValues[codePoint - offset];
                var numOfChars  = mappedValue.Length;
                for (var j = 0; j < numOfChars; j++) { destSb[destStartIndex + j] = mappedValue[j]; }
                return numOfChars;
            }
        }
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            var unicodeJsEscapeRange = unicodeJsEscapeRanges.Values[i];
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
                for (var j = 0; j < escapeLength; j++) { destSb[destStartIndex + j] = encoded[j]; }
                return escapeLength;
            }
        }
        var unmodifiedChars = toMap.EncodeToUtf16(encoded);
        for (var j = 0; j < unmodifiedChars; j++) { destSb[destStartIndex + j] = encoded[j]; }
        return unmodifiedChars;
    }

    protected int ProcessAppendRune(Rune toMap, IStringBuilder destSb)
    {
        var codePoint = toMap.Value;

        Span<char> twoChars = stackalloc char[2];
        Span<char> encoded  = stackalloc char[12];

        for (var i = 0; i < exemptJsEscapeRanges.Length; i++)
        {
            var exemptRange = exemptJsEscapeRanges.Values[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var numOfChars = toMap.EncodeToUtf16(encoded);
                destSb.Append(encoded[..numOfChars]);
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings.Values[i];
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
            var unicodeJsEscapeRange = unicodeJsEscapeRanges.Values[i];
            if (unicodeJsEscapeRange.WithinRange(codePoint))
            {
                var numOfChars   = toMap.EncodeToUtf16(twoChars);
                var escapeLength = 6 * numOfChars;
                encoded[0] = '\\';
                encoded[1] = 'u';
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
            var exemptRange = exemptJsEscapeRanges.Values[i];
            if (exemptRange.WithinRange(codePoint))
            {
                var subSpan    = destSpan[destStartIndex..];
                var numOfChars = 0;
                numOfChars = toMap.EncodeToUtf16(subSpan);
                return numOfChars;
            }
        }
        for (var i = 0; i < cachedJsEscapeMappings.Length; i++)
        {
            var (mappedRange, mappedValues) = cachedJsEscapeMappings.Values[i];
            var offset = mappedRange.StartIncl;
            if (mappedRange.WithinRange(codePoint))
            {
                var mappedValue = mappedValues[codePoint - offset];
                var numOfChars  = mappedValue.Length;
                for (var j = 0; j < numOfChars; j++) { destSpan[destStartIndex + j] = mappedValue[j]; }
                return numOfChars;
            }
        }
        for (var i = 0; i < unicodeJsEscapeRanges.Length; i++)
        {
            var unicodeJsEscapeRange = unicodeJsEscapeRanges.Values[i];
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
        destSb.EnsureCapacity(2);
        return ProcessAppendRune(source.Value, destSb);
    }

    public virtual int Transfer(Rune? source, Span<char> dest, int destIndex)
    {
        if (source == null) return 0;
        return ProcessRune(source.Value, dest, destIndex);
    }

    public int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, IStringBuilder destSb)
    {
        if (!encodePrefix)
        {
            var i = 0;

            for (; i < source.Length; i++)
            {
                var nextChar = source[i];
                if (nextChar.IsValidJsonTypeOpening())
                    destSb.Append(nextChar);
                else
                    break;
            }
            if (i < source.Length) { return UffixTransfer(source, i, destSb) + i; }
            return i;
        }
        else { return UffixTransfer(source, 0, destSb); }
    }

    public int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex)
    {
        if (!encodePrefix)
        {
            var i = 0;

            for (; i < source.Length; i++)
            {
                var nextChar = source[i];
                if (nextChar.IsValidJsonTypeOpening())
                    destSpan.OverWriteAt(destStartIndex + i, nextChar);
                else
                    break;
            }
            if (i < source.Length) { return UffixTransfer(source, i, destSpan, destStartIndex + i) + i; }
            return i;
        }
        else { return UffixTransfer(source, 0, destSpan, destStartIndex); }
    }

    public int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeSuffix)
    {
        var transferAmount = 0;
        if (!encodeSuffix)
        {
            if (source.Length > 0)
            {
                var i = source.Length - 1;

                for (; i >= 0; i--)
                {
                    var prevChar = source[i];
                    if (!prevChar.IsValidJsonTypeClosing()) break;
                    transferAmount++;
                }
                i += 1;
                var encodedCount = UffixTransfer(source, 0, destSb, maxTransferCount: i);
                for (var j = i; j < source.Length; j++) { destSb.Append(source[j]); }
                return encodedCount + transferAmount;
            }
            return transferAmount;
        }
        else { return UffixTransfer(source, 0, destSb); }
    }

    public int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeSuffix)
    {
        if (!encodeSuffix)
        {
            var i = source.Length - 1;

            for (; i >= 0; i--)
            {
                var prevChar = source[i];
                if (!prevChar.IsValidJsonTypeClosing()) break;
            }
            i += 1;
            if (source.Length > 0)
            {
                var encodedCount = UffixTransfer(source, 0, destSpan, destStartIndex, maxTransferCount: i);
                var escapedChars = source.Length - i;
                for (var j = 0; j < escapedChars; j++) { destSpan.OverWriteAt(destStartIndex + encodedCount + j, source[i + j]); }
                return encodedCount + escapedChars;
            }
            return 0;
        }
        else { return UffixTransfer(source, 0, destSpan, destStartIndex); }
    }

    protected int UffixTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom      = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength    = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        var preAppendLength = destSb.Length;
        destSb.EnsureCapacity(cappedLength);
        var lastCharWasBrcOpn = false;
        var lastCharWasBrcCls = false;
        for (int i = cappedFrom; i < cappedLength; i++)
        {
            var charToTransfer = source[i];
            if (charToTransfer.IsBrcOpn())
            {
                if (lastCharWasBrcOpn) { lastCharWasBrcOpn = false; }
                else
                {
                    lastCharWasBrcOpn = true;
                    if (charToTransfer.IsSingleCharRune())
                    {
                        var iRune = new Rune(charToTransfer);
                        ProcessAppendRune(iRune, destSb);
                    }
                    else if (i + 1 < cappedLength)
                    {
                        var iRune = new Rune(charToTransfer, source[++i]);
                        ProcessAppendRune(iRune, destSb);
                    }
                }
            }
            else if (charToTransfer.IsBrcCls())
            {
                if (lastCharWasBrcCls) { lastCharWasBrcCls = false; }
                else
                {
                    lastCharWasBrcCls = true;

                    if (charToTransfer.IsSingleCharRune())
                    {
                        var iRune = new Rune(charToTransfer);
                        ProcessAppendRune(iRune, destSb);
                    }
                    else if (i + 1 < cappedLength)
                    {
                        var iRune = new Rune(charToTransfer, source[++i]);
                        ProcessAppendRune(iRune, destSb);
                    }
                }
            }
            else
            {
                lastCharWasBrcOpn = false;
                lastCharWasBrcCls = false;
                if (charToTransfer.IsSingleCharRune())
                {
                    var iRune = new Rune(charToTransfer);
                    ProcessAppendRune(iRune, destSb);
                }
                else if (i + 1 < cappedLength)
                {
                    var iRune = new Rune(charToTransfer, source[++i]);
                    ProcessAppendRune(iRune, destSb);
                }
            }
        }
        return destSb.Length - preAppendLength;
    }

    protected int UffixTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

        var lastCharWasBrcOpn = false;
        var lastCharWasBrcCls = false;

        int countAdded = 0;
        for (int i = cappedFrom; i < cappedLength; i++)
        {
            var charToTransfer = source[i];
            if (charToTransfer.IsBrcOpn())
            {
                if (lastCharWasBrcOpn) { lastCharWasBrcOpn = false; }
                else
                {
                    lastCharWasBrcOpn = true;
                    if (charToTransfer.IsSingleCharRune())
                    {
                        var iRune = new Rune(charToTransfer);
                        countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                    }
                    else if (i + 1 < cappedLength && destStartIndex + countAdded + 1 < destSpan.Length)
                    {
                        var iRune = new Rune(charToTransfer, source[++i]);
                        countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                    }
                }
            }
            else if (charToTransfer.IsBrcCls())
            {
                if (lastCharWasBrcCls) { lastCharWasBrcCls = false; }
                else
                {
                    lastCharWasBrcCls = true;
                    if (charToTransfer.IsSingleCharRune())
                    {
                        var iRune = new Rune(charToTransfer);
                        countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                    }
                    else if (i + 1 < cappedLength && destStartIndex + countAdded + 1 < destSpan.Length)
                    {
                        var iRune = new Rune(charToTransfer, source[++i]);
                        countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                    }
                }
            }
            else
            {
                lastCharWasBrcOpn = false;
                lastCharWasBrcCls = false;
                if (charToTransfer.IsSingleCharRune())
                {
                    var iRune = new Rune(charToTransfer);
                    countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                }
                else if (i + 1 < cappedLength && destStartIndex + countAdded + 1 < destSpan.Length)
                {
                    var iRune = new Rune(charToTransfer, source[++i]);
                    countAdded += ProcessRune(iRune, destSpan, destStartIndex + countAdded);
                }
            }
        }
        return countAdded;
    }

    public virtual int AppendTransfer(ReadOnlySpan<char> source, IStringBuilder destSb)
    {
        return JsEscapingTransfer(source, 0, destSb, destSb.Length);
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex)
    {
        return JsEscapingTransfer(source, 0, destSb, destStartIndex, isInsert: true);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex)
    {
        return JsEscapingTransfer(source, 0, destSb, destStartIndex);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int currentEndIndex)
    {
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, preAppendDestSpanEnd: currentEndIndex, isInsert: true);
    }

    public virtual int AppendTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destSb.Length, maxTransferCount);
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount, true);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int currentEndIndex, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount, currentEndIndex, true);
    }

    protected int CountEncodedChars(Rune toMap, Span<char> dummyBuffer)
    {
        return ProcessRune(toMap, dummyBuffer, 0);
    }

    public int CalculateEncodedLength(ReadOnlySpan<char> source, int sourceFrom = 0, int maxTransferCount = int.MaxValue)
    {
        var capLen    = Math.Clamp(maxTransferCount, 0, source.Length);
        var i         = Math.Clamp(sourceFrom, 0, source.Length);
        var end       = Math.Clamp(i + capLen, i, source.Length);
        int charCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];

        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
        }
        return charCount;
    }

    public int CalculateEncodedLength(char[] source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);

        int charCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
        }
        return charCount;
    }

    public int CalculateEncodedLength(ICharSequence source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        int charCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
        }
        return charCount;
    }

    public int CalculateEncodedLength(StringBuilder source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        int charCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                charCount += CountEncodedChars(iRune, dummyBuffer);
            }
        }
        return charCount;
    }

    public int CalculateLengthForCappedEncodeLength(int cappedLength, ReadOnlySpan<char> source, int sourceFrom = 0
      , int maxTransferCount = int.MaxValue)
    {
        if (source.Length == 0) { return 0; }

        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);

        int charCount     = 0;
        int nextRuneCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];

        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            if (charCount + nextRuneCount > cappedLength) { return Math.Max(0, i - 1); }
            charCount += nextRuneCount;
        }
        return i;
    }

    public int CalculateLengthForCappedEncodeLength(int cappedLength, char[] source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        if (source.Length == 0) { return 0; }

        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        var charCount     = 0;
        var nextRuneCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];

        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            if (charCount + nextRuneCount > cappedLength) { return Math.Max(0, i - 1); }
            charCount += nextRuneCount;
        }
        return i;
    }

    public int CalculateLengthForCappedEncodeLength(int cappedLength, ICharSequence source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        if (source.Length == 0) { return 0; }

        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        var charCount     = 0;
        var nextRuneCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];

        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            if (charCount + nextRuneCount > cappedLength) { return Math.Max(0, i - 1); }
            charCount += nextRuneCount;
        }
        return i;
    }

    public int CalculateLengthForCappedEncodeLength(int cappedLength, StringBuilder source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        if (source.Length == 0) { return 0; }

        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        var charCount     = 0;
        var nextRuneCount = 0;

        Span<char> dummyBuffer = stackalloc char[16];

        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                nextRuneCount = CountEncodedChars(iRune, dummyBuffer);
            }
            if (charCount + nextRuneCount > cappedLength) { return Math.Max(0, i - 1); }
            charCount += nextRuneCount;
        }
        return i;
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue, bool isInsert = false)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(capLen + i, i, source.Length);

        var encodedSize = CalculateEncodedLength(source, sourceFrom, capLen);
        sb.EnsureCapacity(encodedSize);

        var  originalLength = sb.Length;
        bool isAppend       = false;
        var  desti          = Math.Clamp(destStartIndex, 0, sb.Length);
        if (destStartIndex == int.MaxValue || destStartIndex >= sb.Length)
        {
            sb.Length += encodedSize;
            desti     =  originalLength;
        }
        else if(isInsert)
        {
            sb.Length      += encodedSize;
            var oldLength = originalLength;
            for (var j = oldLength - 1; j >= destStartIndex; j--) { sb[j + encodedSize] = sb[j]; }
        }
        else
        {
            sb.Length = Math.Max(originalLength, destStartIndex + encodedSize);
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                var added = ProcessRune(iRune, sb, desti);
                countAdded += added;
                desti      += added;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                var added = ProcessRune(iRune, sb, desti);
                countAdded += added;
                desti      += added;
            }
        }
        // if (!isAppend && !isInsert)
        // {
        //     sb.Length = Math.Max(originalLength, destStartIndex + countAdded);
        //     return Math.Max(0, destStartIndex + countAdded - originalLength);
        // }
        return countAdded;
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue, int preAppendDestSpanEnd = int.MaxValue, bool isInsert = false)
    {
        var sourceAppendLen = source.Length;

        var capLen      = Math.Clamp(maxTransferCount, 0, sourceAppendLen);
        var i           = Math.Clamp(sourceFrom, 0, sourceAppendLen);
        var end         = Math.Clamp(capLen + i, 0, sourceAppendLen);
        var desti       = destStartIndex;
        
        var encodedSize = CalculateEncodedLength(source, sourceFrom, capLen);
        if(isInsert)
        {
            var oldLength = preAppendDestSpanEnd;
            for (var j = oldLength - 1; j >= destStartIndex; j--) { destination[j + encodedSize] = destination[j]; }
        }
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

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb,
        int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        var preTransferLen = destSb.Length;
        if (parentJsonOptions.CharBufferWritesAsCharCollection)
        {
            int j;
            var capLen     = Math.Clamp(maxTransferCount, 0, source.Length);
            int cappedFrom = j = Math.Clamp(sourceFrom, 0, source.Length);
            var cappedEnd  = Math.Clamp(capLen, 0, source.Length);


            destSb.EnsureCapacity(CalculateEncodedLength(source, sourceFrom, capLen));

            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; j < cappedEnd; j++)
            {
                var item = source[j];
                if (j > 0) stringFormatter.AddCollectionElementSeparator(typeof(char), destSb, j);
                lastAdded = lastAdded == 0 && j > cappedFrom
                    ? stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), j - 1, destSb, "")
                    : stringFormatter.CollectionNextItemFormat(item, j, destSb, "");
                previousChar = lastAdded == 0 ? item : '\0';
            }
            return destSb.Length - preTransferLen;
        }
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        if (parentJsonOptions.CharBufferWritesAsCharCollection)
        {
            var charsAdded = 0;
            var capLen     = Math.Clamp(maxTransferCount, 0, source.Length);
            int i          = Math.Clamp(sourceFrom, 0, source.Length);
            var cappedEnd  = Math.Clamp(capLen + i, 0, source.Length);

            int  lastAdded    = 0;
            char previousChar = '\0';
            for (; i < cappedEnd; i++)
            {
                var item = source[i];

                if (i > 0 && lastAdded > 0)
                    charsAdded += stringFormatter.AddCollectionElementSeparatorAndPadding(typeof(char), destSpan,
                                                                                          destStartIndex + charsAdded, i);

                lastAdded = lastAdded > 0 || i == sourceFrom
                    ? stringFormatter.CollectionNextItemFormat(item, i, destSpan, destStartIndex + charsAdded, "")
                    : stringFormatter.CollectionNextItemFormat(new Rune(previousChar, item), i - 1,
                                                               destSpan, destStartIndex + charsAdded, "");
                previousChar =  lastAdded == 0 ? item : '\0';
                charsAdded   += lastAdded;
            }
            return i;
        }
        return JsEscapingTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue,
        int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source.AsSpan(), sourceFrom, sb, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source.AsSpan(), sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(StringBuilder source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        StringValueDelimiter(destSb);
        JsEscapingTransfer(source, 0, destSb, destStartIndex);
        StringValueDelimiter(destSb);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(StringBuilder source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var charsAdded = StringValueDelimiter(destSpan, destStartIndex);
        charsAdded += JsEscapingTransfer(source, 0, destSpan, destStartIndex + charsAdded, maxTransferCount);
        charsAdded += StringValueDelimiter(destSpan, destStartIndex + charsAdded);
        return charsAdded;
    }

    public virtual int Transfer(StringBuilder source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
    }

    public virtual int Transfer(StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(capLen + i, 0, source.Length);

        sb.EnsureCapacity(CalculateEncodedLength(source, sourceFrom, capLen));

        var  originalLength = sb.Length;
        bool isAppend       = false;
        if (destStartIndex == int.MaxValue || destStartIndex > sb.Length)
        {
            destStartIndex = originalLength;
            isAppend       = true;
        }
        else if (destStartIndex < sb.Length)
        {
            if (destStartIndex + end > sb.Length) { sb.Length = destStartIndex + end; }
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                int appended;
                if (isAppend) { appended = ProcessAppendRune(iRune, sb); }
                else
                {
                    if (destStartIndex + countAdded + 6 > sb.Length) { sb.Length = destStartIndex + countAdded + 6; }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                int appended;
                if (isAppend) { appended = ProcessAppendRune(iRune, sb); }
                else
                {
                    if (destStartIndex + countAdded + 12 > sb.Length) { sb.Length = destStartIndex + countAdded + 12; }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
        }
        if (!isAppend) { sb.Length = Math.Max(originalLength, destStartIndex + countAdded); }
        return sb.Length - originalLength;
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(capLen + i, 0, source.Length);
        var desti  = destStartIndex;
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

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
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
        JsEscapingTransfer(source, sourceFrom, destSb, destStartIndex, maxTransferCount);
        return destSb.Length - preTransferLen;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var charsAdded = 0;
        charsAdded += JsEscapingTransfer(source, sourceFrom, destSpan, destStartIndex, maxTransferCount);
        return charsAdded;
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, 0, source.Length);

        sb.EnsureCapacity(CalculateEncodedLength(source, sourceFrom, capLen));

        var  originalLength = sb.Length;
        bool isAppend       = false;
        if (destStartIndex == int.MaxValue || destStartIndex > sb.Length)
        {
            destStartIndex = originalLength;
            isAppend       = true;
        }
        else if (destStartIndex < sb.Length)
        {
            if (destStartIndex + end > sb.Length) { sb.Length = destStartIndex + end; }
        }
        int countAdded = 0;
        for (; i < end; i++)
        {
            var iChar = source[i];
            if (iChar.IsSingleCharRune())
            {
                var iRune = new Rune(iChar);
                int appended;
                if (isAppend) { appended = ProcessAppendRune(iRune, sb); }
                else
                {
                    if (destStartIndex + countAdded + 6 > sb.Length) { sb.Length = destStartIndex + countAdded + 6; }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
            else if (i + 1 < end)
            {
                var iRune = new Rune(iChar, source[++i]);
                int appended;
                if (isAppend) { appended = ProcessAppendRune(iRune, sb); }
                else
                {
                    if (destStartIndex + countAdded + 12 > sb.Length) { sb.Length = destStartIndex + countAdded + 12; }
                    appended = ProcessRune(iRune, sb, destStartIndex + countAdded);
                }
                countAdded += appended;
            }
        }
        if (!isAppend) { sb.Length = Math.Max(originalLength, destStartIndex + countAdded); }
        return sb.Length - originalLength;
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, Span<char> destination
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var desti  = Math.Clamp(capLen + i, 0, source.Length);
        var end    = Math.Min(source.Length, maxTransferCount + sourceFrom);
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
    
    public override JsonEscapingEncodingTransfer CopyFrom(JsonEscapingEncodingTransfer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        cachedJsEscapeMappings = source.cachedJsEscapeMappings;
        exemptJsEscapeRanges   = source.exemptJsEscapeRanges;
        unicodeJsEscapeRanges  = source.unicodeJsEscapeRanges;
        layoutEncoder  = source.layoutEncoder;
        layoutEncoder?.IncrementRefCount();
        return this;
    }

    public override void StateReset()
    {
        layoutEncoder?.DecrementRefCount();
        layoutEncoder          = null;
        cachedJsEscapeMappings = new CappedCachedUnicodeEscapeRangesValuesInlineArray();
        exemptJsEscapeRanges   = new CappedUnicodeEscapeRangesInlineArray();
        unicodeJsEscapeRanges = new CappedUnicodeEscapeRangesInlineArray();
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IEncodingTransfer ICloneable<IEncodingTransfer>.Clone() => Clone();

    public override JsonEscapingEncodingTransfer Clone()
    {
        return AlwaysRecycler.Borrow<JsonEscapingEncodingTransfer>().CopyFrom(this);
    }
}
