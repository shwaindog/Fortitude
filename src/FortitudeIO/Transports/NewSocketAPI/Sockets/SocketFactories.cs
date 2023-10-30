#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Receiving;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketFactories
{
    IOSNetworkingController? NetworkingController { get; }
    ISocketFactory? SocketFactory { get; }
    ISocketReceiverFactory? SocketReceiverFactory { get; }
    ISocketSenderFactory? SocketSenderFactory { get; }
    ISocketDispatcherResolver? SocketDispatcherResolver { get; }
    Func<ISocketSessionContext, ISocketConnectivityChanged>? ConnectionChangedHandlerResolver { get; }
    IOSParallelController? ParallelController { get; }
}

public class SocketFactories : ISocketFactories
{
    public ISocketFactory? SocketFactory { get; set; }
    public ISocketReceiverFactory? SocketReceiverFactory { get; set; }
    public ISocketSenderFactory? SocketSenderFactory { get; set; }
    public IOSNetworkingController? NetworkingController { get; set; }
    public ISocketDispatcherResolver? SocketDispatcherResolver { get; set; }
    public Func<ISocketSessionContext, ISocketConnectivityChanged>? ConnectionChangedHandlerResolver { get; set; }
    public IOSParallelController? ParallelController { get; set; }

    public static SocketFactories GetRealSocketFactories()
    {
        var networkingController = new OSNetworkingController();
        var socketFactories = new SocketFactories
        {
            NetworkingController = networkingController
            , SocketFactory = new Controls.SocketFactory(networkingController)
            , SocketReceiverFactory = new SocketReceiverFactory()
            , SocketSenderFactory = new SocketSenderFactory(networkingController.DirectOSNetworkingApi)
            , SocketDispatcherResolver = new SingletonSocketDispatcherResolver(networkingController)
            , ParallelController = new OSParallelController()
        };

        ISocketConnectivityChanged ConnectionChangedFactory(ISocketSessionContext sessionOperator) =>
            new SocketStateChangeHandler(sessionOperator);

        socketFactories.ConnectionChangedHandlerResolver = ConnectionChangedFactory;
        return socketFactories;
    }
}
