// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactJsonTypeFormatting : JsEscapingFormatter, IStyledTypeFormatting
{
    protected const string Cma = ",";
    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);

    public virtual ITypeMolderDieCast<TB> AppendValueTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type valueType, string? alternativeName = null) where TB : TypeMolder =>
        typeBuilder;

    public virtual ITypeMolderDieCast<TB> AppendValueTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder, Type valueType) where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendComplexTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder, Type complexType
      , string? alternativeName = null)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(BrcOpn).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AppendFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, string fieldName)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(DblQt).Append(fieldName).Append(DblQt).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AppendFieldValueSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(Cln).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AddNextFieldSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(Cma).ToInternalTypeBuilder(typeBuilder);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel)
    {
        return sb.InsertAt(Cma, atIndex).ReturnCharCount(1);
    }

    public virtual ITypeMolderDieCast<TB> AppendTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return typeBuilder.Sb.Append(BrcCls).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null)
    {
        if (formatString.IsNotNullOrEmpty())
            sb.Append(DblQt).AppendFormat(this, formatString, source).Append(DblQt);
        else
            sb.Append(DblQt).Append(source).Append(DblQt);

        return sb;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source
      , string? formatString = null) where TB : TypeMolder =>
        typeBuilder.Sb.Append(DblQt).Append(source ? True : False).Append(DblQt).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : TypeMolder =>
        (source != null
            ? typeBuilder.Sb.Append(DblQt).Append(source.Value ? True : False).Append(DblQt)
            : typeBuilder.Sb.Append(DblQt).Append(typeBuilder.Settings.NullStyle).Append(DblQt)).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : TypeMolder where TFmt : ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        var fmtType = typeof(TFmt);
        if (source is DateTime sourceDateTime)
        {
            if (typeBuilder.Settings.DateTimeIsNumber)
            {
                var converted = typeBuilder.Settings.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                sb.Append(DblQt);
                base.Format(converted, typeBuilder.Sb, formatString);
                sb.Append(DblQt);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    formatString = typeBuilder.Settings.DateTimeAsStringFormatString;
                }
                sb.Append(DblQt);
                base.Format(source, typeBuilder.Sb, formatString);
                sb.Append(DblQt);
            }
        }
        else if (source is DateOnly sourceDateOnly)
        {
            if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
            {
                formatString = typeBuilder.Settings.DateTimeStringYyyyMMddOnly;
            }
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        else if (source is TimeOnly sourceTimeOnly)
        {
            if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
            {
                formatString = typeBuilder.Settings.TimeAsStringFormatString;
            }
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        else
        {
            if (WrapValuesInQuotes) sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            if (WrapValuesInQuotes) sb.Append(DblQt);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : TypeMolder where TFmt : struct, ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        if (!source.HasValue)
        {
            sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        return FormatFieldName(typeBuilder, source.Value, formatString);
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle
      , StringBearerRevealState<TBase> styler)
        where TB : TypeMolder where T : TBase
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeBuilder.Master);
        if (sb.Length == preAppendLen) return typeBuilder;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeBuilder;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer styledObj)
        where TB : TypeMolder
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styledObj.RevealState(typeBuilder.Master);
        if (sb.Length == preAppendLen) return typeBuilder;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeBuilder;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual IStringBuilder FormatFieldContentsMatch<T>(IStringBuilder sb, T source, string? formatString = null)
    {
        if (source == null) return sb;
        string rawValue;
        if (source is JsonNode jsonNode)
        {
            rawValue = jsonNode.ToJsonString();
        }
        else
        {
            rawValue = source.ToString() ?? "";
        }
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, rawValue);
        else
            sb.Append(rawValue);
        return sb;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source
      , string? formatString = null)
        where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        if (WrapValuesInQuotes) sb.Append(DblQt);
        typeBuilder.Sb.Append(source ? True : False);
        if (WrapValuesInQuotes) sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source
      , string? formatString = null)
        where TB : TypeMolder
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

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : TypeMolder where TFmt : ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        var fmtType = typeof(TFmt);
        if (fmtType.IsValueType && fmtType.IsNumericType())
        {
            var wrapInQuotes = WrapValuesInQuotes;
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
            if (wrapInQuotes) sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            if (wrapInQuotes) sb.Append(DblQt);
        }
        else if (source is DateTime sourceDateTime)
        {
            if (typeBuilder.Settings.DateTimeIsNumber)
            {
                var converted = typeBuilder.Settings.DateTimeTicksToNumberPrecision(sourceDateTime.Ticks);
                if (WrapValuesInQuotes) sb.Append(DblQt);
                base.Format(converted, typeBuilder.Sb, formatString);
                if (WrapValuesInQuotes) sb.Append(DblQt);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    formatString = typeBuilder.Settings.DateTimeAsStringFormatString;
                }
                sb.Append(DblQt);
                base.Format(source, typeBuilder.Sb, formatString);
                sb.Append(DblQt);
            }
        }
        else if (source is DateOnly dateTimeOnly)
        {
            if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
            {
                formatString = typeBuilder.Settings.DateTimeStringYyyyMMddOnly;
            }
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        else if (source is TimeOnly sourceTimeOnly)
        {
            if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
            {
                formatString = typeBuilder.Settings.TimeAsStringFormatString;
            }
            sb.Append(DblQt);
            base.Format(source, typeBuilder.Sb, formatString);
            sb.Append(DblQt);
        }
        else
        {
            if (source is not null)
            {
                sb.Append(DblQt);
                base.Format(source, typeBuilder.Sb, formatString);
                sb.Append(DblQt);
            }
            else
            {
                sb.Append(typeBuilder.Settings.NullStyle);
            }
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null)
        where TB : TypeMolder where TFmt : struct, ISpanFormattable
    {
        var sb      = typeBuilder.Sb;
        if (!source.HasValue)
        {
            sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        return FormatFieldContents(typeBuilder, source.Value, formatString);
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue)
        where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        if (CharArrayWritesString)
        {
            sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
            sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
            var charType     = typeof(char);
            FormatCollectionStart(typeBuilder, charType, cappedLength > 0, source.GetType());

            for (int i = cappedFrom; i < cappedLength; i++)
            {
                if (i > 0) AddCollectionElementSeparator(typeBuilder, charType, i);

                CollectionNextItem(source[cappedFrom + i], i, sb);
            }
            ;
            FormatCollectionEnd(typeBuilder, charType, cappedLength);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        if (CharArrayWritesString)
        {
            sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
            sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var charType = typeof(char);
            FormatCollectionStart(typeBuilder, charType, cappedLength > 0, source.GetType());

            for (int i = cappedFrom; i < cappedLength; i++)
            {
                if (i > 0) AddCollectionElementSeparator(typeBuilder, charType, i);

                CollectionNextItem(source[cappedFrom + i], i, sb);
            }
            ;
            FormatCollectionEnd(typeBuilder, charType, cappedLength);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle
      , StringBearerRevealState<TBase> styler)
        where TB : TypeMolder where T : TBase
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeBuilder.Master);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder
      , IStringBearer styledObj)
        where TB : TypeMolder
    {
        var sb           = typeBuilder.Sb;
        var preAppendLen = sb.Length;
        styledObj.RevealState(typeBuilder.Master);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeBuilder;
    }


    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type keyedCollectionType, Type keyType, Type valueType) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        if (typeBuilder.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktOpn);
        }
        else
        {
            sb.Append(BrcOpn).ToInternalTypeBuilder(typeBuilder);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        if (typeBuilder.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktCls);
        }
        else
        {
            sb.Append(BrcCls).ToInternalTypeBuilder(typeBuilder);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue>(ITypeMolderDieCast<TB> typeBuilder
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null)
        where TB : TypeMolder
    {
        if (typeBuilder.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeBuilder, keyedCollectionType);
            AppendFieldName(typeBuilder, "Key").FieldEnd();
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true)
                : typeBuilder.AppendMatchOrNull(key, true);
            AddNextFieldSeparator(typeBuilder);
            AppendFieldName(typeBuilder, "Value").FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(value, valueFormatString)
                : typeBuilder.AppendMatchOrNull(value);
            AppendTypeClosing(typeBuilder);
        }
        else
        {
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(value, valueFormatString)
                : typeBuilder.AppendMatchOrNull(value);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TVBase>(ITypeMolderDieCast<TB> typeBuilder
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null)
        where TB : TypeMolder where TValue : TVBase
    {
        if (typeBuilder.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeBuilder, keyedCollectionType);
            AppendFieldName(typeBuilder, "Key").FieldEnd();
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true)
                : typeBuilder.AppendMatchOrNull(key, true);
            AddNextFieldSeparator(typeBuilder);
            AppendFieldName(typeBuilder, "Value").FieldEnd();
            valueStyler(value, typeBuilder.Master);
            AppendTypeClosing(typeBuilder);
        }
        else
        {
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
            valueStyler(value, typeBuilder.Master);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TB> typeBuilder
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler)
        where TB : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        if (typeBuilder.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeBuilder, keyedCollectionType);
            AppendFieldName(typeBuilder, "Key").FieldEnd();
            keyStyler(key, typeBuilder.Master);
            AddNextFieldSeparator(typeBuilder);
            AppendFieldName(typeBuilder, "Value").FieldEnd();
            valueStyler(value, typeBuilder.Master);
            AppendTypeClosing(typeBuilder);
        }
        else
        {
            keyStyler(key, typeBuilder.Master);
            typeBuilder.FieldEnd();
            valueStyler(value, typeBuilder.Master);
        }
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionNextItem<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type keyedCollectionType, Type keyType, Type valueType, int previousItemCount) where TB : TypeMolder
    {
        typeBuilder.Sb.Append(Cma).ToInternalTypeBuilder(typeBuilder);
        return typeBuilder;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destination, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return destination.OverWriteAt(destStartIndex, DblQt);
        return destination.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public virtual ITypeMolderDieCast<TB> FormatCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type itemElementType, bool hasItems, Type collectionType) where TB : TypeMolder
    {
        base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(ITypeMolderDieCast<TB> typeBuilder
      , TCustStyle item
      , int retrieveCount, StringBearerRevealState<TCustBase> styler) where TB : TypeMolder where TCustStyle : TCustBase
    {
        styler(item, typeBuilder.Master);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, string? item
      , int retrieveCount
      , string? formatString = null) where TB : TypeMolder
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

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCharSeq>(ITypeMolderDieCast<TB> typeBuilder, TCharSeq? item
      , int retrieveCount, string? formatString = null) where TB : TypeMolder where TCharSeq : ICharSequence
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

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder? item
      , int retrieveCount, string? formatString = null) where TB : TypeMolder
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

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder
      , IStringBearer? item
      , int retrieveCount) where TB : TypeMolder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        item.RevealState(typeBuilder.Master);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AddCollectionElementSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type elementType, int nextItemNumber)
        where TB : TypeMolder
    {
        base.AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType
      , int totalItemCount) where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder;
    }
}
