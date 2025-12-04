// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandlingExtensions;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot       = ".";
    protected const string Cma       = ",";
    protected const string CmaSpc    = ", ";
    protected const string Spc       = " ";
    protected const string Cln       = ":";
    protected const string BrcOpn    = "{";
    protected const string BrcOpnSpc = "{ ";
    protected const string SpcBrcCls = " }";
    protected const string EqlsSpc   = "= ";

    protected const string SqBrktOpnSpc = "[ ";
    protected const string RndBrktOpn   = "(";
    protected const string RndBrktCls   = ")";

    public GraphTrackingBuilder GraphBuilder { get; protected set; } = null!;

    public virtual CompactLogTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        GraphBuilder = graphTrackingBuilder;
        Options      = styleOptions;

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);
    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual FieldContentHandling ResolveContentFormattingFlags<T>(IStringBuilder sb, T input
      , FieldContentHandling callerFormattingFlags, string? formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags()) { return callerFormattingFlags; }

        FieldContentHandling setFlags = callerFormattingFlags;
        setFlags |= (FieldContentHandling)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormattingHandlingFlags)setFlags, formatString ?? "");

        var typeofT = typeof(T);
        if (typeofT.IsAnyTypeHoldingChars())
        {
            var notAsStringOrValue = !(callerFormattingFlags.HasAsStringContentFlag()
                                    || callerFormattingFlags.HasAsValueContentFlag());
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && notAsStringOrValue
                ? EnsureFormattedDelimited
                : None;
            setFlags |= setFlags.ShouldDelimit() || callerFormattingFlags.HasAsStringContentFlag()
                ? EncodeAll
                : EncodeInnerContent;
        }
        return setFlags;
    }

    public virtual FieldContentHandling ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = "")
    {
        var typeOfT                     = typeof(T);
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (input == null && (fallbackValue.Length == 0 && !isAnyTypeHoldingChars)) return AsValueContent;
        if (isAnyTypeHoldingChars)
            return DisableAutoDelimiting | AsValueContent;
        return AsValueContent;
    }

    public virtual FieldContentHandling ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = "")
    {
        var typeOfT                     = typeof(T);
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullable();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars)
            return DisableAutoDelimiting | AsStringContent;
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
            return DisableAutoDelimiting | AsStringContent;
        return AsStringContent;
    }

    public virtual ContentSeparatorRanges AppendValueTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag())
        {
            if (alternativeName.IsNotNullOrEmpty())
                sb.Append(alternativeName);
            else
                buildingType.AppendShortNameInCSharpFormat(sb);
            // space considered content
            GraphBuilder.AppendContent(EqlsSpc);
        }
        return GraphBuilder.ContentEndToRanges(formatFlags);
    }

    public virtual ContentSeparatorRanges AppendValueTypeClosing(ITypeMolderDieCast moldInternal)
    {
        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, this, DefaultCallerTypeFlags);
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public virtual ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            buildingType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return GraphBuilder
               .AppendContent(BrcOpn)
               .AppendPadding(Spc)
               .Complete(formatFlags);
    }

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

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
        sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);

    public virtual ContentSeparatorRanges AppendTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;

        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartAppendContent(SpcBrcCls, sb, this, DefaultCallerTypeFlags);
        return GraphBuilder.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
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

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        keyedCollectionType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return sb.Append(BrcOpnSpc).ToStringBuilder(sb);
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        return sb.Append(SpcBrcCls);
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) => sb.Append(CmaSpc);

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName)
    {
        GraphBuilder.StartAppendContent(fieldName, sb, this, DefaultCallerTypeFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, source);
        else
            sb.Append(source);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        Format(source, sb, formatString ?? "");
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (source != null)
            Format(source, sb, formatString ?? "");
        else
            base.Format(StyleOptions.NullString, 0, sb, formatString);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sb, formatString ?? "");
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : struct, ISpanFormattable
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sb, formatString ?? "");
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ITheOneString tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        Format(source, sb, formatSpan, formatFlags: (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        Format(source, sb, formatSpan, formatFlags: (FormattingHandlingFlags)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        var markInsertStartIndex = sb.Length;
        base.Format(source, sb, formatSpan, (FormattingHandlingFlags)formatFlags);
        if (source is Enum)
        {
            var insertLength = sb.Length - markInsertStartIndex;
            var enumName     = source.GetType().Name;

            sb.InsertAt(enumName, markInsertStartIndex);
            sb.InsertAt(".", markInsertStartIndex + enumName.Length);
            Span<char> replaceWithPipeEnumName = stackalloc char[enumName.Length + 1 + 3];
            replaceWithPipeEnumName.OverWriteAt(0, " | ");
            replaceWithPipeEnumName.OverWriteAt(3, enumName);
            replaceWithPipeEnumName.OverWriteAt(enumName.Length + 3, ".");
            sb.Replace(", ", replaceWithPipeEnumName, markInsertStartIndex + enumName.Length + 1, insertLength);
        }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        formatString ??= "";

        if (source == null)
        {
            AppendFormattedNull(sb, formatString, formatFlags);
            return sb;
        }

        return FormatFieldContents(sb, source.Value, formatString, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, allowEmptyContent);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default! , formatFlags, formatString);

        return FormatFieldContents(sb, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, allowEmptyContent);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(SqBrktClsChar);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, allowEmptyContent);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, allowEmptyContent);
        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
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
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;

        var contentStart = sb.Length;
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType, bool? hasItems, Type collectionType
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (!hasItems.HasValue)
        {
            GraphBuilder.MarkContentEnd();
            return sb;
        }
        var coreElementType = collectionType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? itemElementType;
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionNames.Any(s => coreElementType.FullName?.StartsWith(s) ?? false)))
        {
            sb.Append(RndBrktOpn);
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(RndBrktCls);
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormattingHandlingFlags)formatFlags);
        if (hasItems == true)
            AddCollectionElementPadding(moldInternal, itemElementType, 1, formatFlags);
        else
            GraphBuilder.Complete(formatFlags);
        return sb;
    }

    public override int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        GraphBuilder.AppendContent(SqBrktOpn);
        return 2;
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags);
        GraphBuilder.MarkContentStart(destStartIndex);
        var charsAdded = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        GraphBuilder.MarkContentEnd(destStartIndex + charsAdded).Complete((FieldContentHandling)formatFlags);
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
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        FormatFieldContents(sb, item, formatString ?? "", formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        FormatFieldContents(sb, item.Value, formatString ?? "", formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, ""); }
        var contentStart = sb.Length;
        styler(item, tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        Format(item, 0, sb, formatString ?? "", item.Length);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        Format(item, 0, sb, formatString ?? "", item.Length);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextStringBearerFormat<TBearer>(ITheOneString tos, TBearer item, int retrieveCount)
        where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, ""); }
        var contentStart = sb.Length;
        item.RevealState(tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return tos.WriteBuffer;
    }

    public virtual Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(moldInternal, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(moldInternal, elementType, nextItemNumber, formatFlags);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return 0;
        GraphBuilder.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.MarkSeparatorEnd();
        var charsAdded
            = destSpan.OverWriteAt(atIndex, formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        GraphBuilder.MarkPaddingEnd(atIndex + charsAdded);
        GraphBuilder.Complete(fmtFlgs);
        return charsAdded;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
            if (StyleOptions.NullWritesEmpty)
            {
                CollectionStart(itemElementType, sb, false, (FormattingHandlingFlags)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormattingHandlingFlags)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }

            GraphBuilder.MarkContentEnd();

            return sb;
        }

        return CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type collectionType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        var preClsLen = sb.Length;
        var lastChar  = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, (FieldContentHandling)formatFlags, true);
        if (lastChar != SqBrktOpnChar) { GraphBuilder.AppendContent(Spc); }
        GraphBuilder.AppendContent(SqBrktCls);
        return sb.Length - preClsLen;
    }

    public override int CollectionEnd(Type collectionType, Span<char> destSpan, int destIndex, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var originalDestIndex = destIndex;
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var lastChar = GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
        GraphBuilder.MarkContentStart(destIndex);

        if (lastChar != SqBrktOpnChar) destIndex += destSpan.OverWriteAt(destIndex, Spc);
        destIndex += destSpan.OverWriteAt(destIndex, SqBrktCls);
        GraphBuilder.MarkContentEnd(destIndex);
        return destIndex - originalDestIndex;
    }
}
