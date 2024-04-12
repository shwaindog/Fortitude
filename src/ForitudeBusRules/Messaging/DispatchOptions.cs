#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Messaging;

public readonly struct DispatchOptions
{
    public readonly EventQueueType TargetEventQueueType;
    public readonly RoutingFlags RoutingFlags;
    public readonly IRule? TargetRule;
    public readonly int TimeoutMs = 60_000;

    public DispatchOptions()
    {
        RoutingFlags = RoutingFlags.DefaultPublish;
        TargetEventQueueType = EventQueueType.All;
    }

    public DispatchOptions(RoutingFlags routingFlags
            = RoutingFlags.DefaultPublish
        , EventQueueType targetEventQueueType = EventQueueType.All, IRule? targetRule = null, int timeoutMs = 60_000)
    {
        RoutingFlags = routingFlags;
        TargetEventQueueType = targetEventQueueType;
        TargetRule = targetRule;
        TimeoutMs = timeoutMs;
    }

    public static bool operator ==(DispatchOptions lhs, DispatchOptions rhs) =>
        Equals(lhs.TargetEventQueueType, rhs.TargetEventQueueType) &&
        Equals(lhs.RoutingFlags, rhs.RoutingFlags) && Equals(lhs.TargetRule, rhs.TargetRule) && lhs.TimeoutMs == rhs.TimeoutMs;

    public static bool operator !=(DispatchOptions lhs, DispatchOptions rhs) => !(lhs == rhs);

    public bool Equals(DispatchOptions other) =>
        TargetEventQueueType == other.TargetEventQueueType && RoutingFlags == other.RoutingFlags &&
        Equals(TargetRule, other.TargetRule) && TimeoutMs == other.TimeoutMs;

    public override bool Equals(object? obj) => obj is DispatchOptions other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)TargetEventQueueType, (int)RoutingFlags, TargetRule, TimeoutMs);
}
