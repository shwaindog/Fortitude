#region

using FortitudeCommon.EventProcessing;
using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicRepository.TopicEndpoints;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public class TopicTransportRepository
{
    private readonly IDictionary<string, IDictionary<ConversationType, ITopic>> started
        = new Dictionary<string, IDictionary<ConversationType, ITopic>>();

    private IDictionary<string, IDictionary<ConversationType, ITopic>> changed
        = new Dictionary<string, IDictionary<ConversationType, ITopic>>();

    private IDictionary<string, IDictionary<ConversationType, ITopic>> stopped
        = new Dictionary<string, IDictionary<ConversationType, ITopic>>();

    private TopicInstanceRegistry topicInstanceRegistry;

    public TopicTransportRepository(TopicInstanceRegistry topicInstanceRegistry)
    {
        this.topicInstanceRegistry = topicInstanceRegistry;

        topicInstanceRegistry.TopicConnectionInstanceUpdate += TopicInstanceRegistry_TopicConnectionInstanceUpdate;
    }

    private void TopicInstanceRegistry_TopicConnectionInstanceUpdate(EventType updateType, string topicName
        , ITopicEndpointInfo topicEndpointInfo)
    {
        if (updateType == EventType.Created)
            if (started.TryGetValue(topicName, out var topicDict))
                if (topicDict.TryGetValue(topicEndpointInfo.ConversationType, out var registeredCallback))
                    registeredCallback?.Start();
    }

    private class ConverterCallbackPair { }
}
