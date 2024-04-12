#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Rules;

public readonly struct DeploymentOptions
{
    public readonly EventQueueType EventGroupType;
    public readonly RoutingFlags RoutingFlags;
    public readonly uint Instances;
    public readonly string? SpecificEventQueueName = null;
    public readonly int TimeoutMs = 60_000;

    public DeploymentOptions()
    {
        RoutingFlags = RoutingFlags.DefaultDeploy;
        EventGroupType = EventQueueType.Event;
        Instances = 1;
    }

    public DeploymentOptions(RoutingFlags routingFlags = RoutingFlags.DefaultDeploy
        , EventQueueType eventGroupType = EventQueueType.Event
        , uint instances = 1, string? specificEventQueueName = null, int timeoutMs = 60_000)
    {
        Instances = instances;
        EventGroupType = eventGroupType;
        SpecificEventQueueName = specificEventQueueName;
        TimeoutMs = timeoutMs;
        RoutingFlags = routingFlags;
    }
}

public static class DeploymentOptionsExtensions
{
    public static DeploymentOptions Copy(this DeploymentOptions original
        , RoutingFlags xorRoutingFlags = RoutingFlags.None
        , EventQueueType xorEventGroupType = EventQueueType.None, uint? instances = null
        , string? specificEventQueueName = null) =>
        new(xorRoutingFlags ^ original.RoutingFlags, xorEventGroupType ^ original.EventGroupType
            , instances ?? original.Instances
            , specificEventQueueName ?? original.SpecificEventQueueName);

    public static bool RequiresSpecificEventQueue(this DeploymentOptions check) => check.SpecificEventQueueName.IsNotNullOrEmpty();
}
