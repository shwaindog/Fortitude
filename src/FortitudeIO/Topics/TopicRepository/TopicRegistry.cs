﻿namespace FortitudeIO.Topics.TopicRepository;

public interface ITopicRegistry
{
    void RegisterTopic(Topic topic);
    void Remove(Topic topic);
    PublisherTopic? SupplyPublisherTopic(string topicName, string instanceName);
    ConversationSubscriberTopic? SupplySubscriberTopic(string topicName, string instanceName);
    RequesterTopic? SupplyRequestResponseRequesterTopic(string topicName, string instanceName);
    ResponderTopic? SupplyRequestResponseResponderTopic(string topicName, string instanceName);
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
        if (generatedTopics.TryGetValue(topic.Name, out var instanceDict)) instanceDict.Remove(topic.InstanceName);
    }

    public PublisherTopic? SupplyPublisherTopic(string topicName, string instanceName)
    {
        if (generatedTopics.TryGetValue(topicName, out var findTopic)) return (PublisherTopic)findTopic[instanceName];
        return null;
    }

    public ConversationSubscriberTopic? SupplySubscriberTopic(string topicName, string instanceName)
    {
        if (generatedTopics.TryGetValue(topicName, out var findTopic))
            return (ConversationSubscriberTopic)findTopic[instanceName];
        return null;
    }

    public RequesterTopic? SupplyRequestResponseRequesterTopic(string topicName, string instanceName)
    {
        if (generatedTopics.TryGetValue(topicName, out var findTopic))
            return (RequesterTopic)findTopic[instanceName];
        return null;
    }

    public ResponderTopic? SupplyRequestResponseResponderTopic(string topicName, string instanceName)
    {
        if (generatedTopics.TryGetValue(topicName, out var findTopic))
            return (ResponderTopic)findTopic[instanceName];
        return null;
    }
}
