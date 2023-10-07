#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketSessionContext : ISocketConversation
{
    int Id { get; }
    new string ConversationDescription { get; set; }
    SocketSessionState SocketSessionState { get; }
    SocketConversationProtocol SocketConversationProtocol { get; }
    ISocketConnectionConfig SocketConnectionConfig { get; }
    ISocketConnection? SocketConnection { get; }
    ISocketReceiver? SocketReceiver { get; set; }
    ISocketSender? SocketSender { get; set; }
    ISocketDispatcher SocketDispatcher { get; }
    ISocketFactories SocketFactories { get; }
    ISerdesFactory SerdesFactory { get; }
    event Action<SocketSessionState>? StateChanged;
    event Action<ISocketConnection>? SocketConnected;
    event Action? Disconnecting;
    void OnSocketStateChanged(SocketSessionState newSessionState);
    void OnDisconnected();
    void OnDisconnecting();
    void OnConnected(ISocketConnection socketConnection);
}

public class SocketSessionContext : ISocketSessionContext
{
    private static int idGen;
    private ISocketDispatcher? socketDispatcher;

    public SocketSessionContext(ConversationType conversationType,
        SocketConversationProtocol socketConversationProtocol,
        string sessionDescription, ISocketConnectionConfig socketConnectionConfig,
        ISocketFactories socketFactories, ISerdesFactory serdesFactory)
    {
        SocketFactories = socketFactories;
        SerdesFactory = serdesFactory;
        ConversationType = conversationType;
        SocketConversationProtocol = socketConversationProtocol;
        SocketConnectionConfig = socketConnectionConfig;
        ConversationDescription = sessionDescription;
        StateChanged = socketFactories.ConnectionChangedHandlerResolver!(this)
            .GetOnConnectionChangedHandler();
        Id = Interlocked.Increment(ref idGen);
    }

    public int Id { get; }
    public ConversationType ConversationType { get; }
    public SocketConversationProtocol SocketConversationProtocol { get; }
    public ISocketFactories SocketFactories { get; }
    public ISerdesFactory SerdesFactory { get; }

    public ISocketDispatcher SocketDispatcher
    {
        get => socketDispatcher ??= SocketFactories.SocketDispatcherResolver!.Resolve(this);
        set => socketDispatcher = value;
    }

    public ISocketConnection? SocketConnection { get; private set; }
    public ISocketConnectionConfig SocketConnectionConfig { get; }
    public SocketSessionState SocketSessionState { get; set; }
    public string ConversationDescription { get; set; }
    public ISocketReceiver? SocketReceiver { get; set; }
    IConversationListener? ISocketConversation.ConversationListener => SocketReceiver;
    public ISocketSender? SocketSender { get; set; }
    IConversationPublisher? ISocketConversation.ConversationPublisher => SocketSender;

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
            switch (SocketSessionState)
            {
                case SocketSessionState.New: return ConversationState.New;
                case SocketSessionState.Connected: return ConversationState.Started;
                case SocketSessionState.Connecting: return ConversationState.Starting;
                case SocketSessionState.Disconnecting: return ConversationState.Stopping;
                case SocketSessionState.Reconnecting: return ConversationState.Starting;
                case SocketSessionState.Disconnected: return ConversationState.Stopped;
                default: return ConversationState.Stopped;
            }
        }
    }

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
}
