#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ICanCarrySocketSenderPayload : ICanCarryTaskCallbackPayload
{
    bool IsSocketSenderItem { get; }
    ISocketSender? SocketSender { get; }
    void SetAsSocketSenderItem(ISocketSender socketSender);
}
