using System;
using FortitudeIO.Sockets;
using FortitudeMarketsApi.Trading.Orders.Server;

namespace FortitudeMarketsApi.Trading.Orders
{
    public interface IOrderPublisher : IDisposable
    {
        bool Publish(IOrderUpdate orderUpdate);
        ISession UnderlyingSession { get; }
    }
}