#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Rules;

public readonly struct DeploymentOptions
{
    public readonly MessageQueueType MessageGroupType;
    public readonly RoutingFlags RoutingFlags;
    public readonly uint Instances;
    public readonly string? SpecificEventQueueName = null;
    public readonly int TimeoutMs = 60_000;

    public DeploymentOptions()
    {
        RoutingFlags = RoutingFlags.DefaultDeploy;
        MessageGroupType = MessageQueueType.Event;
        Instances = 1;
    }

    public DeploymentOptions(RoutingFlags routingFlags = RoutingFlags.DefaultDeploy
        , MessageQueueType messageGroupType = MessageQueueType.Event
        , uint instances = 1, string? specificEventQueueName = null, int timeoutMs = 60_000)
    {
        Instances = instances;
        MessageGroupType = messageGroupType;
        SpecificEventQueueName = specificEventQueueName;
        TimeoutMs = timeoutMs;
        RoutingFlags = routingFlags;
    }
}

public static class DeploymentOptionsExtensions
{
    public static DeploymentOptions Copy(this DeploymentOptions original
        , RoutingFlags xorRoutingFlags = RoutingFlags.None
        , MessageQueueType xorMessageGroupType = MessageQueueType.None, uint? instances = null
        , string? specificEventQueueName = null) =>
        new(xorRoutingFlags ^ original.RoutingFlags, xorMessageGroupType ^ original.MessageGroupType
            , instances ?? original.Instances
            , specificEventQueueName ?? original.SpecificEventQueueName);

    public static bool RequiresSpecificEventQueue(this DeploymentOptions check) => check.SpecificEventQueueName.IsNotNullOrEmpty();
}
