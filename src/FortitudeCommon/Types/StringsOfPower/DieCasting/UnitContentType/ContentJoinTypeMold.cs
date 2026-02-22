// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentJoinTypeMold<TFromMold, TToMold> : KnownTypeMolder<TToMold>, 
    IMigrateFrom<TFromMold, TToMold>
    where TFromMold : TypeMolder
    where TToMold : ContentJoinTypeMold<TFromMold, TToMold>
{
    private bool initialWasComplex;
    private bool wasUpgradedToComplexType;

    public override bool IsComplexType => initialWasComplex || wasUpgradedToComplexType;

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        throw new NotImplementedException("Should never be called!");
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        throw new NotImplementedException("Should never be called!");
    }

    public override void AppendClosing()
    {
        var sf = MoldStateField.Sf;
        
        var shouldSwitchEncoders = wasUpgradedToComplexType && sf.ContentEncoder.Type != sf.LayoutEncoder.Type;

        if (shouldSwitchEncoders)
        {
            sf = sf.PreviousContextOrThis;
        }
        if (MoldStateField.CurrentWriteMethod.SupportsMultipleFields())
        {
            sf.AppendComplexTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CurrentWriteMethod);
        }
        else
        {
            sf.AppendSimpleTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CurrentWriteMethod);
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
        PortableState.CopyFrom(((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.PortableState);
        SourceBuilderComponentAccess(((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.CurrentWriteMethod);

        MoldStateField.CopyFrom(((ITypeBuilderComponentSource)source).MoldState, copyMergeFlags);
        initialWasComplex        = source.IsComplexType;
        wasUpgradedToComplexType = initialWasComplex && source is SimpleContentTypeMold;
        

        return (TToMold)this;
    }

    protected override void InheritedStateReset()
    {
        initialWasComplex        = false;
        wasUpgradedToComplexType = false;
        
        base.InheritedStateReset();
    }
}
