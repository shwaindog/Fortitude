#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeMarkets.Trading.Orders.Server;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface IOrderPublisher : IDisposable, IRecyclableObject
{
    IConversationRequester? UnderlyingSession { get; }
    bool Publish(IOrderUpdate orderUpdate);
}
