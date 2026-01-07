// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentJoinTypeMold<TMold> : KnownTypeMolder<ContentJoinTypeMold<TMold>>, IMigrateFrom<TMold, ContentJoinTypeMold<TMold>>
    where TMold : TypeMolder
{
    private bool initialWasComplex;
    
    public override bool IsComplexType => initialWasComplex;

    public override void AppendOpening()
    {
        throw new NotImplementedException("Should never be called!");
    }
    
    public override void AppendClosing()
    {
        if (IsComplexType)
            MoldStateField.StyleFormatter.AppendTypeClosing(MoldStateField);
        else
            MoldStateField.StyleFormatter.AppendValueTypeClosing(MoldStateField);
    }
    
    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        switch (source)
        {
            case SimpleContentTypeMold simpleSource:   CopyFrom(simpleSource, copyMergeFlags); break;
            case ComplexContentTypeMold complexSource: CopyFrom(complexSource, copyMergeFlags); break;
        }
        return this;
    }

    public ContentJoinTypeMold<TMold> CopyFrom(TMold source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PortableState = ((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.PortableState;
        SourceBuilderComponentAccess();
        
        MoldStateField.CopyFrom(((ITypeBuilderComponentSource)source).MoldState, copyMergeFlags);
        initialWasComplex = source.IsComplexType;

        return this;
    }
}
