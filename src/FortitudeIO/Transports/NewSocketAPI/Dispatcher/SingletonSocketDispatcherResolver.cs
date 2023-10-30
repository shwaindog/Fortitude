#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherResolver
{
    ISocketDispatcher Resolve(ISocketSessionContext socketSessionContext);
}

public class SingletonSocketDispatcherResolver : ISocketDispatcherResolver
{
    private readonly string descriptionPostFix;
    private readonly IOSNetworkingController networkingController;
    private readonly int timeoutMs;

    public SingletonSocketDispatcherResolver(IOSNetworkingController networkingController,
        int timeoutMs = 500, string descriptionPostFix = "_Dispatcher")
    {
        this.timeoutMs = timeoutMs;
        this.descriptionPostFix = descriptionPostFix;
        this.networkingController = networkingController ?? new OSNetworkingController();
    }

    public ISocketDispatcher Resolve(ISocketSessionContext socketSessionContext) =>
        new SocketDispatcher(
            new SocketDispatcherListener(new SocketSelector(timeoutMs, networkingController)
                , socketSessionContext.ConversationDescription),
            new SocketDispatcherSender(socketSessionContext.ConversationDescription + descriptionPostFix));
}
