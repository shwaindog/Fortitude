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

    public DispatchOptions()
    {
        RoutingFlags = RoutingFlags.DefaultPublish;
        TargetEventQueueType = EventQueueType.All;
    }

    public DispatchOptions(RoutingFlags routingFlags
            = RoutingFlags.DefaultPublish
        , EventQueueType targetEventQueueType = EventQueueType.All, IRule? targetRule = null)
    {
        RoutingFlags = routingFlags;
        TargetEventQueueType = targetEventQueueType;
        TargetRule = targetRule;
    }

    public static bool operator ==(DispatchOptions lhs, DispatchOptions rhs) =>
        Equals(lhs.TargetEventQueueType, rhs.TargetEventQueueType) &&
        Equals(lhs.RoutingFlags, rhs.RoutingFlags) && Equals(lhs.TargetRule, rhs.TargetRule);

    public static bool operator !=(DispatchOptions lhs, DispatchOptions rhs) => !(lhs == rhs);

    public bool Equals(DispatchOptions other) =>
        TargetEventQueueType == other.TargetEventQueueType && RoutingFlags == other.RoutingFlags &&
        Equals(TargetRule, other.TargetRule);

    public override bool Equals(object? obj) => obj is DispatchOptions other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)TargetEventQueueType, (int)RoutingFlags, TargetRule);
}
