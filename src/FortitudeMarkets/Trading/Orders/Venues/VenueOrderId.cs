// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueOrderId : ReusableObject<IVenueOrderId>, IVenueOrderId
{
    public VenueOrderId()
    {
        VenueClientOrderId   = null!;
        VenueOrderIdentifier = null!;
    }

    public VenueOrderId(IVenueOrderId toClone)
    {
        VenueClientOrderId   = toClone.VenueClientOrderId;
        VenueOrderIdentifier = toClone.VenueOrderIdentifier;
    }

    public VenueOrderId(string marketClientOrderId, string marketOrderIdentifier)
        : this((MutableString)marketClientOrderId, (MutableString)marketClientOrderId) { }

    public VenueOrderId(IMutableString marketClientOrderId, IMutableString marketOrderIdentifier)
    {
        VenueClientOrderId   = marketClientOrderId;
        VenueOrderIdentifier = marketOrderIdentifier;
    }

    public IMutableString VenueClientOrderId   { get; set; }
    public IMutableString VenueOrderIdentifier { get; set; }


    public override IVenueOrderId Clone() => Recycler?.Borrow<VenueOrderId>().CopyFrom(this) ?? new VenueOrderId(this);

    public override IVenueOrderId CopyFrom(IVenueOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueClientOrderId   = source.VenueClientOrderId;
        VenueOrderIdentifier = source.VenueOrderIdentifier;
        return this;
    }
}