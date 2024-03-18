#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class SocketConversation : ISocketConversation
{
    private readonly IStreamControls initiateControls;
    protected readonly ISocketSessionContext SocketSessionContext;

    public SocketConversation(ISocketSessionContext socketSessionContext, IStreamControls initiateControls)
    {
        SocketSessionContext = socketSessionContext;
        this.initiateControls = initiateControls;
    }

    public ISerdesFactory? SerdesFactory => SocketSessionContext.SerdesFactory;

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
        initiateControls.Connect();
    }

    public void Stop()
    {
        initiateControls.Disconnect();
    }

    public void Connect() => initiateControls.Connect();

    public void Disconnect() => initiateControls.Disconnect();

    public void StartMessaging() => initiateControls.StartMessaging();

    public void StopMessaging() => initiateControls.StopMessaging();
}
