#region

using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messaging;

#endregion

namespace FortitudeBusRules.Rules.Serdes;

public class TopicDeserializationRepositoryAmendingRule : Rule
{
    private ISubscription? listenForPublishSubscriptions;

    public TopicDeserializationRepositoryAmendingRule(string topic, IEventPublishingDeserializer eventPublishingDeserializer)
    {
        Topic = topic;
        EventPublishingDeserializer = eventPublishingDeserializer;
    }

    private string Topic { get; }
    private IEventPublishingDeserializer EventPublishingDeserializer { get; }

    public override ValueTask StartAsync()
    {
        listenForPublishSubscriptions = Context.EventBus.RegisterListener<EventContextPublishRegistration>(this, Topic, UpdateRequest);
        return ValueTask.CompletedTask;
    }

    private void UpdateRequest(IMessage<EventContextPublishRegistration> contextPublishRegistrationMsg)
    {
        var eventContextPublishRegistration = contextPublishRegistrationMsg.PayLoad.Body!;
        if (eventContextPublishRegistration.AddRemoveCommand == AddRemoveCommand.Add)
            EventPublishingDeserializer.IncrementPublishToContext(eventContextPublishRegistration.EventContext);
        else
            EventPublishingDeserializer.DecrementPublishToContext(eventContextPublishRegistration.EventContext);
    }
}
