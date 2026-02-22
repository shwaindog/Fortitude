// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
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
    protected const string Spc        = " ";
    protected const string Cln        = ":";
    protected const string ClnSpc     = ": ";
    protected const char   BrcOpnChar = '{';
    protected const string BrcOpn     = "{";
    protected const string BrcCls     = "}";

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

    public virtual FormatFlags ResolveContentFormatFlags<T>(IStringBuilder sb, T input
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

    public virtual FormatFlags ResolveContentAsValueFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT = typeof(T);

        var isAnyTypeHoldingChars = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (input == null && (fallbackValue.Length == 0 && !isAnyTypeHoldingChars)) return formatFlags | AsValueContent;
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsValueContent;
        return formatFlags | AsValueContent;
    }

    public virtual FormatFlags ResolveContentAsStringFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT = typeof(T);

        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullable();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsStringContent;
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return formatFlags | DisableAutoDelimiting | AsStringContent;
        return formatFlags | AsStringContent;
    }

    public (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags<T>(ITheOneString tos, T forValue, Type actualType
      , WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var (resolvedWrittenAs, resolvedFlags) = ResolveMoldWriteAsFormatFlags(tos, actualType, proposedWriteType, visitResult, formatFlags);
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

        return (resolvedWrittenAs, resolvedFlags);
    }

    public (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags     = formatFlags;
        var resolvedWrittenAs = proposedWriteType;

        var settings = tos.Settings;
        switch (proposedWriteType)
        {
            case AsObject | AsRaw:
                resolvedFlags |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedFlags     |= ContentAllowComplexType;
                    resolvedWrittenAs =  (resolvedWrittenAs & ~AsSimple) | AsComplex | AsRaw  | AsObject;
                    break;
                }

                resolvedWrittenAs =  (resolvedWrittenAs & ~AsComplex) | AsSimple | AsRaw  | AsObject;
                break;
            case AsContent | AsComplex: resolvedFlags = formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
            case AsRaw | AsContent:
            case AsContent | AsSimple:
                resolvedFlags = formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
                if (actualType.IsSpanFormattableOrNullableCached() || actualType.IsBoolOrNullable())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision = StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                        ? LogSuppressTypeNames
                        : DefaultCallerTypeFlags;
                    if (!actualType.IsValueType && tos.Settings.InstanceTrackingIncludeSpanFormattableClasses && visitResult.IsARevisit)
                    {
                        resolvedFlags |= shouldSuppressTypeNameDecision;
                        if (settings.InstanceMarkingIncludeSpanFormattableContents)
                        {
                            resolvedWrittenAs = (resolvedWrittenAs & ~AsSimple) | AsComplex | AsContent;
                        }
                    }
                    else { resolvedFlags |= SuppressOpening | shouldSuppressTypeNameDecision | SuppressClosing; }
                }
                if (actualType.IsAnyTypeHoldingCharsCached())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision = StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                        ? LogSuppressTypeNames
                        : DefaultCallerTypeFlags;
                    if (resolvedWrittenAs.HasAsRawFlag() && visitResult.IsARevisit)
                    {
                        if ((settings.InstanceTrackingIncludeStringInstances
                          && actualType.IsString()
                          && settings.InstanceMarkingIncludeStringContents)
                         || (settings.InstanceTrackingIncludeStringBuilderInstances
                          && actualType.IsStringBuilder()
                          && settings.InstanceMarkingIncludeStringBuilderContents)
                         || (settings.InstanceTrackingIncludeCharSequenceInstances
                          && actualType.IsCharSequence()
                          && settings.InstanceMarkingIncludeCharSequenceContents)
                         || (settings.InstanceTrackingIncludeCharArrayInstances
                          && actualType.IsCharArray()
                          && settings.InstanceMarkingIncludeCharArrayContents))
                        {
                            resolvedWrittenAs =  AsRaw | AsComplex | AsContent;
                            resolvedFlags     |= shouldSuppressTypeNameDecision;
                        }
                    }
                    else
                    {
                        resolvedFlags = (resolvedFlags & ~ContentAllowComplexType) | SuppressOpening | shouldSuppressTypeNameDecision | SuppressClosing;
                    }
                }
                break;
            case AsComplex | AsObject:
                if (actualType.IsSpanFormattableOrNullableCached())
                {
                    var actualTypeFullName = actualType.FullName;
                    var shouldSuppressTypeNameDecision =
                        StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                            ? LogSuppressTypeNames
                            : DefaultCallerTypeFlags;
                    resolvedFlags |= shouldSuppressTypeNameDecision;
                }
                break;
            case AsRaw | WrittenAsFlags.AsCollection:
                resolvedFlags     |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedWrittenAs =  AsRaw | AsComplex | WrittenAsFlags.AsCollection;
                    resolvedFlags     |= ContentAllowComplexType;
                }
                else
                {
                    resolvedWrittenAs =  AsRaw | AsSimple | WrittenAsFlags.AsCollection;
                }
                break;
            case AsComplex | WrittenAsFlags.AsCollection:
                resolvedFlags     |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType;
                break;
            case AsSimple | WrittenAsFlags.AsCollection:
                resolvedFlags     |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedWrittenAs =  AsComplex | WrittenAsFlags.AsCollection;
                    resolvedFlags     |= formatFlags | ContentAllowComplexType;
                }
                break;
            case AsMapCollection:
                resolvedFlags     |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType;
                resolvedWrittenAs = visitResult.IsARevisit
                    ? AsSimple | AsMapCollection
                    : AsComplex | AsMapCollection;
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return (resolvedWrittenAs, resolvedFlags);
    }

    public virtual ContentSeparatorRanges StartSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (openAs.SupportsMultipleFields()) { return StartComplexTypeOpening(instanceToOpen, mws, openAs); }
        var sb = mws.Sb;

        var buildingType         = instanceToOpen is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceToOpen is Type asType 
                ? asType 
                : (instanceToOpen?.GetType() ?? typeof(T)));
        var buildingTypeFullName = buildingType.FullName ?? "";
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag())
        {
            var showTypeName = false;

            showTypeName |= (openAs.HasAnyOf(AsContent | AsObject)
                          && !(StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildingTypeFullName.StartsWith(s))));

            if (!showTypeName)
            {
                var elementType          = buildingType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? buildingType;
                var elementTypeFullName = elementType.FullName ?? "";
                showTypeName |= (openAs.HasAsCollectionFlag() 
                               && !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => buildingTypeFullName.StartsWith(s))
                                && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => elementTypeFullName.StartsWith(s)))
                              || (mws.MoldGraphVisit.IsARevisit && mws is ICollectionMoldWriteState { IsSimple: true }));
            }

            if (showTypeName)
            {
                sb.Append(RndBrktOpn);
                buildingType.AppendShortNameInCSharpFormat(sb);
                mws.WroteOuterTypeName = true;
            }
        }
        return Gb.ContentEndToRanges(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (openAs.SupportsMultipleFields()) { return FinishComplexTypeOpening(instanceToOpen, mws, openAs); }
        if (mws is { WroteOuterTypeName: true })
        {
            Gb.AppendContent(RndBrktCls);
            if (!mws.SkipBody)
            {
                // space considered content
                Gb.AppendContent(Spc);
            }
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendSimpleTypeClosing<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openAs)
    {
        if (openAs.SupportsMultipleFields()) { return AppendComplexTypeClosing(instanceToOpen, mdc, openAs); }
        if (Gb.HasCommitContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }

        var sb = mdc.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (mdc.CreateMoldFormatFlags.HasSuppressClosing())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
            return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
        }
        Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(mdc.Sb, DefaultCallerTypeFlags);
        return Gb.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public virtual ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;


        var buildingType         = instanceToOpen is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceToOpen?.GetType() ?? typeof(T));
        var buildTypeFullName = buildingType.FullName ?? "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);

        var mergedFlags = formatFlags | mdc.CreateMoldFormatFlags;

        if (mergedFlags.HasSuppressOpening()) return Gb.Complete(formatFlags);
        if (mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
             (formatFlags.HasAddTypeNameFieldFlag() ||
              !StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildTypeFullName.StartsWith(s)))))
        {
            var isSimpleOrContentType = mdc.CurrentWriteMethod.HasAnyOf(AsContent | AsSimple | WrittenAsFlags.AsCollection) 
                                     || mdc.CurrentWriteMethod.HasAllOf(AsRaw | AsObject);
            if (isSimpleOrContentType) { Gb.AppendContent(RndBrktOpn); }
            buildingType.AppendShortNameInCSharpFormat(sb);
            if (isSimpleOrContentType) { Gb.AppendContent(RndBrktCls); }
            sb.Append(Spc);
            mdc.WroteOuterTypeName = true;
        }
        return Gb
               .AppendContent(BrcOpn)
               .AppendPadding(Spc)
               .Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openAs)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }

        var sb = mdc.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (mdc.CreateMoldFormatFlags.HasSuppressClosing()) { Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true); }
        else
        {
            var lastContentChar = Gb.RemoveLastSeparatorAndPadding();
            if (lastContentChar != BrcOpnChar) { Gb.StartAppendContent(Spc, sb, this, DefaultCallerTypeFlags).AppendContent(BrcCls); }
            else { Gb.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
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

    public WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var writeFlags = AsNull;
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
        {
            var contentStart           = sb.Length;
            var formatStringBufferSize = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);

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

    public virtual ContentSeparatorRanges StartKeyedCollectionOpen(IMoldWriteState mws
      , Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;

        var keyedCollectionType = mws.TypeBeingBuilt;

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var kvpTypes = keyedCollectionType.GetKeyedCollectionTypes();
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag()
         && !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => keyedCollectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Key.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Value.FullName?.StartsWith(s) ?? false)))
        {
            keyedCollectionType.AppendShortNameInCSharpFormat(sb);
            mws.WroteOuterTypeName = true;
            Gb.MarkContentEnd(sb.Length);
            return Gb.Complete(formatFlags);
        }
        Gb.AppendContent(BrcOpn);
        return AddNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishKeyedCollectionOpen(IMoldWriteState mws)
    {
        if (mws.WroteOuterTypeName)
        {
            Gb.AppendContent(Spc);
            Gb.AppendContent(BrcOpn);
            return AddNextFieldPadding(mws.CreateMoldFormatFlags);
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendKeyedCollectionClose(IMoldWriteState mws
      , Type keyType, Type valueType, int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;

        if (formatFlags.HasSuppressClosing()) { return ContentSeparatorRanges.None; }

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (lastContentChar != BrcOpnChar) { AddNextFieldPadding(formatFlags); }

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.AppendContent(BrcCls);
        var range = Gb.Complete(formatFlags);

        Gb.AddHighWaterMark();
        return range;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue>(
        IMoldWriteState<TMold> mws
      , Type keyedCollectionType
      , TKey key
      , TValue value
      , int retrieveCount
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        mws.FieldEnd();
        mws.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        mws.FieldEnd();

        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName);
        mws.FieldEnd();
        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        var sb = mws.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, mws.Master); }
        mws.FieldEnd();
        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        var sb = mws.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, mws.Master); }
        mws.FieldEnd();
        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        var sb = mws.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, mws.Master); }
        mws.FieldEnd();
        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        var sb = mws.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, mws.Master); }
        mws.FieldEnd();
        if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
        else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        return mws;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags) => sourceLength;

    public virtual (int, int) InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex
      , WrittenAsFlags writtenAs, int indexToInsertAt, FormatFlags createTypeFlags, int contentLength = -1
      , IMoldWriteState? liveMoldInternal = null)
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

        var alreadySupportsMultipleFields = writtenAs.SupportsMultipleFields();

        var deltaIndent = 0;

        var needsBracesWrap  = false;
        var prefixInsertSize = 0;
        var suffixInsertSize = 0;
        var actualLength     = contentLength < 0 ? sb.Length - typeOpenIndex : contentLength;

        // first entry spot maybe removed if empty so backtrack to open add one;
        var firstFieldPad    = SizeNextFieldPadding(createTypeFlags);
        var isEmpty          = contentLength >= 0 && indexToInsertAt - firstFieldPad + 1 == typeOpenIndex + contentLength;
        var fronInsertLength = actualLength - (indexToInsertAt - typeOpenIndex);
        int contentNewLines  = sb.SubSequenceOccurrenceCount(indexToInsertAt, fronInsertLength, StyleOptions.NewLineStyle);
        if (!alreadySupportsMultipleFields)
        {
            // needsBracesWrap = writtenAs.HasAsRawFlag() && writtenAs.HasNoneOf(AsContent);
            needsBracesWrap = writtenAs.HasAsRawFlag() && writtenAs.HasNoneOf(AsObject);
            var actualTypeFullName = actualType.FullName ?? "";
            if (writtenAs.HasAllOf(AsSimple | AsContent))
            {
                if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
                 || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName.StartsWith(s))) { needsBracesWrap = true; }
            }
            if (writtenAs.HasAllOf(AsSimple | WrittenAsFlags.AsCollection))
            {
                if (liveMoldInternal is { WroteOuterTypeName: false } ||
                    (StyleOptions.LogSuppressDisplayCollectionNames.Any(s => actualTypeFullName.StartsWith(s))
                  && StyleOptions.LogSuppressDisplayCollectionNames.Any(s => actualTypeFullName.StartsWith(s)))) { needsBracesWrap = true; }
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
        if (liveMoldInternal != null) { liveMoldInternal.CurrentWriteMethod |= WithInstanceId; }
        if (!alreadySupportsMultipleFields)
        {
            if (needsBracesWrap)
            {
                AddToNextFieldSeparatorAndPadding(createTypeFlags);
                AppendInstanceValuesFieldName(typeof(object), createTypeFlags);
                if (contentNewLines > 0)
                {
                    deltaIndent++;
                    Span<char> indent = stackalloc char[StyleOptions.IndentSize];
                    indent.OverWriteRepatAt(0, StyleOptions.IndentChar, StyleOptions.IndentSize);
                    fronInsertLength = sb.IndentSubsequentLines(StyleOptions.NewLineStyle, indent
                                                              , indexToInsertAt + prefixInsertSize, fronInsertLength);
                }
                if (contentLength >= 0)
                {
                    Gb.IndentLevel--;
                    suffixInsertSize += SizeNextFieldPadding(createTypeFlags);
                    suffixInsertSize += 1; // close Brace

                    insertBuilder.StartInsertAt(indexToInsertAt + prefixInsertSize + fronInsertLength, suffixInsertSize);
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
        return (sb.Length - preAppendLength, deltaIndent);
    }

    public int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var preAppendLength = Gb.Sb.Length;
        Gb.AppendContent("$values");
        AppendFieldValueSeparator();
        return Gb.Sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(IMoldWriteState mws, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        var sb = mws.Sb;

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
                var actualTypeFullName = mws.TypeBeingBuilt.FullName;
                if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
                 || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)) { needsBracesWrap = true; }
            }
            if (needsBracesWrap)
            {
                mws.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
                StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
                FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
            }
            else { Gb.AppendContent(RndBrktOpn); }
        }
        AppendFieldName(mws, "$ref");
        AppendFieldValueSeparator();
        FormatFieldContents(mws, refId, "", createTypeFlags);
        mws.CurrentWriteMethod |= WithReferenceToInstanceId;

        if (!alreadySupportsMultipleFields && !needsBracesWrap)
        {
            Gb.AppendContent(RndBrktCls);
            Gb.Complete(createTypeFlags);
        }
        else
        {
            mws.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
        }
        return sb.Length - preAppendLength;
    }

    public virtual int AppendInstanceInfoField(IMoldWriteState mws, string fieldName, ReadOnlySpan<char> description
      , WrittenAsFlags writeMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck()) return 0; // fieldNames are marked with this and
        var sb = mws.Sb;

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
        AppendFieldName(mws, fieldName);
        AppendFieldValueSeparator();
        FormatFieldContents(mws, description, 0, "\"{0}\"", formatFlags: createTypeFlags);

        if (!alreadySupportsMultipleFields) { Gb.StartAppendContentAndComplete(RndBrktCls, sb, DefaultCallerTypeFlags); }
        else
        {
            mws.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
        }
        return sb.Length - preAppendLength;
    }

    public virtual WrittenAsFlags AppendFieldName(IMoldWriteState mws, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartAppendContent(fieldName, sb, this, DefaultCallerTypeFlags);
        return AsValue;
    }

    public virtual WrittenAsFlags FormatFieldNameMatch<TAny>(IMoldWriteState mws, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, source);
        else
            sb.Append(source);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(source, sb, formatString ?? "");
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        if (source != null)
            Format(source, sb, formatString ?? "");
        else
            base.Format(StyleOptions.NullString, 0, sb, formatString);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName<TFmt>(IMoldWriteState mws, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        return FormatFieldContents(mws, source, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldName<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        return FormatFieldContents(mws, source, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary FormatFieldName<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TCloaked), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }

        var appendSummary = valueRevealer(value, mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, value.GetType());
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
         || !callerFormatFlags.HasAsStringContentFlag()
         || sb[contentStart] == DblQtChar)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldName<TBearer>(IMoldWriteState mws, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, styledObj.GetType());
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
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
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

    public virtual WrittenAsFlags FormatFieldContentsMatch<TAny>(IMoldWriteState mws, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;

        if (formatFlags.ShouldDelimit() && !formatString.IsDblQtBounded()) { Gb.AppendDelimiter(DblQt); }
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && !formatString.IsDblQtBounded()) { Gb.AppendDelimiter(DblQt); }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFieldContents<TFmt>(IMoldWriteState mws, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFieldContents<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mws.Sb;
        formatString ??= "";

        if (source == null) { return AppendFormattedNull(sb, formatString, formatFlags); }

        return FormatFieldContents(mws, source.Value, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFallbackFieldContents<TAny>(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        formatString ??= "";

        formatFlags = ResolveContentFormatFlags<TAny>(sb, default!, formatFlags, formatString);

        return FormatFieldContents(mws, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
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

    public virtual AppendSummary FormatFieldContents<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TCloaked), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }

        var appendSummary = valueRevealer(value, mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, value.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || appendSummary.WrittenAs.HasAsStringFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
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
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldContents<TBearer>(IMoldWriteState mws, TBearer styledObj,
        string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = mws.Sb;

        var withMoldInherited = callerFormatFlags | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, styledObj.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
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
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType, bool? hasItems
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteInnerTypeOpen || hasItems != true) { return ContentSeparatorRanges.None; }
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        mws.WroteInnerTypeOpen = true;
        if (mws.SkipBody) return Gb.Complete(formatFlags);
        if (mws is ICollectionMoldWriteState { IsSimple: true } scmdc)
        {
            if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags); }
            else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
            {
                if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags); }
                else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
                {
                    var reg = mws.Master.ActiveGraphRegistry;

                    GraphNodeVisit derivedMold = reg[mws.MoldGraphVisit.VisitId.VisitIndex];

                    var checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;

                    GraphNodeVisit checkMold = reg[checkMoldIndex];

                    do
                    {
                        derivedMold    = checkMold;
                        checkMoldIndex = reg[checkMoldIndex].ParentVisitId.VisitIndex;
                        checkMold      = reg[Math.Max(0, checkMoldIndex)];
                    } while (checkMoldIndex >= 0
                          && (checkMold.MoldState?.MoldGraphVisit.IsBaseOfInitial ?? false)
                          && ReferenceEquals(checkMold.VisitedInstance, derivedMold.VisitedInstance));
                    var initialDc = derivedMold.MoldState;

                    if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
                    {
                        AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags);
                    }
                }
            }
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)formatFlags);
        return AddCollectionElementPadding(mws, itemElementType, 1, formatFlags);
    }

    public override int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        var currFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, currFmtFlags);
        Gb.AppendContent(SqBrktOpn);
        return Gb.Complete(currFmtFlags).Length;
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        Gb.ResetCurrent((FormatFlags)formatSwitches);
        Gb.MarkContentStart(destStartIndex);
        var charsAdded = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        return Gb.MarkContentEnd(destStartIndex + charsAdded).Complete((FormatFlags)formatSwitches).Length;
    }

    public virtual int AppendCloseCollection(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb          = mws.Sb;
        var preAppendAt = sb.Length;
        if (mws.SkipBody || mws.WroteInnerTypeClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeClose = true;
        if (!(totalItemCount > 0))
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
            if (totalItemCount.HasValue || StyleOptions.NullWritesEmpty)
            {
                if (!mws.WroteInnerTypeOpen) CollectionStart(itemElementType, sb, true, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }

            Gb.MarkContentEnd();
            return sb.Length - preAppendAt;
        }
        CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.Length - preAppendAt;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (previousContentPadSpacing.PreviousFormatFlags.HasSuppressClosing())
        {
            Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
            return 0;
        }
        var lastChar = Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(sb, (FormatFlags)formatSwitches, true);
        if (lastChar != SqBrktOpnChar) { Gb.AppendContent(Spc); }
        return Gb.AppendContent(SqBrktCls).Complete((FormatFlags)formatSwitches).Length;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var lastChar = Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        Gb.ResetCurrent((FormatFlags)formatSwitches, true);
        Gb.MarkContentStart(destIndex);

        if (lastChar != SqBrktOpnChar) destIndex += destSpan.OverWriteAt(destIndex, Spc);
        destIndex += destSpan.OverWriteAt(destIndex, SqBrktCls);
        return Gb.MarkContentEnd(destIndex).Complete((FormatFlags)formatSwitches).Length;
    }

    public WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public AppendSummary CollectionNextItemFormat<TFmt>(IMoldWriteState mws, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = item?.GetType() ?? typeof(TFmt);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        FormatFieldContents(mws, item, formatString ?? "", formatFlags);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(IMoldWriteState mws, TFmtStruct? item, int retrieveCount
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mws.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        FormatFieldContents(mws, item.Value, formatString ?? "", formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(IMoldWriteState mws
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler, string? callerFormatString
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var actualType = item?.GetType() ?? typeof(TCloaked);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(callerFormatFlags);
        var contentStart       = sb.Length;
        var stateExtractResult = styler(item, mws.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, string? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        var contentStart = sb.Length;
        sb.AppendFormat(this, formatString ?? "", item);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, char[]? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        var contentStart = sb.Length;
        sb.AppendFormat(this, formatString ?? "", item);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextCharSeqFormat<TCharSeq>(IMoldWriteState mws, TCharSeq item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = item?.GetType() ?? typeof(TCharSeq);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(item, 0, sb, formatString ?? "", item.Length);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(item, 0, sb, formatString ?? "", item.Length);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextStringBearerFormat<TBearer>(IMoldWriteState mws, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = item?.GetType() ?? typeof(TBearer);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString, callerFormatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = item.RevealState(mws.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual Range? AddCollectionElementSeparator(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        Gb.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(formatFlags);
    }

    public ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(mws, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(mws, elementType, nextItemNumber, formatFlags);
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
