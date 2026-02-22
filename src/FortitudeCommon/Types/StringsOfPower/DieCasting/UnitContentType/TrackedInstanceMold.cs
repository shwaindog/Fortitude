// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class TrackedInstanceMold : KnownTypeMolder<TrackedInstanceMold>
{
    private bool isDefaultSuppressOpenCloseType;
    
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
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);


        return this;
    }

    protected MoldWriteState<TrackedInstanceMold> Msf => (MoldWriteState<TrackedInstanceMold>)MoldStateField;

    public override bool IsComplexType => Msf.SupportsMultipleFields;
    
    public override void StartTypeOpening(FormatFlags formatFlags)
    {
        var mws = MoldStateField;
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        var usingFormatter = (CreateFormatFlags.HasAsStringContentFlag()
                           && mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
                           && mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
            ? mws.StyleFormatter.PreviousContextOrThis
            : mws.StyleFormatter;
      
        StartTypeOpening(usingFormatter, formatFlags);
        MyAppendGraphFields(mws.InstanceOrType, mws.MoldGraphVisit, usingFormatter, mws.CurrentWriteMethod, mws.CreateMoldFormatFlags);
    }

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mws = MoldStateField;
        // usingFormatter.Gb.StartNextContentSeparatorPaddingSequence(mws.Sb, mws.CreateMoldFormatFlags);
        // var typeBeingBuilt = mws.TypeBeingBuilt;
        // isDefaultSuppressOpenCloseType = typeBeingBuilt.IsInputConstructionTypeCached();
        // if (MoldStateField.CurrentWriteMethod.HasAsContentFlag() && !isDefaultSuppressOpenCloseType && MoldStateField.Style.IsLog())
        // {
        //     MoldStateField.CurrentWriteMethod = mws.CurrentWriteMethod.SupportsMultipleFields() 
        //         ?  AsComplex | AsContent | AsRaw
        //         : AsSimple | AsContent | AsRaw;
        // }
        if(mws.CurrentWriteMethod.HasAsComplexFlag())
        {
            usingFormatter.StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
        }
        if(mws.CurrentWriteMethod.HasAsSimpleFlag())
        {
            usingFormatter.StartSimpleTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
        }
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mws         = State;
        if(mws.CurrentWriteMethod.HasAsComplexFlag())
        {
            usingFormatter.FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
        }
        if(mws.CurrentWriteMethod.HasAsSimpleFlag())
        {
            usingFormatter.FinishSimpleTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
        }
    }

    public override void AppendClosing()
    {
        var mws         = State;
        var writeMethod = mws.CurrentWriteMethod;
        if(writeMethod.HasAsComplexFlag())
        {
            mws.CurrentWriteMethod = AsComplex;
            var sf = mws.StyleFormatter;
            if (sf.ContentEncoder.Type != sf.LayoutEncoder.Type) { sf = sf.PreviousContextOrThis; }
            sf.AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (mws.Sf.Gb.CurrentSectionRanges.HasNonZeroLengthContent)
            {
                mws.Sf.Gb.SnapshotLastAppendSequence(mws.CreateMoldFormatFlags);
                mws.Sf.Gb.AddHighWaterMark();
            }
        }

        mws.Sf.Gb.MarkContentEnd();
    }
}
