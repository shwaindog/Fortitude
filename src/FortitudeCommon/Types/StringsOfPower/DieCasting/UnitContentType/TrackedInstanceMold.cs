// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class TrackedInstanceMold : KnownTypeMolder<TrackedInstanceMold>
{
    private IStyledTypeFormatting? trackedInstanceFormatter;

    private FormatFlags innerContentFormatFLags;

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

        innerContentFormatFLags = innerContentFormatFlags;
        return this;
    }

    protected MoldWriteState<TrackedInstanceMold> Mws => (MoldWriteState<TrackedInstanceMold>)MoldStateField;

    public override bool IsComplexType => Mws.SupportsMultipleFields;

    public override void StartTypeOpening(FormatFlags formatFlags)
    {
        trackedInstanceFormatter =
            (CreateFormatFlags.HasAsStringContentFlag()
          && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
            && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
           || (Mws.MoldGraphVisit.IsARevisit && innerContentFormatFLags.HasAsStringContentFlag()))
                ? Mws.StyleFormatter.PreviousContextOrThis
                : Mws.StyleFormatter;

        StartTypeOpening(trackedInstanceFormatter, formatFlags);
    }

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mergedCreateAndInner = formatFlags | innerContentFormatFLags;
        
        base.StartTypeOpening(usingFormatter, mergedCreateAndInner);
    }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
        trackedInstanceFormatter ??=
            (CreateFormatFlags.HasAsStringContentFlag()
          && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
           && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (Mws.MoldGraphVisit.IsARevisit && innerContentFormatFLags.HasAsStringContentFlag()))
                ? Mws.StyleFormatter.PreviousContextOrThis
                : Mws.StyleFormatter;
        FinishTypeOpening(trackedInstanceFormatter, formatFlags);
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mergedCreateAndInner = formatFlags | innerContentFormatFLags;
        base.FinishTypeOpening(usingFormatter, mergedCreateAndInner);
    }

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
        var innerFormatFlags = innerContentFormatFLags.RemoveContentTreatmentFlags() | Mws.CreateMoldFormatFlags.OnlyContentTreatmentFlags();
        if (Mws.Style.IsJson() && Mws is { TypeBeingBuilt.IsValueType: false } 
                               && innerFormatFlags.HasAsStringContentFlag())
        {
            var visitId         = Mws.MoldGraphVisit.VisitId;
            var state           = Mws.Master.ActiveGraphRegistry[visitId.VisitIndex];
            var writtenAs       = Mws.CurrentWriteMethod;
            var firstFieldStart = state.FirstFieldBufferIndex;
            if (formatFlags.HasAsStringContentFlag())
            {
                writtenAs       = AsSimple | AsContent | AsString;
                firstFieldStart = state.TypeOpenBufferIndex;
                if (Mws.CreateMoldFormatFlags.DoesNotHaveAsStringContentFlag())
                {
                    Mws.Master.SetBufferFirstFieldStartAndIndentLevel(visitId, firstFieldStart, state.IndentLevel + 1);
                }
            }
            Mws.Master.SetBufferFirstFieldStartAndWrittenAs(visitId, firstFieldStart, writtenAs);
            Mws.Master.UpdateVisitFormatter(visitId, trackedInstanceFormatter);
        }
        if (Mws.MoldGraphVisit.IsARevisit)
        {
            if (Mws.Style.IsLog()) { trackedInstanceFormatter?.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
            else { trackedInstanceFormatter?.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
        }
        else if (Mws.Style.IsLog())
        {
            if (Mws.WroteTypeOpen) { trackedInstanceFormatter?.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, innerFormatFlags); }
        }

        trackedInstanceFormatter?.Gb.MarkContentEnd();
    }
}
