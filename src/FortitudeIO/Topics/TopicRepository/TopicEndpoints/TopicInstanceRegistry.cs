#region

using FortitudeCommon.EventProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicRepository.Synchronisation;
using FortitudeIO.Topics.TopicRepository.Synchronisation.ORX;

#endregion

namespace FortitudeIO.Topics.TopicRepository.TopicEndpoints;

public delegate void TopicInstanceEvent(ITopicInstanceUpdateMessage topicInstanceUpdateMessage);

public delegate void TopicConnectionEvent(EventType updateType, string topic, ITopicEndpointInfo topicEndpointInfo);

public class TopicInstanceRegistry
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TopicInstanceRegistry));

    private static readonly IInstanceEndpointSet EmptyConnectionDetails = new InstanceEndpointSet();

    private readonly IDictionary<string, IDictionary<ConversationType, IInstanceEndpointSet>>
        topicClientConnectionDetails =
            new Dictionary<string, IDictionary<ConversationType, IInstanceEndpointSet>>();

    public event TopicInstanceEvent? TopicInstanceUpdate;
    public event TopicConnectionEvent? TopicConnectionInstanceUpdate;

    public void OnTopicClientConnectionUpdate(ITopicInstanceUpdateMessage topicInstanceUpdateMessage)
    {
        if ((topicInstanceUpdateMessage.UpdateType & EventType.Deleted) > 0)
            CheckRemoveAndAlertTopics(topicInstanceUpdateMessage);
        if ((topicInstanceUpdateMessage.UpdateType & EventType.Updated) > 0)
            CheckUpdateAndAlertTopics(topicInstanceUpdateMessage, false);
        if ((topicInstanceUpdateMessage.UpdateType & EventType.Created) > 0)
            CheckCreateAndAlertTopics(topicInstanceUpdateMessage);
        if ((topicInstanceUpdateMessage.UpdateType & EventType.Retrieved) > 0)
            DropCreateAndAlertTopics(topicInstanceUpdateMessage);
    }

    public IInstanceEndpointSet FindTopicConfig(string topicName,
        ConversationType subscriptionType)
    {
        if (topicClientConnectionDetails.TryGetValue(topicName, out var topicSubscriptions))
            if (topicSubscriptions.TryGetValue(subscriptionType, out var matchingSubscriptions))
                return matchingSubscriptions;
        return EmptyConnectionDetails;
    }

    private void DropCreateAndAlertTopics(ITopicInstanceUpdateMessage topicInstanceUpdateMessage)
    {
        CheckCreateAndAlertTopics(topicInstanceUpdateMessage);
    }

    private void CheckCreateAndAlertTopics(ITopicInstanceUpdateMessage topicInstanceUpdateMessage)
    {
        foreach (var topicCreatedConnection in topicInstanceUpdateMessage.TopicsAffected!)
        {
            if (!topicClientConnectionDetails.TryGetValue(topicCreatedConnection.TopicName!, out var topicDetails))
            {
                topicDetails = new Dictionary<ConversationType, IInstanceEndpointSet>();
                topicClientConnectionDetails[topicCreatedConnection.TopicName!] = topicDetails;
            }

            if (!topicDetails.TryGetValue(topicCreatedConnection.ConversationType,
                    out var listOfConnectionsOfThatType))
            {
                listOfConnectionsOfThatType = new InstanceEndpointSet(4);
                topicDetails[topicCreatedConnection.ConversationType] = listOfConnectionsOfThatType;
            }

            foreach (var topicEndpointInfo in topicCreatedConnection.Connections!)
                if (!listOfConnectionsOfThatType.Contains(topicEndpointInfo))
                {
                    listOfConnectionsOfThatType.Add(topicEndpointInfo);
                    OnTopicConnectionUpdate(EventType.Created, topicCreatedConnection.TopicName!, topicEndpointInfo);
                }
        }

        OnTopicInstanceUpdate(topicInstanceUpdateMessage);
    }

    private void CheckUpdateAndAlertTopics(ITopicInstanceUpdateMessage topicInstanceUpdateMessage
        , bool removeUnaddressed)
    {
        foreach (var topicUpdatedConnection in topicInstanceUpdateMessage.TopicsAffected!)
        {
            if (!topicClientConnectionDetails.TryGetValue(topicUpdatedConnection.TopicName!, out var topicDetails))
                topicDetails = new Dictionary<ConversationType, IInstanceEndpointSet>();

            if (!topicDetails.TryGetValue(topicUpdatedConnection.ConversationType,
                    out var listOfConnectionsOfThatType))
                listOfConnectionsOfThatType = new InstanceEndpointSet(4);
            var unmentionedConnections = removeUnaddressed ?
                new InstanceEndpointSet(listOfConnectionsOfThatType) :
                EmptyConnectionDetails;
            foreach (var topicEndpointInfo in topicUpdatedConnection.Connections!)
                if (listOfConnectionsOfThatType.Contains(topicEndpointInfo))
                {
                    Logger.Warn("Already had this connection as created in TopicInstanceRegistry {0}"
                        , topicUpdatedConnection);
                }
                else
                {
                    var foundUpdate = false;
                    foreach (var tei in listOfConnectionsOfThatType)
                        if (!tei.Equals(topicEndpointInfo) && tei.EquivalentEndpoint(topicEndpointInfo))
                        {
                            listOfConnectionsOfThatType.Remove(tei);
                            unmentionedConnections.Remove(tei);
                            listOfConnectionsOfThatType.Add(topicEndpointInfo);
                            OnTopicConnectionUpdate(EventType.Updated, topicUpdatedConnection.TopicName!
                                , topicEndpointInfo);
                            foundUpdate = true;
                            break;
                        }

                    if (!foundUpdate)
                    {
                        Logger.Warn("Does not have this connection to update TopicInstanceRegistry {0}",
                            topicUpdatedConnection);
                        listOfConnectionsOfThatType.Add(topicEndpointInfo);
                        OnTopicConnectionUpdate(EventType.Created, topicUpdatedConnection.TopicName!
                            , topicEndpointInfo);
                    }
                }

            if (removeUnaddressed && unmentionedConnections.Any())
                foreach (var unmentionedConnection in unmentionedConnections)
                    OnTopicConnectionUpdate(EventType.Deleted, topicUpdatedConnection.TopicName!
                        , unmentionedConnection);
        }

        OnTopicInstanceUpdate(topicInstanceUpdateMessage);
    }

    private void CheckRemoveAndAlertTopics(ITopicInstanceUpdateMessage topicInstanceUpdateMessage)
    {
        foreach (var topicRemoveConnection in topicInstanceUpdateMessage.TopicsAffected!)
            if (topicClientConnectionDetails.TryGetValue(topicRemoveConnection.TopicName!, out var topicDetails))
            {
                topicDetails = new Dictionary<ConversationType, IInstanceEndpointSet>();

                if (topicDetails.TryGetValue(topicRemoveConnection.ConversationType,
                        out var listOfConnectionsOfThatType))
                {
                    listOfConnectionsOfThatType = new InstanceEndpointSet(4);

                    foreach (var topicEndpointInfo in topicRemoveConnection.Connections!)
                        CheckRemoveEndpoint(topicRemoveConnection.TopicName!, listOfConnectionsOfThatType
                            , topicEndpointInfo, topicRemoveConnection);
                }
            }

        OnTopicInstanceUpdate(topicInstanceUpdateMessage);
    }

    private void CheckRemoveEndpoint(string topicName, IInstanceEndpointSet listOfConnectionsOfThatType,
        ITopicEndpointInfo topicEndpointInfo, OrxTopicClientConnectionDetails topicCreatedConnection)
    {
        if (listOfConnectionsOfThatType.Contains(topicEndpointInfo))
        {
            listOfConnectionsOfThatType.Remove(topicEndpointInfo);
            OnTopicConnectionUpdate(EventType.Deleted, topicName, topicEndpointInfo);
        }
        else
        {
            Logger.Warn("Does not contain had this connection as created in TopicInstanceRegistry {0}",
                topicCreatedConnection);
        }
    }

    protected void OnTopicInstanceUpdate(ITopicInstanceUpdateMessage topicInstanceUpdateMessage)
    {
        TopicInstanceUpdate?.Invoke(topicInstanceUpdateMessage);
    }

    protected void OnTopicConnectionUpdate(EventType updateType, string topic
        , ITopicEndpointInfo topicInstanceUpdateMessage)
    {
        TopicConnectionInstanceUpdate?.Invoke(updateType, topic, topicInstanceUpdateMessage);
    }
}
