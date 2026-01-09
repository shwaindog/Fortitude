// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FieldContentHandlingExtensions;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot        = ".";
    protected const string Cma        = ",";
    protected const string CmaSpc     = ", ";
    protected const string Spc        = " ";
    protected const string Cln        = ":";
    protected const char   BrcOpnChar = '{';
    protected const string BrcOpn     = "{";
    protected const string BrcCls     = "}";
    protected const string EqlsSpc    = "= ";

    protected const string SqBrktOpnSpc = "[ ";
    protected const string RndBrktOpn   = "(";
    protected const string RndBrktCls   = ")";

    public GraphTrackingBuilder GraphBuilder { get; set; } = null!;

    public virtual CompactLogTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        GraphBuilder = graphTrackingBuilder;
        Options      = styleOptions;

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);
    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual FormatFlags ResolveContentFormattingFlags<T>(IStringBuilder sb, T input
      , FormatFlags callerFormattingFlags, string? formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags()) { return callerFormattingFlags; }

        FormatFlags setFlags = callerFormattingFlags;
        setFlags |= (FormatFlags)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormatSwitches)setFlags, formatString ?? "");

        var callerInputType = typeof(T);
        var typeofT         = input?.GetType() ?? callerInputType;
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
        if (typeofT.IsKeyedCollection())
        {
            var kvpType = typeof(KeyValuePair<,>);
            if (typeofT.IsFrameworkDictionary() || typeofT.IsArrayOfGeneric(kvpType) || typeofT.IsListOfGeneric(kvpType)
             // || typeofT.IsJustReadOnlyListOfGeneric(kvpType) 
             || typeofT.IsSpanOfGeneric(kvpType) || typeofT.IsReadOnlySpanOfGeneric(kvpType)
             || callerInputType.IsJustEnumerableOfGeneric(kvpType) || callerInputType.IsJustEnumeratorOfGeneric(kvpType))
            {
                setFlags |= LogSuppressTypeNames;
            }
        }
        return setFlags;
    }

    public virtual FormatFlags ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT               = typeof(T);
        var isAnyTypeHoldingChars = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (input == null && (fallbackValue.Length == 0 && !isAnyTypeHoldingChars)) return formatFlags | AsValueContent;
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsValueContent;
        return formatFlags | AsValueContent;
    }

    public virtual FormatFlags ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT                     = typeof(T);
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullable();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsStringContent;
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return formatFlags | DisableAutoDelimiting | AsStringContent;
        return formatFlags | AsStringContent;
    }

    public SkipTypeParts GetNextValueTypePartFlags<T>(ITheOneString tos, T forValue, Type actualType, FormatFlags formatFlags)
    {
        var isLastType = tos.IsLastVisitedObject(forValue);
        if (forValue is ISpanFormattable || isLastType) { return SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd; }
        return SkipTypeParts.None;
    }

    public SkipTypeParts GetNextComplexTypePartFlags<T>(ITheOneString tos, T forValue, Type actualType, FormatFlags formatFlags)
    {
        var isLastType = tos.IsLastVisitedObject(forValue);
        var skipParts  = SkipTypeParts.None;
        if (isLastType) { skipParts = SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd; }
        skipParts |= formatFlags.HasSuppressOpening() ? SkipTypeParts.TypeStart : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressClosing() ? SkipTypeParts.TypeEnd : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressTypeNamesFlag() ? SkipTypeParts.TypeName : SkipTypeParts.None;
        return skipParts;
    }

    public virtual ContentSeparatorRanges AppendValueTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag()
         && !(StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildingType.FullName?.StartsWith(s) ?? false))
           )
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (!moldInternal.AppendSettings.SkipTypeParts.HasTypeNameFlag())
        {
            if (alternativeName != null)
                sb.Append(alternativeName);
            else
                buildingType.AppendShortNameInCSharpFormat(sb);
            sb.Append(Spc);
        }
        return GraphBuilder
               .AppendContent(BrcOpn)
               .AppendPadding(Spc)
               .Complete(formatFlags);
    }

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

    public virtual Range? AddNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainFieldSeparator() ? StyleOptions.MainItemSeparator : StyleOptions.AlternateFieldSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainFieldPadding() ? StyleOptions.MainFieldPadding : StyleOptions.AlternateFieldPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddNextFieldSeparator(formatFlags);
        return AddNextFieldPadding(formatFlags);
    }

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) =>
        sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);

    public virtual ContentSeparatorRanges AppendTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;

        var lastContentChar = GraphBuilder.RemoveLastSeparatorAndPadding();

        if (moldInternal.AppendSettings.SkipTypeParts.HasTypeEndFlag())
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags, true);
        }
        else
        {
            if (lastContentChar != BrcOpnChar) { GraphBuilder.StartAppendContent(Spc, sb, this, DefaultCallerTypeFlags).AppendContent(BrcCls); }
            else { GraphBuilder.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return GraphBuilder.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isFieldName = false)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
        {
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatSwitches: FormatSwitches.DefaultCallerTypeFlags);
        }
        else { sb.Append(StyleOptions.NullString); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        var kvpTypes = keyedCollectionType.GetKeyedCollectionTypes();
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => keyedCollectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Key.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Value.FullName?.StartsWith(s) ?? false)))
        {
            keyedCollectionType.AppendShortNameInCSharpFormat(sb);
            GraphBuilder.AppendContent(Spc);
        }
        GraphBuilder.AppendContent(BrcOpn);
        AddNextFieldPadding(formatFlags);     
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var lastContentChar = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (lastContentChar != BrcOpnChar)
            GraphBuilder.AppendContent(Spc).AppendContent(BrcCls);
        else { GraphBuilder.AppendContent(BrcCls); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : struct, TVRevealBase
        where TVRevealBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value.Value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FormatFlags valueFlags = DefaultCallerTypeFlags)
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

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey? key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value.Value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey? key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler, FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { valueStyler(value.Value, typeMold.Master); }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddNextFieldSeparator(formatFlags);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName)
    {
        GraphBuilder.StartAppendContent(fieldName, sb, this, DefaultCallerTypeFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        Format(source, sb, formatString ?? "");
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (source != null)
            Format(source, sb, formatString ?? "");
        else
            base.Format(StyleOptions.NullString, 0, sb, formatString);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        FormatFieldContents(sb, source, formatString, formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatFieldContents(sb, source, formatString, formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ISecretStringOfPower tos, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (styledObj == null) { AppendFormattedNull(sb, callerFormatString, callerFormatFlags); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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

        Format(source, sb, formatSpan, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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

        Format(source, sb, formatSpan, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
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
        base.Format(source, sb, formatSpan, (FormatSwitches)formatFlags);
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
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
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default!, formatFlags, formatString);

        return FormatFieldContents(sb, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
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

        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection)) sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection)) sb.Append(SqBrktClsChar);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb           = tos.WriteBuffer;
        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ISecretStringOfPower tos, TBearer styledObj,
        string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;

        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType, bool? hasItems, Type collectionType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)formatFlags);
        if (hasItems == true)
            AddCollectionElementPadding(moldInternal, itemElementType, 1, formatFlags);
        else
            GraphBuilder.Complete(formatFlags);
        return sb;
    }

    public override int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        GraphBuilder.AppendContent(SqBrktOpn);
        return 2;
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        GraphBuilder.ResetCurrent((FormatFlags)formatSwitches);
        GraphBuilder.MarkContentStart(destStartIndex);
        var charsAdded = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        GraphBuilder.MarkContentEnd(destStartIndex + charsAdded).Complete((FormatFlags)formatSwitches);
        return charsAdded;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        FormatFieldContents(sb, item, formatString ?? "", formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
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
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        Format(item, 0, sb, formatString ?? "", item.Length);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(moldInternal, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(moldInternal, elementType, nextItemNumber, formatFlags);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        GraphBuilder.AppendSeparator(formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatSwitches.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.MarkSeparatorEnd();
        var charsAdded
            = destSpan.OverWriteAt(atIndex, formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        GraphBuilder.MarkPaddingEnd(atIndex + charsAdded);
        GraphBuilder.Complete(fmtFlgs);
        return charsAdded;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
            if (StyleOptions.NullWritesEmpty)
            {
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }

            GraphBuilder.MarkContentEnd();

            return sb;
        }

        return CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormatSwitches)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }

        var preClsLen = sb.Length;
        var lastChar  = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, (FormatFlags)formatSwitches, true);
        if (lastChar != SqBrktOpnChar) { GraphBuilder.AppendContent(Spc); }
        GraphBuilder.AppendContent(SqBrktCls);
        return sb.Length - preClsLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var originalDestIndex = destIndex;
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var lastChar = GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        GraphBuilder.ResetCurrent((FormatFlags)formatSwitches, true);
        GraphBuilder.MarkContentStart(destIndex);

        if (lastChar != SqBrktOpnChar) destIndex += destSpan.OverWriteAt(destIndex, Spc);
        destIndex += destSpan.OverWriteAt(destIndex, SqBrktCls);
        GraphBuilder.MarkContentEnd(destIndex);
        return destIndex - originalDestIndex;
    }

    public override void StateReset()
    {
        GraphBuilder = null!;
        base.StateReset();
    }
}
