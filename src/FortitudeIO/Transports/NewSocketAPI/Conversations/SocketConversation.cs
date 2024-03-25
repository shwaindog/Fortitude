#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class SocketConversation : ISocketConversation
{
    private static int nextSessionId;
    protected readonly ISocketSessionContext SocketSessionContext;

    public SocketConversation(ISocketSessionContext socketSessionContext, IStreamControls streamControls)
    {
        socketSessionContext.OwningConversation = this;
        socketSessionContext.StreamControls = streamControls;
        SocketSessionContext = socketSessionContext;
    }

    public ISerdesFactory? SerdesFactory => SocketSessionContext.SerdesFactory;

    public IListener? ConversationListener => SocketSessionContext.SocketReceiver;

    public IPublisher? ConversationPublisher => SocketSessionContext.SocketSender;

    public bool IsStarted => SocketSessionContext.SocketConnection?.IsConnected ?? false;

    public event Action? Started
    {
        add => SocketSessionContext.Connected += value;
        remove => SocketSessionContext.Connected -= value;
    }

    public event Action? Stopped
    {
        add => SocketSessionContext.Disconnected += value;
        remove => SocketSessionContext.Disconnected -= value;
    }

    public event Action<string, int>? Error
    {
        add => SocketSessionContext.Error += value;
        remove => SocketSessionContext.Error -= value;
    }

    public event Action? Connected
    {
        add => SocketSessionContext.Connected += value;
        remove => SocketSessionContext.Connected -= value;
    }

    public event Action? Disconnected
    {
        add => SocketSessionContext.Disconnected += value;
        remove => SocketSessionContext.Disconnected -= value;
    }

    public event Action<SocketSessionState>? StateChanged
    {
        add => SocketSessionContext.StateChanged += value;
        remove => SocketSessionContext.StateChanged -= value;
    }

    public event Action<ISocketConnection>? SocketConnected
    {
        add => SocketSessionContext.SocketConnected += value;
        remove => SocketSessionContext.SocketConnected -= value;
    }

    public event Action? Disconnecting
    {
        add => SocketSessionContext.Disconnecting += value;
        remove => SocketSessionContext.Disconnecting -= value;
    }

    public int Id { get; } = Interlocked.Increment(ref nextSessionId);
    public IConversationSession Session => SocketSessionContext.Session;

    public ConversationType ConversationType { get; set; }

    public string Name
    {
        get => SocketSessionContext.Name;
        set => SocketSessionContext.Name = value;
    }

    public ConversationState ConversationState => SocketSessionContext.ConversationState;

    public void Start()
    {
        SocketSessionContext.StreamControls?.Start();
    }

    public void Stop()
    {
        SocketSessionContext.StreamControls?.Stop();
    }

    public virtual void Connect() => SocketSessionContext.StreamControls?.Connect();

    public void Disconnect() => SocketSessionContext.StreamControls?.Disconnect();

    public void StartMessaging() => SocketSessionContext.StreamControls?.StartMessaging();

    public void StopMessaging() => SocketSessionContext.StreamControls?.StopMessaging();
}
