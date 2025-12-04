// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandlingExtensions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactJsonTypeFormatting : JsonFormatter, IStyledTypeFormatting
{
    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);

    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual CompactJsonTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        GraphBuilder = graphTrackingBuilder;
        Options      = styleOptions;

        return this;
    }

    public GraphTrackingBuilder GraphBuilder { get; protected set; } = null!;

    public override IEncodingTransfer LayoutEncoder
    {
        get =>
            GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags.HasReformatMultiLineFlag()
                ? GraphBuilder.ParentGraphEncoder
                : ContentEncoder;
        set => GraphBuilder.GraphEncoder = value;
    }

    public virtual FieldContentHandling ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FieldContentHandling callerFormattingFlags
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
            setFlags |= setFlags.ShouldDelimit() && callerFormattingFlags.DoesNotHaveAsValueContentFlag() ? EncodeAll : EncodeInnerContent;
        }
        return setFlags;
    }

    public virtual FieldContentHandling ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = "")
    {
        var typeOfT                     = typeof(T);
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullableCached();
        // if (input == null && fallbackValue.Length > 0) return DefaultCallerTypeFlags;
        var isAnyTypeHoldingChars = typeOfT.IsAnyTypeHoldingCharsCached() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars) return DisableAutoDelimiting | AsValueContent;
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return EnsureFormattedDelimited | AsValueContent;
        return AsValueContent;
    }

    public virtual FieldContentHandling ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = "")
    {
        var typeOfT                     = typeof(T);
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullableCached();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingCharsCached() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars) return DisableAutoDelimiting | AsStringContent;
        var isJsonStringExemptType = typeOfT.IsJsonStringExemptTypeCached();
        var isDoubleQuoteDelimitedSpanFormattable
            = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString) || !isJsonStringExemptType;
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return DisableAutoDelimiting | AsStringContent;
        return AsStringContent;
    }

    public virtual ContentSeparatorRanges AppendValueTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, this, formatFlags);
        return GraphBuilder.SnapshotLastAppendSequence(formatFlags);
    }

    public virtual ContentSeparatorRanges AppendValueTypeClosing(ITypeMolderDieCast moldInternal)
    {
        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, this, DefaultCallerTypeFlags, true);
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public virtual ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder.StartAppendContentAndComplete(BrcOpn, moldInternal.Sb, this, formatFlags);

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder.AppendSeparator(Cln)
                    .SnapshotLastAppendSequence(formatFlags).SeparatorPaddingRange!.Value;


    public virtual Range? AddNextFieldSeparator(ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainFieldSeparator() ? StyleOptions.MainItemSeparator : StyleOptions.AlternateFieldSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddNextFieldPadding(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainFieldPadding() ? StyleOptions.MainFieldPadding : StyleOptions.AlternateFieldPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddNextFieldSeparatorAndPadding(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        AddNextFieldSeparator(moldInternal, formatFlags);
        return AddNextFieldPadding(moldInternal, formatFlags);
    }

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) =>
        sb.InsertAt(Cma, atIndex).ReturnCharCount(1);

    public virtual ContentSeparatorRanges AppendTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb                        = moldInternal.Sb;
        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;

        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags, true);

        return GraphBuilder.StartAppendContentAndComplete(BrcCls, sb, this, previousContentPadSpacing.PreviousFormatFlags);
    }


    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        formatString ??= "";
        if (formatString.HasFormatStringPadding() || formatString.PrefixSuffixLength() > 0)
        {
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatFlags: FormattingHandlingFlags.DefaultCallerTypeFlags);
        }
        else { sb.Append(StyleOptions.NullString); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName)
    {
        GraphBuilder
            .StartAppendContent(DblQt, sb, this, DefaultCallerTypeFlags)
            .AppendContent(fieldName)
            .AppendContent(DblQt);
        return sb;
    }

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
        if (!source.HasValue) { return sb.Append(StyleOptions.NullString); }
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

    public virtual IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ITheOneString tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        if (sb.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return tos.WriteBuffer;
        sb.Insert(preAppendLen, DblQt);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        if (sb.Length == preAppendLen) return sb;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return sb;
        sb.Insert(preAppendLen, DblQt);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (source == null)
        {
            GraphBuilder.MarkContentEnd();
            return sb;
        }
        string rawValue;
        if (source is JsonNode jsonNode) { rawValue = jsonNode.ToJsonString(); }
        else { rawValue                             = source.ToString() ?? ""; }
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        sb.AppendFormat(this, formatString ?? "{0}", rawValue, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        Format(source, sb, formatString, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (source != null)
            FormatFieldContents(sb, source.Value, formatString, formatFlags: formatFlags);
        else
            sb.Append(StyleOptions.NullString);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        formatFlags  =   ResolveContentFormattingFlags(sb, source, formatFlags);
        formatString ??= "";
        base.Format(source, sb, formatString, (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags); }
        formatString ??= "";
        FormatFieldContents(sb, source.Value, formatString, formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default!, formatFlags, formatString);

        var typeofT = typeof(TAny);

        if (typeofT.IsEnumOrNullable()
         && (JsonOptions.WrapValuesInQuotes
          || ((!source.IsValidEnumIntegerSpan()
            || formatFlags.HasAsStringContentFlag()))))
        {
            formatFlags |= EnsureFormattedDelimited;
        }

        return FormatFieldContents(sb, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var fmtHndlingFlags = (FormattingHandlingFlags)formatFlags;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if ((JsonOptions.CharArrayWritesString || fmtHndlingFlags.TreatCharArrayAsString()) && formatFlags.DoesNotHaveAsCollectionFlag())
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
                if (i > 0 && lastAdded > 0) AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtHndlingFlags);

                if (lastAdded > 0 || i == 0) { GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags); }
                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, "", fmtHndlingFlags)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags);
                if (lastAdded > 0) { GraphBuilder.MarkContentEnd(); }
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            GraphBuilder.Complete(formatFlags);
            largeBuffer?.DecrementRefCount();
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var fmtHndlingFlags = (FormattingHandlingFlags)formatFlags;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        if ((JsonOptions.CharArrayWritesString || fmtHndlingFlags.TreatCharArrayAsString()) && formatFlags.DoesNotHaveAsCollectionFlag())
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
                if (i > 0 && lastAdded > 0) AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtHndlingFlags);

                if (lastAdded > 0 || i == 0) { GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags); }
                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, "", fmtHndlingFlags)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags);
                if (lastAdded > 0) { GraphBuilder.MarkContentEnd(); }
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            GraphBuilder.Complete(formatFlags);
            largeBuffer?.DecrementRefCount();
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TRevealBase>(ITheOneString tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb           = tos.WriteBuffer;
        var contentStart = sb.Length;
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?
    {
        var sb           = tos.WriteBuffer;
        var contentStart = sb.Length;
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }


    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { sb.Append(SqBrktOpn); }
        else { sb.Append(BrcOpn); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        sb.RemoveLastWhiteSpacedCommaIfFound();
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { sb.Append(SqBrktCls); }
        else { sb.Append(BrcCls); }
        GraphBuilder.MarkContentEnd();
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
            AppendComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(typeMold, this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true);
            AddNextFieldSeparator(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(typeMold, this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
            AppendTypeClosing(typeMold);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags, true).FieldEnd();
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "");
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(typeMold, this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true);
            AddNextFieldSeparator(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(typeMold, this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { valueStyler(value, typeMold.Master); }
            AppendTypeClosing(typeMold);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags, true).FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { valueStyler(value, typeMold.Master); }
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey? key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(typeMold, this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { keyStyler(key, typeMold.Master); }
            AddNextFieldSeparator(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(typeMold, this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { valueStyler(value, typeMold.Master); }
            AppendTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { keyStyler(key, typeMold.Master); }
            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { valueStyler(value, typeMold.Master); }
        }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb
      , Type keyedCollectionType, Type keyType, Type valueType, int previousItemCount) => sb.Append(Cma);

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal
      , Type itemElementType, bool? hasItems, Type collectionType, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!hasItems.HasValue) return sb;
        CollectionStart(itemElementType, sb, hasItems.Value);
        AddCollectionElementPadding(moldInternal, itemElementType, 1, formatFlags);
        return sb;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems, FormattingHandlingFlags formatFlags
        = FormattingHandlingFlags.EncodeInnerContent)
    {
        var currFmtFlags = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, currFmtFlags);
            var contentStart = sb.Length;
            if ((elementType == typeof(char) && (JsonOptions.CharArrayWritesString || formatFlags.TreatCharArrayAsString())))
            {
                if (formatFlags.ShouldDelimit())
                {
                    GraphBuilder.AppendContent(DblQt).Complete((FieldContentHandling)formatFlags); // could be unicode escaped
                    return sb.Length - contentStart;
                }
            }
            else if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                GraphBuilder.AppendContent(DblQt).Complete((FieldContentHandling)formatFlags); // could be unicode escaped
                return sb.Length - contentStart;
            }
            GraphBuilder.AppendContent(SqBrktOpn);
            return sb.Length - contentStart;
        }
        return 0;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var charsAdded = 0;

        var currFmtFlags = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags);
            GraphBuilder.MarkContentStart(destStartIndex);
            if ((elementType == typeof(char) && (JsonOptions.CharArrayWritesString || formatFlags.TreatCharArrayAsString())))
            {
                if (formatFlags.ShouldDelimit())
                {
                    charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be unicode escaped
                    GraphBuilder.Complete((FieldContentHandling)formatFlags);
                    return charsAdded;
                }
            }
            else if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be unicode escaped
                GraphBuilder.Complete((FieldContentHandling)formatFlags);
                return charsAdded;
            }
            charsAdded += destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
            GraphBuilder.MarkContentEnd(destStartIndex + charsAdded);
        }
        return charsAdded;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var typeofT = item.GetType();
        formatString ??= "";
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        var preAppendLen = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormattingHandlingFlags)formatFlags);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        GraphBuilder.MarkContentStart(preAppendLen);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var typeofT = item.GetType();
        formatString ??= "";
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        var preAppendLen = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormattingHandlingFlags)formatFlags);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        GraphBuilder.MarkContentStart(preAppendLen);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> itemRevealer)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, ""); }

        var contentStart = sb.Length;
        itemRevealer(item, tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        if (formatFlags.HasAsCollectionFlag())
        {
            FormatFieldContents(sb, item, 0, formatString, formatFlags: formatFlags);
            return sb;
        }
        var preAppendLen = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        GraphBuilder.MarkContentStart(preAppendLen);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        if (formatFlags.HasAsCollectionFlag())
        {
            FormatFieldContents(sb, item, 0, formatString, formatFlags: formatFlags);
            return sb;
        }
        var preAppendLen = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        GraphBuilder.MarkContentStart(preAppendLen);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextStringBearerFormat<TBearer>(ITheOneString tos, TBearer? item, int retrieveCount)
        where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, ""); }
        var contentStart = sb.Length;
        item.RevealState(tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if ((elementType == typeof(char) && (JsonOptions.CharArrayWritesString || formatFlags.HasAsStringContentFlag()))
         || (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String))
        {
            GraphBuilder.Complete(formatFlags);
            return sb;
        }

        GraphBuilder.AppendSeparator
            (formatFlags.UseMainItemSeparator()
                 ? Options.MainItemSeparator
                 : Options.AlternateItemSeparator);
        return sb;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) { return 0; }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return 0; }
        var preAppendLen = sb.Length;
        GraphBuilder.AppendSeparator
            (formatFlags.UseMainItemSeparator()
                 ? Options.MainItemSeparator
                 : Options.AlternateItemSeparator);
        return sb.Length - preAppendLen;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) { return 0; }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return 0; }

        GraphBuilder.MarkContentEnd();
        var addedChars =
            destSpan.OverWriteAt
                (atIndex,
                 formatFlags.UseMainItemSeparator()
                     ? Options.MainItemSeparator
                     : Options.AlternateItemSeparator);
        GraphBuilder.MarkSeparatorEnd();
        return addedChars;
    }


    public virtual Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) { return null; }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return null; }
        GraphBuilder.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) { return GraphBuilder.Complete(formatFlags); }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return GraphBuilder.Complete(formatFlags); }
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType
      , int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(moldInternal, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(moldInternal, elementType, nextItemNumber, formatFlags);
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
                CollectionStart(itemElementType, sb, false, (FormattingHandlingFlags)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormattingHandlingFlags)formatFlags);
                GraphBuilder.Complete(formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            return sb;
        }

        CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;

        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, (FieldContentHandling)formatFlags, true);
            if (elementType.IsChar() && (JsonOptions.CharArrayWritesString || formatFlags.TreatCharArrayAsString()))
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag())
                    GraphBuilder.AppendContent(DblQt);
            }
            else if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                CompleteBase64Sequence(sb);
                GraphBuilder.AppendContent(DblQt);
            }
            else if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                GraphBuilder.RemoveLastSeparatorAndPadding();
                GraphBuilder.AppendContent(BrcCls);
            }
            else
            {
                GraphBuilder.RemoveLastSeparatorAndPadding();
                GraphBuilder.AppendContent(SqBrktCls);
            }
        }

        return sb.Length - preAppendLen;
    }

    public override int CollectionEnd(Type collectionType, Span<char> destSpan, int destIndex, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var charsAdded = 0;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
            if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag())
                {
                    charsAdded += GraphBuilder.GraphEncoder.Transfer(DblQt, destSpan, destIndex);
                    GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                    return charsAdded;
                }
                return 0;
            }
            else if (collectionType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded += CompleteBase64Sequence(destSpan, destIndex);
                GraphBuilder.AppendContent(DblQt);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            else if (collectionType == typeof(KeyValuePair<string, JsonNode>))
            {
                GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
                GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
                GraphBuilder.MarkContentStart(destIndex);
                charsAdded += GraphBuilder.GraphEncoder.Transfer(BrcCls, destSpan, destIndex);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }

            GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
            GraphBuilder.MarkContentStart(destIndex);
            charsAdded += GraphBuilder.GraphEncoder.Transfer(SqBrktCls, destSpan, destIndex);
            GraphBuilder.MarkContentEnd(destIndex + charsAdded);
        }
        return charsAdded;
    }
}
