#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class Venue : ReusableObject<IVenue>, IVenue
{
    public Venue() => Name = null!;

    public Venue(IVenue toClone)
    {
        VenueId = toClone.VenueId;
        Name = toClone.Name;
    }

    public Venue(ushort venueId, string name)
        : this(venueId, (MutableString)name) { }

    public Venue(ushort venueId, IMutableString name)
    {
        VenueId = venueId;
        Name = name;
    }

    public ushort VenueId { get; set; }
    public IMutableString Name { get; set; }

    public override IVenue Clone() => Recycler?.Borrow<Venue>().CopyFrom(this) ?? new Venue(this);

    public override IVenue CopyFrom(IVenue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueId = source.VenueId;
        Name = source.Name;
        return this;
    }
}
