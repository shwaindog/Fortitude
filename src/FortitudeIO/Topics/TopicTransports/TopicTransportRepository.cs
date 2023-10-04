#region

using System.Collections.Generic;
using FortitudeCommon.EventProcessing;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicRepository.TopicEndpoints;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Topics.TopicTransports
{
    public class TopicTransportRepository
    {
        private IDictionary<string, IDictionary<ConversationType, ITopic>> changed;

        private IDictionary<string, IDictionary<ConversationType, ITopic>> started;
        private IDictionary<string, IDictionary<ConversationType, ITopic>> stopped;
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
}
