#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class EventContextPublishRegistration
{
    public EventContextPublishRegistration(IEventContext eventContext, AddRemoveCommand addRemoveCommand)
    {
        EventContext = eventContext;
        AddRemoveCommand = addRemoveCommand;
    }

    public IEventContext EventContext { get; }

    public string FullTopicAddress { get; }

    public AddRemoveCommand AddRemoveCommand { get; set; }
}
