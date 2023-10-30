#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface IRequestResponseRequesterTransportTopicConversation : ITransportTopicConversation
{
    IRequestResponseRequesterConversation RequestResponseRequesterConversation { get; }
}

public class RequestResponseRequesterTransportTopicConversation : TransportTopicConversation
    , IRequestResponseRequesterTransportTopicConversation
{
    public RequestResponseRequesterTransportTopicConversation(ITopicEndpointInfo endpoint,
        IRequestResponseRequesterConversation requestResponseRequesterConversation) : base(endpoint) =>
        RequestResponseRequesterConversation = requestResponseRequesterConversation;

    public RequestResponseRequesterTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IRequestResponseRequesterConversation requestResponseRequesterConversation) : base(connectionConfig) =>
        RequestResponseRequesterConversation = requestResponseRequesterConversation;

    public IRequestResponseRequesterConversation RequestResponseRequesterConversation { get; }
}
