// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandlingExtensions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactJsonTypeFormatting : JsonFormatter, IStyledTypeFormatting
{
    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);

    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual CompactJsonTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public FieldContentHandling ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FieldContentHandling callerFormattingFlags
      , string formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags() || input == null) { return callerFormattingFlags; }

        FieldContentHandling setFlags = callerFormattingFlags;
        setFlags |= (FieldContentHandling)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormattingHandlingFlags)setFlags, formatString);
        if (isFieldName) { setFlags |= EnsureFormattedDelimited; }

        var typeofT = typeof(T);
        if (typeofT.IsAnyTypeHoldingChars())
        {
            var notAsStringOrValue = !(callerFormattingFlags.HasAsStringContentFlag()
                                    || callerFormattingFlags.HasAsValueContentFlag());
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && notAsStringOrValue ? EnsureFormattedDelimited : None;
            setFlags |= setFlags.ShouldDelimit() && callerFormattingFlags.DoesNotHaveAsValueContentFlag()  ? EncodeAll : EncodeInnerContent;
        }
        return setFlags;
    }

    public FieldContentHandling ResolveContentAsValueFormattingFlags<T>(T input, bool hasFallbackValue)
    {
        if (input == null && !hasFallbackValue) return DefaultCallerTypeFlags;
        var typeOfT = typeof(T);
        if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
            return DisableAutoDelimiting | AsValueContent;
        var isSpanFormattableOrNullable           = typeOfT.IsSpanFormattableOrNullable();
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable();
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
            return EnsureFormattedDelimited | AsValueContent;
        return AsValueContent;
    }

    public FieldContentHandling ResolveContentAsStringFormattingFlags<T>(T input, bool hasFallbackValue)
    {
        if (input == null && !hasFallbackValue) return DefaultCallerTypeFlags;
        var typeOfT = typeof(T);
        if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
            return DisableAutoDelimiting | AsStringContent;
        var isSpanFormattableOrNullable           = typeOfT.IsSpanFormattableOrNullable();
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable();
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
            return DisableAutoDelimiting | AsStringContent;
        return AsStringContent;
    }

    public virtual IStringBuilder AppendValueTypeOpening(IStringBuilder sb, Type valueType, string? alternativeName = null) => sb;

    public virtual IStringBuilder AppendValueTypeClosing(IStringBuilder sb, Type valueType) =>
        sb.RemoveLastWhiteSpacedCommaIfFound().ToStringBuilder(sb);

    public virtual IStringBuilder AppendComplexTypeOpening(IStringBuilder sb, Type complexType, string? alternativeName = null) =>
        sb.Append(BrcOpn);

    public virtual IStringBuilder AppendFieldValueSeparator(IStringBuilder sb) => sb.Append(Cln);

    public virtual IStringBuilder AddNextFieldSeparator(IStringBuilder sb) => sb.Append(Cma);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) =>
        sb.InsertAt(Cma, atIndex).ReturnCharCount(1);

    public virtual IStringBuilder AppendTypeClosing(IStringBuilder sb) =>
        sb.RemoveLastWhiteSpacedCommaIfFound().ToStringBuilder(sb).Append(BrcCls);

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName) =>
        sb.Append(DblQt).Append(fieldName).Append(DblQt);

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        sb.AppendFormat(this, formatString ?? "", source, (FormattingHandlingFlags)formatFlags);

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        Format(source, sb, formatString).ToStringBuilder(sb);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        Format(source, sb, formatString).ToStringBuilder(sb);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        base.Format(source, sb, formatString, (FormattingHandlingFlags)formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : struct, ISpanFormattable
    {
        if (!source.HasValue) { return sb.Append(StyleOptions.NullStyle); }
        return FormatFieldName(sb, source.Value, formatString);
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle, PalantírReveal<TCloakedBase> styler)
        where TCloaked : TCloakedBase
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        styler(toStyle, tos);
        if (sb.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return tos.WriteBuffer;
        sb.Insert(preAppendLen, DblQt);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer
    {
        var preAppendLen = tos.WriteBuffer.Length;
        styledObj.RevealState(tos);
        if (tos.WriteBuffer.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(tos.WriteBuffer, preAppendLen);
        if (tos.WriteBuffer[preAppendLen] == DblQtChar) return tos.WriteBuffer;
        tos.WriteBuffer.Insert(preAppendLen, DblQt);
        return tos.WriteBuffer.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (source == null) return sb;
        string rawValue;
        if (source is JsonNode jsonNode) { rawValue = jsonNode.ToJsonString(); }
        else { rawValue                             = source.ToString() ?? ""; }
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        sb.AppendFormat(this, formatString ?? "{0}", rawValue, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        Format(source, sb, formatString, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (source != null)
            FormatFieldContents(sb, source.Value, formatString, formatFlags: formatFlags);
        else
            sb.Append(StyleOptions.NullStyle);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        formatFlags = ResolveContentFormattingFlags(sb, source, formatFlags);
        base.Format(source, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (!source.HasValue) { return sb.Append(StyleOptions.NullStyle); }
        return FormatFieldContents(sb, source.Value, formatString, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var fmtHndlingFlags = (FormattingHandlingFlags)formatFlags;
        if (JsonOptions.CharArrayWritesString || fmtHndlingFlags.TreatCharArrayAsString())
        {
            if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtHndlingFlags);
            if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var        formattedLength = cappedLength + 256;
            Span<char> sourceInSpan    = stackalloc char[Math.Min(4096, formattedLength)];

            RecyclingCharArray? largeBuffer = null;

            var asStringHandlingFlags = (fmtHndlingFlags & ~FormattingHandlingFlags.EncodeBounds);
            if (formattedLength < 4096)
            {
                cappedLength = ICustomStringFormatter.DefaultBufferFormatter
                                                     .Format(source, cappedFrom, sourceInSpan, formatString
                                                           , 0, cappedLength, asStringHandlingFlags);
            }
            else
            {
                largeBuffer  = (formattedLength).SourceRecyclingCharArray();
                sourceInSpan = largeBuffer.RemainingAsSpan();
                cappedLength =
                    ICustomStringFormatter.DefaultBufferFormatter
                                          .Format(source, cappedFrom, sourceInSpan
                                                , formatString, 0, cappedLength, asStringHandlingFlags);
            }

            var charType = typeof(char);
            CollectionStart(charType, sb, cappedLength > 0, fmtHndlingFlags);

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = 0; i < cappedLength; i++)
            {
                if (i > 0 && lastAdded > 0) AddCollectionElementSeparator(charType, sb, i, fmtHndlingFlags);

                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, "", fmtHndlingFlags)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags);
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            largeBuffer?.DecrementRefCount();
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var fmtHndlingFlags = (FormattingHandlingFlags)formatFlags;
        if (JsonOptions.CharArrayWritesString || fmtHndlingFlags.TreatCharArrayAsString())
        {
            if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtHndlingFlags);
            if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var        formattedLength = cappedLength + 256;
            Span<char> sourceInSpan    = stackalloc char[Math.Min(4096, formattedLength)];

            RecyclingCharArray? largeBuffer = null;

            var asStringHandlingFlags = (fmtHndlingFlags & ~FormattingHandlingFlags.EncodeBounds);
            if (formattedLength < 4096)
            {
                cappedLength =
                    ICustomStringFormatter.DefaultBufferFormatter
                                          .Format(source, cappedFrom, sourceInSpan, formatString
                                                , 0, cappedLength, asStringHandlingFlags);
            }
            else
            {
                largeBuffer  = (formattedLength).SourceRecyclingCharArray();
                sourceInSpan = largeBuffer.RemainingAsSpan();
                cappedLength =
                    ICustomStringFormatter.DefaultBufferFormatter
                                          .Format(source, cappedFrom, sourceInSpan, formatString
                                                , 0, cappedLength, asStringHandlingFlags);
            }

            var charType = typeof(char);
            CollectionStart(charType, sb, cappedLength > 0, fmtHndlingFlags);

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = 0; i < cappedLength; i++)
            {
                if (i > 0 && lastAdded > 0) AddCollectionElementSeparator(charType, sb, i, fmtHndlingFlags);

                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, "", fmtHndlingFlags)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags);
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            largeBuffer?.DecrementRefCount();
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase
    {
        var preAppendLen = tos.WriteBuffer.Length;
        styler(toStyle, tos);
        if (tos.WriteBuffer.Length != preAppendLen) ProcessAppendedRange(tos.WriteBuffer, preAppendLen);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        styledObj.RevealState(tos);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return sb;
    }


    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { sb.Append(SqBrktOpn); }
        else { sb.Append(BrcOpn); }
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { sb.Append(SqBrktCls); }
        else { sb.Append(BrcCls); }
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags, true).FieldEnd();
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "");
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder where TValue : TVBase
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags, true).FieldEnd();
            valueStyler(value, typeMold.Master);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler
      , PalantírReveal<TKBase> keyStyler, FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            keyStyler(key, typeMold.Master);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {
            keyStyler(key, typeMold.Master);
            typeMold.FieldEnd();
            valueStyler(value, typeMold.Master);
        }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb
      , Type keyedCollectionType, Type keyType, Type valueType, int previousItemCount) => sb.Append(Cma);

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems, FormattingHandlingFlags formatFlags
        = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (elementType == typeof(char) &&
            (JsonOptions.CharArrayWritesString
          || formatFlags.TreatCharArrayAsString()))
            return formatFlags.ShouldDelimit() ? sb.Append(DblQt).ReturnCharCount(1) : 0;
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (elementType == typeof(char) &&
            (JsonOptions.CharArrayWritesString
          || formatFlags.TreatCharArrayAsString()))
            return formatFlags.ShouldDelimit() ? destSpan.OverWriteAt(destStartIndex, DblQt) : 0;
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public virtual IStringBuilder FormatCollectionStart(IStringBuilder sb
      , Type itemElementType, bool hasItems, Type collectionType, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        base.CollectionStart(itemElementType, sb, hasItems).ToStringBuilder(sb);

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked item, int retrieveCount, PalantírReveal<TCloakedBase> styler)
        where TCloaked : TCloakedBase
    {
        styler(item, tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullStyle); }

        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullStyle); }
        if (formatFlags.HasAsStringContentFlag() && formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.HasAsStringContentFlag() && formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence
    {
        if (item == null) { return sb.Append(StyleOptions.NullStyle); }
        if (formatFlags.HasAsStringContentFlag() && formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        FormatFieldContents(sb, item, 0, formatString ?? "");
        if (formatFlags.HasAsStringContentFlag() && formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullStyle); }
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TBearer>(ITheOneString tos, TBearer? item, int retrieveCount)
        where TBearer : IStringBearer
    {
        if (item == null) { return tos.WriteBuffer.Append(StyleOptions.NullStyle); }
        item.RevealState(tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        base.AddCollectionElementSeparator(elementType, sb, nextItemNumber);
        return sb;
    }

    public virtual IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType, int totalItemCount
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        base.CollectionEnd(itemElementType, sb, totalItemCount);
        return sb;
    }
}
