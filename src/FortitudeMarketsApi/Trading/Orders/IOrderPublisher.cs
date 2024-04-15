#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeMarketsApi.Trading.Orders.Server;

#endregion

namespace FortitudeMarketsApi.Trading.Orders;

public interface IOrderPublisher : IDisposable, IRecyclableObject
{
    IConversationRequester? UnderlyingSession { get; }
    bool Publish(IOrderUpdate orderUpdate);
}
