// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;
using FortitudeCommon.Types.StyledToString.Options;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class CompactJsonTypeFormatting : JsEscapingFormatter, IStyledTypeFormatting
{
    protected const string Cma = ",";
    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);

    public IStyleTypeBuilderComponentAccess<TB> AppendValueTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type valueType, string? alternativeName = null) where TB : StyledTypeBuilder =>
        typeBuilder;

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendComplexTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type complexType
      , string? alternativeName = null)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(BrcOpn).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string fieldName)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(DblQt).Append(fieldName).Append(DblQt).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendFieldValueSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(Cln).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AddNextFieldSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(Cma).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendTypeClosing<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(BrcCls).ToInternalTypeBuilder(typeBuilder);

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldNameMatch<TB, T>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T source
      , string? formatString = null)
        where TB : StyledTypeBuilder =>
        (formatString.IsNotNullOrEmpty()
            ? typeBuilder.Sb.Append(DblQt).AppendFormat(this, formatString, source).Append(DblQt)
            : typeBuilder.Sb.Append(DblQt).Append(source).Append(DblQt)).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(DblQt).Append(source ? True : False).Append(DblQt).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        (source != null
            ? typeBuilder.Sb.Append(DblQt).Append(source.Value ? True : False).Append(DblQt)
            : typeBuilder.Sb.Append(DblQt).Append(typeBuilder.Settings.NullStyle).Append(DblQt)).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt source
      , string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        var fmtType = typeof(TFmt);
        if (fmtType == typeof(DateTime))
        {
            switch (typeBuilder.Settings.DateTimeFormat)
            {
                case TimeStyleFormat.StringYyyyMMddToss:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTossFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.StringYyyyMMddToms:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTomsFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.StringYyyyMMddTous:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTousFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.SecondsFromUnixEpoch:
                    if (source is DateTime sourceDateTime)
                    {
                        var ticksFromEpoch = sourceDateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var secsFromEpoch  = ticksFromEpoch / 10_000_000;
                        sb.Append(DblQt);
                        base.Format(secsFromEpoch, typeBuilder.Sb, formatString);
                        sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.MillsFromUnixEpoch:
                    if (source is DateTime dateTime)
                    {
                        var ticksFromEpoch = dateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var millsFromEpoch = ticksFromEpoch / 10_000;
                        sb.Append(DblQt);
                        base.Format(millsFromEpoch, typeBuilder.Sb, formatString);
                        sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.MicrosFromUnixEpoch:
                    if (source is DateTime sourceDate)
                    {
                        var ticksFromEpoch = sourceDate.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var mirosFromEpoch = ticksFromEpoch / 10;
                        sb.Append(DblQt);
                        base.Format(mirosFromEpoch, typeBuilder.Sb, formatString);
                        sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.NanosFromUnixEpoch:
                    if (source is DateTime sourceTime)
                    {
                        var ticksFromEpoch = sourceTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var nanosFromEpoch = ticksFromEpoch * 100;
                        sb.Append(DblQt);
                        base.Format(nanosFromEpoch, typeBuilder.Sb, formatString);
                        sb.Append(DblQt);
                    }
                    break;
            }
        }
        else
        {
            if (WrapValuesInQuotes) sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            if (WrapValuesInQuotes) sb.Append(DblQt);
        }
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable
    {
        if (source != null)
        {
            return FormatFieldName(typeBuilder, source.Value, formatString);
        }
        var sb = typeBuilder.Sb;
        if (WrapValuesInQuotes) sb.Append(DblQt);
        base.Format(source, typeBuilder.Sb, formatString);
        if (WrapValuesInQuotes) sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle
      , CustomTypeStyler<TBase> styler)
        where TB : StyledTypeBuilder where T : TBase
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeBuilder.OwningAppender.ToTypeStringAppender);
        if (sb.Length == preAppendLen) return typeBuilder;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeBuilder;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject styledObj)
        where TB : StyledTypeBuilder
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styledObj.ToString(typeBuilder.OwningAppender);
        if (sb.Length == preAppendLen) return typeBuilder;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeBuilder;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldContentsMatch<TB, T>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T source
      , string? formatString = null)
        where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, source);
        else
            typeBuilder.Sb.Append(source);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source
      , string? formatString = null)
        where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        if (WrapValuesInQuotes) sb.Append(DblQt);
        typeBuilder.Sb.Append(source ? True : False);
        if (WrapValuesInQuotes) sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source
      , string? formatString = null)
        where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        if (WrapValuesInQuotes) sb.Append(DblQt);
        if (source != null)
            typeBuilder.Sb.Append(source.Value ? True : False);
        else
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
        if (WrapValuesInQuotes) sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt source
      , string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        var fmtType = typeof(TFmt);
        if (fmtType.IsValueType && fmtType.IsNumericType() && !(fmtType == typeof(DateTime) || fmtType == typeof(TimeSpan)))
        {
            if (WrapValuesInQuotes) sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            if (WrapValuesInQuotes) sb.Append(DblQt);
        }
        else if (fmtType == typeof(DateTime))
        {
            switch (typeBuilder.Settings.DateTimeFormat)
            {
                case TimeStyleFormat.StringYyyyMMddToss:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTossFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.StringYyyyMMddToms:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTomsFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.StringYyyyMMddTous:
                    if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                    {
                        formatString = typeBuilder.Settings.DateTimeStringYyyyMMddTousFormatString;
                    }
                    sb.Append(DblQt);
                    base.Format(source, typeBuilder.Sb, formatString);
                    sb.Append(DblQt);
                    break;
                case TimeStyleFormat.SecondsFromUnixEpoch:
                    if (source is DateTime sourceDateTime)
                    {
                        var ticksFromEpoch = sourceDateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var secsFromEpoch  = ticksFromEpoch / 10_000_000;
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                        base.Format(secsFromEpoch, typeBuilder.Sb, formatString);
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.MillsFromUnixEpoch:
                    if (source is DateTime dateTime)
                    {
                        var ticksFromEpoch = dateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var millsFromEpoch = ticksFromEpoch / 10_000;
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                        base.Format(millsFromEpoch, typeBuilder.Sb, formatString);
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.MicrosFromUnixEpoch:
                    if (source is DateTime sourceDate)
                    {
                        var ticksFromEpoch = sourceDate.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var mirosFromEpoch = ticksFromEpoch / 10;
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                        base.Format(mirosFromEpoch, typeBuilder.Sb, formatString);
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                    }
                    break;
                case TimeStyleFormat.NanosFromUnixEpoch:
                    if (source is DateTime sourceTime)
                    {
                        var ticksFromEpoch = sourceTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
                        var nanosFromEpoch = ticksFromEpoch * 100;
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                        base.Format(nanosFromEpoch, typeBuilder.Sb, formatString);
                        if (WrapValuesInQuotes) sb.Append(DblQt);
                    }
                    break;
            }
        }
        else
        {
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        var fmtType = typeof(TFmt);
        if (fmtType.IsValueType && fmtType.IsNumericType() || ((fmtType == typeof(DateTime) || fmtType == typeof(TimeSpan)) &&
                                                               typeBuilder.Settings.DateTimeFormat.TimeFormatIsNumber()))
        {
            if (WrapValuesInQuotes) sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            if (WrapValuesInQuotes) sb.Append(DblQt);
        }
        else
        {
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue)
        where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle
      , CustomTypeStyler<TBase> styler)
        where TB : StyledTypeBuilder where T : TBase
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeBuilder.OwningAppender.ToTypeStringAppender);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , IStyledToStringObject styledObj)
        where TB : StyledTypeBuilder
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styledObj.ToString(typeBuilder.OwningAppender);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatCollectionStart<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type itemElementType, bool hasItems, Type collectionType) where TB : StyledTypeBuilder
    {
        base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , TCustStyle item
      , int retrieveCount, CustomTypeStyler<TCustBase> styler) where TB : StyledTypeBuilder where TCustStyle : TCustBase
    {
        styler(item, typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string? item, int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.AppendFormat(this, formatString, item);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCharSeq>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TCharSeq? item
      , int retrieveCount, string? formatString = null) where TB : StyledTypeBuilder where TCharSeq : ICharSequence
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder? item
      , int retrieveCount, string? formatString = null) where TB : StyledTypeBuilder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject? item
      , int retrieveCount) where TB : StyledTypeBuilder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        item.ToString(typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return sb.Append(Cma).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> charSpan, int atIndex, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return charSpan.OverWriteAt(atIndex, Cma);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AddCollectionElementSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type elementType, int nextItemNumber)
        where TB : StyledTypeBuilder
    {
        base.AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatCollectionEnd<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType
      , int totalItemCount) where TB : StyledTypeBuilder
    {
        base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder;
    }
}
