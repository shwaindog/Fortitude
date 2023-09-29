using System.Collections.Generic;

namespace FortitudeIO.Topics.TopicRepository
{
    public interface ITopicRegistry
    {
        void RegisterTopic(Topic topic);
        void Remove(Topic topic);
        PublisherTopic SupplyPublisherTopic(string topicName, string instanceName);
        SubscriberTopic SupplySubscriberTopic(string topicName, string instanceName);
        RequestResponseRequesterTopic SupplyRequestResponseRequesterTopic(string topicName, string instanceName);
        RequestResponseResponderTopic SupplyRequestResponseResponderTopic(string topicName, string instanceName);
    }

    public class TopicRegistry : ITopicRegistry
    {
        private readonly IDictionary<string, IDictionary<string, Topic>> generatedTopics = 
            new Dictionary<string, IDictionary<string, Topic>>();

        public void RegisterTopic(Topic topic)
        {
            if (!generatedTopics.TryGetValue(topic.Name, out var instanceDict))
            {
                instanceDict = new Dictionary<string, Topic>();
                generatedTopics.Add(topic.Name, instanceDict);
            }
            instanceDict[topic.InstanceName] = topic;
        }

        public void Remove(Topic topic)
        {
            if (generatedTopics.TryGetValue(topic.Name, out var instanceDict))
            {
                instanceDict.Remove(topic.InstanceName);
            }
        }

        public PublisherTopic SupplyPublisherTopic(string topicName, string instanceName)
        {
            if (generatedTopics.TryGetValue(topicName, out var findTopic))
            {
                return (PublisherTopic)findTopic[instanceName];
            }
            return null;
        }

        public SubscriberTopic SupplySubscriberTopic(string topicName, string instanceName)
        {
            if (generatedTopics.TryGetValue(topicName, out var findTopic))
            {
                return (SubscriberTopic)findTopic[instanceName];
            }
            return null;
        }

        public RequestResponseRequesterTopic SupplyRequestResponseRequesterTopic(string topicName, string instanceName)
        {
            if (generatedTopics.TryGetValue(topicName, out var findTopic))
            {
                return (RequestResponseRequesterTopic)findTopic[instanceName];
            }
            return null;
        }

        public RequestResponseResponderTopic SupplyRequestResponseResponderTopic(string topicName, string instanceName)
        {
            if (generatedTopics.TryGetValue(topicName, out var findTopic))
            {
                return (RequestResponseResponderTopic)findTopic[instanceName];
            }
            return null;
        }
    }
}
