#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Rules;

public readonly struct DeploymentOptions
{
    public readonly EventQueueType EventGroupType;
    public readonly RoutingStrategySelectionFlags RoutingStrategySelectionFlags;
    public readonly uint Instances;
    public readonly string? SpecificEventQueueName = null;

    public DeploymentOptions(uint instances = 1, EventQueueType eventGroupType = EventQueueType.Event
        , RoutingStrategySelectionFlags routingStrategySelectionFlags
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue
        , string? specificEventQueueName = null)
    {
        Instances = instances;
        EventGroupType = eventGroupType;
        SpecificEventQueueName = specificEventQueueName;
        RoutingStrategySelectionFlags = routingStrategySelectionFlags;
    }
}

public static class DeploymentOptionsExtensions
{
    public static DeploymentOptions Copy(this DeploymentOptions original, uint? instances = null
        , EventQueueType xorEventGroupType = EventQueueType.None
        , RoutingStrategySelectionFlags xorRoutingStrategySelectionFlags = RoutingStrategySelectionFlags.None
        , string? specificEventQueueName = null) =>
        new(instances ?? original.Instances, xorEventGroupType ^ original.EventGroupType
            , xorRoutingStrategySelectionFlags ^ original.RoutingStrategySelectionFlags
            , specificEventQueueName ?? original.SpecificEventQueueName);

    public static bool RequiresSpecificEventQueue(this DeploymentOptions check) =>
        check.SpecificEventQueueName.IsNotNullOrEmpty();
}
