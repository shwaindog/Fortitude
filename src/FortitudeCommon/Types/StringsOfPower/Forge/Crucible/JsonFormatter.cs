// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitchesExtensions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public class JsonFormatter : CustomStringFormatter, ICustomStringFormatter
{
    private byte previousByteUnusedBits;
    private byte previousByteBitCount;

    public const string BrcOpn     = "{";
    public const char   BrcOpnChar = '{';
    public const string BrcCls     = "}";
    public const char   BrcClsChar = '}';
    public const string Cma        = IFormattingOptions.Cma;


    public virtual IJsonFormattingOptions JsonOptions
    {
        get => (IJsonFormattingOptions)(FormatOptions ??= new JsonFormattingOptions());
        set => FormatOptions = value;
    }

    public JsonFormatter(IJsonFormattingOptions jsonFormattingOptions)
    {
        JsonOptions = jsonFormattingOptions;
    }

    public JsonFormatter()
    {
        JsonOptions = new JsonFormattingOptions();
    }


    public override IEncodingTransfer ContentEncoder
    {
        get => StringEncoderTransfer ??= JsonOptions.SourceEncodingTransfer(JsonOptions);
        set => base.ContentEncoder = value;
    }


    public override FormatSwitches ResolveStringFormattingFlags<T>(
        char lastNonWhiteSpace, T input, FormatSwitches callerFormattingFlags
      , string formatString = "")
    {
        if (callerFormattingFlags.HasNoAutoAddCallerTypeFlag() || input == null) { return callerFormattingFlags; }

        FormatSwitches setFlags = callerFormattingFlags;

        var typeofT                           = input.GetType();
        var typeIsSpanFormattableOrNullable = typeofT.IsSpanFormattableOrNullableCached();
        if (formatString.IsNotJsonTypeOpenCloseBounded()
         && typeIsSpanFormattableOrNullable
         && !typeofT.IsEnum() // enums will by default auto select, but will always delimit
            // if EnsureFormattedDelimited unless format string does so already   
         && !callerFormattingFlags.HasDisableAutoDelimiting()
         && !callerFormattingFlags.HasAsValueContentFlag())
        {
            setFlags |= JsonOptions.WrapValuesInQuotes
                     || (input.IsDoubleQuoteDelimitedSpanFormattable()
                      || !typeofT.IsJsonStringExemptType())
                ? EnsureFormattedDelimited
                : None;
        }
        if (callerFormattingFlags.DoesNotHaveReformatMultiLineFlag() && typeIsSpanFormattableOrNullable)
        {
            setFlags |= EncodeInnerContent;
        }
        return setFlags;
    }

    public override int AppendDelimiterStart(Type maybeDelimited, IStringBuilder sb)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars()
         || maybeDelimited.IsChar()
         || (maybeDelimited.IsSpanFormattableOrNullable()
          && maybeDelimited.IsDoubleQuoteDelimitedSpanFormattable())) { return ContentEncoder.StringValueDelimiter(sb); }
        return 0;
    }

    public override int AppendDelimiterEnd(Type maybeDelimited, IStringBuilder sb)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars()
         || maybeDelimited.IsChar()
         || (maybeDelimited.IsSpanFormattableOrNullable()
          && maybeDelimited.IsDoubleQuoteDelimitedSpanFormattable())) { return ContentEncoder.StringValueDelimiter(sb); }
        return 0;
    }

    public override int AppendDelimiterStart(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars()
         || maybeDelimited.IsChar()
         || (maybeDelimited.IsSpanFormattableOrNullable()
          && maybeDelimited.IsDoubleQuoteDelimitedSpanFormattable())) { return ContentEncoder.StringValueDelimiter(destSpan, fromIndex); }
        return 0;
    }

    public override int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars()
         || maybeDelimited.IsChar()
         || (maybeDelimited.IsSpanFormattableOrNullable()
          && maybeDelimited.IsDoubleQuoteDelimitedSpanFormattable())) { return ContentEncoder.StringValueDelimiter(destSpan, fromIndex); }
        return 0;
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
            sb[fromIndex] = DblQtChar;
            var addedChars = ContentEncoder.Transfer(justAppended, 1, sb, fromIndex, justAppended.Length - 2);
            sb[fromIndex + addedChars] = DblQtChar;
            return addedChars + 2;
        }
        if (JsonOptions.WrapValuesInQuotes)
        {
            scratchFull[0]  = '\"';
            scratchFull[^1] = '\"';
            ContentEncoder.Transfer(scratchFull, 0, sb, fromIndex, scratchFull.Length);
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
            var charsAdded = ContentEncoder.Transfer(justAppended, 0, destSpan, fromIndex, justAppended.Length);
            return charsAdded - appendLen;
        }
        if (JsonOptions.WrapValuesInQuotes)
        {
            scratchFull[0]  = '\"';
            scratchFull[^1] = '\"';
            var charsAdded = ContentEncoder.Transfer(scratchFull, 0, destSpan, fromIndex, scratchFull.Length);
            return charsAdded - appendLen;
        }
        return 0;
    }

    public override int Format<TFmt>(TFmt source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (source == null) { return (Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length)); }
        var originalLength = sb.Length;
        var fmtType        = typeof(TFmt);
        fmtType = fmtType == typeof(ISpanFormattable) ? source.GetType() : fmtType;
        var alreadyDblQtDelimited  = formatString.IsDblQtBounded();
        var alreadyFormatDelimited = alreadyDblQtDelimited || formatString.IsSqBrktBounded() || formatString.IsBrcBounded();
        var wrapInQuotes           = formatSwitches.ShouldDelimit();
        if (fmtType.IsValueType && fmtType.IsNumericType())
        {
            if (!wrapInQuotes) { wrapInQuotes = source.IsDoubleQuoteDelimitedSpanFormattable(); }
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            base.Format(source, sb, formatString, formatSwitches);
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
        }
        else if (source is DateTime sourceDateTime)
        {
            if (JsonOptions.DateTimeIsNumber)
            {
                var converted = JsonOptions.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                if (wrapInQuotes && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
                base.Format(converted, sb, formatString, formatSwitches);
                if (wrapInQuotes && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            }
            else
            {
                if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
                {
                    formatString = JsonOptions.DateTimeAsStringFormatString;
                }
                if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
                base.Format(source, sb, formatString, formatSwitches);
                if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            }
        }
        else if (source is DateOnly dateTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.DateOnlyAsStringFormatString;
            }
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            base.Format(dateTimeOnly, sb, formatString, formatSwitches);
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
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
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            base.Format(source, sb, formatString, formatSwitches);
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
        }
        else if (source is Enum)
        {
            var markInsertIndex = sb.Length;

            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            var enumLen = base.Format(source, sb, formatString, formatSwitches);
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            var isInt = enumLen > 0;
            for (int i = 0; i < enumLen && isInt; i++)
            {
                var checkChar = sb[markInsertIndex + i];
                isInt = checkChar.IsDigit() || (checkChar.IsMinus() && i == 0);
            }
            if (!isInt && !wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
            {
                LayoutEncoder.InsertTransfer(DblQt, sb, markInsertIndex);
                LayoutEncoder.Transfer(DblQt, sb);
            }
        }
        else
        {
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
            base.Format(source, sb, formatString, formatSwitches);
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) sb.Append(DblQt);
        }
        return sb.Length - originalLength;
    }

    public override int Format<TFmt>(TFmt source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (source == null) { return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString); }
        var charsAdded             = 0;
        var fmtType                = typeof(TFmt);
        var alreadyDblQtDelimited  = formatString.IsDblQtBounded();
        var alreadyFormatDelimited = alreadyDblQtDelimited || formatString.IsSqBrktBounded() || formatString.IsBrcBounded();
        var wrapInQuotes           = formatSwitches.ShouldDelimit();
        if (fmtType.IsValueType && fmtType.IsNumericType())
        {
            if (!wrapInQuotes) { wrapInQuotes = source.IsDoubleQuoteDelimitedSpanFormattable(); }
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        else if (source is DateTime sourceDateTime)
        {
            if (JsonOptions.DateTimeIsNumber)
            {
                var converted = JsonOptions.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += base.Format(converted, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
                if (!alreadyFormatDelimited && JsonOptions.WrapValuesInQuotes && !formatSwitches.HasDisableAutoDelimiting())
                    charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
            }
            else
            {
                if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
                {
                    formatString = JsonOptions.DateTimeAsStringFormatString;
                }
                if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
                if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                    charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
            }
        }
        else if (source is DateOnly dateTimeOnly)
        {
            if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            {
                formatString = JsonOptions.DateOnlyAsStringFormatString;
            }
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(dateTimeOnly, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
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
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(sourceTimeOnly, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        else if (source is Enum)
        {
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            var enumLen = base.Format(source, destCharSpan, destStartIndex, formatString, formatSwitches);
            if (wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            var isInt = enumLen > 0;
            for (int i = 0; i < enumLen && isInt; i++)
            {
                var checkChar = destCharSpan[destStartIndex + i];
                isInt = checkChar.IsDigit() || (checkChar.IsMinus() && i == 0);
            }
            if (!isInt && !wrapInQuotes && !alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
            {
                charsAdded += LayoutEncoder.InsertTransfer(DblQt, destCharSpan,  destStartIndex, destStartIndex + enumLen);
                charsAdded += LayoutEncoder.Transfer(DblQt, destCharSpan, destStartIndex + enumLen + 1);
                charsAdded +=  enumLen;
            }
            else { charsAdded += enumLen; }
        }
        else
        {
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
            charsAdded += base.Format(source, destCharSpan, destStartIndex + charsAdded, formatString, formatSwitches);
            if (!alreadyFormatDelimited && !formatSwitches.HasDisableAutoDelimiting())
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
        }
        return charsAdded;
    }

    public override int Format<TFmtStruct>(TFmtStruct? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (!source.HasValue) { return (Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length)); }
        return Format(source.Value, sb, formatString, formatSwitches);
    }

    public override int Format<TFmtStruct>(TFmtStruct? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (!source.HasValue) { return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString); }
        return Format(source.Value, destCharSpan, destStartIndex, formatString);
    }


    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;

        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0, formatSwitches); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparatorAndPadding(elementType, sb, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
                }
            }
            else if (item is byte iByte && JsonOptions.ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb, formatSwitches); }
                else { CollectionNextItemFormat(item, i, sb, formatString, formatSwitches); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length, formatSwitches); }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmtStruct);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
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
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Length && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }


    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), sb);
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
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmtStruct);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (i + 1 < arg0.Count && arg0[i + 1] is char lowSurrogateChar)
                {
                    i++;
                    addedChars += ContentEncoder.Transfer(new Rune(iChar, lowSurrogateChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches); }
            }
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { ContentEncoder.Transfer(new Rune(lastChar, iChar), sb); }
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true, formatSwitches);
                hasStartedCollection =  true;
            }
            if (itemCount > 0)
                addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += ContentEncoder.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else
                {
                    addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
                }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false, formatSwitches);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { ContentEncoder.Transfer(new Rune(lastChar, iChar), sb); }
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
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
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
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true, formatSwitches);
                hasStartedCollection =  true;
            }
            if (itemCount > 0)
                addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += ContentEncoder.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else
                {
                    addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
                }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches); }
        else
        {
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false, formatSwitches);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { ContentEncoder.Transfer(new Rune(lastChar, iChar), sb); }
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatSwitches);
        }
        var  itemCount = 0;
        char lastChar  = '\0';
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0)
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += ContentEncoder.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
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
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatSwitches);
                }
                else
                {
                    addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
                }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { ContentEncoder.Transfer(new Rune(iChar), sb); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate()) { ContentEncoder.Transfer(new Rune(lastChar, iChar), sb); }
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
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
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
            var item = arg0.Current;
            if (itemCount > 0)
                addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && !JsonOptions.CharBufferWritesAsCharCollection)
            {
                if (iChar.IsSingleCharRune()) { addedChars += ContentEncoder.Transfer(new Rune(iChar), destCharSpan, destStartIndex + addedChars); }
                else if (iChar.IsTwoCharHighSurrogate()) { lastChar = iChar; }
                else if (iChar.IsTwoCharLowSurrogate())
                {
                    addedChars += ContentEncoder.Transfer(new Rune(lastChar, iChar), destCharSpan, destStartIndex + addedChars);
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

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (elementType == typeof(char) && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
        {
            return formatSwitches.ShouldDelimit() ? sb.Append(DblQt).ReturnCharCount(1) : 0;
        }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcOpn).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (elementType == typeof(char) &&
            (!JsonOptions.CharBufferWritesAsCharCollection
          || formatSwitches.TreatCharArrayAsString()))
            return formatSwitches.ShouldDelimit() ? destSpan.OverWriteAt(destStartIndex, DblQt) : 0;
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(destStartIndex, BrcOpn);
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) &&
            (!JsonOptions.CharBufferWritesAsCharCollection
          || formatSwitches.TreatCharArrayAsString()))
            return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        return sb.Append(Cma).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) &&
            (!JsonOptions.CharBufferWritesAsCharCollection
          || formatSwitches.TreatCharArrayAsString()))
            return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        return destSpan.OverWriteAt(atIndex, Cma);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatSwitches = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, formatSwitches, formatString);
        var preAppendLen = sb.Length;
        switch (nextItem)
        {
            case Rune runeItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag()
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                {
                    if(formatSwitches.HasEncodeInnerContent()) ContentEncoder.Transfer(runeItem, sb);
                    else  sb.Append(runeItem);
                    return sb.Length - preAppendLen;
                }
                if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                if(formatSwitches.HasEncodeInnerContent()) ContentEncoder.Transfer(runeItem, sb);
                else  sb.Append(runeItem);
                if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                return sb.Length - preAppendLen;
            case char charItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag() && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                {
                    if (charItem.IsSingleCharRune())
                    {
                        if(formatSwitches.HasEncodeInnerContent()) ContentEncoder.Transfer(new Rune(charItem), sb);
                        else  sb.Append(charItem);
                        return sb.Length - preAppendLen;
                    }
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    if (charItem.IsSingleCharRune())
                    {
                        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                        if(formatSwitches.HasEncodeInnerContent()) ContentEncoder.Transfer(new Rune(charItem), sb);
                        else  sb.Append(charItem);
                        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                        return sb.Length - preAppendLen;
                    }
                    return 0;
                }
                break;
            case byte byteItem:
                if (JsonOptions.ByteArrayWritesBase64String) return NextBase64Chars(byteItem, sb);
                break;
        }
        Format(nextItem, sb, formatString, formatSwitches);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        formatSwitches = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, formatSwitches, formatString);
        var charsAdded = 0;
        switch (nextItem)
        {
            case Rune runeItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag() 
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                { return ContentEncoder.Transfer(runeItem, destCharSpan, destStartIndex); }

                if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += ContentEncoder.Transfer(runeItem, destCharSpan, destStartIndex + charsAdded);
                if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                return charsAdded;
            case char charItem:
                if ((formatSwitches.DoesNotHaveAsCollectionFlag()) && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                {
                    if (charItem.IsSingleCharRune()) return ContentEncoder.Transfer(new Rune(charItem), destCharSpan, destStartIndex);
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    if (charItem.IsSingleCharRune())
                    {
                        if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                        charsAdded += ContentEncoder.Transfer(new Rune(charItem), destCharSpan, destStartIndex + charsAdded);
                        if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                        return charsAdded;
                    }
                    return 0;
                }
                break;
            case byte byteItem:
                if (JsonOptions.ByteArrayWritesBase64String) return NextBase64Chars(byteItem, destCharSpan, destStartIndex);
                break;
        }
        charsAdded += Format(nextItem, destCharSpan, destStartIndex, formatString, formatSwitches);
        return charsAdded;
    }

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatSwitches = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, formatSwitches, formatString);
        var preAppendLen = sb.Length;
        switch (nextItem)
        {
            case Rune runeItem:
                
                if (formatSwitches.DoesNotHaveAsCollectionFlag() 
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                { return ContentEncoder.Transfer(runeItem, sb); }

                if (formatSwitches.ShouldDelimit()) sb.Append(DblQt).ReturnCharCount(1);

                ContentEncoder.Transfer(runeItem, sb);

                if (formatSwitches.ShouldDelimit()) sb.Append(DblQt).ReturnCharCount(1);
                return sb.Length - preAppendLen;
            case char charItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag() 
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                {
                    if (charItem.IsSingleCharRune()) return ContentEncoder.Transfer(new Rune(charItem), sb);
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    if (charItem.IsSingleCharRune())
                    {
                        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt).ReturnCharCount(1);
                        ContentEncoder.Transfer(new Rune(charItem), sb);
                        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt).ReturnCharCount(1);
                        return sb.Length - preAppendLen;
                    }
                    return 0;
                }
                break;
        }
        Format(nextItem, sb, formatString, formatSwitches);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatSwitches = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, formatSwitches, formatString);
        var charsAdded = 0;
        switch (nextItem)
        {
            case Rune runeItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag() 
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString())) 
                { return ContentEncoder.Transfer(runeItem, destCharSpan, destStartIndex); }

                if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);

                charsAdded += ContentEncoder.Transfer(runeItem, destCharSpan, destStartIndex + charsAdded);

                if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                return charsAdded;
            case char charItem:
                if (formatSwitches.DoesNotHaveAsCollectionFlag() 
                 && (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString()))
                {
                    if (charItem.IsSingleCharRune()) return ContentEncoder.Transfer(new Rune(charItem), destCharSpan, destStartIndex);
                    return 0;
                }
                if (formatString.IsNullOrEmpty())
                {
                    if (charItem.IsSingleCharRune())
                    {
                        if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
                        charsAdded += ContentEncoder.Transfer(new Rune(charItem), destCharSpan, destStartIndex + charsAdded);
                        if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                        return charsAdded;
                    }
                    return 0;
                }
                break;
        }
        Format(nextItem, destCharSpan, destStartIndex, formatString, formatSwitches);
        return charsAdded;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatSwitches = ResolveStringFormattingFlags(sb.LastNonWhiteChar(), nextItem, formatSwitches);
        var preAppendLen = sb.Length;
        if (nextItem == null) { return sb.Append(JsonOptions.NullString).ReturnCharCount(JsonOptions.NullString.Length); }
        switch (nextItem)
        {
            case string stringItem: return sb.Append(DblQt).Append(stringItem).Append(DblQt).ReturnCharCount(stringItem.Length + 2);
            case char[] charArrayItem:
                if (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString())
                {
                    if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                    sb.Append(charArrayItem);
                    if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                    return sb.Length - preAppendLen;
                }
                CollectionStart(typeof(char[]), sb, charArrayItem.Length > 0, formatSwitches);
                ContentEncoder.Transfer(this, charArrayItem, sb);
                CollectionEnd(typeof(char[]), sb, charArrayItem.Length, formatSwitches);
                return sb.Length - preAppendLen;

            case ICharSequence charSequenceItem:
                if (!JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.TreatCharArrayAsString())
                {
                    if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                    sb.Append(charSequenceItem);
                    if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
                    return sb.Length - preAppendLen;
                }
                CollectionStart(charSequenceItem.GetType(), sb, charSequenceItem.Length > 0, formatSwitches);
                ContentEncoder.Transfer(this, charSequenceItem, sb);
                CollectionEnd(charSequenceItem.GetType(), sb, charSequenceItem.Length, formatSwitches);
                return sb.Length - preAppendLen;
            case StringBuilder sbItem: return sb.Append(DblQt).Append(sbItem).Append(DblQt).ReturnCharCount(sbItem.Length + 2);
            case KeyValuePair<string, JsonNode> jsonNodeKvp:
                var jsonString   = jsonNodeKvp.Value.ToJsonString();
                var keyValueLeng = jsonString.Length + jsonNodeKvp.Key.Length;
                return sb.Append(DblQt).Append(jsonNodeKvp.Key).Append(DblQt).Append(":").Append(jsonString).ReturnCharCount(keyValueLeng + 3);
        }
        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
        sb.Append(nextItem);
        if (formatSwitches.ShouldDelimit()) sb.Append(DblQt);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatSwitches = ResolveStringFormattingFlags(destCharSpan.LastNonWhiteChar(destStartIndex), nextItem, formatSwitches);
        var charsAdded = 0;
        switch (nextItem)
        {
            case KeyValuePair<string, JsonNode> jsonNodeKvp:
                var jsonString = jsonNodeKvp.Value.ToJsonString();
                charsAdded =  destCharSpan.OverWriteAt(destStartIndex, DblQt);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, jsonNodeKvp.Key);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, DblQt);
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, ":");
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, jsonString);
                return charsAdded;
        }
        CharSpanCollectionScratchBuffer ??= MutableString.MediumScratchBuffer;
        CharSpanCollectionScratchBuffer.Clear();
        CharSpanCollectionScratchBuffer.Append(nextItem);

        if (formatSwitches.ShouldDelimit()) charsAdded = destCharSpan.OverWriteAt(destStartIndex, DblQt);
        charsAdded += destCharSpan.OverWriteAt(destStartIndex, CharSpanCollectionScratchBuffer);
        if (formatSwitches.ShouldDelimit()) charsAdded += destCharSpan.OverWriteAt(destStartIndex, DblQt);
        return charsAdded;
    }


    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (elementType == typeof(char) &&
            (!JsonOptions.CharBufferWritesAsCharCollection
          || formatSwitches.TreatCharArrayAsString()))
            return formatSwitches.ShouldDelimit() ? sb.Append(DblQt).ReturnCharCount(1) : 0;
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(sb);
            return sb.Append(DblQt).ReturnCharCount(1 + addedChars);
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcCls).ReturnCharCount(1);
        return sb.Append(SqBrktCls).ReturnCharCount(1);
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (elementType == typeof(char) &&
            (!JsonOptions.CharBufferWritesAsCharCollection
          || formatSwitches.TreatCharArrayAsString()))
            return formatSwitches.ShouldDelimit() ? destSpan.OverWriteAt(destIndex, DblQt) : 0;
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(destSpan, destIndex);
            return destSpan.OverWriteAt(destIndex + addedChars, DblQt) + addedChars;
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(destIndex, BrcCls);
        return destSpan.OverWriteAt(destIndex, SqBrktCls);
    }
}
