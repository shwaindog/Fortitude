#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Messages;

public readonly struct DispatchOptions
{
    public readonly MessageQueueType TargetMessageQueueType;
    public readonly PayloadMarshalOptions PayloadMarshalOptions;
    public readonly RoutingFlags RoutingFlags;
    public readonly IRule? TargetRule;
    public readonly int TimeoutMs = 60_000;

    public DispatchOptions()
    {
        RoutingFlags = RoutingFlags.DefaultPublish;
        TargetMessageQueueType = MessageQueueType.All;
        PayloadMarshalOptions = new PayloadMarshalOptions();
    }

    public DispatchOptions(RoutingFlags routingFlags
            = RoutingFlags.DefaultPublish
        , MessageQueueType targetMessageQueueType = MessageQueueType.All
        , IRule? targetRule = null, PayloadMarshalOptions payloadMarshalOptions = new(), int timeoutMs = 60_000)
    {
        PayloadMarshalOptions = payloadMarshalOptions;
        RoutingFlags = routingFlags;
        TargetMessageQueueType = targetMessageQueueType;
        TargetRule = targetRule;
        TimeoutMs = timeoutMs;
    }

    public static bool operator ==(DispatchOptions lhs, DispatchOptions rhs) =>
        Equals(lhs.PayloadMarshalOptions, rhs.PayloadMarshalOptions) &&
        Equals(lhs.TargetMessageQueueType, rhs.TargetMessageQueueType) &&
        Equals(lhs.RoutingFlags, rhs.RoutingFlags) && Equals(lhs.TargetRule, rhs.TargetRule) && lhs.TimeoutMs == rhs.TimeoutMs;

    public static bool operator !=(DispatchOptions lhs, DispatchOptions rhs) => !(lhs == rhs);

    public bool Equals(DispatchOptions other) =>
        Equals(PayloadMarshalOptions, other.PayloadMarshalOptions) && TargetMessageQueueType == other.TargetMessageQueueType &&
        RoutingFlags == other.RoutingFlags &&
        Equals(TargetRule, other.TargetRule) && TimeoutMs == other.TimeoutMs;

    public override bool Equals(object? obj) => obj is DispatchOptions other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)TargetMessageQueueType, (int)RoutingFlags, TargetRule, TimeoutMs);
}
