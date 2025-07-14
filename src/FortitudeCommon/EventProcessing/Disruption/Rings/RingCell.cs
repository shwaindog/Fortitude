using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public sealed class RingCell<T>
{
    public T? Value;
}

public sealed class RingTransferCell<T> : ITransferState<RingTransferCell<T>>
where T : class, IFreezable<T>
{
    public T? Value;

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        if (source is RingTransferCell<T> ringCell)
        {
            Value = ringCell.Value?.Freeze;
        }
        return this;
    }

    public RingTransferCell<T> CopyFrom(RingTransferCell<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Value = source.Value?.Freeze;

        return this;
    }
}
