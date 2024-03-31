#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.Sockets;

#endregion

namespace FortitudeIO.Transports.Network.State;

public interface ISocketSessionContext : ISocketConversation, IConversationSession
{
    new int Id { get; }
    new string Name { get; set; }
    IStreamControls? StreamControls { get; set; }
    ISocketConversation? OwningConversation { get; set; }
    SocketSessionState SocketSessionState { get; }
    SocketConversationProtocol SocketConversationProtocol { get; }
    INetworkTopicConnectionConfig NetworkTopicConnectionConfig { get; }
    ISocketConnection? SocketConnection { get; }
    ISocketReceiver? SocketReceiver { get; set; }
    ISocketSender? SocketSender { get; set; }
    ISocketDispatcher SocketDispatcher { get; }
    ISocketFactoryResolver SocketFactoryResolver { get; }
    ISerdesFactory SerdesFactory { get; }
    void OnSocketStateChanged(SocketSessionState newSessionState);
    void OnDisconnected();
    void OnReconnecting();
    void OnDisconnecting();
    void OnConnected(ISocketConnection socketConnection);
}

public class SocketSessionContext : ISocketSessionContext
{
    private static int idGen;

    public SocketSessionContext(ConversationType conversationType,
        SocketConversationProtocol socketConversationProtocol,
        string sessionDescription, INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketFactoryResolver socketFactoryResolver, ISerdesFactory serdesFactory,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        SocketFactoryResolver = socketFactoryResolver;
        SocketDispatcher
            = socketDispatcherResolver?.Resolve(networkConnectionConfig) ??
              socketFactoryResolver.SocketDispatcherResolver!.Resolve(networkConnectionConfig);
        SerdesFactory = serdesFactory;
        ConversationType = conversationType;
        SocketConversationProtocol = socketConversationProtocol;
        NetworkTopicConnectionConfig = networkConnectionConfig;
        Name = sessionDescription;
        StateChanged = socketFactoryResolver.ConnectionChangedHandlerResolver!(this)
            .GetOnConnectionChangedHandler();
        Id = Interlocked.Increment(ref idGen);
    }

    public int Id { get; }

    public IStreamControls? StreamControls { get; set; }
    public ISocketConversation? OwningConversation { get; set; }
    public ConversationType ConversationType { get; }
    public SocketConversationProtocol SocketConversationProtocol { get; }
    public ISocketFactoryResolver SocketFactoryResolver { get; }
    public ISerdesFactory SerdesFactory { get; }
    public ISocketDispatcher SocketDispatcher { get; set; }
    public ISocketConnection? SocketConnection { get; private set; }
    public INetworkTopicConnectionConfig NetworkTopicConnectionConfig { get; }
    public SocketSessionState SocketSessionState { get; set; }
    public string Name { get; set; }

    public ISocketReceiver? SocketReceiver { get; set; }

    public ISocketSender? SocketSender { get; set; }

    public event Action<SocketSessionState>? StateChanged;

#pragma warning disable 67
    public event Action<string, int>? Error;
#pragma warning restore 67
    public event Action<ISocketConnection>? SocketConnected;
    public event Action? Connected;
    public event Action? Disconnected;
    public event Action? Disconnecting;
    public event Action? Started;
    public event Action? Stopped;

    public ConversationState ConversationState
    {
        get
        {
            return SocketSessionState switch
            {
                SocketSessionState.New => ConversationState.New
                , SocketSessionState.Connected => ConversationState.Started
                , SocketSessionState.Connecting => ConversationState.Starting
                , SocketSessionState.Disconnecting => ConversationState.Stopping
                , SocketSessionState.Reconnecting => ConversationState.Starting
                , SocketSessionState.Disconnected => ConversationState.Stopped, _ => ConversationState.Stopped
            };
        }
    }

    public void Start()
    {
        StreamControls?.Start();
    }

    public void Stop()
    {
        StreamControls?.Stop();
        OnStopped();
    }

    public bool IsStarted => SocketConnection?.IsConnected ?? false;
    public IConversationSession Session => this;

    public void OnSocketStateChanged(SocketSessionState newSessionState)
    {
        var oldState = SocketSessionState;
        SocketSessionState = newSessionState;
        StateChanged?.Invoke(oldState);
    }

    public void OnConnected(ISocketConnection socketConnection)
    {
        SocketConnection = socketConnection;
        OnSocketStateChanged(SocketSessionState.Connected);
        SocketConnected?.Invoke(socketConnection);
        Connected?.Invoke();
        OnStarted();
    }

    public void OnDisconnected()
    {
        if (SocketSessionState != SocketSessionState.Disconnected)
        {
            OnSocketStateChanged(SocketSessionState.Disconnected);
            Disconnected?.Invoke();
        }

        OnStopped();
    }

    public void OnReconnecting()
    {
        OnSocketStateChanged(SocketSessionState.Reconnecting);
    }

    public void OnDisconnecting()
    {
        if (SocketSessionState != SocketSessionState.Disconnecting)
        {
            if (SocketSessionState != SocketSessionState.Disconnected) OnSocketStateChanged(SocketSessionState.Disconnecting);

            Disconnecting?.Invoke();
        }
    }

    public void OnSessionFailure(string reason) => StreamControls?.OnSessionFailure(reason);

    public void OnStarted()
    {
        Started?.Invoke();
    }

    public void OnStopped()
    {
        Stopped?.Invoke();
    }

    public override string ToString() =>
        $"SocketSessionContext({nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(ConversationType)}: {ConversationType}, " +
        $"{nameof(SocketDispatcher)}: {SocketDispatcher}, {nameof(SocketConnection)}: {SocketConnection}, " +
        $"{nameof(SocketSessionState)}: {SocketSessionState}, " +
        $"{nameof(IsStarted)}: {IsStarted})";
}
