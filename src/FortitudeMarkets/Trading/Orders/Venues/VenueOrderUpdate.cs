#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueOrderUpdate : TradingMessage, IVenueOrderUpdate
{
    public VenueOrderUpdate() { }

    public VenueOrderUpdate(IVenueOrderUpdate toClone)
    {
        VenueOrder = toClone.VenueOrder?.Clone();
        UpdateTime = toClone.UpdateTime;
        AdapterSocketReceivedTime = toClone.AdapterSocketReceivedTime;
        AdapterProcessedTime = toClone.AdapterProcessedTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public VenueOrderUpdate(IVenueOrder venueOrder, DateTime updateTime, DateTime socketReceivedTime,
        DateTime adapterProcessedTime, DateTime? clientReceivedTime = null)
    {
        VenueOrder = venueOrder;
        UpdateTime = updateTime;
        AdapterSocketReceivedTime = socketReceivedTime;
        AdapterProcessedTime = adapterProcessedTime;
        ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
    }

    public override uint MessageId => (uint)TradingMessageIds.VenueOrderUpdate;

    public IVenueOrder? VenueOrder { get; set; }
    public DateTime UpdateTime { get; set; }
    public DateTime AdapterSocketReceivedTime { get; set; }
    public DateTime AdapterProcessedTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IVenueOrderUpdate venueOrderUpdate)
        {
            VenueOrder = venueOrderUpdate.VenueOrder?.SyncOrRecycle(VenueOrder as VenueOrder);
            UpdateTime = venueOrderUpdate.UpdateTime;
            AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
            AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
            ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
        }

        return this;
    }

    public override IVenueOrderUpdate Clone() =>
        (IVenueOrderUpdate?)Recycler?.Borrow<VenueOrderUpdate>().CopyFrom(this) ?? new VenueOrderUpdate(this);
}
