#region

using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenueOrderUpdate : OrxTradingMessage, IVenueOrderUpdate
{
    public OrxVenueOrderUpdate() { }

    public OrxVenueOrderUpdate(IVenueOrderUpdate venueOrderUpdate)
    {
        VenueOrder = new OrxVenueOrder(venueOrderUpdate.VenueOrder!);
        UpdateTime = venueOrderUpdate.UpdateTime;
        AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
        AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
        ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
    }

    public OrxVenueOrderUpdate(OrxVenueOrder venueOrder, DateTime updateTime, DateTime socketReceivedTime,
        DateTime adapterProcessedTime, DateTime clientReceivedTime)
    {
        VenueOrder = venueOrder;
        UpdateTime = updateTime;
        AdapterSocketReceivedTime = socketReceivedTime;
        AdapterProcessedTime = adapterProcessedTime;
        ClientReceivedTime = clientReceivedTime;
    }

    [OrxMandatoryField(10)] public OrxVenueOrder? VenueOrder { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.VenueOrderUpdate;

    IVenueOrder? IVenueOrderUpdate.VenueOrder
    {
        get => VenueOrder;
        set => VenueOrder = value as OrxVenueOrder;
    }

    [OrxMandatoryField(11)] public DateTime UpdateTime { get; set; }

    [OrxMandatoryField(12)] public DateTime AdapterSocketReceivedTime { get; set; }

    [OrxMandatoryField(13)] public DateTime AdapterProcessedTime { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    public override IVenueOrderUpdate Clone() =>
        Recycler?.Borrow<OrxVenueOrderUpdate>().CopyFrom(this) ?? new OrxVenueOrderUpdate(this);

    public IVenueOrderUpdate CopyFrom(IVenueOrderUpdate venueOrderUpdate
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(venueOrderUpdate, copyMergeFlags);
        if (venueOrderUpdate.VenueOrder != null)
        {
            var orxVenueOrder = Recycler!.Borrow<OrxVenueOrder>();
            orxVenueOrder.CopyFrom(venueOrderUpdate.VenueOrder, copyMergeFlags);
            VenueOrder = orxVenueOrder;
        }

        UpdateTime = venueOrderUpdate.UpdateTime;
        AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
        AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
        ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
        return this;
    }
}
