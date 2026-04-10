// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class TransitioningTypeMolder<TCurrent, TNext> : KnownTypeMolder<TCurrent>, IMigratableTypeBuilderComponentSource 
  , IMigrateFrom<TCurrent, TNext>
    where TCurrent : TransitioningTypeMolder<TCurrent, TNext>
    where TNext : TypeMolder, IMigrateFrom<TCurrent, TNext>, new()
{
    private bool hasTransitioned;

    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (IsComplexType)
            MoldStateField
                .StyleFormatter
                .AppendComplexTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MigratableMoldState.CurrentWriteMethod, formatFlags);
        else
            MoldStateField
                .StyleFormatter
                .AppendSimpleTypeClosing(MoldStateField.InstanceOrType, MoldStateField, MigratableMoldState.CurrentWriteMethod, formatFlags);
    }

    public virtual TNext TransitionToNextMold()
    {
        var msf      = MoldStateField;
        var sf       = msf.StyleFormatter;
        var gb       = sf.Gb;
        var fmtFlags = gb.CurrentSectionRanges.StartedWithFormatFlags;
        gb.Complete(fmtFlags);
        if (typeof(TCurrent) == typeof(TNext)) return (TNext)(object)this;
        var nextTypeBuilder = MoldStateField.Recycler.Borrow<TNext>();

        if (nextTypeBuilder is IMigrateFrom<TCurrent, TNext> copyFromCurrent)
        {
            copyFromCurrent.CopyFrom(MoldStateField.Mold);
        }
        
        hasTransitioned = true;
        return nextTypeBuilder;
    }

    public IMigratableMoldWriteState MigratableMoldState => MoldStateField;

    public override AppendSummary Complete(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (!hasTransitioned)
        {
            return base.Complete(formatFlags);
        }
        var currentAppenderIndex = MoldStateField.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var ignored               = BuildMoldAppendSummary(typeWriteRange);
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
