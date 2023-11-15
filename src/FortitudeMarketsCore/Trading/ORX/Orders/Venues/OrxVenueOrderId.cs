#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenueOrderId : IVenueOrderId
{
    private int refCount = 0;

    public OrxVenueOrderId()
    {
        VenueClientOrderId = new MutableString();
        VenueOrderIdentifier = new MutableString();
    }

    public OrxVenueOrderId(IVenueOrderId toClone)
    {
        VenueClientOrderId = new MutableString(toClone.VenueClientOrderId);
        VenueOrderIdentifier = new MutableString(toClone.VenueOrderIdentifier);
    }

    public OrxVenueOrderId(string venueClientOrderId, string venueOrderIdentifier)
        : this((MutableString)venueClientOrderId, venueOrderIdentifier) { }

    public OrxVenueOrderId(MutableString venueClientOrderId, MutableString venueOrderIdentifier)
    {
        VenueClientOrderId = venueClientOrderId;
        VenueOrderIdentifier = venueOrderIdentifier;
    }

    [OrxMandatoryField(0)] public MutableString VenueClientOrderId { get; set; }

    [OrxOptionalField(1)] public MutableString VenueOrderIdentifier { get; set; }

    IMutableString IVenueOrderId.VenueClientOrderId
    {
        get => VenueClientOrderId;
        set => VenueClientOrderId = (MutableString)value;
    }

    IMutableString IVenueOrderId.VenueOrderIdentifier
    {
        get => VenueOrderIdentifier;
        set => VenueOrderIdentifier = (MutableString)value;
    }

    public IVenueOrderId Clone() => new OrxVenueOrderId(this);

    public void CopyFrom(IVenueOrderId venueOrderId, CopyMergeFlags copyMergeFlags)
    {
        VenueClientOrderId = venueOrderId?.VenueClientOrderId != null ?
            Recycler!.Borrow<MutableString>().Clear().Append(venueOrderId.VenueClientOrderId) :
            new MutableString();
        VenueOrderIdentifier = venueOrderId?.VenueOrderIdentifier != null ?
            Recycler!.Borrow<MutableString>().Clear().Append(venueOrderId.VenueOrderIdentifier) :
            new MutableString();
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

    protected bool Equals(OrxVenueOrderId other)
    {
        var clientIdSame = Equals(VenueClientOrderId, other.VenueClientOrderId);
        var venueIdSame = Equals(VenueOrderIdentifier, other.VenueOrderIdentifier);
        return clientIdSame && venueIdSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenueOrderId)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((VenueClientOrderId != null ? VenueClientOrderId.GetHashCode() : 0) * 397) ^
                   (VenueOrderIdentifier != null ? VenueOrderIdentifier.GetHashCode() : 0);
        }
    }
}
