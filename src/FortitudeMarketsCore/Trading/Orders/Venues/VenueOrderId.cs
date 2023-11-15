#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueOrderId : IVenueOrderId
{
    private int refCount = 0;

    public VenueOrderId(IVenueOrderId toClone)
    {
        VenueClientOrderId = toClone.VenueClientOrderId;
        VenueOrderIdentifier = toClone.VenueOrderIdentifier;
    }

    public VenueOrderId(string marketClientOrderId, string marketOrderIdentifier)
        : this((MutableString)marketClientOrderId, (MutableString)marketClientOrderId) { }

    public VenueOrderId(IMutableString marketClientOrderId, IMutableString marketOrderIdentifier)
    {
        VenueClientOrderId = marketClientOrderId;
        VenueOrderIdentifier = marketOrderIdentifier;
    }

    public IMutableString VenueClientOrderId { get; set; }
    public IMutableString VenueOrderIdentifier { get; set; }

    public void CopyFrom(IVenueOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueClientOrderId = source.VenueClientOrderId;
        VenueOrderIdentifier = source.VenueOrderIdentifier;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IVenueOrderId)source, copyMergeFlags);
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

    public IVenueOrderId Clone() => new VenueOrderId(this);
}
