// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentJoinTypeMold<TMold> : KnownTypeMolder<ContentJoinTypeMold<TMold>>, 
    IMigrateFrom<TMold, ContentJoinTypeMold<TMold>>
    where TMold : TypeMolder
{
    private bool initialWasComplex;
    private bool wasUpgradedToComplexType;

    public override bool IsComplexType => initialWasComplex;

    public override void StartFormattingTypeOpening()
    {
        throw new NotImplementedException("Should never be called!");
    }

    public override void CompleteTypeOpeningToTypeFields()
    {
        throw new NotImplementedException("Should never be called!");
    }

    public override void AppendClosing()
    {
        var sf = MoldStateField.StyleFormatter;
        var gb = sf.GraphBuilder;

        IEncodingTransfer origGraphEncoder     = sf.GraphBuilder.GraphEncoder;
        IEncodingTransfer origParentEncoder     = sf.GraphBuilder.ParentGraphEncoder;
        
        var shouldSwitchEncoders = wasUpgradedToComplexType && gb.GraphEncoder.Type != gb.ParentGraphEncoder.Type;

        if (shouldSwitchEncoders)
        {
            gb.GraphEncoder       = origParentEncoder; // setting this changes parentGraphEncoder to old value
            gb.ParentGraphEncoder = origParentEncoder;
        }
        if (IsComplexType) { sf.AppendComplexTypeClosing(MoldStateField); }
        else { sf.AppendContentTypeClosing(MoldStateField); }
        if (shouldSwitchEncoders)
        {
            gb.GraphEncoder       = origGraphEncoder;
            
            gb.ParentGraphEncoder = origParentEncoder;
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

    public ContentJoinTypeMold<TMold> CopyFrom(TMold source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PortableState = ((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.PortableState;
        SourceBuilderComponentAccess(((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.WriteMethod);

        MoldStateField.CopyFrom(((ITypeBuilderComponentSource)source).MoldState, copyMergeFlags);
        initialWasComplex        = source.IsComplexType;
        wasUpgradedToComplexType = initialWasComplex && source is SimpleContentTypeMold;

        return this;
    }

    protected override void InheritedStateReset()
    {
        initialWasComplex        = false;
        wasUpgradedToComplexType = false;
        
        base.InheritedStateReset();
    }
}
