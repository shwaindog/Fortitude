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
    IStreamControlsFactory StreamControlsFactory { get; }
}

public class SocketFactories : ISocketFactories
{
    public SocketFactories()
    {
        SocketFactory = new SocketFactory(NetworkingController);
        SocketSenderFactory = new SocketSenderFactory(NetworkingController.DirectOSNetworkingApi);
    }

    public ISocketFactory SocketFactory { get; set; }
    public ISocketReceiverFactory SocketReceiverFactory { get; set; } = new SocketReceiverFactory();
    public ISocketSenderFactory SocketSenderFactory { get; set; }
    public IOSNetworkingController NetworkingController { get; set; } = new OSNetworkingController();
    public ISocketDispatcherResolver SocketDispatcherResolver { get; set; } = new SingletonSocketDispatcherResolver();
    public Func<ISocketSessionContext, ISocketConnectivityChanged>? ConnectionChangedHandlerResolver { get; set; }
    public IOSParallelController ParallelController { get; set; } = new OSParallelController();
    public IStreamControlsFactory StreamControlsFactory { get; set; } = new StreamControlsFactory();

    public static SocketFactories GetRealSocketFactories()
    {
        var networkingController = new OSNetworkingController();
        var socketFactories = new SocketFactories
        {
            NetworkingController = networkingController
            , SocketFactory = new SocketFactory(networkingController)
            , SocketReceiverFactory = new SocketReceiverFactory()
            , SocketSenderFactory = new SocketSenderFactory(networkingController.DirectOSNetworkingApi)
            , SocketDispatcherResolver = new SingletonSocketDispatcherResolver()
            , ParallelController = new OSParallelController()
            , StreamControlsFactory = new StreamControlsFactory()
        };

        ISocketConnectivityChanged ConnectionChangedFactory(ISocketSessionContext sessionOperator) =>
            new SocketStateChangeHandler(sessionOperator);

        socketFactories.ConnectionChangedHandlerResolver = ConnectionChangedFactory;
        return socketFactories;
    }
}
