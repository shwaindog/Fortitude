// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class TrackedInstanceMold : KnownTypeMolder<TrackedInstanceMold>
{
    private bool? shouldShowTypeName;
    private bool  isDefaultSuppressOpenCloseType;
    private bool  wroteTypeNameOnClose;

    private IStyledTypeFormatting? trackedInstanceFormatter;

    private FormatFlags InnerContentFormatFLags;

    public TrackedInstanceMold InitializeRawContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext  
      , FormatFlags parentTypeFormatFlags
      , FormatFlags innerContentFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext, parentTypeFormatFlags);

        InnerContentFormatFLags = innerContentFormatFlags;
        return this;
    }

    protected MoldWriteState<TrackedInstanceMold> Mws => (MoldWriteState<TrackedInstanceMold>)MoldStateField;

    public override bool IsComplexType => Mws.SupportsMultipleFields;

    public override void StartTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        trackedInstanceFormatter =
            (CreateFormatFlags.HasAsStringContentFlag()
          && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
            && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
           || (Mws.MoldGraphVisit.IsARevisit && InnerContentFormatFLags.HasAsStringContentFlag()))
                ? Mws.StyleFormatter.PreviousContextOrThis
                : Mws.StyleFormatter;

        StartTypeOpening(trackedInstanceFormatter, formatFlags);
    }

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var showTypeName = (InnerContentFormatFLags & ~LogSuppressTypeNames) | AddTypeNameField;
        if (Mws.MoldGraphVisit.IsARevisit)
        {
            if (Mws.Style.IsLog())
            {
                usingFormatter.StartSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, showTypeName);
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod | AsObject, MoldStateField.MoldWrittenFlags, InnerContentFormatFLags);
            }
            else { usingFormatter.StartComplexTypeOpening(State.InstanceOrType, State, State.CreateWriteMethod, showTypeName); }
        }
        else if (Mws.Style.IsLog())
        {
            shouldShowTypeName ??= ShowTransientTypeName(Mws.TypeBeingBuilt, InnerContentFormatFLags);
            if (shouldShowTypeName.Value)
            {
                usingFormatter.StartSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, InnerContentFormatFLags);
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod | AsObject, MoldStateField.MoldWrittenFlags, showTypeName);
            }
        }
    }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        trackedInstanceFormatter ??=
            (CreateFormatFlags.HasAsStringContentFlag()
          && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
           && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (Mws.MoldGraphVisit.IsARevisit && InnerContentFormatFLags.HasAsStringContentFlag()))
                ? Mws.StyleFormatter.PreviousContextOrThis
                : Mws.StyleFormatter;
        FinishTypeOpening(trackedInstanceFormatter, formatFlags);
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        if (Mws.MoldGraphVisit.IsARevisit)
        {
            if (Mws.Style.IsLog())
            {
                usingFormatter.FinishSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, InnerContentFormatFLags);
            }
            else
            {
                usingFormatter.FinishComplexTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod
                                                      , InnerContentFormatFLags);
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, InnerContentFormatFLags);
            }
        }
        else if (Mws.Style.IsLog())
        {
            shouldShowTypeName ??= ShowTransientTypeName(Mws.TypeBeingBuilt, InnerContentFormatFLags);
            if (shouldShowTypeName.Value)
            {
                usingFormatter.FinishSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags);
                Mws.WroteTypeOpen = false;
            }
        }
    }

    private bool ShowTransientTypeName(Type typeBeingBuilt, FormatFlags formatFlags)
    {
        var buildingTypeFullName = typeBeingBuilt.FullName ?? "";
        isDefaultSuppressOpenCloseType = typeBeingBuilt.IsInputConstructionTypeCached();
        var openAs                 = Mws.CurrentWriteMethod;
        var isSpanFormattableClass = typeBeingBuilt.IsSpanFormattableOrNullableCached();
        var iStringBearerClass     = typeBeingBuilt.IsStringBearerOrNullableCached();
        var isIgnoredOpenType      = (isDefaultSuppressOpenCloseType || isSpanFormattableClass) && !iStringBearerClass;
        if (!isIgnoredOpenType && InnerContentFormatFLags.DoesNotHaveLogSuppressTypeNamesFlag())
        {
            var showTypeName = false;

            showTypeName |= (openAs.HasAnyOf(AsContent | AsObject)
                          && !(Settings.LogSuppressDisplayTypeNames.Any(s => buildingTypeFullName.StartsWith(s))));

            if (!showTypeName)
            {
                var elementType         = typeBeingBuilt.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeBeingBuilt;
                var elementTypeFullName = elementType.FullName ?? "";
                showTypeName |= (openAs.HasAsCollectionFlag()
                              && !(Settings.LogSuppressDisplayCollectionNames.Any(s => buildingTypeFullName.StartsWith(s))
                                && Settings.LogSuppressDisplayCollectionElementNames.Any(s => elementTypeFullName.StartsWith(s)))
                              || (Mws.MoldGraphVisit.IsARevisit && Mws is ICollectionMoldWriteState { IsSimple: true }));
            }

            return showTypeName;
        }
        return false;
    }

    // public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    // {
    //     var mws = MoldStateField;
    //     // usingFormatter.Gb.StartNextContentSeparatorPaddingSequence(mws.Sb, mws.CreateMoldFormatFlags);
    //     // var typeBeingBuilt = mws.TypeBeingBuilt;
    //     // isDefaultSuppressOpenCloseType = typeBeingBuilt.IsInputConstructionTypeCached();
    //     // if (MoldStateField.CurrentWriteMethod.HasAsContentFlag() && !isDefaultSuppressOpenCloseType && MoldStateField.Style.IsLog())
    //     // {
    //     //     MoldStateField.CurrentWriteMethod = mws.CurrentWriteMethod.SupportsMultipleFields() 
    //     //         ?  AsComplex | AsContent | AsRaw
    //     //         : AsSimple | AsContent | AsRaw;
    //     // }
    //     if(mws.CurrentWriteMethod.HasAsComplexFlag())
    //     {
    //         usingFormatter.StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
    //     }
    //     if(mws.CurrentWriteMethod.HasAsSimpleFlag())
    //     {
    //         usingFormatter.StartSimpleTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
    //         MyAppendGraphFields(mws.InstanceOrType, mws.MoldGraphVisit, usingFormatter
    //                           , mws.CurrentWriteMethod, mws.MoldWrittenFlags, mws.CreateMoldFormatFlags);
    //     }
    // }
    //
    // public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    // {
    //     var mws         = State;
    //     if(mws.CurrentWriteMethod.HasAsComplexFlag())
    //     {
    //         usingFormatter.FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
    //         MyAppendGraphFields(mws.InstanceOrType, mws.MoldGraphVisit, usingFormatter
    //                           , mws.CurrentWriteMethod, mws.MoldWrittenFlags, mws.CreateMoldFormatFlags);
    //     }
    //     if(mws.CurrentWriteMethod.HasAsSimpleFlag())
    //     {
    //         usingFormatter.FinishSimpleTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
    //     }
    // }

    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sf  = Mws.StyleFormatter;
        if (trackedInstanceFormatter == null
         && CreateFormatFlags.HasAsStringContentFlag()
         && Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
         && sf.LayoutEncoder.Type != EncodingType.PassThrough)
        {
            if (sf.ContentEncoder.Type == sf.LayoutEncoder.Type) { sf = sf.PreviousContextOrThis; }

            trackedInstanceFormatter =
                (CreateFormatFlags.HasAsStringContentFlag()
              && Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent))
                    ? sf.PreviousContextOrThis
                    : sf;
        }
        trackedInstanceFormatter ??= sf;
        trackedInstanceFormatter.Gb.SetHistory(Mws.StyleFormatter.Gb);
        if (Mws.Style.IsJson() && Mws is { TypeBeingBuilt.IsValueType: false } && InnerContentFormatFLags.HasAsStringContentFlag())
        {
            var visitId = Mws.MoldGraphVisit.VisitId;
            var state   = Mws.Master.ActiveGraphRegistry[visitId.VisitIndex];
            Mws.Master.SetBufferFirstFieldStartAndWrittenAs(visitId, state.TypeOpenBufferIndex, AsSimple | AsContent | AsString);
            Mws.Master.SetBufferFirstFieldStartAndIndentLevel(visitId, state.TypeOpenBufferIndex, state.IndentLevel + 1);
            Mws.Master.UpdateVisitFormatter(visitId, trackedInstanceFormatter);
        }
        var innerFormatFlags = InnerContentFormatFLags.RemoveContentTreatmentFlags() | Mws.CreateMoldFormatFlags.OnlyContentTreatmentFlags();
        if (Mws.MoldGraphVisit.IsARevisit)
        {
            if (Mws.Style.IsLog()) { trackedInstanceFormatter?.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
            else { trackedInstanceFormatter?.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
        }
        else if (Mws.Style.IsLog())
        {
            if (Mws.WroteTypeOpen) { trackedInstanceFormatter?.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
        }
        // var writeMethod = mws.CurrentWriteMethod;
        // if(writeMethod.HasAsComplexFlag())
        // {
        //     mws.CurrentWriteMethod = AsComplex;
        //     var sf = mws.StyleFormatter;
        //     if (sf.ContentEncoder.Type != sf.LayoutEncoder.Type) { sf = sf.PreviousContextOrThis; }
        //     sf.AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        // }
        // else
        // {
        //     if (mws.Sf.Gb.CurrentSectionRanges.HasNonZeroLengthContent)
        //     {
        //         mws.Sf.Gb.SnapshotLastAppendSequence(mws.CreateMoldFormatFlags);
        //         mws.Sf.Gb.AddHighWaterMark();
        //     }
        // }

        trackedInstanceFormatter?.Gb.MarkContentEnd();
    }
}
