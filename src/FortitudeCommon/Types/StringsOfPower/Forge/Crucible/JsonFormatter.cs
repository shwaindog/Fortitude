// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FieldContentHandlingExtensions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public class JsonFormatter : CustomStringFormatter, ICustomStringFormatter
{
    private byte previousByteUnusedBits;
    private byte previousByteBitCount;

    protected const string BrcOpn     = "{";
    protected const char   BrcOpnChar = '{';
    protected const string BrcCls     = "}";
    protected const char   BrcClsChar = '}';
    protected const string Cma        = ",";


    public virtual IJsonFormattingOptions JsonOptions
    {
        get => (IJsonFormattingOptions)(FormatOptions ??= new JsonFormattingOptions());
        set => FormatOptions = value;
    }

    public JsonFormatter(IJsonFormattingOptions jsonFormattingOptions)
    {
        JsonOptions = jsonFormattingOptions;
    }

    public JsonFormatter() { }


    public override FormattingHandlingFlags ResolveStringFormattingFlags<T>(
        char lastNonWhiteSpace, T input, FormattingHandlingFlags callerFormattingFlags
      , string formatString = "")
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags())
        {
            return callerFormattingFlags;
        }
        
        FormattingHandlingFlags setFlags = callerFormattingFlags;

        var typeofT = typeof(T);
        if (!formatString.IsDblQtBounded() && typeofT.IsSpanFormattable() || typeofT.IsNullableSpanFormattable())
        {
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && JsonOptions.WrapValuesInQuotes
                ? EnsureFormattedDelimited
                : None;

            switch (input)
            {
                case Complex:
                case Version:
                case IPNetwork:
                case IPAddress:
                case Rune:
                case TimeSpan:
                case DateTime:
                case DateOnly:
                case TimeOnly:
                case Uri:
                case Guid:
                    setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() ? EnsureFormattedDelimited : None;
                    break;
                default:
                    setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && !typeofT.IsJsonStringExemptType()
                        ? EnsureFormattedDelimited : None;
                    break;
            }
        }
        return setFlags;
    }

    protected virtual int NextBase64Chars(byte source, IStringBuilder sb)
    {
        var b = source;

        int firstChar;
        switch (previousByteBitCount)
        {
            case 0:
                previousByteUnusedBits = (byte)((0x3 & b) << 4);
                previousByteBitCount   = 2;
                firstChar              = b >> 2;
                sb.Append(Base64LookupTable[firstChar]);
                return 1;
            case 2:
                firstChar              = previousByteUnusedBits | ((0xF0 & b) >> 4);
                previousByteUnusedBits = (byte)((0xF & b) << 2);
                previousByteBitCount   = 4;
                sb.Append(Base64LookupTable[firstChar]);
                return 1;
            case 4:
                var secondChar = (byte)((0x3F & b));
                firstChar = previousByteUnusedBits | ((0xC0 & b) >> 6);
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append(Base64LookupTable[secondChar]);
                previousByteBitCount = 0;
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int CompleteBase64Sequence(IStringBuilder sb)
    {
        int firstChar;
        switch (previousByteBitCount)
        {
            case 0: return 0;
            case 2:
                firstChar = previousByteUnusedBits;
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append('=');
                sb.Append('=');
                previousByteBitCount = 0;
                return 3;
            case 4:
                previousByteBitCount = 0;
                firstChar            = previousByteUnusedBits;
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append('=');
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int NextBase64Chars(byte source, Span<char> dest, int destIndex)
    {
        var b = source;

        int firstChar;
        switch (previousByteBitCount)
        {
            case 0:
                previousByteUnusedBits = (byte)((0x3 & b) << 4);
                previousByteBitCount   = 2;
                firstChar              = b >> 2;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                return 1;
            case 2:
                firstChar              = previousByteUnusedBits | ((0xF0 & b) >> 4);
                previousByteUnusedBits = (byte)((0xF & b) << 2);
                previousByteBitCount   = 4;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                return 1;
            case 4:
                var secondChar = (byte)((0x3F & b));
                firstChar = previousByteUnusedBits | ((0xC0 & b) >> 6);
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, Base64LookupTable[secondChar]);
                previousByteBitCount = 0;
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int CompleteBase64Sequence(Span<char> dest, int destIndex)
    {
        int firstChar;
        switch (previousByteBitCount)
        {
            case 0: return 0;
            case 2:
                firstChar = previousByteUnusedBits;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, '=');
                dest.OverWriteAt(destIndex + 2, '=');
                previousByteBitCount = 0;
                return 3;
            case 4:
                previousByteBitCount = 0;
                firstChar            = previousByteUnusedBits;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, '=');
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    private bool IsDoubleQuoteEnclosed(Span<char> toCheck)    => toCheck[0] == DblQtChar && toCheck[^1] == DblQtChar;
    private bool IsBracesEnclosed(Span<char> toCheck)         => toCheck[0] == BrcOpnChar && toCheck[^1] == BrcClsChar;
    private bool IsSquareBracketsEnclosed(Span<char> toCheck) => toCheck[0] == SqBrktOpnChar && toCheck[^1] == SqBrktClsChar;


    public override int ProcessAppendedRange(IStringBuilder sb, int fromIndex)
    {
        var originalSbLen = sb.Length;
        var appendLen     = originalSbLen - fromIndex;
        if (appendLen < 4096)
        {
            var scratchFull = stackalloc char[appendLen + 2].ResetMemory();
            for (int i = 0; i < appendLen; i++) { scratchFull[i + 1] = sb[fromIndex + i]; }
            return ProcessAppended(sb, scratchFull, fromIndex);
        }
        var largeScrachBuffer = (appendLen + 2).SourceRecyclingCharArray();
        var fullSpan          = largeScrachBuffer.RemainingAsSpan();
        largeScrachBuffer.Insert(1, sb, fromIndex, appendLen);
        var boundedScratchSpan = fullSpan[0.. (appendLen + 2)];
        var change             = ProcessAppended(sb, boundedScratchSpan, fromIndex);
        largeScrachBuffer.DecrementRefCount();
        return change;
    }

    private int ProcessAppended(IStringBuilder sb, Span<char> scratchFull, int fromIndex)
    {
        var originalSbLen = sb.Length;
        var justAppended  = scratchFull[1..^1];
        if (IsBracesEnclosed(justAppended) || IsSquareBracketsEnclosed(justAppended)) { return 0; }
        if (IsDoubleQuoteEnclosed(justAppended))
        {
            JsonOptions.EncodingTransfer.Transfer(this, justAppended, 0, sb, fromIndex, justAppended.Length);
            return sb.Length - originalSbLen;
        }
        if (JsonOptions.WrapValuesInQuotes)
        {
            scratchFull[0]  = '\"';
            scratchFull[^1] = '\"';
            JsonOptions.EncodingTransfer.Transfer(this, scratchFull, 0, sb, fromIndex, scratchFull.Length);
        }
        return sb.Length - originalSbLen;
    }

    public override int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length)
    {
        var appendLen = length - fromIndex;
        if (appendLen < 4096)
        {
            var scratchFull = stackalloc char[appendLen + 2].ResetMemory();
            for (int i = 0; i < appendLen; i++) { scratchFull[i + 1] = destSpan[fromIndex + i]; }
            return ProcessAppended(destSpan, scratchFull, fromIndex, length);
        }
        var largeScrachBuffer = (appendLen + 2).SourceRecyclingCharArray();
        var fullSpan          = largeScrachBuffer.RemainingAsSpan();
        largeScrachBuffer.Insert(1, destSpan, fromIndex, appendLen);
        var boundedScratchSpan = fullSpan[0.. (appendLen + 2)];
        var change             = ProcessAppended(destSpan, boundedScratchSpan, fromIndex, length);
        largeScrachBuffer.DecrementRefCount();
        return change;
    }

    private int ProcessAppended(Span<char> destSpan, Span<char> scratchFull, int fromIndex, int length)
    {
        var appendLen    = length - fromIndex;
        var justAppended = scratchFull[1..^1];
        if (IsBracesEnclosed(justAppended) || IsSquareBracketsEnclosed(justAppended)) { return 0; }
        if (IsDoubleQuoteEnclosed(justAppended))
        {
            var charsAdded = JsonOptions.EncodingTransfer.Transfer(this, justAppended, 0, destSpan, fromIndex, justAppended.Length);
            return charsAdded - appendLen;
        }
        if (JsonOptions.WrapValuesInQuotes)
        {
            scratchFull[0]  = '\"';
            scratchFull[^1] = '\"';
            var charsAdded = JsonOptions.EncodingTransfer.Transfer(this, scratchFull, 0, destSpan, fromIndex, scratchFull.Length);
            return charsAdded - appendLen;
        }
        return 0;
    }

    public override int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast) where TFmt : default
    {
        if (source == null) { return (Options.NullWritesNothing ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length)); }
        var originalLength = sb.Length;
        var fmtType        = typeof(TFmt);
        fmtType = fmtType == typeof(ISpanFormattable) ? source.GetType() : fmtType;
        var hasFormatQuotes = formatString.IsDblQtBounded();
        var wrapInQuotes    = formatFlags.ShouldDelimit();
        if (fmtType.IsValueType && fmtType.IsNumericType())
        {
            if (!wrapInQuotes)
            {
                switch (source)
                {
                    case Complex:
                    case char:
                        wrapInQuotes = true;
                        break;
                    case Half halfSource:     wrapInQuotes = Half.IsNaN(halfSource); break;
                    case float floatSource:   wrapInQuotes = float.IsNaN(floatSource); break;
                    case double doubleSource: wrapInQuotes = double.IsNaN(doubleSource); break;
                }
            }
            if (wrapInQuotes && !hasFormatQuotes) sb.Append(DblQt);
            base.Format(source, sb, formatString);
            if (wrapInQuotes && !hasFormatQuotes) sb.Append(DblQt);
        }
        else if (source is DateTime sourceDateTime)
        {
            if (JsonOptions.DateTimeIsNumber)
            {
                var converted = JsonOptions.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                if (wrapInQuotes) sb.Append(DblQt);
                base.Format(converted, sb, formatString);
                if (wrapInQuotes) sb.Append(DblQt);
            }
            else
            {
                if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
                {
                    formatString = JsonOptions.DateTimeAsStringFormatString;
                }
                if (!hasFormatQuotes) sb.Append(DblQt);
                base.Format(source, sb, formatString);
                if (!hasFormatQuotes) sb.Append(DblQt);
            }
        }
        else if (source is DateOnly dateTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.DateOnlyAsStringFormatString;
            }
            if (!hasFormatQuotes) sb.Append(DblQt);
            base.Format(dateTimeOnly, sb, formatString);
            if (!hasFormatQuotes) sb.Append(DblQt);
        }
        else if (source is TimeOnly sourceTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.TimeAsStringFormatString;
                if (sourceTimeOnly == default) // System.Text.Json changes formatting for default values to just ss precision
                {
                    formatString = "HH:mm:ss";
                }
            }
            if (!hasFormatQuotes) sb.Append(DblQt);
            base.Format(source, sb, formatString);
            if (!hasFormatQuotes) sb.Append(DblQt);
        }
        else if (source is Enum)
        {
            var markInsertIndex = sb.Length;
            var enumLen         = base.Format(source, sb, formatString);
            var isInt           = enumLen > 0;
            for (int i = 0; i < enumLen && isInt; i++)
            {
                var checkChar = sb[markInsertIndex + i];
                isInt = checkChar.IsDigit() || (checkChar.IsMinus() && i == 0);
            }
            if (!isInt)
            {
                sb.InsertAt(DblQt, markInsertIndex);
                sb.Append(DblQt);
            }
        }
        else
        {
            if (wrapInQuotes && !hasFormatQuotes) sb.Append(DblQt);
            base.Format(source, sb, formatString);
            if (wrapInQuotes && !hasFormatQuotes) sb.Append(DblQt);
        }
        return sb.Length - originalLength;
    }

    public override int Format<TFmt>(TFmt? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast) where TFmt : default
    {
        if (source == null) { return Options.NullWritesNothing ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString); }
        var charsAdded = 0;
        var fmtType    = typeof(TFmt);
        if (fmtType.IsValueType && fmtType.IsNumericType())
        {
            var wrapInQuotes = JsonOptions.WrapValuesInQuotes;
            if (!wrapInQuotes)
            {
                switch (source)
                {
                    case char:                wrapInQuotes = true; break;
                    case Half halfSource:     wrapInQuotes = Half.IsNaN(halfSource); break;
                    case float floatSource:   wrapInQuotes = float.IsNaN(floatSource); break;
                    case double doubleSource: wrapInQuotes = double.IsNaN(doubleSource); break;
                }
            }
            if (wrapInQuotes) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString);
            if (wrapInQuotes) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        else if (source is DateTime sourceDateTime)
        {
            if (JsonOptions.DateTimeIsNumber)
            {
                var converted                                  = JsonOptions.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                if (JsonOptions.WrapValuesInQuotes) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += base.Format(converted, destCharSpan, destStartIndex + charsAdded, formatString);
                if (JsonOptions.WrapValuesInQuotes) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
            }
            else
            {
                if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
                {
                    formatString = JsonOptions.DateTimeAsStringFormatString;
                }
                charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
            }
        }
        else if (source is DateOnly dateTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.DateOnlyAsStringFormatString;
            }
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(dateTimeOnly, destCharSpan, destStartIndex + charsAdded, formatString);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        else if (source is TimeOnly sourceTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.TimeAsStringFormatString;
                if (sourceTimeOnly == default) // System.Text.Json changes formatting for default values to just ss precision
                {
                    formatString = "HH:mm:ss";
                }
            }
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(sourceTimeOnly, destCharSpan, destStartIndex + charsAdded, formatString);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        else if (source is Enum)
        {
            var enumLen = base.Format(source, destCharSpan, destStartIndex, formatString);
            var isInt   = enumLen > 0;
            for (int i = 0; i < enumLen && isInt; i++)
            {
                var checkChar = destCharSpan[destStartIndex + i];
                isInt = checkChar.IsDigit() || (checkChar.IsMinus() && i == 0);
            }
            if (!isInt)
            {
                destCharSpan.ShiftByAmount(destStartIndex, destStartIndex + enumLen, 1);
                destCharSpan.OverWriteAt(destStartIndex, DblQt);
                destCharSpan.OverWriteAt(destStartIndex + enumLen + 1, DblQt);
                charsAdded = enumLen + 2;
            }
            else { charsAdded = enumLen; }
        }
        else
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        return charsAdded;
    }

    public override int Format<TFmtStruct>(TFmtStruct? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        if (!source.HasValue) { return (Options.NullWritesNothing ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length)); }
        return Format(source.Value, sb, formatString, formatFlags);
    }

    public override int Format<TFmtStruct>(TFmtStruct? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        if (!source.HasValue) { return Options.NullWritesNothing ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString); }
        return Format(source.Value, destCharSpan, destStartIndex, formatString);
    }


    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var preAppendLen = sb.Length;

        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var preAppendLen = sb.Length;

        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmtStruct);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast) where TFmt : default
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }


    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var preAppendLen = sb.Length;

        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Count > 0); }
        for (var i = 0; i < arg0.Count; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Count); }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var preAppendLen = sb.Length;

        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Count > 0); }
        for (var i = 0; i < arg0.Count; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Count); }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmtStruct);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var  preAppendLen         = sb.Length;
        var  hasStartedCollection = false;
        var  itemCount            = 0;
        var  elementType          = typeof(TFmt);
        char lastChar             = '\0';
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), sb); }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { CollectionEnd(elementType, sb, itemCount); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { sb.Append(Options.NullString); }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var addedChars           = 0;
        var hasStartedCollection = false;

        var  itemCount   = 0;
        var  elementType = typeof(TFmt);
        char lastChar    = '\0';
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;

        var  itemCount   = 0;
        var  elementType = typeof(TFmtStruct);
        char lastChar    = '\0';
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), sb); }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { CollectionEnd(elementType, sb, itemCount); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { sb.Append(Options.NullString); }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var  addedChars           = 0;
        var  hasStartedCollection = false;
        var  itemCount            = 0;
        var  elementType          = typeof(TFmtStruct);
        char lastChar             = '\0';
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var  itemCount = 0;
        char lastChar  = '\0';
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), sb); }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
        where TFmt : default
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext); }
        var  itemCount = 0;
        char lastChar  = '\0';
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var  itemCount = 0;
        char lastChar  = '\0';
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune()) { JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), sb); }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext); }
        var  itemCount = 0;
        char lastChar  = '\0';
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && JsonOptions.CharArrayWritesString)
            {
                if (iChar.IsSingleCharRune())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars);
                }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += JsonOptions.EncodingTransfer.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcOpn).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(destStartIndex, BrcOpn);
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        return sb.Append(Cma).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        return destSpan.OverWriteAt(atIndex, Cma);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast) where TFmt : default
    {
        formatFlags = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, formatFlags, formatString);
        switch (nextItem)
        {
            case Rune runeItem:
                if (JsonOptions.CharArrayWritesString) { return Options.EncodingTransfer.Transfer(runeItem, sb); }
                sb.Append(DblQt);
                var runeElementAdded = Options.EncodingTransfer.Transfer(runeItem, sb);
                sb.Append(DblQt);
                return runeElementAdded + 2;
            case char charItem:
                if (JsonOptions.CharArrayWritesString)
                {
                    if (charItem.IsSingleCharRune()) return Options.EncodingTransfer.Transfer(new Rune(charItem), sb);
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    sb.Append(DblQt);
                    var charsAdded = Options.EncodingTransfer.Transfer(new Rune(charItem), sb);
                    sb.Append(DblQt);
                    return charsAdded + 2;
                }
                break;
            case byte byteItem:
                if (JsonOptions.ByteArrayWritesBase64String) return NextBase64Chars(byteItem, sb);
                break;
        }
        return Format(nextItem, sb, formatString, formatFlags);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast) where TFmt : default
    {
        formatFlags = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, formatFlags, formatString);
        switch (nextItem)
        {
            case Rune runeItem:
                if (JsonOptions.CharArrayWritesString) { return Options.EncodingTransfer.Transfer(runeItem, destCharSpan, destStartIndex); }
                destCharSpan.OverWriteAt(destStartIndex, DblQt);
                var runeElementAdded = Options.EncodingTransfer.Transfer(runeItem, destCharSpan, destStartIndex + 1);
                destCharSpan.OverWriteAt(destStartIndex + runeElementAdded + 2, DblQt);
                return runeElementAdded + 2;
            case char charItem:
                if (JsonOptions.CharArrayWritesString)
                {
                    if (charItem.IsSingleCharRune()) return Options.EncodingTransfer.Transfer(new Rune(charItem), destCharSpan, destStartIndex);
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    if (charItem.IsSingleCharRune())
                    {
                        destCharSpan.OverWriteAt(destStartIndex, DblQt);
                        var charsAdded = Options.EncodingTransfer.Transfer(new Rune(charItem), destCharSpan, destStartIndex + 1);
                        destCharSpan.OverWriteAt(destStartIndex + charsAdded + 1, DblQt);
                        return charsAdded + 2;
                    }
                    return 0;
                }
                break;
            case byte byteItem:
                if (JsonOptions.ByteArrayWritesBase64String) return NextBase64Chars(byteItem, destCharSpan, destStartIndex);
                break;
        }
        return Format(nextItem, destCharSpan, destStartIndex, formatString, formatFlags);
    }

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        formatFlags = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, formatFlags, formatString);
        switch (nextItem)
        {
            case Rune runeItem:
                if (JsonOptions.CharArrayWritesString) { return Options.EncodingTransfer.Transfer(runeItem, sb); }
                sb.Append(DblQt);
                var runeElementAdded = Options.EncodingTransfer.Transfer(runeItem, sb);
                sb.Append(DblQt);
                return runeElementAdded + 2;
            case char charItem:
                if (JsonOptions.CharArrayWritesString)
                {
                    if (charItem.IsSingleCharRune()) return Options.EncodingTransfer.Transfer(new Rune(charItem), sb);
                    return 0;
                }
                if (charItem.IsSingleCharRune())
                {
                    sb.Append(DblQt);
                    var charsAdded = Options.EncodingTransfer.Transfer(new Rune(charItem), sb);
                    sb.Append(DblQt);
                    return charsAdded + 2;
                }
                return 0;
        }
        return Format(nextItem, sb, formatString, formatFlags);
    }

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeAllButPrefixFirstSuffixLast)
    {
        formatFlags = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, formatFlags, formatString);
        switch (nextItem)
        {
            case Rune runeItem:
                if (JsonOptions.CharArrayWritesString) { return Options.EncodingTransfer.Transfer(runeItem, destCharSpan, destStartIndex); }
                destCharSpan.OverWriteAt(destStartIndex, DblQt);
                var runeElementAdded = Options.EncodingTransfer.Transfer(runeItem, destCharSpan, destStartIndex + 1);
                destCharSpan.OverWriteAt(destStartIndex + runeElementAdded + 1, DblQt);
                return runeElementAdded + 2;
            case char charItem:
                if (JsonOptions.CharArrayWritesString)
                {
                    if (charItem.IsSingleCharRune()) return Options.EncodingTransfer.Transfer(new Rune(charItem), destCharSpan, destStartIndex);
                    return 0;
                }
                if (charItem.IsSingleCharRune())
                {
                    destCharSpan.OverWriteAt(destStartIndex, DblQt);
                    var charsAdded = Options.EncodingTransfer.Transfer(new Rune(charItem), destCharSpan, destStartIndex + 1);
                    destCharSpan.OverWriteAt(destStartIndex + charsAdded + 1, DblQt);
                    return charsAdded + 2;
                }
                return 0;
        }
        return Format(nextItem, destCharSpan, destStartIndex, formatString, formatFlags);
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb)
    {
        var formatFlags = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, DefaultCallerTypeFlags);
        var preAppendLen = sb.Length;
        if (nextItem == null) { return sb.Append(JsonOptions.NullStyle).ReturnCharCount(JsonOptions.NullStyle.Length); }
        switch (nextItem)
        {
            case string stringItem: return sb.Append(DblQt).Append(stringItem).Append(DblQt).ReturnCharCount(stringItem.Length + 2);
            case char[] charArrayItem:
                if (!JsonOptions.CharArrayWritesString)
                {
                    CollectionStart(typeof(char[]), sb, charArrayItem.Length > 0);
                    JsonOptions.EncodingTransfer.Transfer(this, charArrayItem, sb);
                    CollectionEnd(typeof(char[]), sb, charArrayItem.Length);
                    return sb.Length - preAppendLen;
                }
                return sb.Append(DblQt).Append(charArrayItem).Append(DblQt).ReturnCharCount(charArrayItem.Length + 2);
            case ICharSequence charSequenceItem:
                if (!JsonOptions.CharArrayWritesString)
                {
                    CollectionStart(charSequenceItem.GetType(), sb, charSequenceItem.Length > 0);
                    JsonOptions.EncodingTransfer.Transfer(this, charSequenceItem, sb);
                    CollectionEnd(charSequenceItem.GetType(), sb, charSequenceItem.Length);
                    return sb.Length - preAppendLen;
                }
                return sb.Append(DblQt).Append(charSequenceItem).Append(DblQt).ReturnCharCount(charSequenceItem.Length + 2);
            case StringBuilder sbItem: return sb.Append(DblQt).Append(sbItem).Append(DblQt).ReturnCharCount(sbItem.Length + 2);
            case KeyValuePair<string, JsonNode> jsonNodeKvp:
                var jsonString   = jsonNodeKvp.Value.ToJsonString();
                var keyValueLeng = jsonString.Length + jsonNodeKvp.Key.Length;
                return sb.Append(DblQt).Append(jsonNodeKvp.Key).Append(DblQt).Append(":").Append(jsonString).ReturnCharCount(keyValueLeng + 3);
        }
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        sb.Append(nextItem);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex)
    {
        var formatFlags = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, DefaultCallerTypeFlags);
        var charsAdded  = 0;
        switch (nextItem)
        {
            case KeyValuePair<string, JsonNode> jsonNodeKvp:
                var jsonString = jsonNodeKvp.Value.ToJsonString();
                charsAdded = destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, jsonNodeKvp.Key);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, ":");
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, jsonString);
                return charsAdded;
        }
        CharSpanCollectionScratchBuffer ??= MutableString.MediumScratchBuffer;
        CharSpanCollectionScratchBuffer.Clear();
        CharSpanCollectionScratchBuffer.Append(nextItem);
        
        if (formatFlags.ShouldDelimit()) charsAdded = destCharSpan.OverWriteAt(destStartIndex, DblQt);
        charsAdded += destCharSpan.OverWriteAt(destStartIndex, CharSpanCollectionScratchBuffer);
        if (formatFlags.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
        return charsAdded;
    }


    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(sb);
            return sb.Append(DblQt).ReturnCharCount(1 + addedChars);
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcCls).ReturnCharCount(1);
        return sb.Append(SqBrktCls).ReturnCharCount(1);
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int index, int itemsCount)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(index, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(destSpan, index);
            return destSpan.OverWriteAt(index + addedChars, DblQt) + addedChars;
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(index, BrcCls);
        return destSpan.OverWriteAt(index, SqBrktCls);
    }
}


public static class JsonFormatterExtensions
{
    public static bool IsJsonStringExemptType(this Type checkType)
    {
        if (checkType.IsUniversalStringExemptNumberType()) return true;
        if (checkType.IsBool()) return true;
        if (checkType.IsNullableBool()) return true;
        return false;
    }
}