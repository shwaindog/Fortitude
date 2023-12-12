#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Messaging;

public enum TargetType
{
    None
    , PublishAll
    , PublishOne
    , RequestResponse
    , TwoWayConversation
}

public readonly struct DispatchOptions
{
    public readonly EventQueueType TargetEventQueueType;
    public readonly TargetType TargetType;
    public readonly RoutingStrategySelectionFlags RoutingStrategySelectionFlags;
    public readonly IRule? TargetRule;

    public DispatchOptions(TargetType targetType = TargetType.PublishAll
        , RoutingStrategySelectionFlags routingStrategySelectionFlags
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue
        , EventQueueType targetEventQueueType = EventQueueType.All, IRule? targetRule = null)
    {
        TargetType = targetType;
        RoutingStrategySelectionFlags = routingStrategySelectionFlags;
        TargetEventQueueType = targetEventQueueType;
        TargetRule = targetRule;
    }
}
