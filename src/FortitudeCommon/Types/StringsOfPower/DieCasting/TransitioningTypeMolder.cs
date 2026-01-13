// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class TransitioningTypeMolder<TCurrent, TNext> : KnownTypeMolder<TCurrent>, IMigratableTypeBuilderComponentSource 
  , IMigrateFrom<TCurrent, TNext>
    where TCurrent : TransitioningTypeMolder<TCurrent, TNext>
    where TNext : TypeMolder, IMigrateFrom<TCurrent, TNext>, new()
{
    private bool hasTransitioned;

    public override void AppendTypeOpeningToGraphFields()
    {
        var formatter = MoldStateField.StyleFormatter;
        if (IsComplexType)
        {
            formatter.StartComplexTypeOpening(MoldStateField);
            formatter.FinishComplexTypeOpening(MoldStateField);
        }
        else
        {
            formatter.StartContentTypeOpening(MoldStateField);
            formatter.FinishContentTypeOpening(MoldStateField);
        }
    }

    public override void AppendClosing()
    {
        if (IsComplexType)
            MoldStateField.StyleFormatter.AppendComplexTypeClosing(MoldStateField);
        else
            MoldStateField.StyleFormatter.AppendContentTypeClosing(MoldStateField);
    }

    public virtual TNext TransitionToNextMold()
    {
        
        var nextTypeBuilder = MoldStateField.Recycler.Borrow<TNext>();

        if (nextTypeBuilder is IMigrateFrom<TCurrent, TNext> copyFromCurrent)
        {
            copyFromCurrent.CopyFrom(MoldStateField.StyleTypeBuilder);
        }
        hasTransitioned = true;
        return nextTypeBuilder;
    }

    public IMigratableTypeMolderDieCast MigratableMoldState => MoldStateField;

    public override StateExtractStringRange Complete()
    {
        if (!hasTransitioned)
        {
            return base.Complete();
        }
        var currentAppenderIndex = MoldStateField.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var ignored               = new StateExtractStringRange(TypeName ?? TypeBeingBuilt.CachedCSharpNameWithConstraints()
                                                              , MoldStateField.Master, typeWriteRange);
        MoldStateField.DecrementRefCount();
        MoldStateField               = null!;
        ((IRecyclableObject)this).DecrementRefCount();
        return ignored;
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        return CopyFrom(source as TCurrent, copyMergeFlags);
    }

    public virtual TNext CopyFrom(TCurrent? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return TransitionToNextMold();
        PortableState = ((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.PortableState;
        SourceBuilderComponentAccess();
        MoldStateField.CopyFrom(source.MoldStateField, copyMergeFlags);
        return TransitionToNextMold();
    }
}
