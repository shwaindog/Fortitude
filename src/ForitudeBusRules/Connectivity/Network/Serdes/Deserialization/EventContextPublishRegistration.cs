#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class EventContextPublishRegistration
{
    public EventContextPublishRegistration(IEventContext eventContext, AddRemoveCommand addRemoveCommand, string requestedFullTopicAddress)
    {
        EventContext = eventContext;
        AddRemoveCommand = addRemoveCommand;
        FullTopicAddress = requestedFullTopicAddress;
    }

    public IEventContext EventContext { get; }

    public string FullTopicAddress { get; }

    public AddRemoveCommand AddRemoveCommand { get; set; }
}
