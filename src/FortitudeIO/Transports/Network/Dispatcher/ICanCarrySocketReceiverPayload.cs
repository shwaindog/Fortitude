#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ICanCarrySocketReceiverPayload : ICanCarryTaskCallbackPayload
{
    bool IsSocketReceiverItem { get; }
    bool IsSocketAdd { get; }
    ISocketReceiver? SocketReceiver { get; }
    void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd);
}
