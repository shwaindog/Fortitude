#region

using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ISocketDispatcherSender : ISocketDispatcherCommon
{
    void EnqueueSocketSender(ISocketSender socketSender);
}
