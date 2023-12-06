#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenueOrderId : ReusableObject<IVenueOrderId>, IVenueOrderId
{
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

    public override IVenueOrderId Clone() =>
        Recycler?.Borrow<OrxVenueOrderId>().CopyFrom(this) ?? new OrxVenueOrderId(this);

    public override IVenueOrderId CopyFrom(IVenueOrderId venueOrderId
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueClientOrderId = venueOrderId.VenueClientOrderId.CopyOrClone(VenueClientOrderId)!;
        VenueOrderIdentifier = venueOrderId.VenueOrderIdentifier.CopyOrClone(VenueOrderIdentifier)!;
        return this;
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
