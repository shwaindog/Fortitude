#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface IRequestResponseRequesterTransportTopicConversation : ITransportTopicConversation
{
    IConversationRequester RequestResponseRequesterConversation { get; }
}

public class RequestResponseRequesterTransportTopicConversation : TransportTopicConversation
    , IRequestResponseRequesterTransportTopicConversation
{
    public RequestResponseRequesterTransportTopicConversation(ITopicEndpointInfo endpoint,
        IConversationRequester requestResponseRequesterConversation) : base(endpoint) =>
        RequestResponseRequesterConversation = requestResponseRequesterConversation;

    public RequestResponseRequesterTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IConversationRequester requestResponseRequesterConversation) : base(connectionConfig) =>
        RequestResponseRequesterConversation = requestResponseRequesterConversation;

    public IConversationRequester RequestResponseRequesterConversation { get; }
}
