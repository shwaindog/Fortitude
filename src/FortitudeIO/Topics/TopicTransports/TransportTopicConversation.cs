#region

using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface ITransportTopicConversation
{
    ITopicEndpointInfo? Endpoint { get; }
    ITopicConnectionConfig? ConnectionConfig { get; }
}

public class TransportTopicConversation : ITransportTopicConversation
{
    public TransportTopicConversation(ITopicConnectionConfig topicConnectionConfig) =>
        TopicConnectionConfig = topicConnectionConfig;

    public TransportTopicConversation(ITopicEndpointInfo endpoint) => Endpoint = endpoint;

    public ITopicConnectionConfig? TopicConnectionConfig { get; }

    public ITopicEndpointInfo? Endpoint { get; }
    public ITopicConnectionConfig? ConnectionConfig { get; }
}
