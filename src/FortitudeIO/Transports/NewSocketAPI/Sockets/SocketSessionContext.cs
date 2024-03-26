#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Receiving;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketSessionContext : ISocketConversation, IConversationSession
{
    new int Id { get; }
    new string Name { get; set; }
    IStreamControls? StreamControls { get; set; }
    ISocketConversation? OwningConversation { get; set; }
    SocketSessionState SocketSessionState { get; }
    SocketConversationProtocol SocketConversationProtocol { get; }
    ISocketTopicConnectionConfig SocketTopicConnectionConfig { get; }
    ISocketConnection? SocketConnection { get; }
    ISocketReceiver? SocketReceiver { get; set; }
    ISocketSender? SocketSender { get; set; }
    ISocketDispatcher SocketDispatcher { get; }
    ISocketFactories SocketFactories { get; }
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
        string sessionDescription, ISocketTopicConnectionConfig socketConnectionConfig,
        ISocketFactories socketFactories, ISerdesFactory serdesFactory,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        SocketFactories = socketFactories;
        SocketDispatcher
            = socketDispatcherResolver?.Resolve(socketConnectionConfig) ??
              socketFactories.SocketDispatcherResolver!.Resolve(socketConnectionConfig);
        SerdesFactory = serdesFactory;
        ConversationType = conversationType;
        SocketConversationProtocol = socketConversationProtocol;
        SocketTopicConnectionConfig = socketConnectionConfig;
        Name = sessionDescription;
        StateChanged = socketFactories.ConnectionChangedHandlerResolver!(this)
            .GetOnConnectionChangedHandler();
        Id = Interlocked.Increment(ref idGen);
    }

    public int Id { get; }

    public IStreamControls? StreamControls { get; set; }
    public ISocketConversation? OwningConversation { get; set; }
    public ConversationType ConversationType { get; }
    public SocketConversationProtocol SocketConversationProtocol { get; }
    public ISocketFactories SocketFactories { get; }
    public ISerdesFactory SerdesFactory { get; }
    public ISocketDispatcher SocketDispatcher { get; set; }
    public ISocketConnection? SocketConnection { get; private set; }
    public ISocketTopicConnectionConfig SocketTopicConnectionConfig { get; }
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
        OnSocketStateChanged(SocketSessionState.Disconnected);
        Disconnected?.Invoke();
        OnStopped();
    }

    public void OnReconnecting()
    {
        OnSocketStateChanged(SocketSessionState.Reconnecting);
    }

    public void OnDisconnecting()
    {
        OnSocketStateChanged(SocketSessionState.Disconnecting);
        Disconnecting?.Invoke();
    }

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
