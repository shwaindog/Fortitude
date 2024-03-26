#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Construction;

public interface ISocketReceiverFactory
{
    bool HasConversationListener(ConversationType conversationType);
    SocketReceiver GetConversationListener(ISocketSessionContext transportConversation);
}

public class SocketReceiverFactory : ISocketReceiverFactory
{
    private readonly IStreamDecoderFactory? decoderFactory;

    public SocketReceiverFactory(IStreamDecoderFactory? decoderFactory = null) => this.decoderFactory = decoderFactory;

    public bool HasConversationListener(ConversationType conversationType) =>
        conversationType == ConversationType.Subscriber
        || conversationType == ConversationType.Requester
        || conversationType == ConversationType.Responder;

    public SocketReceiver GetConversationListener(ISocketSessionContext transportConversation) =>
        new(transportConversation);
}
