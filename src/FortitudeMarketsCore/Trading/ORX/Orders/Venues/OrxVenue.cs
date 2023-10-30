#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenue : IVenue
{
    public OrxVenue() => Name = new MutableString();

    public OrxVenue(IVenue toClone)
    {
        VenueId = toClone.VenueId;
        Name = new MutableString(toClone.Name);
    }

    public OrxVenue(ushort venueId, string name)
        : this(venueId, (MutableString)name) { }

    public OrxVenue(ushort venueId, MutableString name)
    {
        VenueId = venueId;
        Name = name;
    }

    [OrxOptionalField(1)] public MutableString Name { get; set; }

    [OrxMandatoryField(0)] public ushort VenueId { get; set; }

    IMutableString IVenue.Name
    {
        get => Name;
        set => Name = (MutableString)value;
    }

    public IVenue Clone() => new OrxVenue(this);

    public void CopyFrom(IVenue venue, IRecycler recycler)
    {
        VenueId = venue.VenueId;
        Name = recycler.Borrow<MutableString>().Clear().Append(venue.Name);
    }

    protected bool Equals(OrxVenue other)
    {
        var venueIdSame = VenueId == other.VenueId;
        var nameSame = Equals(Name, other.Name);
        return venueIdSame && nameSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenue)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (VenueId.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
