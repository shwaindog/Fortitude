#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public readonly struct RouteSelectionResult
{
    public RouteSelectionResult(IEventQueue eventQueue, string strategyName
        , RoutingFlags routingFlags
        , IRule? rule = null)
    {
        EventQueue = eventQueue;
        StrategyName = strategyName;
        RoutingFlags = routingFlags;
        Rule = rule;
    }

    public readonly IEventQueue EventQueue;
    public readonly IRule? Rule;
    public readonly string StrategyName;
    public readonly RoutingFlags RoutingFlags;
}

public interface ISelectionStrategy
{
    string Name { get; }

    RouteSelectionResult? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions);

    IDispatchSelectionResultSet? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , DispatchOptions dispatchOptions
        , string destinationAddress);
}

public interface IPreviouslyCachedSelections
{
    IDispatchSelectionResultSet? LastDestinationSelectionResultSet(string destinationAddress
        , DispatchOptions requestOptions);

    IDispatchSelectionResultSet? LastSenderDestinationSelectionResultSet(IRule sender, string destinationAddress
        , DispatchOptions requestOptions);

    RouteSelectionResult? LastDeploySelectionResult(EventQueueType eventQueueType, DeploymentOptions requestOptions);
}

public interface ISaveSelectionResult
{
    void Save(IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions, RouteSelectionResult routeSelectionResult);

    void Save(IRule senderRule
        , DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet selectionResult);
}
