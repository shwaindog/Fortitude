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

    public virtual bool IsCompact => true;
    public virtual bool IsPretty => false;

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

    public virtual (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        actualType = actualType.IfRecyclableContainerGetType();
        
        var resolvedFlags     = formatFlags;
        var resolvedWrittenAs = proposedWriteType;

        var settings = tos.Settings;

        var shouldDisplayTypeName = visitResult.IsARevisit;
        if (!shouldDisplayTypeName)
        {
            shouldDisplayTypeName = StyleOptions.ShouldDisplayTypeName(actualType);
        }
        var shouldShowTypeNameDecision = shouldDisplayTypeName ? AddTypeNameField : LogSuppressTypeNames;

        switch (proposedWriteType & WrittenAsFlagsExtensions.ProposedMaskSelectionMask)
        {
            case AsObject | AsRaw:
            // resolvedFlags     |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
            // resolvedWrittenAs =  (resolvedWrittenAs & ~AsComplex) | AsSimple | AsRaw | AsObject;
            // if (visitResult.IsARevisit)
            // {
            //     resolvedWrittenAs |= settings.InstanceMarkingIncludeObjectToStringContents ? ShowSuppressedContents : Empty;
            //     resolvedFlags     |= settings.InstanceMarkingIncludeObjectToStringContents ? AddTypeNameField : DefaultCallerTypeFlags;
            // }
            // break;
            case AsRaw:
            case AsCollectionItem:
            case AsCollectionItem | WrittenAsFlags.AsCollection | AsSimple:
            case AsCollectionItem | WrittenAsFlags.AsCollection | AsComplex:
            case AsCollectionItem | WrittenAsFlags.AsCollection | AsRaw:
            case WrittenAsFlags.AsCollection | AsRaw:
            case WrittenAsFlags.AsCollection | AsSimple:
            case WrittenAsFlags.AsCollection | AsComplex:
            case AsContent | AsRaw:
            case AsContent | AsSimple:
            case AsContent | AsComplex:
            case AsContent | AsCollectionItem | AsRaw:
            case AsContent | AsCollectionItem | AsSimple:
            case AsContent | AsCollectionItem | AsComplex:
                resolvedFlags = formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedFlags     &= ~(SuppressOpening | SuppressClosing | LogSuppressTypeNames);
                    resolvedFlags     |= AddTypeNameField;
                    resolvedWrittenAs |= WithReferenceToInstanceId;
                }
                var isPalantirRevealer = actualType.IsConcreteOfGeneric(typeof(PalantírReveal<>));
                if (actualType.IsStringBearerOrNullableCached() || isPalantirRevealer)
                {
                    resolvedFlags |= shouldShowTypeNameDecision;
                    if (visitResult is { IsARevisit: false }
                     && (proposedWriteType.HasAllOf(AsSimple | WrittenAsFlags.AsCollection)
                      || proposedWriteType.HasAllOf(AsComplex | WrittenAsFlags.AsCollection)
                      || proposedWriteType.HasAllOf(AsRaw | WrittenAsFlags.AsCollection)))
                    {
                        if (!actualType.IsValueType && visitResult.ReusedCount <= 0) { resolvedFlags &= ~(SuppressOpening | SuppressClosing); }
                        resolvedWrittenAs |= AsContent;
                        break;
                    }
                    if ((proposedWriteType.HasAllOf(AsCollectionItem))
                     && proposedWriteType.HasNoneOf(AsContent))
                    {
                        resolvedWrittenAs |= AsContent;
                        if (actualType.IsValueType) { break; }
                        if (visitResult is { IsARevisit: false, ReusedCount: <= 0 })
                        {
                            resolvedFlags     |= SuppressOpening | SuppressClosing;
                            resolvedWrittenAs |= ShowSuppressedContents;
                        }
                        else { resolvedFlags &= ~(SuppressOpening | SuppressClosing); }
                        resolvedFlags     |= ContentAllowComplexType;
                        resolvedWrittenAs &= ~(AsSimple);
                        resolvedWrittenAs |= AsComplex;
                        break;
                    }
                    if (visitResult is { ReusedCount: <= 0 }
                        // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                     && (proposedWriteType.HasAllOf(AsComplex | AsContent | AsOuterType))
                     || (proposedWriteType.HasAllOf(AsComplex | AsCollectionItem)))
                    {
                        resolvedFlags     |= ContentAllowComplexType;
                        resolvedWrittenAs &= ~(AsContent);
                        resolvedWrittenAs |= AsComplex | AsObject;
                        break;
                    }
                    if (visitResult is { ReusedCount: <= 0 }
                        // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                     && !actualType.IsValueType
                     && (proposedWriteType.HasAllOf(AsSimple | AsContent | AsOuterType)
                      || (proposedWriteType.HasAllOf(AsComplex | AsCollectionItem))))
                    {
                        resolvedFlags     |= SuppressOpening | SuppressClosing;
                        resolvedWrittenAs &= ~(AsComplex);
                        resolvedWrittenAs |= AsContent | AsSimple;
                        resolvedWrittenAs |= visitResult.IsARevisit ? Empty : ShowSuppressedContents;
                        break;
                    }
                    if (visitResult is { ReusedCount: <= 0 }
                        // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                     && proposedWriteType.HasAllOf(AsContent | AsInnerType))
                    {
                        if (!actualType.IsValueType && !isPalantirRevealer)
                        {
                            resolvedFlags     |= SuppressOpening | SuppressClosing;
                            resolvedWrittenAs &= ~(AsComplex | AsContent);
                            resolvedWrittenAs |= AsSimple | AsObject;

                            resolvedWrittenAs |= visitResult.IsARevisit ? Empty : ShowSuppressedContents;
                        }
                        else
                        {
                            resolvedFlags |= ContentAllowComplexType;
                            if (proposedWriteType.HasNoneOf(AsCollectionItem))
                            {
                                resolvedWrittenAs &= ~(AsContent);
                                resolvedWrittenAs |= AsObject;
                            }
                        }
                        break;
                    }
                    // else 
                    // if (visitResult is { IsARevisit: true, ReusedCount: <= 0 } 
                    //  && proposedWriteType.HasAsRawFlag() 
                    //  && proposedWriteType.HasNoneOf(WrittenAsFlags.AsCollection))
                    // {
                    //     resolvedFlags     &= ~AddTypeNameField;
                    //     resolvedFlags     |= SuppressOpening | SuppressClosing | LogSuppressTypeNames | ContentAllowComplexType;
                    //     resolvedWrittenAs &= ~(AsContent | AsSimple);
                    //     resolvedWrittenAs |= ShowSuppressedContents;
                    //     resolvedWrittenAs |= AsComplex | AsObject;
                    // }
                    if (visitResult.ReusedCount <= 0
                        // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                     && !actualType.IsValueType
                     && (proposedWriteType.HasAsRawFlag())
                     && !proposedWriteType.HasAsCollectionFlag())
                    {
                        resolvedFlags |= SuppressOpening | SuppressClosing | ContentAllowComplexType;
                        // resolvedFlags     |= LogSuppressTypeNames;
                        resolvedWrittenAs |= ShowSuppressedContents;
                        resolvedWrittenAs &= ~(AsContent);
                        resolvedWrittenAs |= AsComplex | AsObject;
                        break;
                    }
                }
                if (actualType.IsSpanFormattableOrNullableCached() || actualType.IsBoolOrNullable())
                {
                    resolvedFlags |= shouldShowTypeNameDecision;
                    if (visitResult.IsARevisit)
                    {
                        // resolvedFlags     |= AddTypeNameField;
                        if (settings.InstanceMarkingIncludeSpanFormattableContents) { resolvedWrittenAs |= ShowSuppressedContents; }
                        // else
                        // {
                        //     resolvedFlags |= SuppressClosing;
                        // }
                    }
                    else
                    {
                        if (visitResult.ReusedCount <= 0
                            // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                         && (formatFlags.HasContentTreatmentFlags()
                          && (!proposedWriteType.HasAllOf(AsContent | AsInnerType)
                           || proposedWriteType.HasAllOf(AsRaw | AsContent))))
                        {
                            resolvedFlags     |= SuppressOpening | SuppressClosing;
                            resolvedWrittenAs |= ShowSuppressedContents | AsContent;
                        }
                    }
                    // else
                    // {
                    //     var actualTypeFullName        = actualType.FullName;
                    //     var shouldShowRevisitDecision = visitResult.IsARevisit ? WithReferenceToInstanceId : Empty;
                    //     resolvedWrittenAs = shouldShowRevisitDecision;
                    //     var shouldSuppressTypeNameDecision =
                    //         !visitResult.IsARevisit 
                    //      && StyleOptions
                    //         .LogSuppressDisplayTypeNames
                    //         .Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                    //             ? LogSuppressTypeNames
                    //             : DefaultCallerTypeFlags;
                    //     var shouldSuppressOpenCloseDecision = visitResult.IsARevisit ? DefaultCallerTypeFlags : SuppressOpening | SuppressClosing;
                    //     resolvedFlags |= shouldSuppressOpenCloseDecision | shouldSuppressTypeNameDecision;
                    // }
                    break;
                }
                if (actualType.IsAnyTypeHoldingCharsCached())
                {
                    resolvedFlags |= shouldShowTypeNameDecision;
                    if (resolvedWrittenAs.HasAnyOf(AsRaw | AsContent) && visitResult.IsARevisit)
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
                          && settings.InstanceMarkingIncludeCharArrayContents)) { resolvedWrittenAs |= ShowSuppressedContents; }
                        // else { resolvedFlags |= SuppressClosing; }
                    }
                    else
                    {
                        // if(proposedWriteType.HasAsSimpleFlag())
                        // {
                        //     resolvedFlags |= SuppressOpening | SuppressClosing;
                        // }
                        if (visitResult.ReusedCount <= 0
                            // if (visitResult is { IsARevisit: false, ReusedCount: <= 0 } 
                         && (formatFlags.HasContentTreatmentFlags()
                          && (proposedWriteType.HasAllOf(AsContent | AsInnerType)
                           || proposedWriteType.HasAllOf(AsRaw | AsContent))))
                        {
                            resolvedFlags     |= SuppressOpening | SuppressClosing;
                            resolvedWrittenAs |= ShowSuppressedContents | AsContent;
                        }
                    }
                    // else
                    // {
                    //     var actualTypeFullName = actualType.FullName;
                    //     var shouldSuppressTypeNameDecision =
                    //         !visitResult.IsARevisit 
                    //      && StyleOptions
                    //         .LogSuppressDisplayTypeNames
                    //         .Any(s => actualTypeFullName?.StartsWith(s) ?? false)
                    //             ? LogSuppressTypeNames
                    //             : DefaultCallerTypeFlags;
                    //
                    //     var suppressOpenCloseDecision = 
                    //         // proposedWriteType.HasAnyOf(AsContent)
                    //         // ? SuppressOpening | SuppressClosing
                    //         // : 
                    //         DefaultCallerTypeFlags;
                    //     
                    //     resolvedFlags |= (resolvedFlags | suppressOpenCloseDecision | shouldSuppressTypeNameDecision);
                    // }
                    // else
                    // {
                    //     resolvedFlags     |= AddTypeNameField;
                    // }
                    break;
                }
                var isBuildInputType = actualType.IsInputConstructionTypeCached();
                if (!isBuildInputType)
                {
                    resolvedFlags     |= shouldShowTypeNameDecision;
                    resolvedWrittenAs |= (settings.InstanceMarkingIncludeObjectToStringContents ? ShowSuppressedContents : Empty) | AsObject;
                    // resolvedFlags     |= visitResult.IsARevisit ? AddTypeNameField : DefaultCallerTypeFlags;
                }
                break;
            // case AsComplex | AsObject:
            //     if (actualType.IsSpanFormattableOrNullableCached())
            //     {
            //         var actualTypeFullName = actualType.FullName;
            //         var shouldSuppressTypeNameDecision =
            //             StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)
            //                 ? LogSuppressTypeNames
            //                 : DefaultCallerTypeFlags;
            //         resolvedFlags |= shouldSuppressTypeNameDecision;
            //     }
            //     break;
            // case AsRaw | WrittenAsFlags.AsCollection:
            //     resolvedFlags |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
            //     if (visitResult.IsARevisit)
            //     {
            //         resolvedWrittenAs =  AsRaw | AsComplex | WrittenAsFlags.AsCollection;
            //         resolvedFlags     |= ContentAllowComplexType;
            //     }
            //     else { resolvedWrittenAs = AsRaw | AsSimple | WrittenAsFlags.AsCollection; }
            //     break;
            // case AsComplex | WrittenAsFlags.AsCollection:
            //     resolvedFlags |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType |
            //                      ContentAllowComplexType;
            //     break;
            // case AsSimple | WrittenAsFlags.AsCollection:
            //     resolvedFlags |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType;
            //     if (visitResult.IsARevisit)
            //     {
            //         if (actualType.IsInputConstructionTypeCached()) { resolvedWrittenAs = AsSimple | WrittenAsFlags.AsCollection; }
            //         else
            //         {
            //             resolvedWrittenAs = AsSimple | WrittenAsFlags.AsCollection;
            //             // resolvedWrittenAs =  AsComplex | WrittenAsFlags.AsCollection;
            //             // resolvedFlags     |= formatFlags | ContentAllowComplexType;
            //         }
            //     }
            //     break;
            case AsMapCollection:
                resolvedFlags |= formatFlags | ContentAllowNull | ContentAllowNumber | ContentAllowText | ContentAllowAnyValueType |
                                 ContentAllowComplexType;
                resolvedWrittenAs = visitResult.IsARevisit
                    ? AsSimple | AsMapCollection
                    : AsComplex | AsMapCollection;
                break;
            case AsComplex | AsCollectionItem:
            case AsComplex | AsRaw | AsCollectionItem:
            case AsComplex | AsRaw | AsContent:
            case AsComplex | AsObject:
            case AsComplex | AsRaw:
            case AsComplex:
                resolvedFlags |= ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType;
                // if (proposedWriteType.HasAsCollectionItemFlag())
                // {
                //     resolvedWrittenAs |= AsContent;
                // }
                if (visitResult.IsARevisit)
                {
                    // if (visitResult.ReusedCount < 0)
                    // {
                    //     if (formatFlags.HasAsStringContentFlag())
                    //     {
                    //         resolvedFlags |= SuppressOpening | SuppressClosing | LogSuppressTypeNames;
                    //     }
                    //     else
                    //     {
                    //         resolvedFlags |= SuppressOpening | SuppressClosing | AddTypeNameField;
                    //     }
                    // }
                    resolvedFlags     &= (SuppressOpening | SuppressClosing);
                    resolvedWrittenAs |= WithReferenceToInstanceId;
                    break;
                }
                // if (actualType.IsStringBearerOrNullableCached())
                // {
                //     // if (formatFlags.HasContentTreatmentFlags()
                //     if (visitResult.IsARevisit)
                //     {
                //         resolvedWrittenAs |= (resolvedWrittenAs & ~AsComplex) | AsSimple | WithReferenceToInstanceId;
                //     }
                //     break;
                // }
                resolvedWrittenAs |= AsComplex | AsObject;
                resolvedFlags     |= ContentAllowComplexType | shouldShowTypeNameDecision;
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return (resolvedWrittenAs, resolvedFlags);
    }

    public virtual ContentSeparatorRanges StartSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteTypeOpen) return ContentSeparatorRanges.None;
        if (openAs.SupportsMultipleFields()) { return StartComplexTypeOpening(instanceToOpen, mws, openAs); }
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        var sb          = mws.Sb;
        var mergedFlags = formatFlags | mws.CreateMoldFormatFlags;

        var buildingType = instanceToOpen is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceToOpen as Type ?? (instanceToOpen?.GetType() ?? typeof(T)));
        Gb.StartNextContentSeparatorPaddingSequence(sb, mergedFlags);
        if ((!mws.WroteTypeName && mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag()) &&
            (!mergedFlags.HasSuppressOpening() || mergedFlags.HasAddTypeNameFieldFlag()))
        {
            var showTypeName = mergedFlags.HasAddTypeNameFieldFlag() || StyleOptions.ShouldDisplayTypeName(buildingType);

            if (showTypeName)
            {
                sb.Append(RndBrktOpn);
                buildingType.AppendShortNameInCSharpFormat(sb);
                mws.StartedTypeName = true;
            }
        }
        return Gb.ContentEndToRanges(mergedFlags);
    }

    public virtual ContentSeparatorRanges FinishSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteTypeOpen) return ContentSeparatorRanges.None;
        if (openAs.SupportsMultipleFields()) { return FinishComplexTypeOpening(instanceToOpen, mws, openAs); }
        var result      = ContentSeparatorRanges.None;
        var mergedFlags = formatFlags | mws.CreateMoldFormatFlags;
        if (mws is { StartedTypeName: true })
        {
            Gb.StartNextContentSeparatorPaddingSequence(mws.Sb, mergedFlags);
            Gb.AppendContent(RndBrktCls);
            if (!mws.SkipBody || openAs.HasAnyOf(ShowSuppressedContents))
            {
                // space considered content
                Gb.AppendPadding(Spc);
            }
            mws.WroteTypeName   = true;
            mws.StartedTypeName = false;
            result              = Gb.Complete(formatFlags);
        }
        if (!mergedFlags.HasSuppressOpening()) { mws.WroteTypeOpen = true; }
        return result;
    }

    public virtual ContentSeparatorRanges AppendSimpleTypeClosing<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (openAs.SupportsMultipleFields()) { return AppendComplexTypeClosing(instanceToOpen, mdc, openAs, formatFlags); }
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

    public virtual ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        if (mws.WroteTypeOpen) return ContentSeparatorRanges.None;
        var sb = mws.Sb;

        var buildingType = instanceToOpen is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceToOpen?.GetType() ?? typeof(T));

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);

        var mergedFlags = formatFlags | mws.CreateMoldFormatFlags;

        if (!mws.WroteTypeName && mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            (!mergedFlags.HasSuppressOpening() || mergedFlags.HasAddTypeNameFieldFlag()))
        {
            var showTypeName = mergedFlags.HasAddTypeNameFieldFlag() || StyleOptions.ShouldDisplayTypeName(buildingType);

            if (showTypeName)
            {
                var isSimpleOrContentType = mws.CreateWriteMethod.HasAnyOf(AsContent | WrittenAsFlags.AsCollection | AsCollectionItem);
                if (isSimpleOrContentType) { Gb.AppendContent(RndBrktOpn); }
                buildingType.AppendShortNameInCSharpFormat(sb);
                mws.StartedTypeName = true;
            }
        }
        return Gb.Complete(mergedFlags);
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteTypeOpen) return ContentSeparatorRanges.None;
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        var mergedFlags = formatFlags | mws.CreateMoldFormatFlags;

        var result = ContentSeparatorRanges.None;
        Gb.StartNextContentSeparatorPaddingSequence(mws.Sb, mergedFlags);
        if (mws is { StartedTypeName: true, WroteTypeName: false })
        {
            var isSimpleOrContentType = mws.CreateWriteMethod.HasAnyOf(AsContent | WrittenAsFlags.AsCollection | AsCollectionItem);
            if (isSimpleOrContentType) { Gb.AppendContent(RndBrktCls); }

            mws.WroteTypeName   = true;
            mws.StartedTypeName = false;
            if (mws.SkipBody && openAs.HasNoneOf(ShowSuppressedContents)) return Gb.Complete(formatFlags);
            // space considered content
            result = Gb.AppendPadding(Spc).Complete(mergedFlags);
            Gb.StartNextContentSeparatorPaddingSequence(mws.Sb, mergedFlags);
        }
        if ((!mws.SkipBody || openAs.HasShowSuppressedContents()) && !mergedFlags.HasSuppressOpening())
        {
            mws.WroteTypeOpen = true;
            Gb.AppendContent(BrcOpn);
            result = AddNextFieldPadding(mergedFlags);
        }
        return result;
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteTypeClose) return ContentSeparatorRanges.None;
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }

        var sb = mws.Sb;

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);

        if (mws.SkipBody || mws.CreateMoldFormatFlags.HasSuppressClosing()) { Gb.Complete(mws.CreateMoldFormatFlags); }
        else
        {
            mws.WroteTypeClose = true;
            if (lastContentChar != BrcOpnChar) { Gb.StartAppendContent(Spc, sb, this, DefaultCallerTypeFlags).AppendContent(BrcCls); }
            else { Gb.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return Gb.Complete(mws.CreateMoldFormatFlags);
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
            mws.WroteTypeName = true;
            Gb.MarkContentEnd(sb.Length);
            return Gb.Complete(formatFlags);
        }
        Gb.AppendContent(BrcOpn);
        return AddNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishKeyedCollectionOpen(IMoldWriteState mws)
    {
        if (mws.WroteTypeName)
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

    public virtual InsertInfo InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex
      , WrittenAsFlags writtenAs, int indexToInsertAt, FormatFlags createTypeFlags, TypeMoldFlags moldWrittenFlags, int contentLength = -1
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

        var preInsertLength = sb.Length;

        var refDigitsCount = refId.NumOfDigits();

        var toRestore = Gb;
        Gb = insertBuilder;

        var alreadySupportsMultipleFields = writtenAs.SupportsMultipleFields();

        var deltaIndent         = 0;
        var eachNewLineIndentBy = 0;

        var needsBracesWrap  = false;
        var needsTypeName    = false;
        var needsBracketWrap = false;

        string? typeNameString = null;

        var prefixInsertSize = 0;
        var suffixInsertSize = 0;

        var prefixNewLines = 0;
        var suffixNewLines = 0;

        var actualLength = contentLength < 0 ? sb.Length - typeOpenIndex : contentLength;

        // first entry spot maybe removed if empty so backtrack to open add one;
        var firstFieldPad    = SizeNextFieldPadding(createTypeFlags);
        var isEmpty          = contentLength >= 0 && indexToInsertAt - firstFieldPad + 1 == typeOpenIndex + contentLength;
        var fronInsertLength = actualLength - (indexToInsertAt - typeOpenIndex);
        int contentNewLines  = sb.SubSequenceOccurrenceCount(indexToInsertAt, fronInsertLength, StyleOptions.NewLineStyle);
        // if (writtenAs.HasAnyOf(AsSimple | AsRaw) && !moldWrittenFlags.HasInnerSameAsOuterTypeFlag())
        // if (!moldWrittenFlags.HasInnerSameAsOuterTypeFlag())
        // {
        //     needsBracketWrap = true;
        // }
        if (!moldWrittenFlags.HasWroteTypeNameFlag()) { needsTypeName = true; }
        // if (!alreadySupportsMultipleFields && !needsBracketWrap)
        // {
        //     // needsBracesWrap = writtenAs.HasAsRawFlag() && writtenAs.HasNoneOf(AsContent);
        //     needsBracesWrap = writtenAs.HasAsRawFlag() && writtenAs.HasNoneOf(AsObject | WrittenAsFlags.AsCollection);
        //     var actualTypeFullName = actualType.FullName ?? "";
        //     if (writtenAs.HasAllOf(AsSimple | AsContent))
        //     {
        //         if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
        //          || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName.StartsWith(s))) { needsBracesWrap = true; }
        //     }
        //     if (needsBracesWrap)
        //     {
        //         if (liveMoldInternal != null) { liveMoldInternal.CurrentWriteMethod = writtenAs.ToMultiFieldEquivalent(); }
        //         prefixInsertSize += 1; // Open Brace Only close in done later
        //         Gb.IndentLevel++;
        //         prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
        //         if (IsPretty) prefixNewLines += 2;
        //         // insert $id added below
        //         prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
        //         prefixInsertSize += SizeFormatFieldName("$values".Length, createTypeFlags);
        //         prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        //     }
        //     else
        //     {
        //         needsBracketWrap =  true;
        //         prefixInsertSize += 2; // <name>(<id>) id brackets
        //         if (needsTypeName)
        //         {
        //             typeNameString   =  actualType.CachedCSharpNameNoConstraints();
        //             prefixInsertSize += typeNameString.Length;
        //             prefixInsertSize += 1; // (<name>) Space
        //             prefixInsertSize += 2; // (<name>) name brackets
        //             indexToInsertAt  =  typeOpenIndex;
        //         }
        //         isEmpty = true;
        //     }
        // }
        // else 
        if (isEmpty)
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
            alreadySupportsMultipleFields = true;

            needsBracketWrap =  true;
            prefixInsertSize += 2; // Open Brace Only close in done later
            if (needsTypeName)
            {
                typeNameString   =  actualType.CachedCSharpNameNoConstraints();
                prefixInsertSize += typeNameString.Length;
                prefixInsertSize += 1; // (<name>) Space
                prefixInsertSize += 2; // (<name>) name brackets
                indexToInsertAt  =  typeOpenIndex;
            }
            if (IsPretty) prefixNewLines++;
        }
        prefixInsertSize += SizeFormatFieldName("$id".Length, instanceInfoFormatFlags);
        prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        prefixInsertSize += SizeFormatFieldContents(refDigitsCount, instanceIdFormatFlags);

        insertBuilder.StartInsertAt(indexToInsertAt, prefixInsertSize);

        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        if (needsTypeName)
        {
            typeNameString ??= actualType.CachedCSharpNameNoConstraints();
            Gb.AppendContent(RndBrktOpn);
            Gb.AppendContent(typeNameString);
        }
        // if (!alreadySupportsMultipleFields)
        // {
        // if (needsBracesWrap)
        // {
        //     Gb.AppendContent(BrcOpn);
        //     AddNextFieldPadding(createTypeFlags);
        // }
        // else
        // {
        // Gb.AppendContent(RndBrktOpn);
        // }
        // }
        // else if (needsBracketWrap)
        // {
        Gb.AppendContent(RndBrktOpn);
        // }
        // else if (isEmpty) { AddNextFieldPadding(createTypeFlags); }

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
        // if (!alreadySupportsMultipleFields)
        // {
        //     if (needsBracesWrap)
        //     {
        //         AddToNextFieldSeparatorAndPadding(createTypeFlags);
        //         AppendInstanceValuesFieldName(typeof(object), writtenAs, createTypeFlags);
        //         if (contentNewLines > 0)
        //         {
        //             deltaIndent++;
        //             Span<char> indent = stackalloc char[StyleOptions.IndentSize];
        //             eachNewLineIndentBy = StyleOptions.IndentSize;
        //             indent.OverWriteRepatAt(0, StyleOptions.IndentChar, StyleOptions.IndentSize);
        //             fronInsertLength = sb.IndentSubsequentLines(StyleOptions.NewLineStyle, indent
        //                                                       , indexToInsertAt + prefixInsertSize, fronInsertLength);
        //         }
        //         if (contentLength >= 0)
        //         {
        //             Gb.IndentLevel--;
        //             suffixInsertSize += SizeNextFieldPadding(createTypeFlags);
        //             suffixInsertSize += 1; // close Brace
        //             if (IsPretty) suffixNewLines++;
        //
        //             insertBuilder.StartInsertAt(indexToInsertAt + prefixInsertSize + fronInsertLength, suffixInsertSize);
        //             AddNextFieldPadding(createTypeFlags);
        //             Gb.AppendContent(BrcCls);
        //         }
        //     }
        //     else
        //     {
        //         if (needsTypeName) Gb.AppendContent(RndBrktCls);
        //         Gb.AppendContent(RndBrktCls);
        //         if (needsTypeName) Gb.AppendContent(Spc);
        //     }
        // }
        // else if (needsBracketWrap)
        // {
        if (needsTypeName) Gb.AppendContent(RndBrktCls);
        Gb.AppendContent(RndBrktCls);
        if (needsTypeName) Gb.AppendContent(Spc);
        // }
        // else if (isEmpty)
        // {
        //     Gb.IndentLevel--;
        //     AddNextFieldPadding(createTypeFlags);
        // }
        // else { AddToNextFieldSeparatorAndPadding(createTypeFlags); }
        Gb = toRestore;
        var totalIncrease = sb.Length - preInsertLength;
        return new InsertInfo(prefixInsertSize, suffixInsertSize, prefixNewLines, suffixNewLines
                            , totalIncrease, deltaIndent, eachNewLineIndentBy);
    }

    public int AppendInstanceValuesFieldName(Type forType, WrittenAsFlags writtenAs, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (forType.IsInputConstructionTypeCached()) return 0;
        var preAppendLength = Gb.Sb.Length;
        Gb.AppendContent("$values");
        AppendFieldValueSeparator();
        return Gb.Sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(IMoldWriteState mws, int refId, Type forType, WrittenAsFlags currentWriteMethod
      , TypeMoldFlags moldWrittenFlags, FormatFlags createTypeFlags)
    {
        var sb = mws.Sb;

        var preAppendLength = sb.Length;
        // var needsBracesWrap         = false;
        // var needsBracketWrap        = false;
        // var needsDoubleBracketClose = false;
        var needsTypeName = false;

        // var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields();
        Gb.StartNextContentSeparatorPaddingSequence(sb, createTypeFlags);
        // if (currentWriteMethod.HasAnyOf(AsComplex) && moldWrittenFlags.HasWroteTypeNameFlag())
        // {
        //     sb.Length               -= 1;
        //     needsDoubleBracketClose =  true;
        // }
        // if (currentWriteMethod.HasAnyOf(AsSimple | AsRaw | AsContent) 
        //  && (!moldWrittenFlags.HasInnerSameAsOuterTypeFlag() || (currentWriteMethod.HasAsOuterTypeFlag())))
        // {
        //     needsBracketWrap = true;
        if (!moldWrittenFlags.HasWroteTypeNameFlag()
         && !moldWrittenFlags.HasStartedTypeNameFlag()
         && !moldWrittenFlags.HasInnerSameAsOuterTypeFlag()) { needsTypeName = true; }
        // }
        // if (!alreadySupportsMultipleFields && !needsBracketWrap)
        // {
        //     needsBracesWrap = currentWriteMethod.HasAsRawFlag() && currentWriteMethod.HasNoneOf(AsObject | WrittenAsFlags.AsCollection)
        //                    || (currentWriteMethod.HasAllOf(AsContent | AsSimple)
        //                     && createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing());
        //     if (currentWriteMethod.HasAllOf(AsContent | AsSimple))
        //     {
        //         var actualTypeFullName = mws.TypeBeingBuilt.FullName;
        //         if (createTypeFlags.HasSuppressOpening() && createTypeFlags.HasSuppressClosing()
        //          || StyleOptions.LogSuppressDisplayTypeNames.Any(s => actualTypeFullName?.StartsWith(s) ?? false)) { needsBracesWrap = true; }
        //     }
        //     if (needsBracesWrap)
        //     {
        //         mws.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
        //         StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
        //         FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
        //     }
        //     else
        //     {
        //         needsBracketWrap = true;
        //         if (!moldWrittenFlags.HasWroteTypeNameFlag() 
        //          && !moldWrittenFlags.HasStartedTypeNameFlag()
        //          && !moldWrittenFlags.HasInnerSameAsOuterTypeFlag()) { needsTypeName = true; }
        //     }
        // }
        // else if (!needsBracketWrap && !needsTypeName)
        // {
        //     needsBracesWrap        = true;
        //     mws.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
        //     StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
        //     FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
        //
        //     // needsBracketWrap = true;
        //     // if (!moldWrittenFlags.HasWroteTypeNameFlag() && !moldWrittenFlags.HasInnerSameAsOuterTypeFlag())
        //     // {
        //     //     needsTypeName    = true;
        //     // }
        // }
        if (needsTypeName)
        {
            Gb.AppendContent(RndBrktOpn);
            Gb.AppendContent(mws.TypeBeingBuilt.CachedCSharpNameNoConstraints());
            // Gb.AppendContent(RndBrktOpn);
        }
        // else if (needsBracketWrap)
        // {
        Gb.AppendContent(RndBrktOpn);
        // }
        AppendFieldName(mws, "$ref");
        AppendFieldValueSeparator();
        FormatFieldContents(mws, refId, "", createTypeFlags);
        mws.CurrentWriteMethod |= WithReferenceToInstanceId;

        // if (needsBracketWrap)
        // {
        if (needsTypeName) Gb.AppendContent(RndBrktCls);
        Gb.AppendContent(RndBrktCls);
        // if (needsTypeName) Gb.AppendPadding(Spc);
        if (currentWriteMethod.HasShowSuppressedContents() && needsTypeName) Gb.AppendPadding(Spc);
        Gb.Complete(createTypeFlags);
        // }
        // else if (needsBracesWrap)
        // {
        //     mws.IsEmpty = false;
        //     AddToNextFieldSeparatorAndPadding(createTypeFlags);
        // }
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
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags, WrittenAsFlags writeAs = AsString)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb = mws.Sb;

        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited  = parentFlags.GetParentInheritedFlags(grandParentCallFlags);

        var withMoldInherited = callerFormatFlags | inherited;
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);
        mws.Master.SetCallerWriteAs(writeAs);

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
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLinesAndLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldName<TBearer>(IMoldWriteState mws, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags, WrittenAsFlags writeAs = AsString)
        where TBearer : IStringBearer?
    {
        var sb                = mws.Sb;

        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited            = parentFlags.GetParentInheritedFlags(grandParentCallFlags);
        
        var withMoldInherited = callerFormatFlags | inherited;
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | IsFieldName);
        mws.Master.SetCallerWriteAs(writeAs);

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
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLinesAndLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
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
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags, WrittenAsFlags writeAs = Empty)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mws.Sb;

        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited            = parentFlags.GetParentInheritedFlags(grandParentCallFlags);
        
        var withMoldInherited    = callerFormatFlags | inherited;
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);
        mws.Master.SetCallerWriteAs(writeAs);

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
            Gb.Complete(callerFormatFlags);
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        // if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        // {
        //     mws.Master.UpdateVisitLinesAndLength(appendSummary.VisitNumber, charsInserted);
        // }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldContents<TBearer>(IMoldWriteState mws, TBearer styledObj,
        string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags, WrittenAsFlags writeAs = Empty)
        where TBearer : IStringBearer?
    {
        var sb = mws.Sb;

        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited            = parentFlags.GetParentInheritedFlags(grandParentCallFlags);
        
        var withMoldInherited    = callerFormatFlags | inherited;
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);
        mws.Master.SetCallerWriteAs(writeAs);

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
            Gb.Complete(callerFormatFlags);
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        var charsInserted = LayoutEncoder.InsertTransfer(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted, charsInserted);
        charsInserted += LayoutEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        // if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        // {
        //     mws.Master.UpdateVisitLinesAndLength(appendSummary.VisitNumber, charsInserted);
        // }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType, bool? hasItems
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (mws.WroteInnerTypeOpen || mws.SkipBody || hasItems != true) { return ContentSeparatorRanges.None; }
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        mws.WroteInnerTypeOpen = true;
        if (mws.SkipBody) return Gb.Complete(formatFlags);
        // if (mws is ICollectionMoldWriteState { IsSimple: true } scmdc)
        // {
        //     if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags); }
        //     else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
        //     {
        //         if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags); }
        //         else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
        //         {
        //             var reg = mws.Master.ActiveGraphRegistry;
        //
        //             GraphNodeVisit derivedMold = reg[mws.MoldGraphVisit.VisitId.VisitIndex];
        //
        //             var checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;
        //
        //             GraphNodeVisit checkMold = reg[checkMoldIndex];
        //
        //             do
        //             {
        //                 derivedMold    = checkMold;
        //                 checkMoldIndex = reg[checkMoldIndex].ParentVisitId.VisitIndex;
        //                 checkMold      = reg[Math.Max(0, checkMoldIndex)];
        //             } while (checkMoldIndex >= 0
        //                   && (checkMold.MoldState?.MoldGraphVisit.IsBaseOfInitial ?? false)
        //                   && ReferenceEquals(checkMold.VisitedInstance, derivedMold.VisitedInstance));
        //             var initialDc = derivedMold.MoldState;
        //
        //             if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
        //             {
        //                 AppendInstanceValuesFieldName(mws.TypeBeingBuilt, formatFlags);
        //             }
        //         }
        //     }
        // }
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
        
        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited            = parentFlags.GetParentInheritedFlags(grandParentCallFlags);
        var withInherited        = callerFormatFlags | inherited;
        
        mws.Master.SetCallerFormatFlags(withInherited);
        mws.Master.SetCallerWriteAs(AsCollectionItem);
        mws.Master.SetCallerFormatString(callerFormatString);
        
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
        
        var parentFlags          = mws.CreateMoldFormatFlags;
        var grandParentCallFlags = mws.Caller.FormatFlags;
        var inherited            = parentFlags.GetParentInheritedFlags(grandParentCallFlags);
        var withInherited        = callerFormatFlags | inherited;
        
        mws.Master.SetCallerFormatFlags(withInherited);
        mws.Master.SetCallerWriteAs(AsCollectionItem);
        mws.Master.SetCallerFormatString(callerFormatString);
        
        var stateExtractResult = item.RevealState(mws.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, (FormatFlags)formatSwitches);
        var result = base.CollectionNextItem(nextItem, retrieveCount, sb, formatSwitches);
        Gb.MarkContentEnd();
        return result;
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
