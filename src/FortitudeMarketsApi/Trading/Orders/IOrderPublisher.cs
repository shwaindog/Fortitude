#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Sockets;
using FortitudeMarketsApi.Trading.Orders.Server;

#endregion

namespace FortitudeMarketsApi.Trading.Orders;

public interface IOrderPublisher : IDisposable, IRecyclableObject
{
    ISession? UnderlyingSession { get; }
    bool Publish(IOrderUpdate orderUpdate);
}
