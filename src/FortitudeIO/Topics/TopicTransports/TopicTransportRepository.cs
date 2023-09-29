using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.EventProcessing;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicRepository.TopicEndpoints;
using FortitudeIO.Transports;

namespace FortitudeIO.Topics.TopicTransports
{
    public class TopicTransportRepository
    {
        private TopicInstanceRegistry topicInstanceRegistry;

        private class ConverterCallbackPair

        public TopicTransportRepository(TopicInstanceRegistry topicInstanceRegistry)
        {
            this.topicInstanceRegistry = topicInstanceRegistry;

            topicInstanceRegistry.TopicConnectionInstanceUpdate += TopicInstanceRegistry_TopicConnectionInstanceUpdate;

        }

        private void TopicInstanceRegistry_TopicConnectionInstanceUpdate(EventType updateType, string topicName, ITopicEndpointInfo topicEndpointInfo)
        {
            if (updateType == EventType.Created)
            {
                if (started.TryGetValue(topicName, out var topicDict))
                {
                    if (topicDict.TryGetValue(topicEndpointInfo.ConversationType, out var registeredCallback))
                    {
                        registeredCallback?.Invoke();
                    }
                }
            }
        }

        private IDictionary<string, IDictionary<ConversationType, ITopic>> started;
        private IDictionary<string, IDictionary<ConversationType, ITopic>> changed;
        private IDictionary<string, IDictionary<ConversationType, ITopic>> stopped;


    }
}
