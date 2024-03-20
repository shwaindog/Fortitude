#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class SocketConversation : ISocketConversation
{
    protected readonly IStreamControls InitiateControls;
    protected readonly ISocketSessionContext SocketSessionContext;

    public SocketConversation(ISocketSessionContext socketSessionContext, IStreamControls initiateControls)
    {
        SocketSessionContext = socketSessionContext;
        InitiateControls = initiateControls;
    }

    public ISerdesFactory? SerdesFactory => SocketSessionContext.SerdesFactory;

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

    public ConversationType ConversationType { get; set; }

    public string Name
    {
        get => SocketSessionContext.Name;
        set => SocketSessionContext.Name = value;
    }

    public ConversationState ConversationState => SocketSessionContext.ConversationState;

    public IConversationListener? ConversationListener => SocketSessionContext.SocketReceiver;

    public IConversationPublisher? ConversationPublisher => SocketSessionContext.SocketSender;

    public void Start()
    {
        InitiateControls.Connect();
    }

    public void Stop()
    {
        InitiateControls.Disconnect();
    }

    public virtual void Connect() => InitiateControls.Connect();

    public void Disconnect() => InitiateControls.Disconnect();

    public void StartMessaging() => InitiateControls.StartMessaging();

    public void StopMessaging() => InitiateControls.StopMessaging();
}
