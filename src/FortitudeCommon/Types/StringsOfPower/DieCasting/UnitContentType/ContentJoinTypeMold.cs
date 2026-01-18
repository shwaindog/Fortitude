// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentJoinTypeMold<TMold> : KnownTypeMolder<ContentJoinTypeMold<TMold>>, IMigrateFrom<TMold, ContentJoinTypeMold<TMold>>
    where TMold : TypeMolder
{
    private bool initialWasComplex;
    
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
        var formatter = MoldStateField.StyleFormatter;
        if (IsComplexType)
        {
            formatter.AppendComplexTypeClosing(MoldStateField);
        }
        else
        {
            formatter.AppendContentTypeClosing(MoldStateField);
        }
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
        SourceBuilderComponentAccess(((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.WriteMethod);
        
        MoldStateField.CopyFrom(((ITypeBuilderComponentSource)source).MoldState, copyMergeFlags);
        initialWasComplex = source.IsComplexType;

        return this;
    }
}
