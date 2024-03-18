#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface IRequestResponseResponderTransportTopicConversation : ITransportTopicConversation
{
    IConversationResponder RequestResponseResponderConversation { get; }
}

public class RequestResponseResponderTransportTopicConversation : TransportTopicConversation
    , IRequestResponseResponderTransportTopicConversation
{
    public RequestResponseResponderTransportTopicConversation(ITopicEndpointInfo endpoint,
        IConversationResponder requestResponseResponderConversation) : base(endpoint) =>
        RequestResponseResponderConversation = requestResponseResponderConversation;

    public RequestResponseResponderTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IConversationResponder requestResponseResponderConversation) : base(connectionConfig) =>
        RequestResponseResponderConversation = requestResponseResponderConversation;

    public IConversationResponder RequestResponseResponderConversation { get; }
}
