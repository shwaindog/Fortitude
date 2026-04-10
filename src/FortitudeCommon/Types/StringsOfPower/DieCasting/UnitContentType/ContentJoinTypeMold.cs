// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentJoinTypeMold<TFromMold, TToMold> : KnownTypeMolder<TToMold>, 
    IMigrateFrom<TFromMold, TToMold>
    where TFromMold : TypeMolder
    where TToMold : ContentJoinTypeMold<TFromMold, TToMold>
{
    private bool        initialWasComplex;
    private bool        wasUpgradedToComplexType;
    private FormatFlags valueAddedFormatFlags;

    public override bool IsComplexType => initialWasComplex || wasUpgradedToComplexType;
    public override MoldType MoldType => MoldType.SimpleContentMold;

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        throw new NotImplementedException("Should never be called!");
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        throw new NotImplementedException("Should never be called!");
    }
    
    public override AppendSummary Complete(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (State == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        AppendClosing(State.CreateMoldFormatFlags);
        return RunShutdown();
    }

    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var mws = State;
        var usingFormatter =
            (CreateFormatFlags.HasAsStringContentFlag()
          && (mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
           && mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (mws.MoldGraphVisit.IsARevisit && valueAddedFormatFlags.HasAsStringContentFlag()))
                ? mws.StyleFormatter.PreviousContextOrThis
                : mws.StyleFormatter;
        
        if (MoldStateField.CreateWriteMethod.SupportsMultipleFields())
        {
            usingFormatter.AppendComplexTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags | CreateFormatFlags);
        }
        else
        {
            usingFormatter.AppendSimpleTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags | CreateFormatFlags);
        }
        if (mws.Style.IsJson()
         && mws is { InnerSameAsOuterType: true, TypeBeingBuilt.IsValueType: false }
         && valueAddedFormatFlags.HasAsStringContentFlag())
        {
            var visitId = mws.MoldGraphVisit.VisitId;
            var state   = mws.Master.ActiveGraphRegistry[visitId.VisitIndex];
            mws.Master.SetBufferFirstFieldStartAndWrittenAs(visitId, state.TypeOpenBufferIndex, AsSimple | AsContent | AsString);
            // if (mws.TypeBeingBuilt.IsStringBearerOrNullableCached())
            // {
            mws.Master.SetBufferFirstFieldStartAndIndentLevel(visitId, state.TypeOpenBufferIndex, state.IndentLevel + 1);
            // }
            mws.Master.UpdateVisitFormatter(visitId, usingFormatter);
        }
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        switch (source)
        {
            case SimpleContentTypeMold simpleSource:
            {
                CopyFrom(simpleSource, copyMergeFlags);
                if (IsComplexType) { wasUpgradedToComplexType = true; }
                break;
            }
            case ComplexContentTypeMold complexSource: CopyFrom(complexSource, copyMergeFlags); break;
        }
        return this;
    }

    public TToMold CopyFrom(TFromMold source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OriginalStartIndex    = source.OriginalStartIndex;
        var sourceWriteState = ((IMigratableTypeBuilderComponentSource)source).MigratableMoldState;
        var snapWriteState = sourceWriteState.SnapshotWriteState;
        PortableState.CopyFrom(sourceWriteState.PortableState);
        SourceBuilderComponentAccess(sourceWriteState.CurrentWriteMethod);
        sourceWriteState.SnapshotWriteState = snapWriteState;

        MoldStateField.CopyFrom(sourceWriteState, copyMergeFlags);
        initialWasComplex        = source.IsComplexType;
        wasUpgradedToComplexType = initialWasComplex && source is SimpleContentTypeMold;
        if (sourceWriteState is IContentTypeWriteState contentWriteState)
        {
            valueAddedFormatFlags = contentWriteState.ValueAddedFormatFlags;
        }
        
        return (TToMold)this;
    }

    protected override void InheritedStateReset()
    {
        initialWasComplex        = false;
        wasUpgradedToComplexType = false;
        
        base.InheritedStateReset();
    }
}
