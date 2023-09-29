using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher
{
    public interface ISocketDispatcherResolver
    {
        ISocketDispatcher Resolve(ISocketSessionContext socketSessionContext);
    }
    public class SingletonSocketDispatcherResolver : ISocketDispatcherResolver
    {
        private readonly int timeoutMs;
        private readonly string descriptionPostFix;
        private IOSNetworkingController networkingController;

        public SingletonSocketDispatcherResolver(IOSNetworkingController networkingController = null,
            int timeoutMs = 500, string descriptionPostFix = "_Dispatcher")
        {
            this.timeoutMs = timeoutMs;
            this.descriptionPostFix = descriptionPostFix;
            this.networkingController = networkingController ?? new OSNetworkingController();
        }

        public ISocketDispatcher Resolve(ISocketSessionContext socketSessionContext)
        {
            return new SocketDispatcher(new SocketDispatcherListener(new SocketSelector(timeoutMs, networkingController), socketSessionContext.ConversationDescription),
                new SocketDispatcherSender(socketSessionContext.ConversationDescription + descriptionPostFix));
        }
    }
}
