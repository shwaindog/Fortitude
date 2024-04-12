#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ISocketDispatcherListener : ISocketDispatcherCommon
{
    void RegisterForListen(ISocketReceiver receiver);
    void UnregisterForListen(ISocketReceiver receiver);
    void RegisterForListen(IStreamListener receiver);
    void UnregisterForListen(IStreamListener receiver);
}
