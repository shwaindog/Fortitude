#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Orders.Server;

#endregion

namespace FortitudeMarkets.Trading.ORX.Publication;

public class OrxOrderPublisher : RecyclableObject, IOrderPublisher
{
    private bool errorSupport;

    private IOrxMessageResponder? OrxServerMessaging { get; set; }

    public IConversationRequester? UnderlyingSession { get; set; }

    public void Dispose() { }

    public bool Publish(IOrderUpdate orderUpdate)
    {
        //return UnderlyingSession.;
        if (orderUpdate.Order?.OrderPublisher?.UnderlyingSession != null)
        {
            var orxOrderUpdate = Recycler!.Borrow<OrxOrderUpdate>();
            orxOrderUpdate.CopyFrom(orderUpdate);
            orderUpdate.Order.OrderPublisher.UnderlyingSession.Send(new OrxOrderUpdate(orderUpdate));
            orxOrderUpdate.DecrementRefCount();
        }

        return false;
    }

    public void Configure(IConversationRequester underlyingSession, IOrxMessageResponder orxServerMessaging, bool errorSupport)
    {
        this.errorSupport = errorSupport;
        UnderlyingSession = underlyingSession;
        OrxServerMessaging = orxServerMessaging;
    }

    private OrxOrderEvent ToOrxOrderStatus(ITransmittableOrder order)
    {
        switch (order.Status)
        {
            case OrderStatus.New:
            case OrderStatus.PendingNew:
                return OrxOrderEvent.Sent;
            case OrderStatus.Active:
                return OrxOrderEvent.Active;
            case OrderStatus.Cancelling:
                return OrxOrderEvent.CancelSent;
            case OrderStatus.Dead:
                return order.IsInError() && errorSupport ? OrxOrderEvent.Error : OrxOrderEvent.Cancelled;
            default:
                return OrxOrderEvent.Unknown;
        }
    }
}
