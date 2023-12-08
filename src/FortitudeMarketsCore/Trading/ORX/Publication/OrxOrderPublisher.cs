#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Sockets;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public class OrxOrderPublisher : RecyclableObject, IOrderPublisher
{
    private bool errorSupport;

    private IOrxPublisher? OrxServerMessaging { get; set; }

    public ISession? UnderlyingSession { get; set; }

    public void Dispose() { }

    public bool Publish(IOrderUpdate orderUpdate)
    {
        //return UnderlyingSession.;
        if (orderUpdate.Order?.OrderPublisher?.UnderlyingSession != null)
            OrxServerMessaging?.Send(orderUpdate.Order.OrderPublisher.UnderlyingSession,
                new OrxOrderUpdate(orderUpdate));
        return false;
    }

    public void Configure(ISession underlyingSession, IOrxPublisher orxServerMessaging, bool errorSupport)
    {
        this.errorSupport = errorSupport;
        UnderlyingSession = underlyingSession;
        OrxServerMessaging = orxServerMessaging;
    }


    private OrxOrderEvent ToOrxOrderStatus(IOrder order)
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
