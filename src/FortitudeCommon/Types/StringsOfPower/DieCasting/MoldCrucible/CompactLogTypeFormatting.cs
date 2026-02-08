// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FieldContentHandlingExtensions;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting, ICloneable<CompactLogTypeFormatting>
  , ITransferState<CompactLogTypeFormatting>
{
    protected const string Dot        = ".";
    protected const string Cma        = ",";
    protected const string CmaSpc     = ", ";
    protected const string Spc        = " ";
    protected const string Cln        = ":";
    protected const string ClnSpc     = ": ";
    protected const char   BrcOpnChar = '{';
    protected const string BrcOpn     = "{";
    protected const string BrcCls     = "}";
    protected const string EqlsSpc    = "= ";

    protected const string SqBrktOpnSpc   = "[ ";
    protected const string RndBrktOpn     = "(";
    protected const string RndBrktCls     = ")";
    protected const char   RndBrktClsChar = ')';

    private GraphTrackingBuilder? graphBuilder;

    public GraphTrackingBuilder? GraphBuilder
    {
        get => graphBuilder ?? AlwaysRecycler.Borrow<GraphTrackingBuilder>();
        set
        {
            if (ReferenceEquals(graphBuilder, value)) return;
            graphBuilder?.DecrementRefCount();
            value?.IncrementRefCount();
            graphBuilder = value;
        }
    }

    public GraphTrackingBuilder Gb
    {
        get => graphBuilder ?? throw new ArgumentException("Never expect this to be called and not set!");
        set => GraphBuilder = value;
    }

    public IStyledTypeFormatting? PreviousContext { get; set; }

    public IStyledTypeFormatting PreviousContextOrThis => PreviousContext ?? this;

    public bool AddedContextOnThisCall { get; set; }

    public IStyledTypeFormatting ContextStartPushToNext(StyleOptions withStyleOptions)
    {
        var next = Clone();
        next.PreviousContext = this;
        next.StyleOptions    = withStyleOptions;
        return next;
    }

    public IStyledTypeFormatting ContextCompletePopToPrevious()
    {
        var previous = PreviousContext;
        if (previous != null)
        {
            if (previous.GraphBuilder != null && GraphBuilder != null) { previous.GraphBuilder.SetHistory(GraphBuilder); }
            DecrementRefCount();
            return previous;
        }
        return this;
    }

    public virtual IStyledTypeFormatting Initialize(ITheOneString theOneString)
    {
        graphBuilder = AlwaysRecycler.Borrow<GraphTrackingBuilder>().Initialize(this, theOneString.WriteBuffer);

        Options = theOneString.Settings;

        return this;
    }

    protected virtual IStyledTypeFormatting Initialize(GraphTrackingBuilder withGraphBuilder)
    {
        Options      = withGraphBuilder.StyleOptions!;
        graphBuilder = withGraphBuilder.Initialize(this);

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);
    public StyleOptions StyleOptions
    {
        get => (StyleOptions)Options;
        set => Options = value;
    }

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
        if (!callerFormattingFlags.HasContentTreatmentFlags() && typeof(T) == typeof(object))
        {
            var actualType = input?.GetType() ?? typeof(object);
            if (!actualType.IsSpanFormattableCached() && !actualType.IsStringBearer() || actualType.IsAnyTypeHoldingChars())
            {
                setFlags |= EnsureFormattedDelimited;
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

    public FormatFlags GetFormatterContentHandlingFlags<T>(ITheOneString tos, T forValue, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags = GetFormatterContentHandlingFlags(tos, actualType, proposedWriteType, visitResult, formatFlags);
        switch (proposedWriteType)
        {
            case AsComplex:
                if (!actualType.IsValueType)
                {
                    var isBaseOfCallerType = tos.IsCallerSameInstanceAndMoreDerived(forValue);
                    if (isBaseOfCallerType) { resolvedFlags |= SuppressOpening | LogSuppressTypeNames | SuppressClosing; }
                }
                break;
        }

        return resolvedFlags;
    }

    public FormatFlags GetFormatterContentHandlingFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags = formatFlags;
        switch (proposedWriteType)
        {
            case AsContent | AsComplex | AsRaw:
            case AsContent | AsComplex:
                resolvedFlags = formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType;
                break;
            case AsContent | AsSimple | AsRaw:
            case AsContent | AsSimple:
                resolvedFlags = formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType;
                if (actualType.IsSpanFormattableCached() || actualType.IsBoolOrNullable())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision = StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                        ? LogSuppressTypeNames
                        : DefaultCallerTypeFlags;
                    if (!actualType.IsValueType && tos.Settings.InstanceTrackingIncludeSpanFormattableClasses && visitResult.IsARevisit)
                    {
                        resolvedFlags |= shouldSuppressTypeNameDecision;
                    }
                    else { resolvedFlags |= SuppressOpening | shouldSuppressTypeNameDecision | SuppressClosing; }
                }
                if (actualType.IsAnyTypeHoldingCharsCached())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision = StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                        ? LogSuppressTypeNames
                        : DefaultCallerTypeFlags;
                    resolvedFlags = (resolvedFlags & ~ContentAllowComplexType) | SuppressOpening | shouldSuppressTypeNameDecision | SuppressClosing;
                }
                break;
            case AsComplex:
                if (actualType.IsSpanFormattableCached())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision =
                        StyleOptions.LogSuppressDisplayTypeNames
                                    .Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                            ? LogSuppressTypeNames
                            : DefaultCallerTypeFlags;
                    resolvedFlags |= shouldSuppressTypeNameDecision;
                }
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return resolvedFlags;
    }

    public virtual ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (moldInternal.CurrentWriteMethod.SupportsMultipleFields()) { return StartComplexTypeOpening(moldInternal); }
        var sb = moldInternal.Sb;

        var buildingType = moldInternal.TypeBeingBuilt;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag()
            // && !buildingType.IsStringBearer() 
         && !(StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildingType.FullName?.StartsWith(s) ?? false))
           )
        {
            sb.Append(RndBrktOpn);
            buildingType.AppendShortNameInCSharpFormat(sb);
            moldInternal.WroteTypeName = true;
        }
        return Gb.ContentEndToRanges(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (moldInternal.CurrentWriteMethod.SupportsMultipleFields()) { return FinishComplexTypeOpening(moldInternal); }
        if (moldInternal is { WroteTypeName: true })
        {
            Gb.AppendContent(RndBrktCls);
            if (!moldInternal.SkipBody)
            {
                // space considered content
                Gb.AppendContent(Spc);
            }
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast moldInternal)
    {
        if (moldInternal.CurrentWriteMethod.SupportsMultipleFields()) { return AppendComplexTypeClosing(moldInternal); }
        if (Gb.HasCommitContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, DefaultCallerTypeFlags);
        return Gb.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public virtual ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;

        var buildingType      = mdc.TypeBeingBuilt;
        var buildTypeFullName = buildingType.FullName ?? "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);

        var mergedFlags = formatFlags | mdc.CreateMoldFormatFlags;

        if (mergedFlags.HasSuppressOpening()) return Gb.Complete(formatFlags);
        if (mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
             (formatFlags.HasAddTypeNameFieldFlag() ||
              !StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildTypeFullName.StartsWith(s)))))
        {
            var isComplexContentType = mdc.CurrentWriteMethod.HasAsContentFlag();
            if (isComplexContentType) { Gb.AppendContent(RndBrktOpn); }
            buildingType.AppendShortNameInCSharpFormat(sb);
            if (isComplexContentType) { Gb.AppendContent(RndBrktCls); }
            sb.Append(Spc);
            mdc.WroteTypeName = true;
        }
        return Gb
               .AppendContent(BrcOpn)
               .AppendPadding(Spc)
               .Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        return ContentSeparatorRanges.None;
    }

    public virtual int SizeFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        LayoutEncoder.CalculateEncodedLength(ClnSpc);

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        Gb
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

    public virtual int SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return formatFlags.UseMainFieldSeparator()
            ? LayoutEncoder.CalculateEncodedLength(StyleOptions.MainItemSeparator)
            : LayoutEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldSeparator);
    }

    public virtual Range? AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldSeparatorFlag()) return null;
        Gb.AppendSeparator(formatFlags.UseMainFieldSeparator() ? StyleOptions.MainItemSeparator : StyleOptions.AlternateFieldSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual int SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return 0;
        return formatFlags.UseMainFieldPadding()
            ? LayoutEncoder.CalculateEncodedLength(StyleOptions.MainFieldPadding)
            : LayoutEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding);
    }

    public virtual ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return Gb.Complete(formatFlags);
        Gb.AppendPadding(formatFlags.UseMainFieldPadding() ? StyleOptions.MainFieldPadding : StyleOptions.AlternateFieldPadding);
        return Gb.Complete(formatFlags);
    }

    public virtual int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return SizeToNextFieldSeparator(formatFlags) + SizeNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        return AddNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();

        if (moldInternal.CreateMoldFormatFlags.HasSuppressClosing())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
        }
        else
        {
            if (lastContentChar != BrcOpnChar) { Gb.StartAppendContent(Spc, sb, this, DefaultCallerTypeFlags).AppendContent(BrcCls); }
            else { Gb.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var writeFlags = AsNull;
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
        {
            var        contentStart            = sb.Length;
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatSwitches: FormatSwitches.DefaultCallerTypeFlags);
            writeFlags |= sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        }
        else { sb.Append(StyleOptions.NullString); }
        Gb.MarkContentEnd();
        return writeFlags;
    }

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var kvpTypes = keyedCollectionType.GetKeyedCollectionTypes();
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => keyedCollectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Key.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Value.FullName?.StartsWith(s) ?? false)))
        {
            keyedCollectionType.AppendShortNameInCSharpFormat(sb);
            Gb.AppendContent(Spc);
        }
        Gb.AppendContent(BrcOpn);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (lastContentChar != BrcOpnChar)
            Gb.AppendContent(Spc).AppendContent(BrcCls);
        else { Gb.AppendContent(BrcCls); }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue value
      , int retrieveCount
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        typeMold.FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        mdc.FieldEnd();

        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : struct, TVRevealBase
        where TVRevealBase : notnull
    {
        mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        mdc.FieldEnd();
        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = mdc.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, mdc.Master); }
        mdc.FieldEnd();
        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = mdc.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, mdc.Master); }
        mdc.FieldEnd();
        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = mdc.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, mdc.Master); }
        mdc.FieldEnd();
        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = mdc.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, mdc.Master); }
        mdc.FieldEnd();
        if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
        else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mdc;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags) => sourceLength;

    public virtual int InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex
      , WrittenAsFlags writtenAs, int indexToInsertAt, FormatFlags createTypeFlags, int contentLength = -1
      , ITypeMolderDieCast? liveMoldInternal = null)
    {
        var sb = insertBuilder.Sb;
        var instanceInfoFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
                ? DefaultCallerTypeFlags
                : DisableFieldNameDelimiting;
        var instanceIdFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceIdInQuotes
                ? DefaultCallerTypeFlags
                : DisableAutoDelimiting;

        var preAppendLength = sb.Length;

        var refDigitsCount = refId.NumOfDigits();

        var toRestore = Gb;
        Gb = insertBuilder;

        var alreadySupportsMultipleFields = writtenAs.SupportsMultipleFields() ;

        var needsBracesWrap  = false;
        var prefixInsertSize = 0;
        var suffixInsertSize = 0;

        // first entry spot maybe removed if empty so backtrack to open add one;
        var firstFieldPad = SizeNextFieldPadding(createTypeFlags);
        var isEmpty       = indexToInsertAt - firstFieldPad + 1 == typeOpenIndex + contentLength;
        if (!alreadySupportsMultipleFields)
        {
            needsBracesWrap = writtenAs.HasAsRawFlag() && writtenAs.HasNoneOf(AsContent);
            if (writtenAs.HasAllOf(AsSimple | AsContent))
            {
                var actualTypeFullName = actualType.FullName ?? "";
                if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
                 || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName.StartsWith(s))) { needsBracesWrap = true; }
            }
            if (needsBracesWrap)
            {
                if (liveMoldInternal != null) { liveMoldInternal.CurrentWriteMethod = writtenAs.ToMultiFieldEquivalent(); }
                prefixInsertSize += 1; // Open Brace Only close in done later
                Gb.IndentLevel++;
                prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
                // insert $id added below
                prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
                prefixInsertSize += SizeFormatFieldName("$values".Length, createTypeFlags);
                prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
            }
            else
            {
                prefixInsertSize += 2;
                isEmpty          =  true;
            }
        }
        else if (isEmpty)
        {
            indexToInsertAt  -= SizeNextFieldPadding(createTypeFlags);
            prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
            Gb.IndentLevel--;
            // after inserted
            prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
            Gb.IndentLevel++;
        }
        else
        {
            // after inserted
            prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
        }
        prefixInsertSize += SizeFormatFieldName("$id".Length, instanceInfoFormatFlags);
        prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        prefixInsertSize += SizeFormatFieldContents(refDigitsCount, instanceIdFormatFlags);

        insertBuilder.StartInsertAt(indexToInsertAt, prefixInsertSize);

        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        if (!alreadySupportsMultipleFields)
        {
            if (needsBracesWrap)
            {
                Gb.AppendContent(BrcOpn);
                AddNextFieldPadding(createTypeFlags);
            }
            else { Gb.AppendContent(RndBrktOpn); }
        }
        else if (isEmpty) { AddNextFieldPadding(createTypeFlags); }
        
        if (instanceInfoFormatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        Gb.AppendContent("$id");
        if (instanceInfoFormatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        AppendFieldValueSeparator();
        if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) Gb.AppendContent(DblQt);
        Span<char> refSpan = stackalloc char[refDigitsCount];
        if (refId.TryFormat(refSpan, out var charsWritten, ""))
        {
            if (charsWritten != refDigitsCount) { Debugger.Break(); }
            Gb.AppendContent(refSpan);
        }
        else
        {
            Debugger.Break();
            Gb.AppendContent(refId.ToString());
        }
        if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) Gb.AppendContent(DblQt);
        if (liveMoldInternal != null) { liveMoldInternal.WrittenAsFlags |= WithInstanceId; }
        if (!alreadySupportsMultipleFields)
        {
            if (needsBracesWrap)
            {
                AddToNextFieldSeparatorAndPadding(createTypeFlags);
                AppendInstanceValuesFieldName(typeof(object), createTypeFlags);
                if (contentLength >= 0)
                {
                    Gb.IndentLevel--;
                    suffixInsertSize += SizeNextFieldPadding(createTypeFlags);
                    suffixInsertSize += 1; // close Brace

                    insertBuilder.StartInsertAt(typeOpenIndex + contentLength + prefixInsertSize, suffixInsertSize);
                    AddNextFieldPadding(createTypeFlags);
                    Gb.AppendContent(BrcCls);
                }
            }
            else { Gb.AppendContent(RndBrktCls); }
        }
        else if (isEmpty)
        {
            Gb.IndentLevel--;
            AddNextFieldPadding(createTypeFlags);
        }
        else { AddToNextFieldSeparatorAndPadding(createTypeFlags); }
        Gb = toRestore;
        return sb.Length - preAppendLength;
    }

    public int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var preAppendLength = Gb.Sb.Length;
        Gb.AppendContent("$values");
        AppendFieldValueSeparator();
        return Gb.Sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(ITypeMolderDieCast mdc, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        var sb = mdc.Sb;

        var preAppendLength = sb.Length;
        var needsBracesWrap = false;

        var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields();
        Gb.StartNextContentSeparatorPaddingSequence(sb, createTypeFlags);
        if (!alreadySupportsMultipleFields)
        {
            needsBracesWrap = currentWriteMethod.HasAsRawFlag()
                           || (currentWriteMethod.HasAllOf(AsContent | AsSimple)
                            && createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing());
            if (currentWriteMethod.HasAllOf(AsContent | AsSimple))
            {
                var actualTypeFullName = mdc.TypeBeingBuilt.FullName;
                if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
                 || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)) { needsBracesWrap = true; }
            }
            if (needsBracesWrap)
            {
                mdc.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
                StartComplexTypeOpening(mdc, createTypeFlags);
                FinishComplexTypeOpening(mdc, createTypeFlags);
            }
            else { Gb.AppendContent(RndBrktOpn); }
        }
        AppendFieldName(mdc, "$ref");
        AppendFieldValueSeparator();
        FormatFieldContents(mdc, refId, "", createTypeFlags);
        mdc.WrittenAsFlags |= WithReferenceToInstanceId;

        if (!alreadySupportsMultipleFields && !needsBracesWrap) { Gb.AppendContent(RndBrktCls); }
        else
        {
            mdc.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
        }
        return sb.Length - preAppendLength;
    }

    public virtual int AppendInstanceInfoField(ITypeMolderDieCast mdc, string fieldName, ReadOnlySpan<char> description
      , WrittenAsFlags writeMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck()) return 0; // fieldNames are marked with this and
        var sb = mdc.Sb;

        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields();
        var preAppendLength               = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            if (sb[^1] == RndBrktClsChar)
            {
                sb.Length -= 1;
                AddToNextFieldSeparator(createTypeFlags);
                Gb.AppendPaddingAndComplete(" ", DefaultCallerTypeFlags);
            }
            else { Gb.StartAppendContentAndComplete(RndBrktOpn, sb, DefaultCallerTypeFlags); }
        }
        AppendFieldName(mdc, fieldName);
        AppendFieldValueSeparator();
        FormatFieldContents(mdc, description, 0, "\"{0}\"", formatFlags: createTypeFlags);

        if (!alreadySupportsMultipleFields) { Gb.StartAppendContentAndComplete(RndBrktCls, sb, DefaultCallerTypeFlags); }
        else
        {
            mdc.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
        }
        return sb.Length - preAppendLength;
    }

    public virtual WrittenAsFlags AppendFieldName(ITypeMolderDieCast mdc, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartAppendContent(fieldName, sb, this, DefaultCallerTypeFlags);
        return AsValue;
    }

    public virtual WrittenAsFlags FormatFieldNameMatch<TAny>(ITypeMolderDieCast mdc, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, source);
        else
            sb.Append(source);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(source, sb, formatString ?? "");
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        if (source != null)
            Format(source, sb, formatString ?? "");
        else
            base.Format(StyleOptions.NullString, 0, sb, formatString);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName<TFmt>(ITypeMolderDieCast mdc, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        return FormatFieldContents(mdc, source, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldName<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        return FormatFieldContents(mdc, source, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary FormatFieldName<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);

        var visitNumber = mdc.MoldGraphVisit.CurrentVisitIndex;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TCloaked), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }

        var appendSummary = valueRevealer(value, mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, value.GetType());
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || !callerFormatFlags.HasAsStringContentFlag()
         || sb[contentStart] == DblQtChar)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber >= 0)
        {
            mdc.Master.UpdateVisitLength(mdc.Master.ActiveGraphRegistry.RegistryId, appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldName<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);

        var visitNumber = mdc.MoldGraphVisit.CurrentVisitIndex;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, styledObj.GetType());
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || !callerFormatFlags.HasAsStringContentFlag()
         || sb[contentStart] == DblQtChar)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber >= 0)
        {
            mdc.Master.UpdateVisitLength(mdc.Master.ActiveGraphRegistry.RegistryId, appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual int SizeFormatFieldContents(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                 = 0;
        if (formatFlags.DoesNotHaveDisableAutoDelimiting()) size = 2 * LayoutEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual WrittenAsFlags FormatFieldContentsMatch<TAny>(ITypeMolderDieCast mdc, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart       = sb.Length;
        
        if (formatFlags.ShouldDelimit() && !formatString.IsDblQtBounded())
        {
            Gb.AppendDelimiter(DblQt);
        }
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && !formatString.IsDblQtBounded())
        {
            Gb.AppendDelimiter(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart       = sb.Length;
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
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart       = sb.Length;
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
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmt>(ITypeMolderDieCast mdc, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mdc.Sb;
        formatString ??= "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart       = sb.Length;
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

        base.Format(source, sb, formatSpan, (FormatSwitches)formatFlags);
        if (source is Enum)
        {
            var insertLength = sb.Length - contentStart;
            var enumName     = source.GetType().Name;

            sb.InsertAt(enumName, contentStart);
            sb.InsertAt(".", contentStart + enumName.Length);
            Span<char> replaceWithPipeEnumName = stackalloc char[enumName.Length + 1 + 3];
            replaceWithPipeEnumName.OverWriteAt(0, " | ");
            replaceWithPipeEnumName.OverWriteAt(3, enumName);
            replaceWithPipeEnumName.OverWriteAt(enumName.Length + 3, ".");
            sb.Replace(", ", replaceWithPipeEnumName, contentStart + enumName.Length + 1, insertLength);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mdc.Sb;
        formatString ??= "";

        if (source == null) { return AppendFormattedNull(sb, formatString, formatFlags); }

        return FormatFieldContents(mdc, source.Value, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        var allowEmptyContent = true;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
        var contentStart       = sb.Length;
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
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFallbackFieldContents<TAny>(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default!, formatFlags, formatString);

        return FormatFieldContents(mdc, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        var allowEmptyContent = true;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
        var contentStart       = sb.Length;
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

        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection))
            sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection))
            sb.Append(SqBrktClsChar);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        var allowEmptyContent = true;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
        var contentStart       = sb.Length;
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
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        var allowEmptyContent = true;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
        var contentStart       = sb.Length;
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
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary FormatFieldContents<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mdc.MoldGraphVisit.CurrentVisitIndex;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TCloaked), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }

        var appendSummary = valueRevealer(value, mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, value.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || appendSummary.WrittenAs.HasAsStringFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber >= 0)
        {
            mdc.Master.UpdateVisitLength(mdc.Master.ActiveGraphRegistry.RegistryId, appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldContents<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj,
        string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = mdc.Sb;

        var withMoldInherited = callerFormatFlags | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mdc.MoldGraphVisit.CurrentVisitIndex;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, styledObj.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber >= 0)
        {
            mdc.Master.UpdateVisitLength(mdc.Master.ActiveGraphRegistry.RegistryId, appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast mdc, Type itemElementType, bool? hasItems, Type collectionType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!hasItems.HasValue)
        {
            Gb.MarkContentEnd();
            return sb;
        }
        var coreElementType = collectionType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? itemElementType;
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionNames.Any(s => coreElementType.FullName?.StartsWith(s) ?? false)))
        {
            sb.Append(RndBrktOpn);
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(RndBrktCls).Append(Spc);
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)formatFlags);
        if (hasItems == true)
            AddCollectionElementPadding(mdc, itemElementType, 1, formatFlags);
        else
            Gb.Complete(formatFlags);
        return sb;
    }

    public override int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        Gb.AppendContent(SqBrktOpn);
        return 2;
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        Gb.ResetCurrent((FormatFlags)formatSwitches);
        Gb.MarkContentStart(destStartIndex);
        var charsAdded = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        Gb.MarkContentEnd(destStartIndex + charsAdded).Complete((FormatFlags)formatSwitches);
        return charsAdded;
    }

    public WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public AppendSummary CollectionNextItemFormat<TFmt>(ITypeMolderDieCast mdc, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = item?.GetType() ?? typeof(TFmt);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        FormatFieldContents(mdc, item, formatString ?? "", formatFlags);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? item, int retrieveCount
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mdc.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        FormatFieldContents(mdc, item.Value, formatString ?? "", formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(ITypeMolderDieCast mdc
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler, string? callerFormatString
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var actualType = item?.GetType() ?? typeof(TCloaked);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(callerFormatFlags);
        var contentStart       = sb.Length;
        var stateExtractResult = styler(item, mdc.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, string? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        var contentStart = sb.Length;
        sb.AppendFormat(this, formatString ?? "", item);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, char[]? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        var contentStart = sb.Length;
        sb.AppendFormat(this, formatString ?? "", item);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextCharSeqFormat<TCharSeq>(ITypeMolderDieCast mdc, TCharSeq item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = item?.GetType() ?? typeof(TCharSeq);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(item, 0, sb, formatString ?? "", item.Length);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(item, 0, sb, formatString ?? "", item.Length);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextStringBearerFormat<TBearer>(ITypeMolderDieCast mdc, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = item?.GetType() ?? typeof(TBearer);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString, callerFormatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = item.RevealState(mdc.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        Gb.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(formatFlags);
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
        Gb.AppendSeparator(formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(fmtFlags).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.Complete(fmtFlags).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatSwitches.HasNoItemPaddingFlag()) return Gb.Complete(fmtFlags).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.MarkSeparatorEnd();
        var charsAdded
            = destSpan.OverWriteAt(atIndex, formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        Gb.MarkPaddingEnd(atIndex + charsAdded);
        Gb.Complete(fmtFlags);
        return charsAdded;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(fmtFlags).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.Complete(fmtFlags).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast mdc, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        if (!totalItemCount.HasValue)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
            if (StyleOptions.NullWritesEmpty)
            {
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }

            Gb.MarkContentEnd();

            return sb;
        }

        return CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormatSwitches)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }

        var preClsLen = sb.Length;
        var lastChar  = Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(sb, (FormatFlags)formatSwitches, true);
        if (lastChar != SqBrktOpnChar) { Gb.AppendContent(Spc); }
        Gb.AppendContent(SqBrktCls);
        return sb.Length - preClsLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var originalDestIndex = destIndex;
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var lastChar = Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        Gb.ResetCurrent((FormatFlags)formatSwitches, true);
        Gb.MarkContentStart(destIndex);

        if (lastChar != SqBrktOpnChar) destIndex += destSpan.OverWriteAt(destIndex, Spc);
        destIndex += destSpan.OverWriteAt(destIndex, SqBrktCls);
        Gb.MarkContentEnd(destIndex);
        return destIndex - originalDestIndex;
    }

    object ICloneable.Clone() => Clone();

    IStyledTypeFormatting IStyledTypeFormatting.Clone() => Clone();

    public override CompactLogTypeFormatting Clone()
    {
        return AlwaysRecycler.Borrow<CompactLogTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public virtual CompactLogTypeFormatting CopyFrom(CompactLogTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (graphBuilder != null)
        {
            graphBuilder.DecrementRefCount();
            graphBuilder = null;
        }
        var nextGb = source.Gb.Clone();
        Initialize(nextGb);

        return this;
    }

    public override void StateReset()
    {
        Gb              = null!;
        PreviousContext = null;
        base.StateReset();
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {nameof(Gb)}: {graphBuilder?.ToString() ?? "null"} }}";
}
