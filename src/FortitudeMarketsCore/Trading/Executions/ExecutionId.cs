#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.Executions;

public class ExecutionId : IExecutionId
{
    private int refCount = 0;

    public ExecutionId(IExecutionId toClone)
    {
        VenueExecutionId = toClone.VenueExecutionId;
        AdapterExecutionId = toClone.AdapterExecutionId;
        BookingSystemId = toClone.BookingSystemId;
    }

    public ExecutionId(string venueExecutionId, int adapterExecutionId, string bookingSystemId)
        : this((MutableString)venueExecutionId, adapterExecutionId, (MutableString)bookingSystemId) { }

    public ExecutionId(IMutableString venueExecutionId, int adapterExecutionId, IMutableString bookingSystemId)
    {
        VenueExecutionId = venueExecutionId;
        AdapterExecutionId = adapterExecutionId;
        BookingSystemId = bookingSystemId;
    }

    public IMutableString VenueExecutionId { get; set; }
    public int AdapterExecutionId { get; set; }
    public IMutableString BookingSystemId { get; set; }

    public void CopyFrom(IExecutionId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueExecutionId = source.VenueExecutionId;
        AdapterExecutionId = source.AdapterExecutionId;
        BookingSystemId = source.BookingSystemId;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IExecutionId)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }


    public IExecutionId Clone() => new ExecutionId(this);
}
