#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Venues;

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

    public override void StateReset()
    {
        VenueOrder?.DecrementRefCount();
        VenueOrder = null;
        UpdateTime = DateTimeConstants.UnixEpoch;
        AdapterSocketReceivedTime = DateTimeConstants.UnixEpoch;
        AdapterProcessedTime = DateTimeConstants.UnixEpoch;
        ClientReceivedTime = DateTimeConstants.UnixEpoch;
        base.StateReset();
    }

    public override IVenueOrderUpdate Clone() => Recycler?.Borrow<OrxVenueOrderUpdate>().CopyFrom(this) ?? new OrxVenueOrderUpdate(this);

    public IVenueOrderUpdate CopyFrom(IVenueOrderUpdate venueOrderUpdate
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(venueOrderUpdate, copyMergeFlags);
        VenueOrder = venueOrderUpdate.VenueOrder.SyncOrRecycle(VenueOrder);
        UpdateTime = venueOrderUpdate.UpdateTime;
        AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
        AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
        ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("OrxVenueOrderUpdate(");
        if (VenueOrder != null) sb.Append("VenueOrder: ").Append(VenueOrder).Append(", ");
        if (UpdateTime != DateTimeConstants.UnixEpoch) sb.Append("UpdateTime: ").Append(UpdateTime).Append(", ");
        if (AdapterSocketReceivedTime != DateTimeConstants.UnixEpoch)
            sb.Append("AdapterSocketReceivedTime: ").Append(AdapterSocketReceivedTime).Append(", ");
        if (AdapterProcessedTime != DateTimeConstants.UnixEpoch)
            sb.Append("AdapterProcessedTime: ").Append(AdapterProcessedTime).Append(", ");
        if (ClientReceivedTime != DateTimeConstants.UnixEpoch)
            sb.Append("ClientReceivedTime: ").Append(ClientReceivedTime).Append(", ");
        if (sb[^2] == ',')
        {
            sb[^2] = ')';
            sb.Length -= 1;
        }

        return sb.ToString();
    }
}
