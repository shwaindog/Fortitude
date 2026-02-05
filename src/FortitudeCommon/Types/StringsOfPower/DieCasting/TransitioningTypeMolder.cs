// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class TransitioningTypeMolder<TCurrent, TNext> : KnownTypeMolder<TCurrent>, IMigratableTypeBuilderComponentSource 
  , IMigrateFrom<TCurrent, TNext>
    where TCurrent : TransitioningTypeMolder<TCurrent, TNext>
    where TNext : TypeMolder, IMigrateFrom<TCurrent, TNext>, new()
{
    private bool hasTransitioned;

    public override void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter)
    {
        if (IsComplexType)
        {
            usingFormatter.StartComplexTypeOpening(MoldStateField);
            usingFormatter.FinishComplexTypeOpening(MoldStateField);
        }
        else
        {
            usingFormatter.StartContentTypeOpening(MoldStateField);
            usingFormatter.FinishContentTypeOpening(MoldStateField);
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
        var msf      = MoldStateField;
        var sf       = msf.StyleFormatter;
        var gb       = sf.Gb;
        var fmtFlags = gb.CurrentSectionRanges.StartedWithFormatFlags;
        gb.Complete(fmtFlags);
        var nextTypeBuilder = MoldStateField.Recycler.Borrow<TNext>();

        if (nextTypeBuilder is IMigrateFrom<TCurrent, TNext> copyFromCurrent)
        {
            copyFromCurrent.CopyFrom(MoldStateField.StyleTypeBuilder);
        }
        
        hasTransitioned = true;
        return nextTypeBuilder;
    }

    public IMigratableTypeMolderDieCast MigratableMoldState => MoldStateField;

    public override AppendSummary Complete()
    {
        if (!hasTransitioned)
        {
            return base.Complete();
        }
        var currentAppenderIndex = MoldStateField.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var ignored               = BuildMoldStringRange(typeWriteRange);
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
        OriginalStartIndex    = source.StartIndex;
        PortableState = ((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.PortableState;
        SourceBuilderComponentAccess(((IMigratableTypeBuilderComponentSource)source).MigratableMoldState.CurrentWriteMethod);
        MoldStateField.CopyFrom(source.MoldStateField, copyMergeFlags);
        return TransitionToNextMold();
    }
}
